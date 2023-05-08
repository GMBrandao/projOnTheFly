using projOnTheFly.Models;

namespace projOnTheFly.Company.DTO
{
    public class CompanyPutRequest
    {
        public string Name { get; set; }
        public string NameOpt { get; set; }
        public bool Status { get; set; }
        public CompanyAddressRequest Address { get; set; }
    }
}
