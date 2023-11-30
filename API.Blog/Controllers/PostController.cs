using Blog.BL.DTOs.Posts;
using Blog.BL.Exception_Handling;
using Blog.BL.Managers.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Reflection;

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
    public ActionResult<IEnumerable<ReadPostDTO>?> GetAll(
        [FromQuery] string title = "",
        [FromQuery] string body = "",
        [FromQuery] string text = ""
        )
    {
        if (title != "")
        {
            try
            {
                var posts = _postManager.SearchInTitle(title);
                return posts;
            }
            catch (BusinessException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        else if (body != "")
        {
            try
            {
                var posts = _postManager.SearchInBody(body);
                return posts;
            }
            catch (BusinessException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        else if (text != "")
        {
            try
            {
                // searches by text in title and body
                var posts = _postManager.SearchByText(text);
                return posts;
            }
            catch (BusinessException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        else
        {
            try
            {
                // searches by text in title and body
                var posts = _postManager.GetAll();
                return posts;
            }
            catch (BusinessException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<ReadPostDTO?> Get(int id)
    {
        try
        {
            var post = _postManager.GetById(id);
            return post;
        }
        catch (BusinessException ex)
        {
            return StatusCode(ex.StatusCode, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    public ActionResult<ReadPostDTO> Post(WritePostDTO post)
    {
        try
        {
            var addedPost = _postManager.Add(post);
            return addedPost;
        }
        catch (BusinessException ex)
        {
            return StatusCode(ex.StatusCode, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPatch]
    [Route("{id}")]
    public ActionResult<ReadPostDTO> Patch([FromBody] UpdatePostDTO post, int id)
    {
        try
        {
            var updatedPost = _postManager.Update(post, id);
            return updatedPost;
        }
        catch (BusinessException ex)
        {
            return StatusCode(ex.StatusCode, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            _postManager.Delete(id);
            return Ok();
        }
        catch (BusinessException ex)
        {
            return StatusCode(ex.StatusCode, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}
