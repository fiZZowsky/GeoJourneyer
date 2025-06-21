using GeoJourneyer.Domain.Queries;
namespace GeoJourneyer.Application.Repositories;

public interface IBaseRepository<T>
{
    IEnumerable<T> GetAll(BaseQuery query = null);
    T? GetById(int id);
    int Insert(T entity);
    void Update(T entity);
    void Delete(int id);
}