using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitLog
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Задаем имя хоста брокера сообщений
            var factory = new ConnectionFactory() { HostName = "localhost" };
            //Создаем соединение с хостом
            var connection = factory.CreateConnection();
            //Создаем канал для связи
            var channel = connection.CreateModel();

            //Exchange — обменник или точка обмена. В него отправляются сообщения.
            //Exchange распределяет сообщение в одну или несколько очередей.
            //Fanout exchange – все сообщения доставляются во все очереди даже если в сообщении задан ключ маршрутизации.
            channel.ExchangeDeclare("logs", ExchangeType.Fanout);

            var message = GetMessage(args);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("logs", "", null, body);
            Console.WriteLine(" [x] Sent {0}", message);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static string GetMessage(string[] args) =>
            args.Length > 0 ? string.Join(" ", args) : "info: Hello World!";
    }
}
