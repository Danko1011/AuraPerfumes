using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuraPerfumes.Models
{
    public class PerfumeVariant
    {
        public int Id { get; set; }

        [Required]
        public int PerfumeId { get; set; }

        [ForeignKey(nameof(PerfumeId))]
        public Perfume Perfume { get; set; } = null!;

        [Range(1, 10000)]
        public int Ml { get; set; }

        [Range(0.01, 100000)]
        public double Price { get; set; }
    }
}