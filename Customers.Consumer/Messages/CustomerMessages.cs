using MediatR;

namespace Customers.Consumer.Messages;

public class CustomerCreated : ISQSMessage
{
    public Guid Id { get; init; }

    public string GitHubUsername { get; init; }

    public string FullName { get; init; }

    public string Email { get; init; }

    public DateTime DateOfBirth { get; init; }
}

public class CustomerUpdated : ISQSMessage
{
    public Guid Id { get; init; }

    public string GitHubUsername { get; init; }

    public string FullName { get; init; }

    public string Email { get; init; }

    public DateTime DateOfBirth { get; init; }
}

public class CustomerDeleted : ISQSMessage
{
    public Guid Id { get; init; }
}