using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projOnTheFly.Services;
using projOnTheFly.Company.Services;
using System.Net;
using System.Text.RegularExpressions;
using projOnTheFly.Company.DTO;
using System.ComponentModel.DataAnnotations;
using projOnTheFly.Models.Entities;

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
        public  async Task<List<Models.Entities.Company>> Get() => await _companyService.Get();


        [HttpGet("{cnpj}")]
        public async Task<ActionResult<Models.Entities.Company>> Get(string cnpj)
        {
            cnpj = cnpj.Trim();
            Models.Entities.Company company = null;
            string cnpjFixed = Regex.Replace(cnpj, "%2F", "/");
            string formatedCnpj = "";
            if (cnpj.Length == 14)
            {
                formatedCnpj = Regex.Replace(cnpj, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5");
                var validated = ValidatesCnpj.IsCnpj(formatedCnpj);
                if (!validated)
                {
                    return BadRequest("CNPJ INVÁLIDO");
                }
            }
            else
            {                
                var validated = ValidatesCnpj.IsCnpj(cnpjFixed);
                if (!validated)
                {
                    return BadRequest("CNPJ INVÁLIDO");
                }
                company =  _companyService.Get(cnpjFixed);
                if (company == null) return NotFound();
                if (company.Status == false) return BadRequest("STATUS INATIVO");
                return  company;
            }
            company =  _companyService.Get(formatedCnpj);
            if (company == null) return NotFound();
            if (company.Status == false) return BadRequest("STATUS INATIVO");
            return  company;


        }

        [HttpPost]
        public async Task<ActionResult<CompanyPostRequest>> Create(CompanyPostRequest companyRequest)
        {
            companyRequest.Cnpj = companyRequest.Cnpj.Trim();
            var validated = ValidatesCnpj.IsCnpj(companyRequest.Cnpj);
            if (!validated)
            {
                return BadRequest("Cnpj inválido");
            }

            if (companyRequest == null) return NotFound();

            var data = PostOfficeService.GetAddressAsync(companyRequest.Address.ZipCode).Result;

            if (data.ZipCode == null) return BadRequest("CEP inválido");

            string FomatedCnpj = Regex.Replace(companyRequest.Cnpj, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5");

            Models.Entities.Company company = new()
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
            if( (company.NameOpt == null )  || (company.NameOpt =="string") || (company.NameOpt == ""))
            {
                company.NameOpt = companyRequest.Name;
            }
            await  _companyService.Create(company);
            return StatusCode(201);
        }


        [HttpPut("{cnpj}")]
        public async Task<ActionResult<CompanyPutRequest>> Update(string cnpj, CompanyPutRequest companyPutRequest)
        {
            string cnpjFixed = "";
            cnpj = cnpj.Trim();


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

            var found =  _companyService.Get(cnpjFixed);
            if (found.Status == false) return BadRequest("STATUS INATIVO");
            if (found == null) return BadRequest(NotFound());

            var data =  PostOfficeService.GetAddressAsync(companyPutRequest.Address.ZipCode).Result;

            if (data.ZipCode == null) return BadRequest("CEP inválido");

            Models.Entities.Company company = new()
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

            if ((company.NameOpt == null) || (company.NameOpt == "string") || (company.NameOpt == ""))
            {
                company.NameOpt = company.Name;
            }

            _companyService.Update(cnpjFixed, company);

            return StatusCode(200);
        }



        [HttpDelete("{cnpj}")]
        public async Task<ActionResult> Delete(string cnpj)
        {
            cnpj = cnpj.Trim();
            string formatedCnpj = "";           

            if (cnpj.Length == 14)
            {
                formatedCnpj = Regex.Replace(cnpj, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5");
                var validated = ValidatesCnpj.IsCnpj(formatedCnpj);
                if (!validated)
                {
                    return BadRequest("Cnpj inválido");
                }
                var found = _companyService.Get(formatedCnpj);
                if (found == null) return NotFound();

                 _companyService.AddInDeletedCollection(found); 
                    _companyService.Delete(formatedCnpj); 
                return StatusCode(200);
            }
            else
            {
                string cnpjFixed = Regex.Replace(cnpj, "%2F", "/");
                var validated = ValidatesCnpj.IsCnpj(cnpjFixed);
                if (!validated)
                {
                    return BadRequest("Cnpj inválido");
                }
                var found = _companyService.Get(cnpjFixed);
                if (found == null) return NotFound();
                _companyService.AddInDeletedCollection(found); 
                _companyService.Delete(cnpjFixed);
                return StatusCode(200);
            }

        }
    }
}
