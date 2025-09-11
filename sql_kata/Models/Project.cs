using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql_kata.Models
{
    public partial class Project
    {
        public int ProjectId { get; set; }
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
