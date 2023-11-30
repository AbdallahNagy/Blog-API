using Blog.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repos.Generic;

public class GenericRepo<T> : IGenericRepo<T> where T : class
{
    private readonly BlogDbContext _context;
    public GenericRepo(BlogDbContext context) 
    {
        _context = context;
    }
    public List<T> GetAll()
    {
        return _context.Set<T>().AsNoTracking().ToList();
    }
    public T? Get(int id)
    {
        return _context.Set<T>().Find(id);
    }
    public T Add(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }
    public int Delete(int id)
    {
        var entity = _context.Set<T>().Find(id);
        if (entity == null) return 0;
        _context.Set<T>().Remove(entity);
        return 1;
    }
    public T? Update(int id, T entity)
    {
        var existingEntity = _context.Set<T>().Find(id);

        if (existingEntity == null) return null;

        _context.Entry(existingEntity).State = EntityState.Detached;
        _context.Set<T>().Update(entity);
        return entity;
    }
    public int SaveChanges()
    {
        return _context.SaveChanges();
    }
}
