using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projOnTheFly.Services;
using projOnTheFly.Models;
using projOnTheFly.Company.Services;
using System.Net;
using System.Text.RegularExpressions;

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
        public ActionResult<List<Models.Company>> Get() => _companyService.Get();


        [HttpGet("{cnpj}")]
        public ActionResult<Models.Company> Get(string cnpj)
        {
            Models.Company company = null;
            string cnpj2 = Regex.Replace(cnpj, "%2F", "/");
            string formatedCnpj = "";
            if(cnpj.Length == 14)
            {
                 formatedCnpj = Regex.Replace(cnpj, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5");                
            }
            else
            {
                company = _companyService.Get(cnpj2);
                return company;
            }
             company = _companyService.Get(formatedCnpj);
            return company;          

           
        }

        [HttpPost]
        public ActionResult<Models.Company> Create(Models.Company company)
        {
            var validated = ValidatesCnpj.IsCnpj(company.Cnpj);
            if (!validated)
            {
                return NotFound();                
            }

            if (company == null) return NotFound();            

            var data = PostOfficeService.GetAddress(company.Address.ZipCode).Result;
            Address ad = new Address();

            ad.Street = data.Logradouro;
            ad.City = data.City;
            ad.Number = company.Address.Number;
            ad.NeighborHood = data.Bairro;
            ad.Complement = data.Complemento;
            ad.ZipCode = data.CEP;
            ad.State = data.State;
            company.Address = ad;
            string FomatedCnpj = Regex.Replace(company.Cnpj, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5");
            company.Cnpj = FomatedCnpj;
           

            _companyService.Create(company);
            return StatusCode(201);
        }

        [HttpPut("{cnpj:length(18)}")]
        public ActionResult<Models.Company> Update(string cnpj, Models.Company company)
        {
            var a = _companyService.Get(cnpj);
            if (a == null) return NotFound();

            _companyService.Update(cnpj, company);

            return StatusCode(200); 
        }



        [HttpDelete("{cnpj:length(19)}")]
        public ActionResult Delete(string cnpj)
        {
            if (cnpj == null) return NotFound();

            var returned = _companyService.Get(cnpj);
            if (returned == null) return NotFound();
            _companyService.Delete(cnpj);

            return Ok();
        }
    }
}
