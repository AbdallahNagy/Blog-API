using Blog.BL.DTOs.Likes;
using Blog.BL.DTOs.Posts;

namespace Blog.BL.Managers.Likes;

public interface ILikeManager
{
    Task<ReadPostDTO?> LikePost(int id, WriteLikeDTO writeLike);
}
