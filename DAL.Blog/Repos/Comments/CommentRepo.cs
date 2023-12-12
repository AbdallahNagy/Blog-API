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

    public async Task<Comment?> Update(int id, int postId, Comment entity)
    {
        // we should replace post id with user id to tune the performance
        var existingEntity = await _context.Set<Comment>().Where(c => c.PostId == postId).FirstOrDefaultAsync(c => c.Id == id);

        if (existingEntity == null) return null;

        existingEntity.Body = entity.Body;

        return existingEntity;
    }
    async public Task<int> Delete(int id, int postId)
    {
        // we should replace post id with user id to tune the performance
        var entity = await _context.Set<Comment>().Where(c => c.PostId == postId).FirstOrDefaultAsync(c => c.Id == id);
        if (entity == null) return 0;
        _context.Set<Comment>().Remove(entity);
        return 1;
    }
}
