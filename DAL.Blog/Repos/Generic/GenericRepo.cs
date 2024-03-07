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
    virtual async public Task<IEnumerable<T>?> GetAll()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }
    virtual async public Task<T?> Get(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
    virtual async public Task Add(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }
    virtual async public Task AddRange(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
    }
    virtual async public Task<int> Delete(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity == null) return 0;
        _context.Set<T>().Remove(entity);
        return 1;
    }
    virtual async public Task<T?> Update(int id, T entity)
    {
        var existingEntity = await _context.Set<T>().FindAsync(id);

        if (existingEntity == null) return null;

        _context.Entry(existingEntity).State = EntityState.Detached;
        _context.Set<T>().Update(entity);
        return entity;
    }
    async public Task<int> SaveChanges()
    {
        return await _context.SaveChangesAsync();
    }
}
