namespace RiceManagement.DTOs
{
    public class ImportRiceDetailResponse
    {
        public int ImportDetailId { get; set; }
        public int? ImportId { get; set; }
        public string? RiceName { get; set; }
        public int? Quantity { get; set; }
    }
}
