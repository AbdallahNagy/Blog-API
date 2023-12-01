using Azure;
using Blog.BL.DTOs.Posts;
using Blog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BL.Managers.Posts;
public interface IPostManager
{
    Task<List<ReadPostDTO>?> GetAll();
    Task<ReadPostDTO?> GetById(int id);
    Task<ReadPostDTO> Add(WritePostDTO post);
    Task<ReadPostDTO> Update(UpdatePostDTO post, int id);
    Task Delete(int id);
    Task<List<ReadPostDTO>?> SearchByTags(int[] tagsIds);
    Task<List<ReadPostDTO>?> SearchByText(string str);
    Task<List<ReadPostDTO>?> SearchInTitle(string str);
    Task<List<ReadPostDTO>?> SearchInBody(string str);
}
