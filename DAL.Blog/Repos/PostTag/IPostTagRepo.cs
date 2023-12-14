using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;

namespace Blog.DAL.Repos.PostTag;

public interface IPostTagRepo : IGenericRepo<PostsTags>
{
    Task<PostsTags?> GetByCompositeKey(int id1, int id2);
    Task<int> DeleteByCompositeKey(int id1, int id2);
}
