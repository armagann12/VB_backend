using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using InvoiceApi.Data;
using Microsoft.EntityFrameworkCore;


var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

optionsBuilder.UseSqlServer("server=localhost\\sqlexpress;database=invoiceapidb;trusted_connection=true");


var factory = new ConnectionFactory
{
    HostName = "localhost"
};
var connection = factory.CreateConnection();
using
var channel = connection.CreateModel();

channel.QueueDeclare("payQueue", exclusive: false, durable: true, autoDelete: false);
var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, eventArgs) => {
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Product message received: {message}");

    Task.Run(() =>
    {
        using (var _context = new DataContext(optionsBuilder.Options))
        {
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

            bool InvoiceModelExists(int id)
            {
                return (_context.InvoiceModels?.Any(e => e.Id == id)).GetValueOrDefault();
            }
        }
    });
};
channel.BasicConsume(queue: "payQueue", autoAck: true, consumer: consumer);
Console.ReadKey();