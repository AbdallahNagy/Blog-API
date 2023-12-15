using Blog.DAL.Context;
using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Repos.Likes;

public class LikeRepo : GenericRepo<Like>, ILikeRepo
{
    public LikeRepo(BlogDbContext context) : base(context)
    {
    }
}
