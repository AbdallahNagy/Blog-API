using Blog.BL.DTOs.Likes;
using Blog.BL.DTOs.Posts;
using Blog.BL.Exception_Handling;
using Blog.BL.Managers.Posts;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[Route("api/v1/posts")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostManager _postManager;
    public PostController(IPostManager postManager)
    {
        _postManager = postManager;
    }

    [HttpGet]
    async public Task<ActionResult<IEnumerable<ReadPostDTO>?>> GetAll(
        [FromQuery] string title = "",
        [FromQuery] string body = "",
        [FromQuery] int limit = -1,
        [FromQuery] int offset = -1,
        [FromQuery] int tagId = -1
        )
    {
        try
        {
            var posts = await _postManager.Filter(title, body, tagId, limit, offset);
            return posts;
        }
        catch (BusinessException ex)
        {
            return StatusCode(ex.StatusCode, ex.Message);
        }
    }

    [HttpGet]
    [Route("{id}")]
    async public Task<ActionResult<ReadPostDTO?>> Get(int id)
    {
        try
        {
            var post = await _postManager.GetById(id);
            return post;
        }
        catch (BusinessException ex)
        {
            return StatusCode(ex.StatusCode, ex.Message);
        }
    }

    [HttpPost]
    async public Task<ActionResult<ReadPostDTO>> Post([FromBody] WritePostDTO post)
    {
        try
        {
            var addedPost = await _postManager.Add(post);
            return addedPost;
        }
        catch (BusinessException ex)
        {
            return StatusCode(ex.StatusCode, ex.Message);
        }
    }

    [HttpPatch]
    [Route("{id}")]
    async public Task<ActionResult<ReadPostDTO>> Patch([FromBody] UpdatePostDTO post, int id)
    {
        try
        {
            var updatedPost = await _postManager.Update(post, id);
            return updatedPost;
        }
        catch (BusinessException ex)
        {
            return StatusCode(ex.StatusCode, ex.Message);
        }
    }

    [HttpDelete]
    [Route("{id}")]
    async public Task<ActionResult> Delete(int id)
    {
        try
        {
            await _postManager.Delete(id);
            return Ok();
        }
        catch (BusinessException ex)
        {
            return StatusCode(ex.StatusCode, ex.Message);
        }
    }

    [HttpPost]
    [Route("{id}/like")]
    async public Task<ActionResult<ReadPostDTO>> Like([FromBody] WriteLikeDTO writeLike, int id)
    {
        try
        {
            var post = await _postManager.LikePost(id, writeLike);
            return Ok(post);
        }
        catch (BusinessException ex)
        {
            return StatusCode(ex.StatusCode, ex.Message);
        }
    }
}
