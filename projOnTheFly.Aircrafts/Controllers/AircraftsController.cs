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
        private readonly CompanyService _companyService;

        public  AircraftsController(AircraftsService aircraftsService, CompanyService companyService)
        {
            _aircraftsService = aircraftsService;
            _companyService = companyService;
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
            Models.Company company = _companyService.GetCompany(aircraftPost.Company.Cnpj);

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
