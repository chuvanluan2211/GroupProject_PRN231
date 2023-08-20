using System;
using System.Collections.Generic;

namespace RiceManagement.Models
{
    public partial class Import
    {
        public Import()
        {
            ExportDetails = new HashSet<ExportDetail>();
            ImportRiceDetails = new HashSet<ImportRiceDetail>();
        }

        public int ImportId { get; set; }
        public DateTime? ImportDate { get; set; }
        public int? Quantity { get; set; }
        public int? QuantityInStock { get; set; }

        public virtual ICollection<ExportDetail> ExportDetails { get; set; }
        public virtual ICollection<ImportRiceDetail> ImportRiceDetails { get; set; }
    }
}
