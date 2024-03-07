using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.DAL.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "datetime2")]
    public DateTime CreatedAt { get; set; }
    public IEnumerable<PostsTags> PostTags { get; set; } = new HashSet<PostsTags>();

}
