using Blog.DAL.Context;
using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Repos.PostTag;

public class PostTagRepo : GenericRepo<PostsTags>, IPostTagRepo
{
    private readonly BlogDbContext _context;
    public PostTagRepo(BlogDbContext context) : base(context)
    {
        _context = context;
    }
}
