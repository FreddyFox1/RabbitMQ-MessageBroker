using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleReceiver
{
    public class Receiver
    {
        public void Receive()
        {
            //Задаем имя хоста брокера сообщений
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            //Создаем соединение с хостом
            var connection = factory.CreateConnection();
            //Создаем канал для связи
            var channel = connection.CreateModel();
            //Объявляем очередь с именем "hello"
            channel.QueueDeclare("hello", false, false, false, null);

            //Создаем подписчика
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine(" [x] Доставлено {0}", message);
            };

           
            channel.BasicConsume("hello", true, consumer);

            Console.WriteLine(" Нажмите [enter] для выхода.");
            Console.ReadLine();
        }
    }
}
