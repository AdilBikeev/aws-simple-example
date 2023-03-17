using System.Text.Json;
using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using SnsPublisher;

var snsClient = new AmazonSimpleNotificationServiceClient(RegionEndpoint.USEast1);
var topic = await snsClient.FindTopicAsync("customers");
var customer = new CustomerCreated()
{
   Id = Guid.NewGuid(),
   Email = "fuf@mail.ru",
   FullName = "F I O",
   DateOfBirth = new DateTime(1, 1, 1),
   GitHubUsername = "adcoder"
};

var response = await snsClient.PublishAsync(new PublishRequest()
{
   TargetArn = topic.TopicArn,
   Message = JsonSerializer.Serialize(customer),
   MessageAttributes = new Dictionary<string, MessageAttributeValue>()
   {
      {
         "MessageType", new MessageAttributeValue
         {
            DataType = "String",
            StringValue = nameof(CustomerCreated)
         }
      }
   }
});