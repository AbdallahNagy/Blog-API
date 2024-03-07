using Blog.BL.DTOs.Posts;
using Blog.BL.Exception_Handling;
using Blog.DAL.Models;
using Blog.DAL.Repos.Posts;
using Blog.DAL.Repos.PostTag;
using Blog.DAL.Repos.Tags;
using MapsterMapper;

namespace Blog.BL.Managers.Posts;

public class PostManager : IPostManager
{
    private readonly IPostRepo _postRepo;
    private readonly ITagRepo _tagRepo;
    private readonly IPostTagRepo _postTagRepo;
    private readonly IMapper _mapper;

    public PostManager(IPostRepo postRepo, ITagRepo tagRepo, IPostTagRepo postsTags, IMapper mapper)
    {
        _postRepo = postRepo;
        _tagRepo = tagRepo;
        _postTagRepo = postsTags;
        _mapper = mapper;
    }

    async public Task<ReadPostDTO?> GetById(int id)
    {
        var post = await _postRepo.Get(id) ?? throw new BusinessException(404, "Can't find post by this id");

        return _mapper.Map<ReadPostDTO>(post);
    }

    async public Task<ReadPostDTO> Add(WritePostDTO writePost)
    {
        var post = new Post
        {
            Title = writePost.Title,
            Body = writePost.Body,
            AuthorId = writePost.AuthorId,
            TotalLikes = 0,
        };

        await _postRepo.Add(post);
        await _postRepo.SaveChanges();

        var tags = _mapper.Map<IEnumerable<Tag>>(writePost.Tags).ToList();
        await _tagRepo.AddRange(tags);
        await _postRepo.SaveChanges();

        var postsTags = tags.Select(t => new PostsTags
        {
            PostId = post.Id,
            TagId = t.Id
        });

        await _postTagRepo.AddRange(postsTags);
        await _postTagRepo.SaveChanges();

        return _mapper.Map<ReadPostDTO>(post);
    }

    async public Task Delete(int id)
    {
        // 1 for success, 0 for record doesn't exist
        var result = await _postRepo.Delete(id);
        if (result == 0) throw new BusinessException(204, "Record doesn't exist");
        await _postRepo.SaveChanges();
    }

    async public Task<ReadPostDTO> Update(UpdatePostDTO updatePost, int id)
    {
        var post = new Post
        {
            Id = id,
            Title = updatePost.Title,
            Body = updatePost.Body,
        };

        var dbTags = await _tagRepo.GetTagsByPostId(id);
        // check if dbTags is null
        var dbTagsNames = dbTags!.Select(t => t!.Name).ToHashSet();
        var updatedTagsNames = updatePost.Tags.Select(t => t.Name).ToHashSet();

        foreach (var tagName in updatedTagsNames)
        {
            if (!dbTagsNames.Contains(tagName!))
            {
                var tag = await _tagRepo.GetByName(tagName!);

                if (tag == null)
                {
                    tag = new Tag { Name = tagName! };
                    await _tagRepo.Add(tag);
                    await _tagRepo.SaveChanges();
                }

                var postTag = new PostsTags { PostId = id, TagId = tag.Id };
                await _postTagRepo.Add(postTag);
                await _postTagRepo.SaveChanges();
            }
        }

        foreach (var dbTagName in dbTagsNames)
        {
            if (dbTagName == null) break;
            if (!updatedTagsNames.Contains(dbTagName))
            {
                var tag = dbTags!.FirstOrDefault(t => t!.Name == dbTagName);
                await _postTagRepo.DeleteByCompositeKey(id, tag!.Id);
                await _postTagRepo.SaveChanges();
            }
        }

        var updatedPost = await _postRepo.Update(id, post) ?? throw new BusinessException(404, "Can't find record by the provided id");
        await _postRepo.SaveChanges();
        return _mapper.Map<ReadPostDTO>(updatedPost);
    }

    public async Task<List<ReadPostDTO>?> Filter(string title, string body, int tagId, int limit, int offset)
    {
        var posts = await _postRepo.Filter(title, body, tagId, limit, offset);

        if (posts == null || posts.Count == 0) throw new BusinessException(404, "No posts available by this filter");

        return _mapper.Map<List<ReadPostDTO>>(posts);
    }
}
