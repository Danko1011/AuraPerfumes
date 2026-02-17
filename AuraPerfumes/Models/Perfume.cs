using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuraPerfumes.Models
{
    [Table("Book")]
    public class Perfume
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string? PerfumeName { get; set; }
        [Required]
        [MaxLength(40)]
        public string? PerfumeModel { get; set; }
        [Required]
        public double Price { get; set; }
        public string? Image { get; set; }
        [Required]
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
        public List<CartDetail> CartDetail { get; set; }

    }
}
