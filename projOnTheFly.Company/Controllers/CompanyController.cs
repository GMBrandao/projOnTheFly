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
            string cnpjFixed = Regex.Replace(cnpj, "%2F", "/");
            string formatedCnpj = "";
            if(cnpj.Length == 14)
            {
                 formatedCnpj = Regex.Replace(cnpj, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5");                
            }
            else
            {
                company = _companyService.Get(cnpjFixed);
                if (company == null) return NotFound();
                return company;
            }
             company = _companyService.Get(formatedCnpj);
            if (company == null) return NotFound();
            return company;          

           
        }

        [HttpPost]
        public ActionResult<Models.Company> Create(Models.Company company)
        {
            var validated = ValidatesCnpj.IsCnpj(company.Cnpj);
            if (!validated)
            {
                return BadRequest("Cnpj inválido");
            }

            if (company == null) return NotFound();            

            var data = PostOfficeService.GetAddressAsync(company.Address.ZipCode).Result;
            if (data.ZipCode == null) return BadRequest("CEP inválido");
            Address ad = new Address();            
            ad.Street = data.Street;
            ad.City = data.City;
            ad.Number = company.Address.Number;
            ad.NeighborHood = data.NeighborHood;
            ad.Complement = data.Complement;
            ad.ZipCode = data.ZipCode;
            ad.State = data.State;
            company.Address = ad;
            string FomatedCnpj = Regex.Replace(company.Cnpj, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5");
            company.Cnpj = FomatedCnpj;         

            _companyService.Create(company);
            return StatusCode(201);
        }

        [HttpPut("{cnpj}")]
        public ActionResult<Models.Company> Update(string cnpj, Models.Company company)
        {   
            var a = _companyService.Get(cnpj);
            if (a == null) return NotFound();
            company.Id = a.Id;
            company.Cnpj = cnpj;
            var data = PostOfficeService.GetAddressAsync(company.Address.ZipCode).Result;
            if (data.ZipCode == null) return BadRequest("CEP inválido");
            Address ad = new Address();
            ad.Street = data.Street;
            ad.City = data.City;
            ad.Number = company.Address.Number;
            ad.NeighborHood = data.NeighborHood;
            ad.Complement = data.Complement;
            ad.ZipCode = data.ZipCode;
            ad.State = data.State;
            company.Address = ad;

            _companyService.Update(cnpj, company);

            return StatusCode(200); 
        }



        [HttpDelete("{cnpj}")]
        public ActionResult Delete(string cnpj)
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
