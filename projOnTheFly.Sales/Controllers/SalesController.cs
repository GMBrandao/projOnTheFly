using Microsoft.AspNetCore.Mvc;
using projOnTheFly.Models;
using projOnTheFly.Passenger.DTO;
using projOnTheFly.Sales.DTO;
using projOnTheFly.Sales.Service;
using projOnTheFly.Services;

namespace projOnTheFly.Sales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly SaleService _saleService;
        public SalesController(SaleService service)
        {
            _saleService = service;
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
        public async Task<ActionResult<SalePutRequest>> PutSales(string id, SalePutRequest salePutRequest)
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
        public async Task<ActionResult<SalePostSoldRequest>> PostSold(SalePostSoldRequest saleSoldRequest)
        {
            var passagerCount = saleSoldRequest.Passengers.Count;

            if (saleSoldRequest == null) return UnprocessableEntity("Requisição de vendas inválida");

            // testar para ver se atender essa validação
            if (saleSoldRequest.Passengers.Distinct().Count() != passagerCount)
                return BadRequest("A lista de passageiros contém cpfs duplicados");

            PassengerCheck passengerCheck = new()
            {
                CpfList = saleSoldRequest.Passengers
            };

            List<PassengerCheckResponse> passengerRequest = await PassengerService.CheckPassengersAsync(passengerCheck);

            if (passengerRequest == null || !passengerRequest.Any()) return NotFound();

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

            if (flightRequest.Sale < passagerCount) return BadRequest("Não contém assentos disponiveis para essa venda");

            Sale saleSold = new(saleSoldRequest.Passengers, flightRequest);

            saleSold.Sold = saleSoldRequest.Sold;
            saleSold.Reserved = !saleSoldRequest.Sold;

            await _saleService.CreateAsync(saleSold);

            SalePostSoldResponse saleResponse = new()
            {
                Id = saleSold.Id,
                Sold = saleSold.Sold,
            };

            return CreatedAtAction("GetByFlight", new { Id = saleResponse.Id }, saleResponse);
        }

        //api/sales
        [HttpPost("reserved")]
        public async Task<ActionResult<SalePostReservedRequest>> PostReserved(SalePostReservedRequest saleReservedRequest)
        {
            var passagerCount = saleReservedRequest.Passengers.Count;

            if (saleReservedRequest == null) return UnprocessableEntity("Requisição de vendas inválida");

            // testar para ver se atender essa validação
            if (saleReservedRequest.Passengers.Distinct().Count() != passagerCount)
                return BadRequest("A lista de passageiros contém cpfs duplicados");

            PassengerCheck passengerCheck = new()
            {
                CpfList = saleReservedRequest.Passengers
            };

            List<PassengerCheckResponse> passengerRequest = await PassengerService.CheckPassengersAsync(passengerCheck);

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


            if (flightRequest.Sale < passagerCount) return BadRequest("Não contém assentos disponiveis para essa venda");

            Sale saleSold = new(saleReservedRequest.Passengers, flightRequest);

            saleSold.Reserved = saleReservedRequest.Reserved;
            saleSold.Sold = !saleReservedRequest.Reserved;

            await _saleService.CreateAsync(saleSold);

            SalePostSoldResponse saleResponse = new()
            {
                Id = saleSold.Id,
                Sold = saleSold.Sold,
            };

            return CreatedAtAction("GetByFlight", new { Id = saleResponse.Id }, saleResponse);
        }

    }
}
