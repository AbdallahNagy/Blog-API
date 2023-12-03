using Blog.DAL.Context;
using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Repos.PostTag;

public class PostTagRepo : GenericRepo<PostsTags>, IPostTagRepo
{
    private readonly BlogDbContext _context;
    public PostTagRepo(BlogDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PostsTags?> GetByCompositeKey(PostsTags postTag)
    {
        
        return await _context.Set<PostsTags>().FindAsync(postTag);
    }

    public async Task<int> DeleteByCompositeKey(PostsTags postTag)
    {
        var dbPostTag = await GetByCompositeKey(postTag);
        if (dbPostTag == null) return 0;
        _context.Set<PostsTags>().Remove(postTag);
        return 1;
    }
}
