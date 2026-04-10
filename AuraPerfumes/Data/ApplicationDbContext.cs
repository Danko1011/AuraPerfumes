using AuraPerfumes.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuraPerfumes.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Perfume> Perfumes { get; set; }
        public DbSet<Gender> Genders { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        public DbSet<PerfumeVariant> PerfumeVariants { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Ръчна конфигурация според SQL скрипта (ако е необходимо)

            // Настройка на таблицата Perfume (която в началото е била Book)
            builder.Entity<Perfume>().ToTable("Pefume"); // Ако в базата все още се казва Book

            // Комбинирани ключове за Identity (вече са конфигурирани в base, 
            // но тук добавяме спецификите от твоите миграции)

            builder.Entity<CartDetail>()
                .HasOne(cd => cd.Variant)
                .WithMany()
                .HasForeignKey(cd => cd.VariantId)
                .OnDelete(DeleteBehavior.NoAction); // Избягваме циклични изтривания
        }
    }
}
