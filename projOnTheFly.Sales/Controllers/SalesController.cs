using Microsoft.AspNetCore.Mvc;
using projOnTheFly.Models;
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
            return await _saleService.GetByFlight(iata, rab, schedule);
        }

        //api/sales/passagenrs/cpf
        [HttpGet("passagenrs/{cpf}")]
        public async Task<ActionResult<Sale>> GetByPassenger(string cpf)
        {
            return await _saleService.GetSaleByPassenger(cpf);
        }

        //api/sales/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetSales(string id)
        {
            return await _saleService.GetById(id);
        }

        //api/sales/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<SalePutRequest>> PutSales(string id, SalePutRequest salePutRequest)
        {
            Sale saleUpdate = await _saleService.GetById(id);
            if (saleUpdate== null) return NotFound();
          
            saleUpdate.Sold = salePutRequest.Sold;
            saleUpdate.Reserved = !salePutRequest.Sold;

            await _saleService.Update(saleUpdate);

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

            List<Passenger> passengerRequest = await PassengerService.CheckPassengers(saleSoldRequest.Passengers);

            if (passengerRequest == null && !passengerRequest.Any()) return NotFound();


            bool invalidPassagenrs = true;

            foreach (var p in passengerRequest)
            {
                if (p.Status == false && saleSoldRequest.Passengers.Contains(p.CPF))
                {
                    invalidPassagenrs = true;
                }
            }

            if (invalidPassagenrs) 
                return BadRequest("A lista de passageiros contém um inválido");

            Flight? flightRequest = await FlightService.GetFlightAsync(saleSoldRequest.Iata, saleSoldRequest.Rab, saleSoldRequest.Schedule);
            
            if (flightRequest == null) return NotFound();

            if (flightRequest.Sale < passagerCount) return BadRequest("Não contém assentos disponiveis para essa venda");

            Sale saleSold = new(saleSoldRequest.Passengers, flightRequest);

            saleSold.Sold = saleSoldRequest.Sold;
            saleSold.Reserved = !saleSoldRequest.Sold;

            await _saleService.Create(saleSold);

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

            List<Passenger> passengerRequest = await PassengerService.CheckPassengers(saleReservedRequest.Passengers);

            if (passengerRequest == null && !passengerRequest.Any()) return NotFound();


            bool invalidPassagenrs = true;

            foreach (var p in passengerRequest)
            {
                if (p.Status == false && saleReservedRequest.Passengers.Contains(p.CPF))
                {
                    invalidPassagenrs = true;
                }
            }

            if (invalidPassagenrs)
                return BadRequest("A lista de passageiros contém um inválido");

            Flight? flightRequest = await FlightService.GetFlightAsync(saleReservedRequest.Iata, saleReservedRequest.Rab, saleReservedRequest.Schedule);

            if (flightRequest == null) return NotFound();


            if (flightRequest.Sale < passagerCount) return BadRequest("Não contém assentos disponiveis para essa venda");

            Sale saleSold = new(saleReservedRequest.Passengers, flightRequest);

            saleSold.Reserved = saleReservedRequest.Reserved;
            saleSold.Sold = !saleReservedRequest.Reserved;

            await _saleService.Create(saleSold);

            SalePostSoldResponse saleResponse = new()
            {
                Id = saleSold.Id,
                Sold = saleSold.Sold,
            };

            return CreatedAtAction("GetByFlight", new { Id = saleResponse.Id }, saleResponse);
        }

    }
}
