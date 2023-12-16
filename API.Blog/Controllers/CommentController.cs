using Blog.BL.DTOs.Comments;
using Blog.BL.Managers.Comments;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[Route("api/v1/posts/{postId}/comments")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentManager _commentManager;
    public CommentController(ICommentManager commentManager)
    {
        _commentManager = commentManager;
    }

    [HttpGet]
    async public Task<ActionResult<IEnumerable<ReadCommentDTO>>> GetAll(int postId)
    {
        var comments = await _commentManager.GetAll(postId);
        return Ok(comments);
    }

    [HttpPost]
    public async Task<ActionResult<ReadCommentDTO>> Post([FromBody] WriteCommentDTO comment, int postId)
    {
        var addedComment = await _commentManager.Add(comment, postId);
        return Ok(addedComment);
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<ActionResult<ReadCommentDTO>> Patch([FromBody] UpdateCommentDTO comment, int id, int postId)
    {
        var updatedComment = await _commentManager.Update(comment, id, postId);
        return Ok(updatedComment);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> Delete(int id, int postId)
    {
        await _commentManager.Delete(id, postId);
        return Ok();
    }
}
