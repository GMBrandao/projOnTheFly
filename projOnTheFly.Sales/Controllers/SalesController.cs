using Microsoft.AspNetCore.Mvc;

namespace projOnTheFly.Sales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        public void Test()
        {
            var cpfs = new List<string> { "" };
            var passagers = new List<Models.Passenger>(); // _passagerService.Get();


            // testar para ver se atender essa validação
            //if (cpfs.Distinct().Count() != cpfs.Count()) return BadRequest("A lista de passageiros contém cpfs duplicados");

            bool invalido = true;

            foreach (var p in passagers)
            {
                if (p.Status == false && cpfs.Contains(p.CPF))
                {
                    invalido = true;
                }
            }
            
            //if (invalido) return BadRequest("A lista de passageiros contém um inválido");
            // valida se os cpfs existem no microservice de passageiro
            var assentosDisponveis = 1; 
            
            // buscar assentos do voo
            //if(cpfs.Count > assentosDisponveis) return BadRequest("Não contém assentos disponiveis para essa venda");

        }
    }
}
