using Deneme.ApplicationDBContext;
using Deneme.SignalR;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


builder.Services.AddSignalR();
builder.Services.AddSingleton<SignalRHub>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder
                .SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});
builder.Services.AddDbContext<AppDBContext>(cfg => cfg.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<DenemeConsumer>();
    cfg.UsingRabbitMq((context, config) =>
    {
        config.ReceiveEndpoint("Consumer.P2C_Ford_V710.GatewayStatusChanged", e =>
        {
            e.PrefetchCount = 1;
            e.ConfigureConsumer<DenemeConsumer>(context);

        });
        config.Host("Localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

    });

    var tmp = cfg;

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors();
app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<SignalRHub>("/apiSignalRhub");
});

app.MapControllers();

app.Run();
