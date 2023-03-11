using Amazon.Runtime;
using Amazon.SQS;
using Customers.Consumer.Services;
using Customers.Consumer.Settings;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(QueueSettings.Key));
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
builder.Services.AddHostedService<QueryConsumerService>();
builder.Services.AddMediatR(typeof(Program));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();