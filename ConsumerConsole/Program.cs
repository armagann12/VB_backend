using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using InvoiceApi.Data;
using Microsoft.AspNetCore.Builder;
using InvoiceApi.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Consumer Console Started");


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



var factory = new ConnectionFactory
{
    HostName = "localhost"
};

var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare("pay", exclusive: false, durable: false, autoDelete:false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += async (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" Message received: {message}");

    //await Pay.payInvoice(message);

    PayMethod pay = new PayMethod();

    if(pay == null)
    {
        return;
    }
        
    await pay.payInvoice(message);


    
    Thread.Sleep(7000);

    Console.WriteLine("Done");
};


channel.BasicConsume(queue: "pay", autoAck: true, consumer: consumer);
Console.ReadKey();


public class PayMethod
{
    private readonly DataContext _context;

    public PayMethod()
    {
    }

    public PayMethod(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> payInvoice(string data)
    {
        Console.WriteLine(data);

        var id = int.Parse(data);

        Console.WriteLine(id);

        if (_context == null)
        {
            Console.WriteLine("Error");
            return false;
        }
        var invoiceModel = await _context.InvoiceModels.FindAsync(id);

        if (invoiceModel == null)
        {
            Console.WriteLine("Error");
            return false;
        }

        /*
        if (invoiceModel.UserModelId != int.Parse(uid))
        {
            Console.WriteLine("Error");
            return false;
        }

        */

        invoiceModel.Status = true;

        _context.Entry(invoiceModel).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!InvoiceModelExists(id))
            {
                Console.WriteLine("Error");
                return false;
            }
            else
            {
                throw;
            }
            
        }

        Console.WriteLine("Succeses");
        return true;

    }

    private bool InvoiceModelExists(int id)
    {
        return (_context.InvoiceModels?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}



