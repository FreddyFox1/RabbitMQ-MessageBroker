using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSender
{
    public class Sender
    {
        public void Send(string message)
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
            //Кодируем сообщение 
            var Data = Encoding.UTF8.GetBytes(message);
            //Отправляем сообщение в очередь с именем "hello"
            channel.BasicPublish("", "hello", null, Data);

            Console.WriteLine(" [X] Отправлено {0}", message);
            Console.WriteLine(" Нажмите [enter] для выхода.");
            Console.ReadLine();

            //Закрываем соединение
            connection.Close();
            channel.Close();

        }
    }
}
