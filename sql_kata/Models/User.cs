using System;
using System.Collections.Generic;

namespace sql_kata.Models;

public partial class User
{
    public int UserId { get; set; }

    public int TeamId { get; set; }

    public string? Name { get; set; }

    public int? Age { get; set; }

    public virtual Team Team { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
