using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BL.DTOs.Users;

public record LoginDTO(string Email, string Password);
