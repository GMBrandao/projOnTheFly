using System.Text;
using Newtonsoft.Json;
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
                    channel.QueueDeclare(queue: QUEUE_NAME,
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

                        /* para chamar salvar direto no mongo 
                         * 
                         * 
                        var client = new MongoClient(settings.ConnectionString);
                        var database = client.GetDatabase(settings.DatabaseName);
                        _collection = database.GetCollection<Sale>(settings.SaleCollectionName);
                        */

                    };

                    channel.BasicConsume(queue: QUEUE_NAME,
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
