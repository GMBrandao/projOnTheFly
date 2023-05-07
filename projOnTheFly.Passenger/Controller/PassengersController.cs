using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projOnTheFly.Services;

namespace projOnTheFly.Passenger.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult<Models.Passenger>> PostPassenger(Models.Passenger passenger)
        {
            var validateCpf = new ValidateCPF(passenger.CPF);
            if (!validateCpf.IsValid())
                return BadRequest("CPF inválido");
            return Ok();
        }
    }
}
