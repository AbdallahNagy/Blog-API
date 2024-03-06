using Blog.BL.DTOs.Likes;
using Blog.BL.DTOs.Posts;
using Blog.BL.Managers.Likes;
using Blog.BL.Managers.Posts;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[Route("api/v1/posts")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostManager _postManager;
    private readonly ILikeManager _likeManager;
    public PostController(IPostManager postManager, ILikeManager likeManager)
    {
        _postManager = postManager;
        _likeManager = likeManager;

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
        var posts = await _postManager.Filter(title, body, tagId, limit, offset);
        return posts;
    }

    [HttpGet]
    [Route("{id}")]
    async public Task<ActionResult<ReadPostDTO?>> Get(int id)
    {
        var post = await _postManager.GetById(id);
        return post;
    }

    [HttpPost]
    async public Task<ActionResult<ReadPostDTO>> Post([FromBody] WritePostDTO post)
    {
        var addedPost = await _postManager.Add(post);
        return addedPost;
    }

    [HttpPatch]
    [Route("{id}")]
    async public Task<ActionResult<ReadPostDTO>> Patch([FromBody] UpdatePostDTO post, int id)
    {
        var updatedPost = await _postManager.Update(post, id);
        return updatedPost;
    }

    [HttpDelete]
    [Route("{id}")]
    async public Task<ActionResult> Delete(int id)
    {
        await _postManager.Delete(id);
        return Ok();
    }

    [HttpPost]
    [Route("{id}/like")]
    async public Task<ActionResult<ReadPostDTO>> Like([FromBody] WriteLikeDTO writeLike, int id)
    {
        var post = await _likeManager.LikePost(id, writeLike);
        return Ok(post);
    }

    // wait until we use tokens
    //[HttpDelete]
    //[Route("{id}/like")]
    //async public Task<ActionResult<ReadPostDTO>> Unlike()
    //{

    //}
}
