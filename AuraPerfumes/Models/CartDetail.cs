using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuraPerfumes.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {
        public int Id { get; set; }
        [Required]
        public int ShoppingCartId {get; set;}
        [Required]
        public int PerfumeId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public Perfume Perfume { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public int VariantId { get; set; }
        public PerfumeVariant Variant { get; set; }
    }
}
