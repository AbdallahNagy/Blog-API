using Blog.BL.DTOs.Tags;
using Blog.BL.Exception_Handling;
using Blog.BL.Managers.Tags;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[Route("api/v1/tags")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly ITagManager _tagManager;
    public TagController(ITagManager tagManager)
    {
        _tagManager = tagManager;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadTagDTO>>> GetAll()
    {
        var tags = await _tagManager.GetAll();
        return tags!.ToList();
    }
}
