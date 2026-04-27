namespace AuraPerfumes.DTOs
{
    public class PerfumeImportDto
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public string? ImageUrl { get; set; }
        public int GenderId { get; set; }
        public string Description { get; set; } = string.Empty;

        // Example: 30:119.99;50:159.99;100:199.99
        public string Variants { get; set; } = string.Empty;
    }

    public class PerfumeVariantImportDto
    {
        public int Ml { get; set; }
        public decimal Price { get; set; }
    }
}
