using Blog.BL.DTOs.PostTags;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/v1/poststags")]
    [ApiController]
    public class PostsTagsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<ReadPostTagsDTO>> Get(
            [FromQuery] string postId, 
            [FromQuery] string tagId) 
        {
            
            return Ok(postId);
        }

        // on create/update of post
        [HttpPost]
        public ActionResult<ReadPostTagsDTO> Post()
        {
            return Ok();
        }

        [HttpDelete]
        public ActionResult<ReadPostTagsDTO> Delete()
        {
            return Ok();
        }
    }
}

