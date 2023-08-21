namespace RiceManagement.DTOs
{
    public class UpdateImportRequest
    {
        public DateTime? ImportDate { get; set; }
        public int? Quantity { get; set; }
        public int? QuantityInStock { get; set; }
    }
}
