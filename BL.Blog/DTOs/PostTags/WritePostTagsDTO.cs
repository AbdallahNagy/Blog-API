using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BL.DTOs.PostTags;

public record WritePostTagsDTO(int PostID, int TagId);