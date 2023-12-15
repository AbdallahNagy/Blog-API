using Blog.DAL.Context;
using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repos.Posts;

public class PostRepo : GenericRepo<Post>, IPostRepo
{
    private readonly BlogDbContext _context;
    public PostRepo(BlogDbContext context) : base(context)
    {
        _context = context;
    }

    override async public Task<IEnumerable<Post>?> GetAll()
    {
        return await _context.Set<Post>()
            .Include(p => p.PostsTags)
            .ThenInclude(pt => pt.Tag)
            .AsNoTracking()
            .ToListAsync();
    }

    override async public Task<Post?> Get(int id)
    {
        return await _context.Set<Post>()
            .Include(p => p.PostsTags)
            .ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    override async public Task<Post?> Update(int id, Post entity)
    {
        var existingEntity = await _context.Set<Post>()
            .Include(p => p.PostsTags)
            .ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (existingEntity == null) return null;

        existingEntity.Title = entity.Title;
        existingEntity.Body = entity.Body;

        return existingEntity;
    }

    async public Task<List<Post>?> Filter(string title, string body, int tagId, int limit, int offset)
    {
        var posts = _context.Set<Post>()
            .OrderByDescending(p => p.CreatedAt);

        if (!string.IsNullOrEmpty(title))
        {
            posts = (IOrderedQueryable<Post>)posts.Where(p => p.Title!.Contains(title));
        }

        if (!string.IsNullOrEmpty(body))
        {
            posts = (IOrderedQueryable<Post>)posts.Where(p => p.Body!.Contains(body));
        }

        if (tagId != -1)
        {
            posts = (IOrderedQueryable<Post>)posts
                .Include(p => p.PostsTags)
                    .ThenInclude(pt => pt.Tag)
                .Where(p => p.PostsTags.Any(pt => pt.TagId == tagId));
        }

        if (limit > 0 && offset >= 0)
        {
            posts = (IOrderedQueryable<Post>)posts.Skip(offset).Take(limit);
        }

        return await posts.ToListAsync();
    }

    public async Task<Post?> AddLikeToPost(int id)
    {
        var post = await Get(id);
        if (post == null) return null;

        post.TotalLikes++;

        return post;
    }
}
