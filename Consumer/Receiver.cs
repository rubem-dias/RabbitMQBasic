using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Consumer
{
    public class Receiver
    {
        public async Task Receive()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = await factory.CreateConnectionAsync())
            using (var channel = await connection.CreateChannelAsync())
            {
                await channel.QueueDeclareAsync(
                    queue: "Test",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Recebido: {0}", message);

                    await Task.Yield();
                };

                await channel.BasicConsumeAsync(
                    queue: "Test",
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine(" Pressione [Enter] para sair.");
                Console.ReadLine();
            }
        }

        public class Program
        {
            static async Task Main(string[] args)
            {
                var receive = new Receiver();
                await receive.Receive();
            }
        }
    }
}
