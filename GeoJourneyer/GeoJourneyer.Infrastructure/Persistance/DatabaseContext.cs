using Dapper;
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
        var commands = new[]
        {
            "CREATE TABLE IF NOT EXISTS Countries (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, IsoCode TEXT NOT NULL)",
            "CREATE TABLE IF NOT EXISTS Places (Id INTEGER PRIMARY KEY AUTOINCREMENT, CountryId INTEGER NOT NULL, Name TEXT NOT NULL, Latitude REAL, Longitude REAL, Description TEXT)",
            "CREATE TABLE IF NOT EXISTS UserCountries (Id INTEGER PRIMARY KEY AUTOINCREMENT, UserId INTEGER NOT NULL, Country TEXT NOT NULL, Status INTEGER)",
            "CREATE TABLE IF NOT EXISTS TravelPlans (Id INTEGER PRIMARY KEY AUTOINCREMENT, UserId INTEGER NOT NULL, CountryId INTEGER NOT NULL, Name TEXT)",
            "CREATE TABLE IF NOT EXISTS TravelPlanStops (Id INTEGER PRIMARY KEY AUTOINCREMENT, TravelPlanId INTEGER NOT NULL, PlaceId INTEGER NOT NULL, [Order] INTEGER)",
            "CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY AUTOINCREMENT, Username TEXT NOT NULL, Email TEXT NOT NULL UNIQUE, PasswordHash TEXT NOT NULL, FirstName TEXT, LastName TEXT, Age INTEGER, CountryOfOrigin TEXT, Photo BLOB)",
            "CREATE TABLE IF NOT EXISTS FriendRequests (Id INTEGER PRIMARY KEY AUTOINCREMENT, FromUserId INTEGER NOT NULL, ToUserId INTEGER NOT NULL, Status INTEGER)"
        };

        foreach (var cmd in commands)
        {
            connection.Execute(cmd);
        }
    }

    public SqliteConnection CreateConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }
}