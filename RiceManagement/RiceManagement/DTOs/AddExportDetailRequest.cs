namespace RiceManagement.DTOs
{
    public class AddExportDetailRequest
    {
        public int? ExportId { get; set; }
        public int? ImportId { get; set; }
        public int? Quantity { get; set; }
        public int? RiceId { get; set; }
    }
}
