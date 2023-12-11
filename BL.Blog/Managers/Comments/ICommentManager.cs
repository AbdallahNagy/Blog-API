using Blog.BL.DTOs.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BL.Managers.Comments;

public interface ICommentManager
{
    Task<IEnumerable<ReadCommentDTO>?> GetAll(int id);
    Task<ReadCommentDTO> Update(UpdateCommentDTO comment, int id);
    Task Delete(int id);
    Task<ReadCommentDTO> Add(WriteCommentDTO comment);
}
