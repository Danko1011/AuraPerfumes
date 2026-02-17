namespace AuraPerfumes
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Perfume>> GetPerfumes(string sTerm = "", int genderId = 0);
        Task<IEnumerable<Gender>> Genders();
    }
}