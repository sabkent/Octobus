using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OctoBus;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageBroker messageBroker = new MessageBroker();
            messageBroker.Start();
            Console.ReadLine();
        }
    }
}
