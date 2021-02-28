using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSender
{
    class Program
    {
        static void Main(string[] args)
        {
            Sender sender = new Sender();
            Console.Write("Enter your message: ");
            var message = Console.ReadLine();
            sender.Send(message);
        }
    }
}
