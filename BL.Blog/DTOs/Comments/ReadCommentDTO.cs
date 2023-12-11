using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BL.DTOs.Comments;

public record ReadCommentDTO(int Id, string? Body, DateTime CreatedAt, string UserId);
