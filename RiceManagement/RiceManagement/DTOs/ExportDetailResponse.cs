namespace RiceManagement.DTOs
{
    public class ExportDetailResponse
    {
        public int ExportDetailId { get; set; }
        public int? ExportId { get; set; }
        public int? ImportId { get; set; }
        public int? Quantity { get; set; }
        public string? RiceName { get; set; }
    }
}
