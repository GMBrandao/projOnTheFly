using System.Text;
using MongoDB.Driver;
using Newtonsoft.Json;
using projOnTheFly.Sales.Config;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace projOnTheFly.Sale.Consumers
{
    public class SaleConsumer
    {
        private const string QUEUE_NAME = "Sale";

        public void Start(IConnection connection)
        {
            try
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "Sales",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var returnMessage = Encoding.UTF8.GetString(body);
                        var sale = JsonConvert.DeserializeObject<Models.Entities.Sale>(returnMessage);

                        var client = new MongoClient("mongodb://localhost:27017");
                        var database = client.GetDatabase("projOnTheFlySale");
                        var _collection = database.GetCollection<Models.Entities.Sale>("Sale");
                        _collection.InsertOne(sale);
                    };

                    channel.BasicConsume(queue: "Sales",
                                         autoAck: true,
                                         consumer: consumer);

                    Thread.Sleep(2000);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
