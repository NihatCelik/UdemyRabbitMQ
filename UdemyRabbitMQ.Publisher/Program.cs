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

            channel.ExchangeDeclare("logs-topic", ExchangeType.Topic, true);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                Enum.GetNames(typeof(LogNames)).ToList().ForEach(u =>
                {
                    Random rnd = new Random();
                    LogNames log1 = (LogNames)rnd.Next(1, 5);
                    LogNames log2 = (LogNames)rnd.Next(1, 5);
                    LogNames log3 = (LogNames)rnd.Next(1, 5);

                    var routeKey = $"{log1}.{log2}.{log3}";

                    string message = $"log-type: {log1}-{log2}-{log3}";

                    var messageBody = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("logs-topic", routeKey, null, messageBody);

                    Console.WriteLine($"Log gönderilmiştir: {message}");
                });

            });
            Console.ReadLine();
        }
    }
}
