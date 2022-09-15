using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Consumer Console Started");

var factory = new ConnectionFactory
{
    HostName = "localhost"
};

var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare("pay", exclusive: false, durable: false, autoDelete:false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" Message received: {message}");

    Thread.Sleep(7000);

    Console.WriteLine("Done");
};


channel.BasicConsume(queue: "pay", autoAck: true, consumer: consumer);
Console.ReadKey();