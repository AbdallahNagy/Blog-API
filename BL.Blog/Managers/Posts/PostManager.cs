using Blog.BL.DTOs.Posts;
using Blog.BL.DTOs.PostTags;
using Blog.BL.DTOs.Tags;
using Blog.BL.Exception_Handling;
using Blog.DAL.Context;
using Blog.DAL.Models;
using Blog.DAL.Repos.Posts;
using Blog.DAL.Repos.PostTag;
using Blog.DAL.Repos.Tags;
using EntityFramework.Exceptions.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
    private readonly ITagRepo _tagRepo;
    private readonly IPostTagRepo _postTagRepo;

    public PostManager(IPostRepo postRepo, ITagRepo tagRepo, IPostTagRepo postsTags)
    {
        _postRepo = postRepo;
        _tagRepo = tagRepo;
        _postTagRepo = postsTags;
    }

    async public Task<List<ReadPostDTO>?> GetAll()
    {
        try
        {
            var posts = await _postRepo.GetAll()
                ?? throw new BusinessException(204, "No posts available");

            List<ReadPostDTO> readPosts = posts
                .Select(post =>
                    new ReadPostDTO(
                        post.Id,
                        post.Title,
                        post.Body,
                        post.TotalLikes,
                        post.AuthorId,
                        post.CreatedAt,
                        post.PostsTags
                            .Select(pt => pt.Tag)
                            .Select(t => new ReadTagDTO(
                                t?.Id ?? 0, 
                                t?.Name ?? "", 
                                t?.CreatedAt ?? DateTime.MinValue))
                            .ToList()
                        )).ToList();

            return readPosts;
        }
        catch(BusinessException)
        {
            throw new BusinessException(204, "No posts available");
        }
        catch (Exception)
        {
            throw new BusinessException(500, "Interal Server Error");
        }
    }

    async public Task<ReadPostDTO?> GetById(int id)
    {
        try
        {
            var post = await _postRepo.Get(id) ?? throw new BusinessException(204, "Can't find post by this id");

            ReadPostDTO readPost = new(
                post.Id, 
                post.Title, 
                post.Body, 
                post.TotalLikes, 
                post.AuthorId, 
                post.CreatedAt,
                post.PostsTags
                    .Select(pt => pt.Tag)
                    .Select(t => new ReadTagDTO(
                        t?.Id ?? 0,
                        t?.Name ?? "",
                        t?.CreatedAt ?? DateTime.MinValue)));
            return readPost;
        }
        catch(BusinessException)
        {
            throw new BusinessException(204, "Can't find post by this id");
        }
        catch (Exception)
        {
            throw new BusinessException(500, "Interal Server Error");
        }
    }

    async public Task<ReadPostDTO> Add(WritePostDTO writePost)
    {
        var post = new Post
        {
            Title = writePost.Title,
            Body = writePost.Body,
            AuthorId = writePost.AuthorId,
            TotalLikes = 0,
        };

        var tags = writePost.Tags.Select(t => new Tag { Name = t.Name }).ToArray();

        try
        {
            var addedPost = await _postRepo.Add(post);
            await _postRepo.SaveChanges();

            await _tagRepo.AddRange(tags);
            await _tagRepo.SaveChanges();

            var postsTagsWrite = tags.Select(t => new WritePostTagsDTO(addedPost.Id, t.Id));
            var postsTags = postsTagsWrite.Select(p => new PostsTags
            {
                PostId = p.PostID,
                TagId = p.TagId
            });

            await _postTagRepo.AddRange(postsTags);
            await _postTagRepo.SaveChanges();

            return new ReadPostDTO(
                addedPost.Id,
                addedPost.Title,
                addedPost.Body, 
                addedPost.TotalLikes, 
                addedPost.AuthorId, 
                addedPost.CreatedAt,
                post.PostsTags
                    .Select(pt => pt.Tag)
                    .Select(t => new ReadTagDTO(
                        t?.Id ?? 0,
                        t?.Name ?? "",
                        t?.CreatedAt ?? DateTime.MinValue)));
        }
        catch (ReferenceConstraintException)
        {
            throw new BusinessException(400, "ReferenceConstraintException: Can't assign post to non-existing user");
        }
        catch(Exception)
        {
            throw new BusinessException(500, "Interal Server Error");
        }
    }

    async public Task Delete(int id)
    {
        try
        {
            // 1 for success, 0 for record doesn't exist
            var result = await _postRepo.Delete(id);
            if (result == 0) throw new BusinessException(204, "Record doesn't exist");
            await _postRepo.SaveChanges();
        }
        catch (BusinessException)
        {
            throw new BusinessException(204, "Record doesn't exist");
        }
        catch(Exception)
        {
            throw new BusinessException(500, "Interal Server Error");
        }
    }

    async public Task<ReadPostDTO> Update(UpdatePostDTO updatePost, int id)
    {
        var post = new Post
        {
            Id = id,
            Title = updatePost.Title,
            Body = updatePost.Body,
        };

        try
        {
            var updatedPost = await _postRepo.Update(id, post) ?? throw new BusinessException(404, "Can't find record by the provided id");
            await _postRepo.SaveChanges();
            return new ReadPostDTO(
                updatedPost.Id,
                updatedPost.Title,
                updatedPost.Body,
                updatedPost.TotalLikes,
                updatedPost.AuthorId,
                updatedPost.CreatedAt,
                post.PostsTags
                    .Select(pt => pt.Tag)
                    .Select(t => new ReadTagDTO(
                        t?.Id ?? 0,
                        t?.Name ?? "",
                        t?.CreatedAt ?? DateTime.MinValue)));
        }
        catch(BusinessException)
        {
            throw new BusinessException(404, "Can't find record by the provided id");
        }
        catch (Exception)
        {
            throw new BusinessException(500, "Interal Server Error");
        }
    }

    async public Task<List<ReadPostDTO>?> SearchByTags(int[] tagsIds)
    {
        try
        {
            var posts = await _postRepo.SearchByTags(tagsIds)
                ?? throw new BusinessException(204, "No posts available by this filter");

            List<ReadPostDTO> readPosts = posts.Select(post => new ReadPostDTO(
                post.Id,
                post.Title,
                post.Body,
                post.TotalLikes,
                post.AuthorId,
                post.CreatedAt,
                post.PostsTags
                    .Select(pt => pt.Tag)
                    .Select(t => new ReadTagDTO(
                        t?.Id ?? 0,
                        t?.Name ?? "",
                        t?.CreatedAt ?? DateTime.MinValue)))).ToList();

            return readPosts;
        }
        catch(BusinessException)
        {
            throw new BusinessException(204, "No posts available by this filter");
        }
        catch (Exception)
        {
            throw new BusinessException(500, "Interal Server Error");
        }
    }

    async public Task<List<ReadPostDTO>?> SearchByText(string str)
    {
        try
        {
            List<Post> posts = await _postRepo.SearchByText(str) ?? throw new BusinessException(204, "No posts available by this filter");
            List<ReadPostDTO> readPosts = posts.Select(post => new ReadPostDTO(
                post.Id, 
                post.Title, 
                post.Body, 
                post.TotalLikes, 
                post.AuthorId, 
                post.CreatedAt,
                post.PostsTags
                    .Select(pt => pt.Tag)
                    .Select(t => new ReadTagDTO(
                        t?.Id ?? 0,
                        t?.Name ?? "",
                        t?.CreatedAt ?? DateTime.MinValue)))).ToList();

            return readPosts;
        }
        catch (BusinessException)
        {
            throw new BusinessException(204, "No posts available by this filter");
        }
        catch (Exception)
        {
            throw new BusinessException(500, "Interal Server Error");
        }
    }

    async public Task<List<ReadPostDTO>?> SearchInBody(string str)
    {
        try
        {
            List<Post> posts = await _postRepo.SearchInBody(str) ?? throw new BusinessException(204, "No posts available by this filter");
            List<ReadPostDTO> readPosts = posts.Select(post => new ReadPostDTO(
                post.Id,
                post.Title,
                post.Body,
                post.TotalLikes,
                post.AuthorId,
                post.CreatedAt,
                post.PostsTags
                    .Select(pt => pt.Tag)
                    .Select(t => new ReadTagDTO(
                        t?.Id ?? 0,
                        t?.Name ?? "",
                        t?.CreatedAt ?? DateTime.MinValue)))).ToList();

            return readPosts;
        }
        catch (BusinessException)
        {
            throw new BusinessException(204, "No posts available by this filter");
        }
        catch (Exception)
        {
            throw new BusinessException(500, "Interal Server Error");
        }
    }

    async public Task<List<ReadPostDTO>?> SearchInTitle(string str)
    {
        try
        {
            List<Post> posts = await _postRepo.SearchInTitle(str) ?? throw new BusinessException(204, "No posts available by this filter");
            List<ReadPostDTO> readPosts = posts.Select(post => new ReadPostDTO(
                post.Id,
                post.Title,
                post.Body,
                post.TotalLikes,
                post.AuthorId,
                post.CreatedAt,
                post.PostsTags
                    .Select(pt => pt.Tag)
                    .Select(t => new ReadTagDTO(
                        t?.Id ?? 0,
                        t?.Name ?? "",
                        t?.CreatedAt ?? DateTime.MinValue)))).ToList();

            return readPosts;
        }
        catch (BusinessException)
        {
            throw new BusinessException(204, "No posts available by this filter");
        }
        catch (Exception)
        {
            throw new BusinessException(500, "Interal Server Error");
        }
    }
}
