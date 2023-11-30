using Blog.DAL.Models;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Context;

public class BlogDbContext : IdentityDbContext<User>
{
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostsTags> PostsTags { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseExceptionProcessor();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("Users");

        modelBuilder.Entity<Post>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<User>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Like>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Comment>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Tag>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
    }
}
