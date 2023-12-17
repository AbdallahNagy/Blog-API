using Blog.API.Configurations;
using Blog.API.Middlewares;
using Blog.BL.Managers.Comments;
using Blog.BL.Managers.Posts;
using Blog.BL.Managers.Tags;
using Blog.BL.Managers.Users;
using Blog.DAL.Context;
using Blog.DAL.Models;
using Blog.DAL.Repos.Comments;
using Blog.DAL.Repos.Likes;
using Blog.DAL.Repos.Posts;
using Blog.DAL.Repos.PostTag;
using Blog.DAL.Repos.Tags;
using Blog.DAL.Repos.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// database configuration
var connectionString = builder.Configuration.GetConnectionString("connStr");
builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JWTConfig:SecretKey"));
 
builder.Services.AddDefaultIdentity<User>().AddEntityFrameworkStores<BlogDbContext>();

// Repos Registration
builder.Services.AddScoped<IPostRepo, PostRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<ITagRepo, TagRepo>();
builder.Services.AddScoped<IPostTagRepo, PostTagRepo>();
builder.Services.AddScoped<ICommentRepo, CommentRepo>();
builder.Services.AddScoped<ILikeRepo, LikeRepo>();


// Managers Registration
builder.Services.AddScoped<IPostManager, PostManager>();
builder.Services.AddScoped<ICommentManager, CommentManager>();
builder.Services.AddScoped<ITagManager, TagManager>();
builder.Services.AddScoped<IUserManager, BlogUserManager>();


// Middlewares
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    // convert key to array of bytes
    var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JWTConfig:SecretKey").Value ?? "ld2e5nvi1adkq");

    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, // for dev
        ValidateAudience = false, // for dev
        RequireExpirationTime = false,// for dev
        ValidateLifetime = true,
    };

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
