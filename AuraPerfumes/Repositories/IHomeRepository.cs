namespace AuraPerfumes
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Perfume>> GetPerfumes(string sTerm = "", int genderId = 0 ,string designerName = "");
        Task<IEnumerable<Gender>> Genders();
        Task<IEnumerable<string>> Designers();
    }
}