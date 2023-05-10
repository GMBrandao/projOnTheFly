using MongoDB.Driver;
using projOnTheFly.Models.DTO;
using projOnTheFly.Passenger.Settings;

namespace projOnTheFly.Passenger.Service
{
    public class PassengerService
    {
        private readonly IMongoCollection<Models.Entities.Passenger> _collection;
        public PassengerService(IProjOnTheFlyPassengerSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Models.Entities.Passenger>(settings.PassengerCollectionName);
        }
        public async Task<List<Models.Entities.Passenger>> GetAsync() => await _collection.Find(c => c.Status == true).ToListAsync();

        public Task<Models.Entities.Passenger> GetAsync(string cpf) => _collection.Find(c => c.CPF == cpf && c.Status == true).FirstOrDefaultAsync();

        public async Task<List<PassengerCheckResponseDTO>> PostCheckAsync(List<string> cpfList)
        {  
             var passengers = await _collection.Find(c => cpfList.Contains(c.CPF)).ToListAsync();

            List <PassengerCheckResponseDTO> passengerCheck = new(); 

            foreach (var passenger in passengers)
            {
                PassengerCheckResponseDTO passengerCheckResponse = new()
                {
                    CPF = passenger.CPF,
                    Name = passenger.Name,
                    Status = passenger.Status,
                    Underage = passenger.IsUnderage()
                };

                passengerCheck.Add(passengerCheckResponse);
            }
            return passengerCheck;
        }
        public async Task<Models.Entities.Passenger> CreateAsync(Models.Entities.Passenger passenger)
        {
            await _collection.InsertOneAsync(passenger);
            return passenger;
        }
        public async Task<Models.Entities.Passenger> UpdateAsync(Models.Entities.Passenger passenger)
        {
            await _collection.ReplaceOneAsync(c => c.CPF == passenger.CPF, passenger);
            return passenger;
        }
        public Task DeleteAsync(string cpf) => _collection.DeleteOneAsync(c => c.CPF == cpf);

    }
}
