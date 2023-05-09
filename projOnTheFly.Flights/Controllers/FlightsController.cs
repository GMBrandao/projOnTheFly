using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using projOnTheFly.Flights.Service;
using projOnTheFly.Models;
using projOnTheFly.Services;
using FlightService = projOnTheFly.Flights.Service.FlightService;

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

        [HttpGet]
        public async Task<List<Flight>> Get() => await _flightService.Get();

        [HttpGet(Name ="GetOneFlight")]
        public async Task<ActionResult<Flight>> Get(string iata, string rab, DateTime schedule) => await _flightService.Get(iata, rab, schedule);


        [HttpPost]
        public async Task<ActionResult<FlightDTO>> Create(FlightDTO flight)
        {
            AirportDTO airport = await GetAirport.GetAirportAsync(flight.Iata);

            Flight f = new()
            {
                Sale = flight.Sales,
                Status = flight.Status,
                Airport = airport
                
            };

             await _flightService.Create(f);
            return StatusCode(201); 
        }

        [HttpPut]
        public async Task<ActionResult<Flight>> Update(Flight flight)
        {
            _flightService.Update( flight);

            return StatusCode(200);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(string iata, string rab, DateTime schedule)
        {

            _flightService.Delete(iata, rab, schedule);
            return StatusCode(200);

        }
      
        [HttpGet("{iata}/{rab}/{schedule}")]
        public async Task<Flight> GetByFilters(string iata, string rab, DateTime schedule) 
            => await _flightService.GetByFilters(iata, rab,schedule);

    }
}
