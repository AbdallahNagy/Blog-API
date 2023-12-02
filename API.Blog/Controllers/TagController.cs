﻿using Blog.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/v1/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly List<Post> Posts = new()
        {
            new Post { Id = 1, Title = "First Post", Body = "This is the body of the first post.", TotalLikes = 10, AuthorId = "user1" },
            new Post { Id = 2, Title = "Second Post", Body = "This is the body of the second post.", TotalLikes = 5, AuthorId = "user2" },
        };

        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetAll()
        {
            return Ok(Posts);
        }

        [HttpPost]
        public ActionResult<Post> Post(Post post)
        {
            return Ok(post);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
