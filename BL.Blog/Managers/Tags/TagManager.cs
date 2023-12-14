using Blog.BL.DTOs.Tags;
using Blog.BL.Exception_Handling;
using Blog.DAL.Repos.Tags;

namespace Blog.BL.Managers.Tags;

public class TagManager : ITagManager
{
    private readonly ITagRepo _tagRepo;
    public TagManager(ITagRepo tagRepo)
    {
        _tagRepo = tagRepo;
    }
    public async Task<IEnumerable<ReadTagDTO>?> GetAll()
    {
        try
        {
            var tags = await _tagRepo.GetAll()
                ?? throw new BusinessException(404, "Tags not found");

            return tags.Select(t => new ReadTagDTO(t.Id, t.Name, t.CreatedAt));
        }
        catch (BusinessException ex)
        {
            await Console.Out.WriteLineAsync($"business exception error with status: {ex.StatusCode}. and Message: {ex.Message}.");
            throw new BusinessException(ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync($"500 Internal Server Error. Message: {ex.Message}.");
            throw new BusinessException(500, "Internal Server Error");
        }
    }
}
