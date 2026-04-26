using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
