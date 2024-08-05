using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Email;
using GulDiyet.Core.Domain.Entities;

namespace GulDiyet.Core.Application.Services
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly ILogger<RabbitMQService> _logger;
        private readonly RabbitMQConfiguration _rabbitMQConfig;

        public RabbitMQService(ILogger<RabbitMQService> logger, IOptions<RabbitMQConfiguration> rabbitMQConfig)
        {
            _logger = logger;
            _rabbitMQConfig = rabbitMQConfig.Value;
        }

        public void SendMessage(SaveEmailViewModel emailViewModel)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQConfig.HostName,
                UserName = _rabbitMQConfig.UserName,
                Password = _rabbitMQConfig.Password
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "emailQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(emailViewModel));

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "", routingKey: "emailQueue", basicProperties: properties, body: body);

            _logger.LogInformation("Sent message to emailQueue");
        }

        public void StartListening()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQConfig.HostName,
                UserName = _rabbitMQConfig.UserName,
                Password = _rabbitMQConfig.Password
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "emailQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var emailViewModel = JsonSerializer.Deserialize<SaveEmailViewModel>(message);

                _logger.LogInformation("Received message from emailQueue");

                // kulanıcıya maıl gondermek ıslemlerı ıcınolay tetıkleme
                await Task.Run(() => {  });

                _logger.LogInformation("Email processed successfully");
            };

            channel.BasicConsume(queue: "emailQueue", autoAck: true, consumer: consumer);
        }
    }
}
