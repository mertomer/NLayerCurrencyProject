using NLayerCore.Interfaces;
using NLayerInfrastructure.MessageBroker; // Burada ExchangeRateService'in doðru versiyonunu belirliyoruz
using NLayerService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddScoped<IExchangeRateService, NLayerInfrastructure.MessageBroker.ExchangeRateService>(); // Tam yol kullanýlarak belirsizlik giderildi
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
