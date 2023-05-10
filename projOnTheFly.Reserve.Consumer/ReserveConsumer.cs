using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Newtonsoft.Json;
using projOnTheFly.Models.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace projOnTheFly.Reserve.Consumer
{
    public class ReserveConsumer
    {

        public void Start(IConnection connection)
        {
            try
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "Reserve",
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

                    channel.BasicConsume(queue: "Reserve",
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
