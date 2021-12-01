using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace UdemyRabbitMQ.Publisher
{
    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Warning = 3,
        Info = 4
    }

    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://gweyobmb:zym80wGtIiruyAK4DrYTQNQQvBf8h7nR@fox.rmq.cloudamqp.com/gweyobmb");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare("logs-direct", ExchangeType.Direct, true);

            Enum.GetNames(typeof(LogNames)).ToList().ForEach(u =>
            {
                var routeKey = $"route-{u}";

                var queueName = $"direct-queue-{u}";
                channel.QueueDeclare(queueName, true, false, false);

                channel.QueueBind(queueName, "logs-direct", routeKey, null);
            });


            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                LogNames log = (LogNames)new Random().Next(1, 5);

                string message = $"log {log}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                var routeKey = $"route-{log}";

                channel.BasicPublish("logs-direct", routeKey, null, messageBody);

                Console.WriteLine($"Log gönderilmiştir: {message}");

            });
            Console.ReadLine();
        }
    }
}
