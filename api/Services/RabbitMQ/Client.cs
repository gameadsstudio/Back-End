using System;
using System.Net;
using System.Text;
using System.Text.Json;
using api.Errors;
using RabbitMQ.Client;

namespace api.Services.RabbitMQ
{
    public class Client: IClient
    {
        private readonly string _hostname;
        private readonly string _queueName;
        private IConnection _connection;

        public Client(string queueName)
        {
            _queueName = queueName;
            _hostname = Environment.GetEnvironmentVariable("GAS_RABBITMQ_HOSTNAME") ??
                        throw new Exception("Env GAS_RABBITMQ_HOSTNAME not specified");
        }

        public void SendPayload(object payload)
        {
            if (!ConnectionExists())
            {
                throw new ApiError(HttpStatusCode.FailedDependency, "Cannot connect to message broker");
            }
            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var json = JsonSerializer.Serialize(payload);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                throw new ApiError(HttpStatusCode.FailedDependency, "Cannot connect to message broker");
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }
    }
}