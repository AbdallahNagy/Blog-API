using Blog.BL.DTOs.Comments;
using Blog.BL.Exception_Handling;
using Blog.BL.Managers.Comments;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
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
            try
            {
                var comments = await _commentManager.GetAll(postId);
                return Ok(comments);
            }
            catch (BusinessException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ReadCommentDTO>> Post([FromBody] WriteCommentDTO comment, int postId)
        {
            try
            {
                var addedComment = await _commentManager.Add(comment, postId);
                return Ok(addedComment);
            }
            catch (BusinessException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<ActionResult<ReadCommentDTO>> Patch([FromBody] UpdateCommentDTO comment, int id, int postId)
        {
            try
            {
                var updatedComment = await _commentManager.Update(comment, id, postId);
                return Ok(updatedComment);
            }
            catch (BusinessException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(int id, int postId)
        {
            try
            {
                await _commentManager.Delete(id, postId);
                return Ok();
            }
            catch (BusinessException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }
    }
}
