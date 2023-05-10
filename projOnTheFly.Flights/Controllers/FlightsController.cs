using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using projOnTheFly.Flights.DTO;
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

        [HttpGet("{iata}/{rab}/{schedule}")]
        public async Task<Flight> GetByFilters(string iata, string rab, DateTime schedule) 
            => await _flightService.GetByFilters(iata, rab,schedule);

        [HttpPost]
        public async Task<ActionResult<Models.FlightDTO>> Create(Models.FlightDTO flightDTO)
        {
            
            AirportDTO airport = await GetAirport.GetAirportAsync(flightDTO.Iata);
            if (flightDTO.Status == false) return BadRequest("status de vôo cancelado");            
            Aircraft aircraft = await GetAircraft.GetAircraftAsync(flightDTO.Rab);
            if (aircraft.Company.Status == false) return BadRequest("Companhia com restrição");

            Flight f = new()
            {
                Sale = flightDTO.Sales,
                Status = flightDTO.Status,
                Airport = airport,
                Aircraft = aircraft,
                Schedule = DateTime.Now,
                
            };

             await _flightService.Create(f);
            return StatusCode(201); 
        }

        [HttpPut]
        public async Task<ActionResult> Update(string iata, string rab, DateTime schedule)
        {

            _flightService.Update(iata, rab, schedule);            

            return StatusCode(204);
        }







        /*public async Task<ActionResult<Flight>> Update(Flight flight)
        {
            _flightService.Update( flight);

            return StatusCode(200);
        }*/

        [HttpDelete]
        public async Task<ActionResult> Delete(string iata, string rab, DateTime schedule)
        {
            var found = await _flightService.GetByFilters(iata, rab, schedule);
            if (found == null) return NotFound();
            _flightService.Delete(iata, rab, schedule);          
           
            return StatusCode(200);

        }
      

    }
}
