
namespace Blog.DAL.Repos.Generic;

public interface IGenericRepo<T> where T : class
{
    Task<List<T>?> GetAll();
    Task<T?> Get(int id);
    Task<T> Add(T entity);
    Task AddRange(IEnumerable<T> entities);
    Task<T?> Update(int id, T entity);
    Task<int> Delete(int id);
    Task<int> SaveChanges();
}
