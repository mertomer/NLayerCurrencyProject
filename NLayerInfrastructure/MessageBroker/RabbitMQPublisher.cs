using NLayerCore.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace NLayerInfrastructure.MessageBroker
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly ConnectionFactory _factory;
        private readonly string _queueName = "currency_queue";

        public RabbitMQPublisher()
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };
        }

        public void Publish(string message)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
            }
        }
    }
}
