﻿using System.Text.Json;
using Customers.Consumer.Messages;
using MediatR;

namespace Customers.Consumer.Handlers;

public class CustomerCreatedHandler : IRequestHandler<CustomerCreated>
{
    private readonly ILogger<CustomerCreatedHandler> _logger;

    public CustomerCreatedHandler(ILogger<CustomerCreatedHandler> logger)
    {
        _logger = logger;
    }
    
    public Task<Unit> Handle(CustomerCreated request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(JsonSerializer.Serialize(request));
        return Unit.Task;
    }
}