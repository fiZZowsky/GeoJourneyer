using Microsoft.Data.Sqlite;

namespace GeoJourneyer.Infrastructure.Persistance;

public class DatabaseContext
{
    private readonly string _connectionString;

    public DatabaseContext(string connectionString)
    {
        _connectionString = connectionString;
        EnsureDatabase();
    }

    private void EnsureDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
    }

    public SqliteConnection CreateConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }
}