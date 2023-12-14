using Blog.BL.DTOs.Tags;

namespace Blog.BL.DTOs.Posts
{
    public record ReadPostDTO(int Id, string? Title, string? Body, int TotalTotalLikes, string? AuthorId, DateTime Created_At, IEnumerable<ReadTagDTO> Tags);
}
