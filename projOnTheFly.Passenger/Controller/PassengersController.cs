﻿using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projOnTheFly.Models;
using projOnTheFly.Passenger.DTO;
using projOnTheFly.Passenger.Service;
using projOnTheFly.Services;

namespace projOnTheFly.Passenger.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {
        private readonly PassengerService _passengerService;
        public PassengersController(PassengerService passengerService)
        {
            _passengerService = passengerService;
        }

        [HttpGet]
        public Task<List<Models.Passenger>> GetAll() =>_passengerService.Get();

        [HttpGet("{cpf}")]
        public async Task<ActionResult<Models.Passenger>> GetPassengerByCPF(string cpf)
        {
            var validateCpf = new ValidateCPF(cpf);

            if (!validateCpf.IsValid()) return BadRequest("CPF inválido");

            return  await _passengerService.Get(cpf);
        }

        [HttpPost]
        public async Task<ActionResult<PassengerResponse>> Post(PassengerRequest passengerRequest)
        {
            var validateCpf = new ValidateCPF(passengerRequest.CPF);

            if (!validateCpf.IsValid()) return BadRequest("CPF inválido");
            
            if (passengerRequest == null) return UnprocessableEntity("Requisição de passageiro inválida");

            AddressDTO? postOffice = await PostOfficeService.GetAddressAsync(passengerRequest.Address.ZipCode!);

            if(postOffice == null)  return BadRequest("CEP inválido");

            char charToUpper = char.ToUpper(passengerRequest.Gender);

            if (charToUpper != 'M' || charToUpper != 'F') 
                return BadRequest("Gênero inválido");

            Models.Passenger passenger = new()
            {
                CPF = passengerRequest.CPF,
                Name = passengerRequest.Name,
                Gender = charToUpper,
                DateBirth = passengerRequest.DateBirth,
                DtRegister = DateTime.Now,
                Status = passengerRequest.Status,
                Address = new Address
                {
                    City = postOffice.City,
                    ZipCode = postOffice.ZipCode,
                    Complement = postOffice.Complement,
                    NeighborHood = postOffice.NeighborHood,
                    Number = passengerRequest.Address.Number,
                    State = postOffice.State,
                    Street = postOffice.Street,
                    Country = postOffice.Country
                },
            };

            passenger.Phone = passenger.RemovePhoneMask(passengerRequest.Phone);

            await _passengerService.Create(passenger);

            PassengerResponse passengerResponse = new()
            {
                Name = passenger.Name,
                DtRegister = passenger.DtRegister,
            };

            passengerResponse.Status = passengerResponse.StatusPassenger(passenger.Status);

            return CreatedAtAction("GetPassengerByCPF", new { cpf = passenger.CPF }, passengerResponse);

        }

        //[HttpPut("{cpf}")]
        //public async Task<ActionResult> Update(string cpf,PassengerRequest passengerRequest)
        //{
        //    var validateCpf = new ValidateCPF(cpf);

        //    if (!validateCpf.IsValid()) return BadRequest("CPF inválido");

        //    Models.Passenger passenger = new()
        //    {
        //        CPF = cpf,
        //        Name = passengerRequest.Name,
        //        Gender = passengerRequest.Gender,
        //        Phone = passengerRequest.Phone,
        //        DateBirth = passengerRequest.DateBirth,
        //        DtRegister = passengerRequest.DtRegister,
        //        Status = passengerRequest.Status,
        //        Address = passengerRequest.Address,
        //    };

        //    var passengerUpdate = _passengerService.Get(passenger.CPF);

        //    if (passengerUpdate == null) return NotFound();
            
        //    await _passengerService.Update(passenger);
            
        //    return NoContent();
        //}

        [HttpDelete("{cpf}")]
        public async Task<ActionResult> Delete(string cpf)
        {
            var validateCpf = new ValidateCPF(cpf);

            if (!validateCpf.IsValid()) return BadRequest("CPF inválido");

            var passengerDelete = _passengerService.Get(cpf);
            
            if (passengerDelete== null) return NotFound();
            
            await _passengerService.Delete(cpf);
            
            return NoContent();
        }


    }
}
