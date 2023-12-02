using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;

namespace Blog.DAL.Repos.Tags;

public interface ITagRepo : IGenericRepo<Tag>
{
    Task<Tag?> GetByName(string name);
    Task<List<Tag?>> GetTagsByPostId(int postId);
}
