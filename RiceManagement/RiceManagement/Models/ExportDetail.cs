using System;
using System.Collections.Generic;

namespace RiceManagement.Models
{
    public partial class ExportDetail
    {
        public int ExportDetailId { get; set; }
        public int? ExportId { get; set; }
        public int? ImportId { get; set; }
        public int? Quantity { get; set; }
        public int? RiceId { get; set; }

        public virtual Export? Export { get; set; }
        public virtual Import? Import { get; set; }
        public virtual Rice? Rice { get; set; }
    }
}
