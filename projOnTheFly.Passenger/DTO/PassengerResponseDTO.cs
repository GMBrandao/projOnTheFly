using projOnTheFly.Models;

namespace projOnTheFly.Passenger.DTO
{
    public class PassengerResponseDTO
    {
        public string Name { get; set; }
        public DateTime DtRegister { get; set; }
        public string Status { get; set; }

        public string StatusPassenger(bool status)
        {
            if (status == true) return "Ativo";
            else return "Inativo";
        }
    }
}
