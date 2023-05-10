using Microsoft.AspNetCore.Mvc;
using projOnTheFly.Aircrafts.DTO;
using projOnTheFly.Aircrafts.Services;
using projOnTheFly.Models;
using projOnTheFly.Services;

namespace projOnTheFly.Aircrafts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftsController : ControllerBase
    {
        private readonly AircraftsService _aircraftsService;

        public  AircraftsController(AircraftsService aircraftsService)
        {
            _aircraftsService = aircraftsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Aircraft>>> Get()
        {
            var aircraftExists = await _aircraftsService.GetAll();
            if (aircraftExists.Count == 0) return NotFound("Não há aeronaves cadastradas");
            return Ok(aircraftExists);

        }

        [HttpGet("{rab}", Name = "GetRab")]
        public async Task<ActionResult<Aircraft>> Get(string rab)
        {
            var aircraft = await _aircraftsService.Get(rab.ToUpper());
            if(aircraft == null) return BadRequest("RAB inválido");
            return Ok(aircraft);
        }

        [HttpPost]
        public async Task<ActionResult<AircraftPost>> Create(AircraftPost aircraftPost)
        {
            var validRAB = new ValidateRAB(aircraftPost.Rab);
            if (!validRAB.IsValid()) return BadRequest("RAB inválido");
            if (aircraftPost == null) return UnprocessableEntity("Requisição de aeronave inválida");
            Models.Company company = await GetCompany.GetCompanyAsync(aircraftPost.cnpjCompany);
            if (company == null) return BadRequest("CNPJ da empresa inválido");
            Aircraft aircraft = new()
            {
                Rab = aircraftPost.Rab.ToUpper(),
                Capacity = aircraftPost.Capacity,
                DtRegistry = DateTime.Now,
                DtLastFlight = null,
                Company = company,
            };
            await _aircraftsService.Create(aircraft);
            return Ok();
        }

        [HttpPut("{rab}",Name = "Update")]
        public async Task<ActionResult<AircraftPut>> Update( string rab, AircraftPut aircraftPut)
        {
            var aircraftExists = await GetAircraft.GetAircraftAsync(rab);
            if (aircraftExists == null) return NotFound();
            var validRAB = new ValidateRAB(rab);
            if (!validRAB.IsValid()) return BadRequest("RAB inválido");
            Models.Company company = await GetCompany.GetCompanyAsync(aircraftPut.cnpjCompany);
            if (company == null) return BadRequest("CNPJ da empresa inválido");

            Aircraft aircraft = new()
            {
                Rab = rab.ToUpper(),
                DtLastFlight = aircraftPut.DtLastFlight,
                Company = company,
            };

            aircraft.DtRegistry = aircraftExists.DtRegistry;

            await _aircraftsService.Update(aircraft.Rab, aircraft);

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(string rab)
        {
            var aircraftExists = await GetAircraft.GetAircraftAsync(rab);
            if (aircraftExists == null) return NotFound();
             await _aircraftsService.Delete(rab);
            return Ok();
        }

    }
}
