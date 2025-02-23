using RabbitMQ.Client;
using System.Text;

namespace Sender
{
    public class Sender
    {
        public async Task Send()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = await factory.CreateConnectionAsync())
            using (var channel = await connection.CreateChannelAsync())
            {
                await channel.QueueDeclareAsync(queue: "Test", durable: false, exclusive: false, autoDelete: false, arguments: null);

                string message = "GettingStarted";
                var body = Encoding.UTF8.GetBytes(message);

                var properties = new BasicProperties();

                await channel.BasicPublishAsync(exchange: "", routingKey: "Test", mandatory: false, basicProperties: properties, body: body);

                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        public class Program
        {
            static async Task Main(string[] args)
            {
                var sender = new Sender();
                await sender.Send();
            }
        }
    }
}
