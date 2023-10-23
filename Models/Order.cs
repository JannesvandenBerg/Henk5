using System.ComponentModel.DataAnnotations;

namespace Henk5.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public bool Done { get; set; }
        [Required]
        public decimal? TotalPrice { get; set; }

        public virtual ICollection<Item>? Items { get; set; }
        public virtual Owner? Owner { get; set; }

        public int? OwnerId { get; set; }
        public virtual User? User { get; set; }

        public int? UserId { get; set; }

    }
}
