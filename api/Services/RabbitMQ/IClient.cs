namespace api.Services.RabbitMQ
{
    public interface IClient
    {
        public void SendPayload(object payload);
    }
}