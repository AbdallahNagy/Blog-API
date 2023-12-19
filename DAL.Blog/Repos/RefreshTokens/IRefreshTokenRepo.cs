using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Repos.RefreshTokens;

public interface IRefreshTokenRepo : IGenericRepo<RefreshToken>
{

}
