using Blog.BL.DTOs.Comments;
using Blog.BL.Exception_Handling;
using Blog.DAL.Models;
using Blog.DAL.Repos.Comments;
using Mapster;

namespace Blog.BL.Managers.Comments;
public class CommentManager(ICommentRepo commentRepo) : ICommentManager
{
    private readonly ICommentRepo _commentRepo = commentRepo;

    async public Task<ReadCommentDTO> Add(WriteCommentDTO commentToAdd, int postId)
    {
        var comment = new Comment
        {
            Body = commentToAdd.Body,
            UserId = commentToAdd.UserId,
            PostId = postId
        };

        await _commentRepo.Add(comment);
        await _commentRepo.SaveChanges();

        await Console.Out.WriteLineAsync(comment.Id.ToString());

        return comment.Adapt<ReadCommentDTO>();
    }

    public async Task Delete(int id, int postId)
    {
        var result = await _commentRepo.Delete(id, postId);
        if (result == 0) throw new BusinessException(204, "Record doesn't exist");

        await _commentRepo.SaveChanges();
    }

    async public Task<IEnumerable<ReadCommentDTO>?> GetAll(int postId)
    {
        // get all comments by post id
        var comments = await _commentRepo.GetAllCommentsByPostId(postId)
            ?? throw new BusinessException(404, "No comments in this post");

        return comments.Adapt<IEnumerable<ReadCommentDTO>>();
    }

    public async Task<ReadCommentDTO> Update(UpdateCommentDTO commentToUpdate, int id, int postId)
    {
        var comment = new Comment
        {
            Body = commentToUpdate.Body,
        };

        var updatedComment = await _commentRepo.Update(id, postId, comment)
            ?? throw new BusinessException(404, "Record doesn't exist");

        await _commentRepo.SaveChanges();
        return updatedComment.Adapt<ReadCommentDTO>();
    }
}
