using AuraPerfumes.DTOs;
using AuraPerfumes.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AuraPerfumes.Services
{
    public class PerfumeImportService
    {
        private readonly ApplicationDbContext _context;

        public PerfumeImportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> ImportFromCsvAsync(Stream csvStream)
        {
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                TrimOptions = TrimOptions.Trim
            });

            var rows = csv.GetRecords<PerfumeImportDto>().ToList();
            var importedCount = 0;

            foreach (var row in rows)
            {
                var perfume = await _context.Perfumes
                    .FirstOrDefaultAsync(p =>
                        p.PerfumeName == row.Brand &&
                        p.PerfumeModel == row.Model);

                if (perfume == null)
                {
                    perfume = new Perfume
                    {
                        PerfumeName = row.Brand,
                        PerfumeModel = row.Model,
                        Price = (double)row.BasePrice,
                        Image = row.ImageUrl,
                        GenderId = row.GenderId,
                        Description = row.Description
                    };

                    _context.Perfumes.Add(perfume);
                    await _context.SaveChangesAsync();
                    importedCount++;
                }

                var variants = ParseVariants(row.Variants);

                foreach (var variant in variants)
                {
                    var exists = await _context.PerfumeVariants
                        .AnyAsync(v => v.PerfumeId == perfume.Id && v.Ml == variant.Ml);

                    if (!exists)
                    {
                        _context.PerfumeVariants.Add(new PerfumeVariant
                        {
                            PerfumeId = perfume.Id,
                            Ml = variant.Ml,
                            Price = (double)variant.Price
                        });
                    }
                }

                await _context.SaveChangesAsync();
            }

            return importedCount;
        }

        private static List<PerfumeVariantImportDto> ParseVariants(string variantsText)
        {
            var result = new List<PerfumeVariantImportDto>();

            if (string.IsNullOrWhiteSpace(variantsText))
                return result;

            var parts = variantsText.Split(';', StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                var mlAndPrice = part.Split(':', StringSplitOptions.RemoveEmptyEntries);

                if (mlAndPrice.Length != 2)
                    continue;

                if (!int.TryParse(mlAndPrice[0], out var ml))
                    continue;

                if (!decimal.TryParse(mlAndPrice[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
                    continue;

                result.Add(new PerfumeVariantImportDto
                {
                    Ml = ml,
                    Price = price
                });
            }

            return result;
        }
    }
}