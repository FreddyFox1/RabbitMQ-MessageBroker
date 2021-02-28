using System;
using System.Text;
using RabbitMQ.Client;

namespace WorkQueues_Task
{
    public class NewTask
    {
        public void Sender()
        {
            //Задаем имя хоста брокера сообщений
            var factory = new ConnectionFactory() { HostName = "localhost" };

            //Создаем соединение с хостом
            var connection = factory.CreateConnection();
            //Создаем канал для связи
            var channel = connection.CreateModel();
            //Объявляем очередь с именем "task_queue", 
            //durable = true(в случае ошибки очередь будет сохранена)
            channel.QueueDeclare("task_queue", true, false,
                false, null);
            var args = Console.ReadLine();
            var message = GetMessage(args);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            //Отправляем сообщение в очередь с именем "task_queue"
            channel.BasicPublish("", "task_queue", properties, body);
            Console.WriteLine(" [x] Отправлено {0}", message);

            Console.WriteLine(" Нажмите [enter] для выхода.");
            Console.ReadLine();

            //Закрываем соединения
            connection.Close();
            channel.Close();
        }

        private static string GetMessage(string[] args)
        {
            return args.Length > 0 ? string.Join(" ", args) : "Hello World!";
        }
    }
}