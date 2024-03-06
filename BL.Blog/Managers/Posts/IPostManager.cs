using Blog.BL.DTOs.Posts;

namespace Blog.BL.Managers.Posts;
public interface IPostManager
{
    Task<ReadPostDTO?> GetById(int id);
    Task<ReadPostDTO> Add(WritePostDTO post);
    Task<ReadPostDTO> Update(UpdatePostDTO post, int id);
    Task Delete(int id);
    Task<List<ReadPostDTO>?> Filter(string title, string body, int tagId, int limit, int offset);
}
