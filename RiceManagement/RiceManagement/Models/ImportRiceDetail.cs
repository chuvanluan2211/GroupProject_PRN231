using System;
using System.Collections.Generic;

namespace RiceManagement.Models
{
    public partial class ImportRiceDetail
    {
        public int ImportDetailId { get; set; }
        public int? ImportId { get; set; }
        public int? RiceId { get; set; }
        public int? Quantity { get; set; }

        public virtual Import? Import { get; set; }
        public virtual Rice? Rice { get; set; }
    }
}
