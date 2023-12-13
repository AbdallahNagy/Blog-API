using Blog.BL.Managers.Comments;
using Blog.BL.Managers.Posts;
using Blog.BL.Managers.Tags;
using Blog.DAL.Context;
using Blog.DAL.Repos.Comments;
using Blog.DAL.Repos.Posts;
using Blog.DAL.Repos.PostTag;
using Blog.DAL.Repos.Tags;
using Blog.DAL.Repos.Users;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// database configuration
var connectionString = builder.Configuration.GetConnectionString("connStr");
builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(connectionString));

// Repos Registration
builder.Services.AddScoped<IPostRepo, PostRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<ITagRepo, TagRepo>();
builder.Services.AddScoped<IPostTagRepo, PostTagRepo>();
builder.Services.AddScoped<ICommentRepo, CommentRepo>();


// Managers Registration
builder.Services.AddScoped<IPostManager, PostManager>();
builder.Services.AddScoped<ICommentManager, CommentManager>();
builder.Services.AddScoped<ITagManager, TagManager>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
