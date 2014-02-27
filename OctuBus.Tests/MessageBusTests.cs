using NUnit.Framework;
using OctoBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctuBus.Tests
{
    [TestFixture]
    public class MessageBusTests
    {
        
        [Test]
        public void Publish()
        {
            using (var messageBus = new MessageBus())
            {
                messageBus.Send("hello");
            }
        }

        [Test]
        public void Consume()
        {
            MessageBroker broker = new MessageBroker();

            broker.Start();
        }
    }
}
