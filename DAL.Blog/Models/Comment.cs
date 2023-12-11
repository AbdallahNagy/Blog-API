using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.DAL.Models;

public class Comment
{
    public int Id { get; set; }
    public string? Body { get; set; }
    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; } 
    public int  PostId { get; set; }
    public Post? Post { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime CreatedAt { get; set; }
}
