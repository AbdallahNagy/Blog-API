using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;

namespace Blog.DAL.Repos.Comments;
public interface ICommentRepo : IGenericRepo<Comment>
{
    Task<IEnumerable<Comment>?> GetAllCommentsByPostId(int postId);
    Task<Comment?> Update(int id, int postId, Comment entity);
    Task<int> Delete(int id, int postId);
}
