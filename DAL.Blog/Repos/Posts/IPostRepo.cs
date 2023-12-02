using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;

namespace Blog.DAL.Repos.Posts;

public interface IPostRepo : IGenericRepo<Post>
{
    Task<List<Post?>?> SearchByTags(int[] tagsIds);
    Task<List<Post>?> SearchByText(string str);
    Task<List<Post>?> SearchInTitle(string str);
    Task<List<Post>?>  SearchInBody(string str);
}

