using Blog.BL.Managers.Posts;
using Blog.DAL.Context;
using Blog.DAL.Repos.Posts;
using Blog.DAL.Repos.Tags;
using Blog.DAL.Repos.Users;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

// Managers Registration
builder.Services.AddScoped<IPostManager, PostManager>();


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
