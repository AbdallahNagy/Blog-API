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

    public List<Post?> SearchByTags(int[] tagsIds)
    {
        return _context.Set<PostsTags>()
            .Where(postTag => tagsIds.Contains(postTag.TagId))
            .Select(postTag => postTag.Post)
            .ToList();
    }

    public List<Post> SearchByText(string str)
    {
        return _context.Set<Post>()
            .Where(post => post.Title!.Contains(str) || post.Body!.Contains(str))
            .AsNoTracking()
            .ToList();
    }

    public List<Post> SearchInTitle(string str)
    {
        return _context.Set<Post>()
            .Where(post => post.Title!.Contains(str))
            .AsNoTracking()
            .ToList();
    }

    public List<Post> SearchInBody(string str)
    {
        return _context.Set<Post>()
            .Where(post => post.Body!.Contains(str))
            .AsNoTracking()
            .ToList();
    }
}
