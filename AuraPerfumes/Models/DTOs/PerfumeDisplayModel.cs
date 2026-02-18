namespace AuraPerfumes.Models.DTOs
{
    public class PerfumeDisplayModel
    {

        public IEnumerable<Perfume> Perfumes { get; set; }

        public IEnumerable<Gender> Genders { get; set; }
        public string STerm { get; set; } = " ";
        public int GenderId { get; set; } = 0;
    }
}
    