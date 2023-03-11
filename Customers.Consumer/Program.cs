using Amazon.Runtime;
using Amazon.SQS;
using Customers.Consumer.Services;
using Customers.Consumer.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(QueueSettings.Key));
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
builder.Services.AddHostedService<QueryConsumerService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();