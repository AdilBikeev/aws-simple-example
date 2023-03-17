using Amazon.SimpleNotificationService.Model;

namespace Customers.Api.Messaging;

public interface ISqsMessenger
{
    Task<PublishResponse> SendMessageAsync<T>(T message);
}