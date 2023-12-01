using Blog.BL.DTOs.Tags;
using Blog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blog.BL.DTOs.Posts;

public record UpdatePostDTO(string? Title, string? Body, IEnumerable<WriteTagDTO> Tags);