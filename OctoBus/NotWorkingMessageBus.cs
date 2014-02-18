using System.Text;
using RabbitMQ.Client;

namespace OctoBus
{
    public class NotWorkingMessageBus
    {
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _model;

        private string _exchangeName = "OctoBus";
        private string _queueName = "OctoBusQueue";
        public NotWorkingMessageBus()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = _connectionFactory.CreateConnection();
            _model = _connection.CreateModel();

            _model.ExchangeDeclare(_exchangeName, ExchangeType.Fanout, true);
            _model.QueueDeclare(queue:_queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _model.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: "");
        }

        public void Publish(string message)
        {
            var properties = _model.CreateBasicProperties();
            properties.SetPersistent(true);

            var messageData = Encoding.Default.GetBytes(message);
            //_model.BasicPublish(_exchangeName, "", properties, messageData);
            _model.BasicPublish(exchange: _exchangeName, routingKey: "", basicProperties: properties, body: messageData);
        }
    }
}
