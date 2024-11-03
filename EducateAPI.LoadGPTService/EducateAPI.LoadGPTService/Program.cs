using EducateAPI.LoadGPTService;
using EducateAPI.LoadGPTService.Interfaces;
using EducateAPI.LoadGPTService.RabbitMQ;
using EducateAPI.LoadGPTService.RabbitMQ.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IGPTClient, GPTClient>();
builder.Services.AddSingleton<IRabbitMQConnectionFactory, RabbitMQConnection>();
builder.Services.AddSingleton(typeof(IRabbitMQPublisher<>), typeof(RabbitMQPublisher<>));
builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMQ"));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
