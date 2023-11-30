using Blog.DAL.Context;
using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;

namespace Blog.DAL.Repos.Users;

public class UserRepo : GenericRepo<User>, IUserRepo
{
    public UserRepo(BlogDbContext context) : base(context)
    {
    }
}
