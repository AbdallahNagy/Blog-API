﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.DAL.Models;

public class Tag
{
    public int Id { get; set; }
    public string? Name { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime CreatedAt { get; set; }
}