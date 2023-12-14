using Blog.BL.DTOs.Tags;

namespace Blog.BL.DTOs.Posts;

public record UpdatePostDTO(string? Title, string? Body, IEnumerable<WriteTagDTO> Tags);