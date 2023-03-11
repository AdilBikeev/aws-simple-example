using Amazon.SQS;
using Amazon.SQS.Model;
using Customers.Consumer.Messages;
using Customers.Consumer.Settings;
using Microsoft.Extensions.Options;

namespace Customers.Consumer.Services;

public class QueryConsumerService : BackgroundService
{
    private readonly IAmazonSQS _sqs;
    private readonly IOptions<QueueSettings> _queueSettings;
    private string? _queueUrl;
    
    public QueryConsumerService(
        IAmazonSQS sqs,
        IOptions<QueueSettings> queueSettings)
    {
        _sqs = sqs;
        _queueSettings = queueSettings;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueUrl = await GetQueueUrlAsync(stoppingToken);
        var receiveMessageRequest = new ReceiveMessageRequest()
        {
            QueueUrl = queueUrl
        };
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var recieveMessage = await _sqs.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);
    
            foreach (var message in recieveMessage.Messages)
            {
                var messageType = message.MessageAttributes["MessageType"].StringValue;
                switch (messageType)
                {
                    case nameof(CustomerCreated):
                        Console.WriteLine(message.Body);
                        break;
                }
                
                await _sqs.DeleteMessageAsync(queueUrl, message.ReceiptHandle, stoppingToken);
            }

            await Task.Delay(3000, stoppingToken);
        }
    }
    
    private async Task<string> GetQueueUrlAsync(CancellationToken cancellationToken)
    {
        if (_queueUrl is not null)
        {
            return _queueUrl;
        }
        
        var queueUrlResponse = await _sqs.GetQueueUrlAsync(_queueSettings.Value.Name, cancellationToken);
        _queueUrl = queueUrlResponse.QueueUrl;
        return _queueUrl;
    }
}