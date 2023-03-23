using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Customers.Api.Contracts.Data;

namespace Customers.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private const string TableName = "customers";
    private readonly IAmazonDynamoDB _dynamoDb;

    public CustomerRepository(IAmazonDynamoDB dynamoDb)
    {
        _dynamoDb = dynamoDb;
    }

    public async Task<bool> CreateAsync(CustomerDto customer)
    {
        var customerAsJson = JsonSerializer.Serialize(customer);
        var customerAsAttributes = Document.FromJson(customerAsJson).ToAttributeMap();

        var putItemRequest = new PutItemRequest()
        {
            TableName = TableName,
            Item = customerAsAttributes
        };

        var response = await _dynamoDb.PutItemAsync(putItemRequest);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<CustomerDto?> GetAsync(Guid id)
    {
        var getItemRequest = new GetItemRequest()
        {
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>()
            {
                { "pk", new AttributeValue() { S = id.ToString() } },
                { "sk", new AttributeValue() { S = id.ToString() } },
            }
        };

        var response = await _dynamoDb.GetItemAsync(getItemRequest);
        if (!response.Item.Any())
        {
            return null;
        }

        var itemAsJson = Document.FromAttributeMap(response.Item).ToJson();
        var customer = JsonSerializer.Deserialize<CustomerDto>(itemAsJson);

        return customer;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var scanRequest = new ScanRequest()
        {
            TableName = TableName
        };
        
        var response = await _dynamoDb.ScanAsync(scanRequest);
        if (!response.Items.Any())
        {
            return Enumerable.Empty<CustomerDto>();
        }

        var customers = response.Items.Select(x =>
        {
            var json = Document.FromAttributeMap(x).ToJson();
            return JsonSerializer.Deserialize<CustomerDto>(json);
        });

        return customers!;
    }

    public async Task<bool> UpdateAsync(CustomerDto customer)
    {
        customer.UpdateAt = DateTime.UtcNow;
        var itemAsJson = JsonSerializer.Serialize(customer);
        var itemAsAttributes = Document.FromJson(itemAsJson).ToAttributeMap();
        
        var putItemRequest = new PutItemRequest()
        {
            TableName = TableName,
            Item = itemAsAttributes
        };

        var response = await _dynamoDb.PutItemAsync(putItemRequest);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleteItemRequest = new DeleteItemRequest()
        {
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>()
            {
                { "pk", new AttributeValue() { S = id.ToString() } },
                { "sk", new AttributeValue() { S = id.ToString() } },
            }
        };

        var response = await _dynamoDb.DeleteItemAsync(deleteItemRequest);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }
}