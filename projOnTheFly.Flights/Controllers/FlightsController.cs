using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using projOnTheFly.Flights.DTO;
using projOnTheFly.Flights.Service;
using projOnTheFly.Models.DTO;
using projOnTheFly.Models.Entities;
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

        [HttpPost("check")]
        public async Task<Flight> CheckFlight(FlightCheck flightCheck) 
            => await _flightService.CheckFlight(flightCheck.Iata, flightCheck.Rab, flightCheck.Schedule);

        [HttpPost("decrement")]
        public async Task<ActionResult> DecrementSaleFlight(FlightDecrementCheckDTO flightCheck)
        {
            await _flightService.DecrementSale(flightCheck.Iata, flightCheck.Rab, flightCheck.Schedule, flightCheck.Number);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Models.DTO.FlightDTO>> Create(Models.DTO.FlightDTO flightDTO)
        {
            
            AirportDTO airport = await GetAirport.GetAirportAsync(flightDTO.Iata);
            Aircraft aircraft = await GetAircraft.GetAircraftAsync(flightDTO.Rab);
            if (aircraft == null) return NotFound("não existe no banco de dados");
            if (aircraft.Company.Status == false) return BadRequest("Companhia com restrição");

            Flight f = new()
            {                
                
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



        [HttpDelete]
        public async Task<ActionResult> Delete(string iata, string rab, DateTime schedule)
        {
            
            _flightService.Delete(iata, rab, schedule);          
           
            return StatusCode(200);

        }
      

    }
}
