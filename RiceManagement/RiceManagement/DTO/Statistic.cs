using AutoMapper.Configuration.Conventions;

namespace RiceManagement.DTO
{
    public class Statistic
    {
        public int ImportId { get; set; }
        public int ExportId { get; set; }
        public string Date { get; set; }
        public int? QuantityInStock { get; set; }
        public int ?ImportQuantity { get; set; }
        public int ?ExportQuantity { get; set; }
    }
}
