using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.DAL.Models;

[PrimaryKey(nameof(PostId), nameof(TagId))]
public class PostsTags
{
    public int PostId { get; set; }
    public Post? Post { get; set; }
    public int TagId { get; set; }
    public Tag? Tag { get; set; }
    public IEnumerable<PostsTags> PostTags { get; set; } = new HashSet<PostsTags>();
}
