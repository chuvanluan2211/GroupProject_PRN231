namespace RiceManagement.DTOs
{
    public class AddImportRiceDetailRequest
    {
        public int? ImportId { get; set; }

        public int? RiceId { get; set; }
        public int? Quantity { get; set; }
    }
}
