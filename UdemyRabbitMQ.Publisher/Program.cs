﻿using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace UdemyRabbitMQ.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://gweyobmb:zym80wGtIiruyAK4DrYTQNQQvBf8h7nR@fox.rmq.cloudamqp.com/gweyobmb");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.QueueDeclare("hello-queue", true, false, false);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                string message = $"hello world{x}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

                Console.WriteLine($"Mesaj gönderilmiştir: {message}");

            });
            Console.ReadLine();
        }
    }
}