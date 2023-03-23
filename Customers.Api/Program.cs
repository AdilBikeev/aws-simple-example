using Amazon.SimpleNotificationService;
using Customers.Api.Database;
using Customers.Api.Messaging;
using Customers.Api.Repositories;
using Customers.Api.Services;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
config.AddEnvironmentVariables("CustomersApi_");

// Add services to the container.
builder.Services.Configure<TopicSettings>(builder.Configuration.GetSection(TopicSettings.Key));
builder.Services.AddSingleton<IAmazonSimpleNotificationService, AmazonSimpleNotificationServiceClient>();
builder.Services.AddSingleton<ISqsMessenger, SqsMessenger>();

builder.Services.AddSingleton<ICustomerRepository, CustomerLocalRepository>();
builder.Services.AddSingleton<ICustomerService, CustomerService>();
builder.Services.AddSingleton<IGitHubService, GitHubService>();

builder.Services.AddHttpClient("GitHub", httpClient =>
{
    httpClient.BaseAddress = new Uri(config.GetValue<string>("GitHub:ApiBaseUrl")!);
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.Accept, "application/vnd.github.v3+json");
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.UserAgent, $"Course-{Environment.MachineName}");
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitializeAsync();

app.MapControllers();

app.Run();