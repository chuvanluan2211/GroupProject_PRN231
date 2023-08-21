using System;
using System.Collections.Generic;

namespace RiceManagement.Models
{
    public partial class Rice
    {
        public Rice()
        {
            ExportDetails = new HashSet<ExportDetail>();
            ImportRiceDetails = new HashSet<ImportRiceDetail>();
        }

        public int RiceId { get; set; }
        public int? CategoryId { get; set; }
        public string? Name { get; set; }
        public int? QuantityInStock { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<ExportDetail> ExportDetails { get; set; }
        public virtual ICollection<ImportRiceDetail> ImportRiceDetails { get; set; }
    }
}
