﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BL.DTOs.Tags;
public record ReadTagDTO(int Id, string? Name, DateTime CreatedAt);
