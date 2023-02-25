using System.Text.Json;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

using SqsPublisher;

const string CustomersQueueName = "customers";

var sqsClient = new AmazonSQSClient(RegionEndpoint.USEast1);

var customer = new CustomerCreated()
{
    Id = Guid.NewGuid(),
    Email = "fuf@mail.ru",
    FullName = "F I O",
    DateOfBirth = new DateTime(1, 1, 1),
    GitHubUsername = "adcoder"
};

var queryUrlResponse = await sqsClient.GetQueueUrlAsync(CustomersQueueName);

var message = sqsClient.SendMessageAsync(new SendMessageRequest()
{
    MessageBody = JsonSerializer.Serialize(customer),
    QueueUrl = queryUrlResponse.QueueUrl
});