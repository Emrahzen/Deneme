using Deneme.ApplicationDBContext;
using Deneme.Models;
using MassTransit;
using System.Threading.Tasks;

public class DenemeConsumer : IConsumer<DenemeClass>
{
    private readonly AppDBContext _dbContext;    



    public DenemeConsumer(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Task Consume(ConsumeContext<DenemeClass> context)
    {
        try
        {
            // Consumer içinde yapılacak işlemler
            var receivedMessage = context.Message;
            Console.WriteLine($"Received message: Id={receivedMessage.Id}, Name={receivedMessage.Heat}");
            _dbContext.denemeClass.Add(receivedMessage);
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }


        // ...işlemler...

        return Task.CompletedTask;
    }
}