using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projOnTheFly.Aircrafts.DTO;
using projOnTheFly.Aircrafts.Services;
using projOnTheFly.Company.Services;
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
        public Task<List<Aircraft>> Get() => _aircraftsService.GetAll();

        [HttpGet("{rab}", Name = "GetRab")]
        public async Task<ActionResult<Aircraft>> Get(string rab)
        {
            var aircraft = _aircraftsService.Get(rab);
            if(aircraft == null) return BadRequest("RAB inválido");
            return Ok(aircraft);
        }

        [HttpPost]
        public async Task<ActionResult<AircraftPost>> Create(AircraftPost aircraftPost)
        {
            var validRAB = new ValidateRAB(aircraftPost.Rab);
            if (!validRAB.IsValid()) return BadRequest("RAB inválido");
            if (aircraftPost == null) return UnprocessableEntity("Requisição de aeronave inválida");
            Models.Company company = await GetCompany.GetCompanyAsync(aircraftPost.Company_Cnpj);
            if (company == null) return BadRequest("CNPJ da empresa inválido");
            Aircraft aircraft = new()
            {
                Rab = aircraftPost.Rab,
                Capacity = aircraftPost.Capacity,
                DtRegistry = DateTime.Now,
                DtLastFlight =  null,
                Company = company,
            };
            await _aircraftsService.Create(aircraft);
            return Ok();
        }

        [HttpDelete]
        public ActionResult Delete(string rab)
        {
            if(rab == null) return NotFound();
            _aircraftsService.Delete(rab);
            return Ok();
        }

    }
}
