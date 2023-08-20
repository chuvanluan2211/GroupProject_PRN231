using System;
using System.Collections.Generic;

namespace RiceManagement.Models
{
    public partial class Export
    {
        public Export()
        {
            ExportDetails = new HashSet<ExportDetail>();
        }

        public int ExprotId { get; set; }
        public DateTime? ExportDate { get; set; }
        public int? Quantity { get; set; }

        public virtual ICollection<ExportDetail> ExportDetails { get; set; }
    }
}
