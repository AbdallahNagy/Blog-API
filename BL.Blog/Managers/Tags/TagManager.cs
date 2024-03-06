using Blog.BL.DTOs.Tags;
using Blog.BL.Exception_Handling;
using Blog.DAL.Repos.Tags;
using Mapster;

namespace Blog.BL.Managers.Tags;

public class TagManager(ITagRepo tagRepo) : ITagManager
{
    private readonly ITagRepo _tagRepo = tagRepo;

    public async Task<IEnumerable<ReadTagDTO>?> GetAll()
    {
        var tags = await _tagRepo.GetAll()
            ?? throw new BusinessException(404, "Tags not found");

        return tags.Adapt<IEnumerable<ReadTagDTO>>();
    }
}
