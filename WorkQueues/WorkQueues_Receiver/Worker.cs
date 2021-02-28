using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WorkQueues_Worker
{
    public class Worker
    {
        public void Receive()
        {
            //Задаем имя хоста брокера сообщений
            var factory = new ConnectionFactory() { HostName = "localhost" };
            //Создаем соединение с хостом
            var connection = factory.CreateConnection();
            //Создаем канал для связи
            var channel = connection.CreateModel();
            //Объявляем очередь с именем "task_queue"
            channel.QueueDeclare("task_queue", true, false,
                false, null);

            //Dispatching есть два типа: Round-robin и Fair dispatch. 
            //1.Round-robin отсылаем сообщение к следующему Worker'у в последовательности. (Неровная нагрузка между Worker'ами )
            //2.Fair dispatch - не дает больше одного сообщения Worker'у в один и тот же промежуток времени.
            
            //Fair dispatch
            channel.BasicQos(0, 1, false);

            Console.WriteLine("[*] Ожидание сообщений.");
            //
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnReceived;
            
            //Ack = false -> Есть два типа AutoAck и ManualAck
            //AutoAck когда сообщение доставлено, оно помечается на удаление.
            //ManualAck когда работа закончена вызывается метод BasicAck который позволяет RabbitMq удалить сообщение.
            channel.BasicConsume("task_queue", false, consumer);

            Console.WriteLine("Нажмите [enter] для выхода.");
            Console.ReadLine();
            
            void OnReceived(object model, BasicDeliverEventArgs ea)
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());

                Console.WriteLine(" [x] Получено {0}", message);
                DoHardWork(message);
                Console.WriteLine(" [x] Готово.");
                //Вызваем BasicAck который говорит что сообщение доставлено и его можно удалить.  
                channel.BasicAck(ea.DeliveryTag, false);
            }
        }

        //В зависимости от количества точек в сообщение имитируем сложность работы.
        private static void DoHardWork(string message)
        {
            int dots = message.Split('.').Length - 1;
            Thread.Sleep(dots * 1000);
        }
    }
}