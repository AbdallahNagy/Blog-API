using Blog.BL.DTOs.Posts;
using Blog.BL.Exception_Handling;
using Blog.DAL.Models;
using Blog.DAL.Repos.Posts;
using EntityFramework.Exceptions.Common;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BL.Managers.Posts;

public class PostManager : IPostManager
{
    private readonly IPostRepo _postRepo;
    public PostManager(IPostRepo postRepo)
    {
        _postRepo = postRepo;
    }

    public List<ReadPostDTO>? GetAll()
    {
        var posts = _postRepo.GetAll();

        if (posts == null) throw new BusinessException(204, "No posts available");

        List<ReadPostDTO> readPosts = posts
            .Select(post =>
                new ReadPostDTO(
                    post.Id,
                    post.Title,
                    post.Body,
                    post.Likes,
                    post.AuthorId,
                    post.CreatedAt)).ToList();

        return readPosts;
    }

    public ReadPostDTO? GetById(int id)
    {
        var post = _postRepo.Get(id) ?? throw new BusinessException(204, "Can't find post by this id");
        ReadPostDTO readPost = new(post.Id, post.Title, post.Body, post.Likes, post.AuthorId, post.CreatedAt);

        return readPost;
    }

    public ReadPostDTO Add(WritePostDTO writePost)
    {
        var post = new Post
        {
            Title = writePost.Title,
            Body = writePost.Body,
            AuthorId = writePost.AuthorId,
            Likes = 0,
        };

        try
        {
            var addedPost = _postRepo.Add(post);
            _postRepo.SaveChanges();
            return new ReadPostDTO(addedPost.Id, addedPost.Title, addedPost.Body, addedPost.Likes, addedPost.AuthorId, addedPost.CreatedAt);
        }
        catch (ReferenceConstraintException)
        {
            throw new BusinessException(400, "ReferenceConstraintException: Can't assign post to non-existing user");
        }
    }

    public int Delete(int id)
    {
        // 1 for success, 0 for record doesn't exist
        var result = _postRepo.Delete(id);
        if (result == 0) throw new BusinessException(204, "Record doesn't exist");
        _postRepo.SaveChanges();

        return result;
    }

    public ReadPostDTO Update(UpdatePostDTO updatePost, int id)
    {
        var post = new Post
        {
            Id = id,
            Title = updatePost.Title,
            Body = updatePost.Body,
        };

        var updatedPost = _postRepo.Update(id, post) ?? throw new BusinessException(404, "Can't find record by the provided id");
        _postRepo.SaveChanges();
        return new ReadPostDTO(updatedPost.Id, updatedPost.Title, updatedPost.Body, updatedPost.Likes, updatedPost.AuthorId, updatedPost.CreatedAt);
    }

    public List<ReadPostDTO>? SearchByTags(int[] tagsIds)
    {
        List<Post> posts = _postRepo.SearchByTags(tagsIds) ?? throw new BusinessException(204, "No posts available by this filter");
        List<ReadPostDTO> readPosts = posts.Select(post => new ReadPostDTO(post.Id, post.Title, post.Body, post.Likes, post.AuthorId, post.CreatedAt)).ToList();
        return readPosts;
    }

    public List<ReadPostDTO>? SearchByText(string str)
    {
        List<Post> posts = _postRepo.SearchByText(str) ?? throw new BusinessException(204, "No posts available by this filter");
        List<ReadPostDTO> readPosts = posts.Select(post => new ReadPostDTO(post.Id, post.Title, post.Body, post.Likes, post.AuthorId, post.CreatedAt)).ToList();
        return readPosts;
    }

    public List<ReadPostDTO>? SearchInBody(string str)
    {
        List<Post> posts = _postRepo.SearchInBody(str) ?? throw new BusinessException(204, "No posts available by this filter");
        List<ReadPostDTO> readPosts = posts.Select(post => new ReadPostDTO(post.Id, post.Title, post.Body, post.Likes, post.AuthorId, post.CreatedAt)).ToList();
        return readPosts;
    }

    public List<ReadPostDTO>? SearchInTitle(string str)
    {
        List<Post> posts = _postRepo.SearchInTitle(str);
        if (posts == null) throw new BusinessException(204, "No posts available by this filter");
        List<ReadPostDTO> readPosts = posts.Select(post => new ReadPostDTO(post.Id, post.Title, post.Body, post.Likes, post.AuthorId, post.CreatedAt)).ToList();
        return readPosts;
    }
}
