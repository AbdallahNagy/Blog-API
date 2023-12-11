﻿using Blog.DAL.Models;
using Blog.DAL.Repos.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Repos.Comments;
public interface ICommentRepo : IGenericRepo<Comment>
{
    Task<IEnumerable<Comment>?> GetAllCommentsByPostId(int postId);
}
