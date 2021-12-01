using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace UdemyRabbitMQ.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://gweyobmb:zym80wGtIiruyAK4DrYTQNQQvBf8h7nR@fox.rmq.cloudamqp.com/gweyobmb");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            //channel.ExchangeDeclare("logs-fanout", ExchangeType.Fanout, true);

            var randomQueueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(randomQueueName, "logs-fanout", "", null);

            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(randomQueueName, false, consumer);

            Console.WriteLine("Loglar Dinleniyor...");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine("Gelen Mesaj: " + message);
                Thread.Sleep(500);
                channel.BasicAck(e.DeliveryTag, false);
            };

            Console.ReadLine();
        }
    }
}
