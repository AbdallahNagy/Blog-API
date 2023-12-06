using Blog.DAL.Context;
using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;
using Microsoft.AspNetCore.JsonPatch;
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

    public async Task<Post?> PatchUpdate(int id, JsonPatchDocument<Post> entity)
    {
        var existingEntity = await _context.Set<Post>()
            .FirstOrDefaultAsync(p => p.Id == id);

        if(existingEntity == null) return null;

        entity.ApplyTo(existingEntity);
        return existingEntity;
    }

    async public Task<List<Post?>?> SearchByTags(int[] tagsIds)
    {
        return await _context.Set<PostsTags>()
            .Where(postTag => tagsIds.Contains(postTag.TagId))
            .Select(postTag => postTag.Post)
            .ToListAsync();
    }

    async public Task<List<Post>?> SearchByText(string str)
    {
        return await _context.Set<Post>()
            .Where(post => post.Title!.Contains(str) || post.Body!.Contains(str))
            .AsNoTracking()
            .ToListAsync();
    }

    async public Task<List<Post>?> SearchInTitle(string str)
    {
        return await _context.Set<Post>()
            .Where(post => post.Title!.Contains(str))
            .AsNoTracking()
            .ToListAsync();
    }

    async public Task<List<Post>?> SearchInBody(string str)
    {
        return await _context.Set<Post>()
            .Where(post => post.Body!.Contains(str))
            .AsNoTracking()
            .ToListAsync();
    }
}
