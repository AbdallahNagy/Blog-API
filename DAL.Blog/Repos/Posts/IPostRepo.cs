using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;

namespace Blog.DAL.Repos.Posts;

public interface IPostRepo : IGenericRepo<Post>
{
    Task<List<Post>?> Filter(string title, string body, int tagId, int limit, int offset);
    Task<Post?> AddLikeToPost(int id);
}

