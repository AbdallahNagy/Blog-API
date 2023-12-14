using Blog.BL.DTOs.Comments;

namespace Blog.BL.Managers.Comments;

public interface ICommentManager
{
    Task<IEnumerable<ReadCommentDTO>?> GetAll(int id);
    Task<ReadCommentDTO> Update(UpdateCommentDTO comment, int id, int postId);
    Task Delete(int id, int postId);
    Task<ReadCommentDTO> Add(WriteCommentDTO comment, int postId);
}
