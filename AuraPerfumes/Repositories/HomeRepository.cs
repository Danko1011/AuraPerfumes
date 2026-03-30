
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

        public async Task<IEnumerable<Perfume>> GetPerfumes(string model = "", int genderId = 0, string designerName = "")
        {
            model = (model ?? "").ToLower().Trim();
            designerName = (designerName ?? "").Trim();

            var query =
                from perfume in _db.Perfumes
                join gender in _db.Genders on perfume.GenderId equals gender.Id
                select new Perfume
                {
                    Id = perfume.Id,
                    Image = perfume.Image,
                    PerfumeName = perfume.PerfumeName,     // Designer
                    PerfumeModel = perfume.PerfumeModel,   // Model
                    GenderId = perfume.GenderId,
                    Price = perfume.Price,
                    GenderName = gender.GenderLabel
                };

            if (genderId > 0)
                query = query.Where(p => p.GenderId == genderId);

            if (!string.IsNullOrWhiteSpace(designerName))
                query = query.Where(p => p.PerfumeName == designerName);

            if (!string.IsNullOrWhiteSpace(model))
                query = query.Where(p => p.PerfumeModel.ToLower().Contains(model));

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<string>> Designers()
        {
            return await _db.Perfumes
                .Select(p => p.PerfumeName)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();
        }
        public async Task<Perfume?> GetPerfumeDetails(int id)
        {
            return await _db.Perfumes
                .Include(p => p.Gender)
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<List<Perfume>> GetRelatedPerfumes(int genderId, int excludeId)
        {
            return await _db.Perfumes
                .Include(p => p.Variants)
                .Where(p => p.GenderId == genderId && p.Id != excludeId)
                .Take(3)
                .ToListAsync();
        }
    }
}
