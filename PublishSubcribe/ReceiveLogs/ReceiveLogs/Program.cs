using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiveLogs
{
    class Program
    {
        public static void Main()
        {
            //Задаем имя хоста брокера сообщений
            var factory = new ConnectionFactory() { HostName = "localhost" };
            //Создаем соединение с хостом
            var connection = factory.CreateConnection();
            //Создаем канал для связи
            var channel = connection.CreateModel();
            //Создаем Exchange
            channel.ExchangeDeclare("logs", ExchangeType.Fanout);
            //Получаем автоматически сгенерированное имя очереди и записываем его в переменную для дальшней работы с ним
            var queueName = channel.QueueDeclare().QueueName;
            //Биндинг Exchang'a для пересылки сообщений в очередь logs
            channel.QueueBind(queueName, "logs", "");

            Console.WriteLine(" [*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine(" [x] {0}", message);
            };

            //Передаем имя очереди
            //Ack = true: удаление сообщения как только оно достаавлено.
            channel.BasicConsume(queueName, true, consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
