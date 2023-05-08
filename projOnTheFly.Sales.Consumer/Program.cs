using projOnTheFly.Sale.Consumers;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };
var saleConsumer = new SaleConsumer();
using (var connection = factory.CreateConnection())
{
    while (true)
    {
        saleConsumer.Start(connection);
    }
}

