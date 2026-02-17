
using Microsoft.EntityFrameworkCore;

namespace AuraPerfumes.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Gender>> Genders()
        {
            return  await _db.Genders.ToListAsync();
        }
        
        public async Task<IEnumerable<Perfume>> GetPerfumes(string sTerm="", int genderId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Perfume> perfumes = await (from perfume in _db.Perfumes
                            join gender in _db.Genders
                            on perfume.GenderId equals gender.Id
                            where string.IsNullOrWhiteSpace(sTerm) ||
                            (perfume!=null  && perfume.PerfumeName.ToLower().StartsWith(sTerm))
                            select new Perfume
                            {
                                Id = perfume.Id,
                                Image = perfume.Image,
                                PerfumeName = perfume.PerfumeName,
                                PerfumeModel = perfume.PerfumeModel,
                                GenderId = perfume.GenderId,
                                Price = perfume.Price,
                                GenderName = perfume.GenderName,
                            }
                            )
                            .ToListAsync();
            if(genderId > 0)
            {
                perfumes = perfumes.Where(a => a.GenderId == genderId).ToList();
            }
            return perfumes;

        }
    }
}
