using System.Text.Json;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

const string CustomersQueueName = "customers";
var cts = new CancellationTokenSource();




var sqsClient = new AmazonSQSClient(RegionEndpoint.USEast1);

var queryUrlResponse = await sqsClient.GetQueueUrlAsync(CustomersQueueName);

var receiveMessageRequest = new ReceiveMessageRequest()
{
    QueueUrl = queryUrlResponse.QueueUrl
};

while (!cts.IsCancellationRequested)
{
    var recieveMessage = await sqsClient.ReceiveMessageAsync(receiveMessageRequest, cts.Token);
    
    foreach (var message in recieveMessage.Messages)
    {
        Console.WriteLine($"Message ID: {message.MessageId}");
        Console.WriteLine($"Message Body: {message.Body}");
        await sqsClient.DeleteMessageAsync(queryUrlResponse.QueueUrl, message.ReceiptHandle, cts.Token);
    }

    await Task.Delay(3000);
}