namespace AuraPerfumes.Models.DTOs
{
    public class PerfumeDisplayModel
    {
        public IEnumerable<Perfume> Perfumes { get; set; }
        public IEnumerable<Gender> Genders { get; set; }

        public IEnumerable<string> Designers { get; set; } = Enumerable.Empty<string>();
        public string DesignerName { get; set; } = "";
        public string Model { get; set; } = "";

        public string STerm { get; set; } = "";   // ако искаш може да го махнеш по-късно
        public int GenderId { get; set; } = 0;
    }
}
    