using Blog.BL.DTOs.Likes;
using Blog.BL.DTOs.Posts;
using Blog.BL.Exception_Handling;
using Blog.DAL.Models;
using Blog.DAL.Repos.Likes;
using Blog.DAL.Repos.Posts;
using MapsterMapper;

namespace Blog.BL.Managers.Likes;

public class LikeManager : ILikeManager
{
    private readonly ILikeRepo _likeRepo;
    private readonly IPostRepo _postRepo;
    private readonly IMapper _mapper;
    public LikeManager(ILikeRepo likeRepo, IPostRepo postRepo, IMapper mapper)
    {
        _likeRepo = likeRepo;
        _postRepo = postRepo;
        _mapper = mapper;
    }
    public async Task<ReadPostDTO?> LikePost(int id, WriteLikeDTO writeLike)
    {
        var like = new Like
        {
            PostId = id,
            UserId = writeLike.UserId,
        };

        await _likeRepo.Add(like);
        await _likeRepo.SaveChanges();

        var post = await _postRepo.AddLikeToPost(id)
            ?? throw new BusinessException(404, "Record doesn't exist");

        await _postRepo.SaveChanges();

        return _mapper.Map<ReadPostDTO>(post);
    }
}
