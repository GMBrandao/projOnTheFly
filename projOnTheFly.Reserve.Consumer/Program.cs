using projOnTheFly.Reserve.Consumer;
using RabbitMQ.Client;


Console.WriteLine("Reserve Consumer");
var factory = new ConnectionFactory() { HostName = "localhost" };
var saleConsumer = new ReserveConsumer();
using (var connection = factory.CreateConnection())
{
    while (true)
    {
        saleConsumer.Start(connection);
    }
}