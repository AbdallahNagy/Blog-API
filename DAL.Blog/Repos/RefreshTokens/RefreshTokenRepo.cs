using Blog.DAL.Context;
using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Repos.RefreshTokens;

public class RefreshTokenRepo : GenericRepo<RefreshToken>, IRefreshTokenRepo
{
    private readonly BlogDbContext _dbContext;
    public RefreshTokenRepo(BlogDbContext context) : base(context)
    {
        _dbContext = context;
    }

    async public Task<RefreshToken?> GetRefreshToken(string refreshToken)
    {
        return await _dbContext.Set<RefreshToken>().FirstOrDefaultAsync(t => t.Token == refreshToken);
    }
}
