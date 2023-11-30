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
    List<ReadPostDTO>? GetAll();
    ReadPostDTO? GetById(int id);
    ReadPostDTO Add(WritePostDTO post);
    ReadPostDTO Update(UpdatePostDTO post, int id);
    int Delete(int id);
    List<ReadPostDTO>? SearchByTags(int[] tagsIds);
    List<ReadPostDTO>? SearchByText(string str);
    List<ReadPostDTO>? SearchInTitle(string str);
    List<ReadPostDTO>? SearchInBody(string str);
}
