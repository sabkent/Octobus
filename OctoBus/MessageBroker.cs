using System.Collections;
using System.Diagnostics;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace OctoBus
{
    /// <summary>
    /// http://fygt.wordpress.com/tag/eventingbasicconsumer/
    /// </summary>
    public class MessageBroker
    {
        private IConnection _connection;
        private IModel _model;
        private string _exchangeName = "FUCKINGWORK";
        private string _queueName = "FUCKINGWORK";

        public MessageBroker()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = connectionFactory.CreateConnection();
            _model = _connection.CreateModel();
            _model.BasicQos(0, 1, false);

            const bool durable = true, exchangeAutoDelete = false, queueAutoDelete = false, exclusive = false;

            _model.ExchangeDeclare(_exchangeName, "fanout", durable, exchangeAutoDelete, null);

            // replicate the queue to all hosts. Queue arguments are optional
            var queueArgs = new Dictionary<string, object>{{"x-ha-policy", "all"}};

            _model.QueueDeclare(_queueName, durable, exclusive, queueAutoDelete, queueArgs);
            _model.QueueBind(_queueName, _exchangeName, "", null);
        }

        public void Start()
        {
            var consumer = new EventingBasicConsumer {Model = _model};
            consumer.Received += (sender, args) =>
            {
                Console.WriteLine("message received");
            };
            
            _model.BasicConsume(_queueName, false, consumer);
        }
    }
}
