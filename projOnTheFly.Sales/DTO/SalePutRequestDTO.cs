using System.ComponentModel.DataAnnotations;
using projOnTheFly.Models;

namespace projOnTheFly.Sales.DTO
{
    public class SalePutRequestDTO
    {
        [Required]
        public bool Sold { get; set; }
    }
}
