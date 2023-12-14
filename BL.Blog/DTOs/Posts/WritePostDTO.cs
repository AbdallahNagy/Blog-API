using Blog.BL.DTOs.Tags;

namespace Blog.BL.DTOs.Posts;

public record WritePostDTO(string? Title, string? Body, IEnumerable<WriteTagDTO> Tags, string? AuthorId);
