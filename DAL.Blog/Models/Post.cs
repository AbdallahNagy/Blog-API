using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.DAL.Models;

public class Post
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public int Likes { get; set; }
    [ForeignKey(nameof(User))]
    public string? AuthorId { get; set; }
    public User? User { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime CreatedAt { get; set; }
    public IEnumerable<PostsTags> PostsTags { get; set; } = new HashSet<PostsTags>();
}
