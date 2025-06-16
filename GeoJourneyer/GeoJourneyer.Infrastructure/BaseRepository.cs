using Dapper;
using Microsoft.Data.Sqlite;
using GeoJourneyer.Domain.Queries;

namespace GeoJourneyer.Infrastructure;

public abstract class BaseRepository<T>
{
    protected readonly DatabaseContext Context;
    protected abstract string TableName { get; }

    protected BaseRepository(DatabaseContext context)
    {
        Context = context;
    }

    public virtual IEnumerable<T> GetAll(BaseQuery? query = null)
    {
        using var connection = Context.CreateConnection();
        var sql = $"SELECT * FROM {TableName}";
        if (query != null)
        {
            var properties = query.GetType()
                .GetProperties()
                .Where(p => p.GetValue(query) != null)
                .ToArray();
            if (properties.Any())
            {
                var conditions = string.Join(
                    " AND ",
                    properties.Select(p => $"{p.Name} = @{p.Name}")
                );
                sql += $" WHERE {conditions}";
            }
        }
        return connection.Query<T>(sql, query);
    }

    public virtual T? GetById(int id)
    {
        using var connection = Context.CreateConnection();
        return connection.QuerySingleOrDefault<T>($"SELECT * FROM {TableName} WHERE Id = @id", new { id });
    }

    public virtual int Insert(T entity)
    {
        using var connection = Context.CreateConnection();
        var parameters = (object)entity!;
        var columns = string.Join(",", typeof(T).GetProperties().Where(p => p.Name != "Id").Select(p => p.Name));
        var values = string.Join(",", typeof(T).GetProperties().Where(p => p.Name != "Id").Select(p => "@" + p.Name));
        var sql = $"INSERT INTO {TableName} ({columns}) VALUES ({values}); SELECT last_insert_rowid();";
        var id = connection.ExecuteScalar<long>(sql, parameters);
        var prop = typeof(T).GetProperty("Id");
        if (prop != null && prop.CanWrite)
        {
            prop.SetValue(entity, (int)id);
        }
        return (int)id;
    }

    public virtual void Update(T entity)
    {
        using var connection = Context.CreateConnection();
        var properties = typeof(T).GetProperties().Where(p => p.Name != "Id");
        var setters = string.Join(",", properties.Select(p => $"{p.Name} = @{p.Name}"));
        var sql = $"UPDATE {TableName} SET {setters} WHERE Id = @Id";
        connection.Execute(sql, entity);
    }

    public virtual void Delete(int id)
    {
        using var connection = Context.CreateConnection();
        connection.Execute($"DELETE FROM {TableName} WHERE Id = @id", new { id });
    }
}