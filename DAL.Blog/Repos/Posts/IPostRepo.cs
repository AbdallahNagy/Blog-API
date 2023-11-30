using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;

namespace Blog.DAL.Repos.Posts;

public interface IPostRepo : IGenericRepo<Post>
{
    List<Post> SearchByTags(int[] tagsIds);
    List<Post> SearchByText(string str);
    List<Post> SearchInTitle(string str);
    List<Post> SearchInBody(string str);
}

