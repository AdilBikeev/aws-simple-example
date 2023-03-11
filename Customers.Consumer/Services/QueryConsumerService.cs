using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Customers.Consumer.Messages;
using Customers.Consumer.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace Customers.Consumer.Services;

public class QueryConsumerService : BackgroundService
{
    private readonly IAmazonSQS _sqs;
    private readonly IMediator _mediator;
    private readonly IOptions<QueueSettings> _queueSettings;
    private readonly ILogger<QueryConsumerService> _logger;
    private string? _queueUrl;
    
    public QueryConsumerService(
        IAmazonSQS sqs,
        IMediator mediator,
        IOptions<QueueSettings> queueSettings,
        ILogger<QueryConsumerService> logger)
    {
        _sqs = sqs;
        _mediator = mediator;
        _queueSettings = queueSettings;
        _logger = logger;
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
                var type = Type.GetType($"Customers.Consumer.Messages.{messageType}");
                
                if (type is null)
                {
                    _logger.LogWarning("Unknown message type: {MessageType}", messageType);
                    await _sqs.DeleteMessageAsync(queueUrl, message.ReceiptHandle, stoppingToken);
                    continue;
                }

                var body = JsonSerializer.Deserialize(message.Body, type);

                try
                {
                    await _mediator.Send(body, stoppingToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception.ToString());
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