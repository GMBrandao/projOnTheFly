﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using projOnTheFly.Models.DTO;
using projOnTheFly.Models.Entities;
using projOnTheFly.Sales.DTO;
using projOnTheFly.Sales.Service;
using projOnTheFly.Services;
using RabbitMQ.Client;
using System.Text;

namespace projOnTheFly.Sales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly SaleService _saleService;
        private readonly ConnectionFactory _factory;

        public SalesController(SaleService service, ConnectionFactory factory)
        {
            _saleService = service;
            _factory = factory;
        }

        //api/sales/flight/cpf
        [HttpGet("flight/{iata}/{rab}/{schedule}")]
        public async Task<ActionResult<Sale>> GetByFlight(string iata, string rab, string schedule)
        {
            return await _saleService.GetByFlightAsync(iata, rab, schedule);
        }

        //api/sales/passagenrs/cpf
        [HttpGet("passagenrs/{cpf}")]
        public async Task<ActionResult<Sale>> GetByPassenger(string cpf)
        {
            return await _saleService.GetSaleByPassengerAsync(cpf);
        }

        //api/sales/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetSales(string id)
        {
            return await _saleService.GetByIdAsync(id);
        }

        //api/sales/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<SalePutRequestDTO>> PutSales(string id, SalePutRequestDTO salePutRequest)
        {
            Sale saleUpdate = await _saleService.GetByIdAsync(id);
            if (saleUpdate == null) return NotFound();

            saleUpdate.Sold = salePutRequest.Sold;
            saleUpdate.Reserved = !salePutRequest.Sold;

            await _saleService.UpdateAsync(saleUpdate);

            return NoContent();
        }

        //api/sales
        [HttpPost("sold")]
        public async Task<ActionResult<SalePostSoldRequestDTO>> PostSold(SalePostSoldRequestDTO saleSoldRequest)
        {
            var passagerCount = saleSoldRequest.Passengers.Count;

            if (saleSoldRequest == null) return UnprocessableEntity("Requisição de vendas inválida");

            if (saleSoldRequest.Passengers.Distinct().Count() != passagerCount)
                return BadRequest("A lista de passageiros contém cpfs duplicados");

            PassengerCheckDTO passengerCheck = new()
            {
                CpfList = saleSoldRequest.Passengers
            };

            List<PassengerCheckResponseDTO> passengerRequest = await PassengerService.CheckPassengersAsync(passengerCheck);

            if (passengerRequest == null || !passengerRequest.Any()) return BadRequest("Dados de passageiros não encontrados");

            var passengerCheckAge = passengerRequest.First();

            if (passengerCheckAge.Underage == true)
                return BadRequest("O primeiro passegeiro da lista não possui idade para " +
                "realizar a compra da passagem");

            bool invalidPassagenrs = false;

            foreach (var p in passengerRequest)
            {
                if (p.Status == false && saleSoldRequest.Passengers.Contains(p.CPF))
                {
                    invalidPassagenrs = true;
                }
            }

            if (invalidPassagenrs)
                return BadRequest("A lista de passageiros contém um inválido");

            Flight? flightRequest = await FlightService.CheckFlightAsync(saleSoldRequest.Iata, saleSoldRequest.Rab, saleSoldRequest.Schedule);

            if (flightRequest == null) return NotFound();

            if (flightRequest.Aircraft.Capacity < passagerCount) return BadRequest("Não contém assentos disponiveis para essa venda");

            Sale sale = new(saleSoldRequest.Passengers, flightRequest);

            sale.Sold = saleSoldRequest.Sold;
            sale.Reserved = !saleSoldRequest.Sold;

            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    channel.QueueDeclare(
                        queue: "Sales",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );

                    var stringfieldMessage = JsonConvert.SerializeObject(sale);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfieldMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "Sales",
                        basicProperties: null,
                        body: bytesMessage
                        );
                }
            }

            await FlightService.DecrementSaleAsync(saleSoldRequest.Iata, saleSoldRequest.Rab, saleSoldRequest.Schedule, passagerCount);

            SalePostSoldResponseDTO saleResponse = new()
            {
                Id = sale.Id,
                Sold = sale.Sold,
            };

            return Ok();
        }

        //api/sales
        [HttpPost("reserved")]
        public async Task<ActionResult<SalePostReservedRequestDTO>> PostReserved(SalePostReservedRequestDTO saleReservedRequest)
        {
            var passagerCount = saleReservedRequest.Passengers.Count;

            if (saleReservedRequest == null) return UnprocessableEntity("Requisição de vendas inválida");

            if (saleReservedRequest.Passengers.Distinct().Count() != passagerCount)
                return BadRequest("A lista de passageiros contém cpfs duplicados");

            PassengerCheckDTO passengerCheck = new()
            {
                CpfList = saleReservedRequest.Passengers
            };

            List<PassengerCheckResponseDTO> passengerRequest = await PassengerService.CheckPassengersAsync(passengerCheck);

            if (passengerRequest == null || !passengerRequest.Any()) return NotFound();

            var passengerCheckAge = passengerRequest.First();

            if (passengerCheckAge.Underage == true)
                return BadRequest("O primeiro passegeiro da lista não possui idade para " +
                "realizar a compra da passagem");

            bool invalidPassagenrs = false;

            foreach (var p in passengerRequest)
            {
                if (p.Status == false && saleReservedRequest.Passengers.Contains(p.CPF))
                {
                    invalidPassagenrs = true;
                }
            }

            if (invalidPassagenrs)
                return BadRequest("A lista de passageiros contém um inválido");

            Flight? flightRequest = await FlightService.CheckFlightAsync(saleReservedRequest.Iata, saleReservedRequest.Rab, saleReservedRequest.Schedule);

            if (flightRequest == null) return NotFound();

            if (flightRequest.Status != true)
            {
                return BadRequest("Esse vôo foi cancelado");
            }

            if((flightRequest.Sale + passagerCount) < flightRequest.Aircraft.Capacity) return BadRequest("Não contém assentos disponíveis para essa venda");

            Sale sale = new(saleReservedRequest.Passengers, flightRequest);

            sale.Reserved = saleReservedRequest.Reserved;
            sale.Sold = !saleReservedRequest.Reserved;

            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    channel.QueueDeclare(
                        queue: "Reserve",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );

                    var stringfieldMessage = JsonConvert.SerializeObject(sale);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfieldMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "Reserve",
                        basicProperties: null,
                        body: bytesMessage
                        );
                }
            }

            await FlightService.DecrementSaleAsync(saleReservedRequest.Iata, saleReservedRequest.Rab, saleReservedRequest.Schedule, passagerCount);

            SalePostSoldResponseDTO saleResponse = new()
            {
                Id = sale.Id,
                Sold = sale.Sold,
            };

            return Ok();
        }

        //api/sales/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _saleService.DeleteOneAsync(id);

            return NoContent();
        }
    }
}
