using NLayerCore.Interfaces;
using NLayerInfrastructure.MessageBroker;
using NLayerInfrastructure.Services;
using NLayerService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// RedisService'i DI sistemine kaydedin
builder.Services.AddSingleton(new RedisService("localhost:6379")); // Redis baðlantý dizesi

// Dependency Injection
builder.Services.AddScoped<IExchangeRateService, NLayerInfrastructure.MessageBroker.ExchangeRateService>();
builder.Services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();
builder.Services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
