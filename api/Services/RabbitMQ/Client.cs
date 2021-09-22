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
        private IConnection _connection;
        private readonly IModel _channel;

        public Client(string queueName)
        {
            _hostname = Environment.GetEnvironmentVariable("GAS_RABBITMQ_HOSTNAME") ??
                        throw new Exception("Env GAS_RABBITMQ_HOSTNAME not specified");

            if (!ConnectionExists())
            {
                throw new ApiError(HttpStatusCode.FailedDependency, "Cannot connect to message broker");
            }
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(
                exchange: "gas.media",
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false
            );
            _channel.QueueDeclare(
                queue: $"gas.media.{queueName}",
                durable: false,
                exclusive: false,
                autoDelete: false
            );
            _channel.QueueBind(
                queue: $"gas.media.{queueName}",
                exchange: "gas.media",
                routingKey: ""
            );
        }

        public void SendPayload(object payload)
        {
            var props = _channel.CreateBasicProperties();

            props.ContentType = $"{System.Net.Mime.MediaTypeNames.Application.Json}; charset={Encoding.UTF8.HeaderName}";
            props.ContentEncoding = "identity";

            var json = JsonSerializer.Serialize(payload);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(exchange: "gas.media", routingKey: "", basicProperties: props, body: body);
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