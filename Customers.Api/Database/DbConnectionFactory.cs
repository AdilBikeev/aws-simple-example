using System.Data;
using Microsoft.Data.Sqlite;

namespace Customers.Api.Database;

public interface IDbConnectionFactory
{
    public Task<IDbConnection> CreateConnectionAsync();
}

public class SqliteConnectionFactory : IDbConnectionFactory
{
    private const string ConnectionString = "Data Source=./customers.db";

    public SqliteConnectionFactory()
    {
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new SqliteConnection(ConnectionString);
        await connection.OpenAsync();
        return connection;
    }
}