using Blog.DAL.Context;
using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;

namespace Blog.DAL.Repos.Tags;

public class TagRepo : GenericRepo<Tag>, ITagRepo
{
    public TagRepo(BlogDbContext context) : base(context)
    {
    }
}
