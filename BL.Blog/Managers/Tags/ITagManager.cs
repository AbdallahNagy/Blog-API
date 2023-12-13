using Blog.BL.DTOs.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BL.Managers.Tags;

public interface ITagManager
{
    Task<IEnumerable<ReadTagDTO>?> GetAll();
}
