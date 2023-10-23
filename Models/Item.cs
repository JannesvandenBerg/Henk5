using System.ComponentModel.DataAnnotations;

namespace Henk5.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [DataType(DataType.Currency)]
        public decimal? Price { get; set; }
        public virtual ICollection<Sale>? Sales { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual Category? Category { get; set; }
        public int? CategoryId { get; set; }
    }
}
