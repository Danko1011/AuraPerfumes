using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuraPerfumes.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int PerfumeId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double MlPrice { get; set; }
        public Order Order { get; set; }
        public Perfume Perfume { get; set; }

    }
}
