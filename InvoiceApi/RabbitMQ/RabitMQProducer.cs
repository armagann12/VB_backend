﻿using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace InvoiceApi.RabbitMQ
{
    public class RabitMQProducer : IRabitMQProducer
    {
        public void SendProductMessage<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare("payQueue", exclusive: false, autoDelete: false, durable: true);

            var json = JsonConvert.SerializeObject(message);

            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "pay", body: body);

            Console.WriteLine($" Message send: {message}");
        }
    }
}
