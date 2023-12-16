using Blog.BL.DTOs.Comments;
using Blog.BL.Exception_Handling;
using Blog.DAL.Models;
using Blog.DAL.Repos.Comments;
using EntityFramework.Exceptions.Common;

namespace Blog.BL.Managers.Comments;
public class CommentManager : ICommentManager
{
    private readonly ICommentRepo _commentRepo;
    public CommentManager(ICommentRepo commentRepo)
    {
        _commentRepo = commentRepo;
    }
    async public Task<ReadCommentDTO> Add(WriteCommentDTO commentToAdd, int postId)
    {
        var comment = new Comment
        {
            Body = commentToAdd.Body,
            UserId = commentToAdd.UserId,
            PostId = postId
        };

        var createdComment = await _commentRepo.Add(comment);
        await _commentRepo.SaveChanges();

        var readComment = new ReadCommentDTO(createdComment.Id, createdComment.Body, createdComment.CreatedAt, createdComment.UserId);

        return readComment;
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

        return comments.Select(c => new ReadCommentDTO(c.Id, c.Body, c.CreatedAt, c.UserId));
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
        return new ReadCommentDTO(updatedComment.Id, updatedComment.Body, updatedComment.CreatedAt, updatedComment.UserId);
    }
}
