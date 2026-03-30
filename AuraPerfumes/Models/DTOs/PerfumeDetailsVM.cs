using AuraPerfumes.Models;

namespace AuraPerfumes.Models.DTOs
{
    public class PerfumeDetailsVM
    {
        public Perfume Perfume { get; set; } = null!;
        public int SelectedVariantId { get; set; }
        public List<Perfume> RelatedPerfumes { get; set; } = new();
    }
}