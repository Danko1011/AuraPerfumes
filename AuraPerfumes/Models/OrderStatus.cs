using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuraPerfumes.Models
{
    [Table("OrderStatus")]
    public class OrderStatus
    {
        public int Id { get; set; }
        [Required]
        public int Status { get; set; }
        [Required, MaxLength(20)]
        public string ?StatusName { get; set; }
    }
}
