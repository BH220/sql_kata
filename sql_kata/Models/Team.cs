using System;
using System.Collections.Generic;

namespace sql_kata.Models;

public partial class Team
{
    public int TeamId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
