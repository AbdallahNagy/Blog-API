using Blog.DAL.Context;
using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Repos.Comments;

public class CommentRepo : GenericRepo<Comment>, ICommentRepo
{
    private readonly BlogDbContext _context;
    public CommentRepo(BlogDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comment>?> GetAllCommentsByPostId(int postId)
    {
        return await _context.Set<Comment>().Where(c => c.PostId == postId).ToListAsync();
    }
}
