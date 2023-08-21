namespace RiceManagement.DTOs
{
    public class AddImportRequest
    {
        public DateTime? ImportDate { get; set; }
        public int? Quantity { get; set; }
        public int? QuantityInStock { get; set; }
    }
}
