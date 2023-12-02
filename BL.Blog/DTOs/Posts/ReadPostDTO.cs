using Blog.BL.DTOs.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BL.DTOs.Posts
{
    public record ReadPostDTO(int Id, string? Title, string? Body, int Likes, string? AuthorId, DateTime Created_At, IEnumerable<ReadTagDTO> Tags);
}
