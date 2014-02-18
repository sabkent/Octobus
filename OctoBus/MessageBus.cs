using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace OctoBus
{
    public class MessageBus
    {
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _model;
        private string _exchangeName = "OctoBusExchange";
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

            _model.ExchangeDeclare(_exchangeName, ExchangeType.Fanout, true);
            _model.QueueDeclare("OctoBusQueue", true, false, false, null);

            _model.QueueBind("OctoBusQueue", _exchangeName, "");
            
        }

        public void Send(string message)
        {
            var properties = _model.CreateBasicProperties();
            properties.SetPersistent(true);

            var data = Encoding.Default.GetBytes(message);

            _model.BasicPublish(_exchangeName, "", properties, data);
        }
    }
}
