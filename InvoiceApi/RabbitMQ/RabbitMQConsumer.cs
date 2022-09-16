using InvoiceApi.Data;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace InvoiceApi.RabbitMQ
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private readonly DataContext _context;
        public static string message;

        public RabbitMQConsumer(DataContext context)
        {
            _context = context;
        }
        public void RecieveProductMessage()
        {


            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare("payQueue", exclusive: false, durable: true, autoDelete: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {

                var body = eventArgs.Body.ToArray();
                message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" Message received: {message}");

                //Thread.Sleep(7000);


                //uid controlü eksik

                if (_context.InvoiceModels == null)
                {
                    Console.WriteLine("error");
                }
                var invoiceModel = _context.InvoiceModels.Find(int.Parse(message));

                if (invoiceModel == null)
                {
                    Console.WriteLine("error");
                }

                invoiceModel.Status = true;

                _context.Entry(invoiceModel).State = EntityState.Modified;

                try
                {
                     _context.SaveChanges();
                    Console.WriteLine($" PAYED {message}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceModelExists(int.Parse(message)))
                    {
                        Console.WriteLine("Error");
                    }
                    else
                    {
                        throw;
                    }
                }

            };

            channel.BasicConsume(queue: "pay", autoAck: true, consumer: consumer);
            Console.ReadKey();

        }

        private bool InvoiceModelExists(int id)
        {
            return (_context.InvoiceModels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
