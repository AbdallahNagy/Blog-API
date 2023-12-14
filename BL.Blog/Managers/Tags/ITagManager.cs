using Blog.BL.DTOs.Tags;

namespace Blog.BL.Managers.Tags;

public interface ITagManager
{
    Task<IEnumerable<ReadTagDTO>?> GetAll();
}
