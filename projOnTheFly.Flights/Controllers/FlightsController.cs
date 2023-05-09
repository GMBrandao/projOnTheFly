using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projOnTheFly.Flights.Service;
using projOnTheFly.Models;

namespace projOnTheFly.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly FlightService _flightService;


        public FlightsController(FlightService flightService)
        {
            _flightService = flightService;
        }

        // api/Flights/{iata}/{rab}/{schedule}
        [HttpGet("{iata}/{rab}/{schedule}")]
        public async Task<Flight> GetByFilters(string iata, string rab, DateTime schedule) 
            => await _flightService.GetByFilters(iata, rab,schedule);

    }
}
