using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Models;

public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresOn { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
    public DateTime CreatedAt { get; set; }
    public DateTime? RevokedOn { get; set; }
    public bool IsActive => RevokedOn == null && !IsExpired;
}
