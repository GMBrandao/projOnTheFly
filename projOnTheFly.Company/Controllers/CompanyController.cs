using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projOnTheFly.Services;
using projOnTheFly.Models;
using projOnTheFly.Company.Services;
using System.Net;
using System.Text.RegularExpressions;
using projOnTheFly.Company.DTO;
using System.ComponentModel.DataAnnotations;

namespace projOnTheFly.Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyService _companyService;


        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;

        }

        [HttpGet]
        public  async Task<List<Models.Company>> Get() => await _companyService.Get();


        [HttpGet("{cnpj}")]
        public async Task<ActionResult<Models.Company>> Get(string cnpj)
        {
            Models.Company company = null;
            string cnpjFixed = Regex.Replace(cnpj, "%2F", "/");
            string formatedCnpj = "";
            if (cnpj.Length == 14)
            {
                formatedCnpj = Regex.Replace(cnpj, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5");
            }
            else
            {
                company = await _companyService.Get(cnpjFixed);
                if (company == null) return NotFound();
                if (company.Status == false) return BadRequest("STATUS INATIVO");
                return  company;
            }
            company = await _companyService.Get(formatedCnpj);
            if (company == null) return NotFound();
            if (company.Status == false) return BadRequest("STATUS INATIVO");
            return  company;


        }

        [HttpPost]
        public async Task<ActionResult<CompanyPostRequest>> Create(CompanyPostRequest companyRequest)
        {
            var validated = ValidatesCnpj.IsCnpj(companyRequest.Cnpj);
            if (!validated)
            {
                return BadRequest("Cnpj inválido");
            }

            if (companyRequest == null) return NotFound();

            var data = PostOfficeService.GetAddressAsync(companyRequest.Address.ZipCode).Result;

            if (data.ZipCode == null) return BadRequest("CEP inválido");

            string FomatedCnpj = Regex.Replace(companyRequest.Cnpj, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5");

            Models.Company company = new()
            {
                Cnpj = FomatedCnpj,
                Name = companyRequest.Name,
                NameOpt = companyRequest.NameOpt,
                DtOpen = DateTime.Now,
                Status = companyRequest.Status,
                Address = new Address()
                {
                    Street = data.Street,
                    City = data.City,
                    Number = companyRequest.Address.Number,
                    NeighborHood = data.NeighborHood,
                    Complement = data.Complement,
                    ZipCode = data.ZipCode,
                    State = data.State,

                }

            };
            await  _companyService.Create(company);
            return StatusCode(201);
        }


        [HttpPut("{cnpj}")]
        public async Task<ActionResult<CompanyPutRequest>> Update(string cnpj, CompanyPutRequest companyPutRequest)
        {
            string cnpjFixed = "";
            
                                   
            if (cnpj.Length == 14)
            {
                cnpjFixed = Regex.Replace(cnpj, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5");               
            }
            else
            {
                cnpjFixed = Regex.Replace(cnpj, "%2F", "/");              
            }

            var validated = ValidatesCnpj.IsCnpj(cnpjFixed);
            if (!validated)
            {
                return BadRequest("Cnpj inválido");
            }

            var found = await _companyService.Get(cnpjFixed);
            if (found.Status == false) return BadRequest("STATUS INATIVO");
            if (found == null) return BadRequest(NotFound());

            var data =  PostOfficeService.GetAddressAsync(companyPutRequest.Address.ZipCode).Result;

            if (data.ZipCode == null) return BadRequest("CEP inválido");

            Models.Company company = new()
            {
                
                Name = companyPutRequest.Name,
                NameOpt = companyPutRequest.NameOpt,               
                Status = companyPutRequest.Status,
                Cnpj = cnpjFixed,
                Address = new Address()
                {
                    Street = data.Street,
                    City = data.City,
                    Number = companyPutRequest.Address.Number,
                    NeighborHood = data.NeighborHood,
                    Complement = data.Complement,
                    ZipCode = data.ZipCode,
                    State = data.State,
                }
            };           

            _companyService.Update(cnpjFixed, company);

            return StatusCode(200);
        }



        [HttpDelete("{cnpj}")]
        public async Task<ActionResult> Delete(string cnpj)
        {
            string cnpjFixed = Regex.Replace(cnpj, "%2F", "/");
            string formatedCnpj = "";

            var validated = ValidatesCnpj.IsCnpj(cnpjFixed);
            if (!validated)
            {
                return BadRequest("Cnpj inválido");
            }
            var found = _companyService.Get(cnpjFixed);
            if (found == null) return NotFound();

            if (cnpj.Length == 14)
            {
                formatedCnpj = Regex.Replace(cnpj, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5");
                _companyService.Delete(formatedCnpj);
                return StatusCode(200);
            }
            else
            {
                _companyService.Delete(cnpjFixed);
                return StatusCode(200);
            }

        }
    }
}
