using NLayerCore.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace NLayerInfrastructure.MessageBroker
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private readonly ConnectionFactory _factory;
        private readonly string _queueName = "currency_queue";

        public RabbitMQConsumer()
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };
        }

        public string Consume()
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var result = channel.BasicGet(queue: _queueName, autoAck: true);
                if (result != null)
                {
                    var message = Encoding.UTF8.GetString(result.Body.ToArray());
                    return message;
                }
                return null;
            }
        }
    }
}
