using System;
using System.Collections.Generic;

namespace RiceManagement.Models
{
    public partial class Category
    {
        public Category()
        {
            Rice = new HashSet<Rice>();
        }

        public int CategoryId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Rice> Rice { get; set; }
    }
}
