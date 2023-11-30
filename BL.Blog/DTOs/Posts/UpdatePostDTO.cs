using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blog.BL.DTOs.Posts;

public record UpdatePostDTO
{
    public string? Title { get; set; }
    public string? Body { get; set; }
}
