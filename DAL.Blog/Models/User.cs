using Blog.DAL.DataTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.DAL.Models;

public class User : IdentityUser
{
    public string? DisplayName { get; set; }
    public Gender Gender { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime CreatedAt { get; set; }
    public List<RefreshToken>? RefreshTokens { get; set; }
}
