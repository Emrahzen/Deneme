
using Deneme.ApplicationDBContext;
using Deneme.Models;
using denemeclass.SignalR;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using TemperatureProducer;

public class Program
{
    private static RGateSignalRClient _signalR;
    public static void Main(string[] args)
    {
        StartTemperatureProducer();
        SignalR();
        while (true)
        {
            Console.WriteLine("Enter'a basınca veri degişecek");
            Console.ReadLine();

            ChangeTemparature();

            Thread.Sleep(300);

            SendSignalRMessage();

        }
    }
    public static void StartTemperatureProducer()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(new Uri("rabbitmq://localhost/"), h =>
            {
                h.Username("guest");
                h.Password("guest");
            });
        });

        var tempService = new TempService(cancellationTokenSource.Token, bus);

        try
        {
            bus.Start(); // RabbitMQ'ya bağlanmayı başlatıyoruz
            Console.WriteLine("RabbitMQ bağlantısı başarıyla kuruldu.");

            while (true)
            {
                Console.WriteLine("Enter'a basınca veri değişecek");
                Console.ReadLine();

 

                tempService.StartTemp(); // TempService ile sıcaklık verisini RabbitMQ'ya gönderiyoruz

                
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hata oluştu: " + ex.Message);
        }
        finally
        {
            bus.Stop(); // RabbitMQ bağlantısını durduruyoruz
        }
    }
    public static int GenerateRandomTemperature()
    {
        Random random = new Random();
        int randomTemperature = random.Next(-20, 51);
        return randomTemperature;
    }

    public static void ChangeTemparature()
    {
        ///TODO : random bir sıcaklık üret

        int randomTemperature = GenerateRandomTemperature();

        var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>();
        optionsBuilder.UseSqlServer("Server=DESKTOP-LMFD98G\\SQLSERVERENES;Database=Deneme;Trusted_Connection=True;Integrated Security=True;");

        using (var dbContext = new AppDBContext(optionsBuilder.Options))
        {
            dbContext.denemeClass.Add(new DenemeClass { Heat = randomTemperature });
            dbContext.SaveChanges();
        }
        ///TODO: daha sonrasında veritabanına kaydet
    }

    public static void SignalR()
    {
        _signalR = new RGateSignalRClient("https://localhost:7254/apiSignalRhub");
        _signalR.Start();
    }
    public static void SendSignalRMessage()
    {
        _signalR.SendMessageService("message");
    }

}