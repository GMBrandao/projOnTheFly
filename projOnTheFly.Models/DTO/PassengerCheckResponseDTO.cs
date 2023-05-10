namespace projOnTheFly.Models.DTO
{
    public class PassengerCheckResponseDTO
    {
        public string CPF { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public bool Underage { get; set; }
    }
}
