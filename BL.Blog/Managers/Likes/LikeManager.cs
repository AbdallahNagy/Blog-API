using Blog.BL.DTOs.Likes;
using Blog.BL.DTOs.Posts;
using Blog.BL.DTOs.Tags;
using Blog.BL.Exception_Handling;
using Blog.DAL.Models;
using Blog.DAL.Repos.Likes;
using Blog.DAL.Repos.Posts;

namespace Blog.BL.Managers.Likes;

public class LikeManager : ILikeManager
{
    private readonly ILikeRepo _likeRepo;
    private readonly IPostRepo _postRepo;
    public LikeManager(ILikeRepo likeRepo, IPostRepo postRepo)
    {
        _likeRepo = likeRepo;
        _postRepo = postRepo;
    }
    public async Task<ReadPostDTO?> LikePost(int id, WriteLikeDTO writeLike)
    {
        var like = new Like
        {
            PostId = id,
            UserId = writeLike.UserId,
        };

        var createdLike = await _likeRepo.Add(like);
        await _likeRepo.SaveChanges();

        var post = await _postRepo.AddLikeToPost(id)
            ?? throw new BusinessException(404, "Record doesn't exist");

        await _postRepo.SaveChanges();

        return new ReadPostDTO(
            post.Id,
            post.Title,
            post.Body,
            post.TotalLikes,
            post.AuthorId,
            post.CreatedAt,
            post.PostsTags
                .Select(pt => pt.Tag)
                .Select(t => new ReadTagDTO(
                    t?.Id ?? 0,
                    t?.Name ?? "",
                    t?.CreatedAt ?? DateTime.MinValue)));
    }
}
