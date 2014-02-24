using System;
using System.Text;
using RabbitMQ.Client;

namespace OctoBus
{
    public class MessageBus : IDisposable
    {
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _model;
        private string _exchangeName = "OctoBusExchange";
        private string _queueName = "OctoBusQeueue";
        private string _routingKey = "";
        public MessageBus()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = _connectionFactory.CreateConnection();
            _model = _connection.CreateModel();

            _model.QueueDeclare(_queueName, true, false, false, null);
            _model.ExchangeDeclare(_exchangeName, ExchangeType.Fanout, true);
            _model.QueueBind(_queueName, _exchangeName, _routingKey);
        }

        public void Send(string message)
        {
            var properties = _model.CreateBasicProperties();
            properties.SetPersistent(true);

            var data = Encoding.ASCII.GetBytes(message);

            _model.BasicPublish(_exchangeName, _routingKey, properties, data);
        }

        public void Dispose()
        {
            _model.Dispose();
            _connection.Dispose();
        }
    }
}
