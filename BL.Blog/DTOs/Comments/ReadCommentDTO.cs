namespace Blog.BL.DTOs.Comments;

public record ReadCommentDTO(int Id, string? Body, DateTime CreatedAt, string UserId);
