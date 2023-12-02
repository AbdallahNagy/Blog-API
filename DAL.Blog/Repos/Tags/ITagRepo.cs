using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;

namespace Blog.DAL.Repos.Tags;

public interface ITagRepo : IGenericRepo<Tag>
{
    Task<List<Tag?>> GetTagsByPostId(int postId);
}
