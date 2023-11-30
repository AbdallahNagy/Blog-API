
namespace Blog.DAL.Repos.Generic;

public interface IGenericRepo<T> where T : class
{
    List<T> GetAll();
    T? Get(int id);
    T Add(T entity);
    T? Update(int id, T entity);
    int Delete(int id);
    int SaveChanges();
}
