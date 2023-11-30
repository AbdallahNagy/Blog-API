using Blog.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly List<Post> Posts = new()
        {
            new Post { Id = 1, Title = "First Post", Body = "This is the body of the first post.", Likes = 10, AuthorId = "user1" },
            new Post { Id = 2, Title = "Second Post", Body = "This is the body of the second post.", Likes = 5, AuthorId = "user2" },
            // Add more posts as needed
        };

        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetAll()
        {
            return Ok(Posts);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Post> Get(int id)
        {
            return Ok(Posts[id]);
        }

        [HttpPost]
        public ActionResult<Post> Post(Post post)
        {
            return Ok(post);
        }

        [HttpPatch]
        [Route("{id}")]
        public ActionResult<Post> Patch(Post post)
        {
            return Patch(post);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
