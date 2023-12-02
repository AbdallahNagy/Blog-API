using Blog.DAL.Context;
using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repos.Tags;

public class TagRepo : GenericRepo<Tag>, ITagRepo
{
    private readonly BlogDbContext _context;
    public TagRepo(BlogDbContext context) : base(context)
    {
        _context = context;
    }

    async public Task<List<Tag?>> GetTagsByPostId(int postId)
    {
        var postTags = await _context.Set<PostsTags>()
            .Where(pt => pt.PostId == postId)
            .Select(pt => pt.Tag)
            .ToListAsync();

        return postTags;
    }
}
