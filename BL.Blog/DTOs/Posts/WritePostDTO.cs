using Blog.BL.DTOs.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BL.DTOs.Posts;

public record WritePostDTO(string? Title, string? Body, IEnumerable<WriteTagDTO> Tags, string? AuthorId);
