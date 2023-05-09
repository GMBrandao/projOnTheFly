using Microsoft.AspNetCore.Mvc;
using projOnTheFly.Models;
using projOnTheFly.Sales.DTO;
using projOnTheFly.Sales.Service;

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
            var saleUpdate = _saleService.GetById(id);

            if (saleUpdate== null) return NotFound();

            /* chamada do microsservico de passageiro e de voo (falta criar os microservicos)
             * 
            Flight flightRequest = await FlightService.GetFlightAsync(saleSoldRequest.Iata, saleSoldRequest.Rab, saleSoldRequest.Schedule);
            if (flightRequest == null) return NotFound();
            Passenger passengerRequest = await PassengerService.GetPassengerAsync(saleSoldRequest);
            if(passengerRequest == null) return NotFound();
            */

            if(salePutRequest.Passengers == null && !salePutRequest.Passengers.Any()) return NotFound();

            var flightRequest = new Flight();

            Sale sale = new(salePutRequest.Passengers, flightRequest, salePutRequest.Sold);
         
            await _saleService.Update(sale);

            return NoContent();
        }

        //api/sales
        [HttpPost("sold")]
        public async Task<ActionResult<SalePostSoldRequest>> PostSold(SalePostSoldRequest saleSoldRequest)
        {
            if (saleSoldRequest == null) return UnprocessableEntity("Requisição de vendas inválida");

            /* chamada do microsservico de passageiro e de voo (falta criar os microservicos)
             * 
            Flight flightRequest = await FlightService.GetFlightAsync(saleSoldRequest.Iata, saleSoldRequest.Rab, saleSoldRequest.Schedule);
            if (flightRequest == null) return NotFound();
            Passenger passengerRequest = await PassengerService.GetPassengerAsync(saleSoldRequest);
            if(passengerRequest == null) return NotFound();
            */
            var flightRequest = new Flight();

            Sale saleSold = new(saleSoldRequest.Passengers, flightRequest, saleSoldRequest.Sold );

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
        public async Task<ActionResult<Sale>> PostReserved(Sale sale)
        {
            await _saleService.Create(sale);

            return CreatedAtAction("GetByFlight", new { Id = sale.Id }, sale);
        }



        //Regras de serviço que faltam validar 
        public void Test()
        {
            var cpfs = new List<string> { "" };
            var passagers = new List<Models.Passenger>(); // _passagerService.Get();


            // testar para ver se atender essa validação
            //if (cpfs.Distinct().Count() != cpfs.Count()) return BadRequest("A lista de passageiros contém cpfs duplicados");

            bool invalido = true;

            foreach (var p in passagers)
            {
                if (p.Status == false && cpfs.Contains(p.CPF))
                {
                    invalido = true;
                }
            }
            
            //if (invalido) return BadRequest("A lista de passageiros contém um inválido");
            // valida se os cpfs existem no microservice de passageiro
            var assentosDisponveis = 1; 
            
            // buscar assentos do voo ()
            //if(cpfs.Count > assentosDisponveis) return BadRequest("Não contém assentos disponiveis para essa venda");
        }
    }
}
