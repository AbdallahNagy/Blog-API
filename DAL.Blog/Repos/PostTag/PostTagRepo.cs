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

    public async Task<PostsTags?> GetByCompositeKey(int id1, int id2)
    {
        
        return await _context.Set<PostsTags>().FindAsync(id1, id2);
    }

    public async Task<int> DeleteByCompositeKey(int id1, int id2)
    {
        var dbPostTag = await GetByCompositeKey(id1, id2);
        if (dbPostTag == null) return 0;
        _context.Set<PostsTags>().Remove(dbPostTag);
        return 1;
    }
}
