using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmallShop.Entities;
using SmallShop.Persistence.EF.Categories;

namespace SmallShop.Persistence.EF
{
    public class EFDataContext : DbContext
    {
        public EFDataContext(string connectionString) :
            this(new DbContextOptionsBuilder()
                .UseSqlServer(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .Options)

        { 
        }

        public EFDataContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly
                (typeof(CategoryEntityMap).Assembly);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Goods> Goodss { get; set; }
        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
        public DbSet<SaleInvoice> SaleInvoices { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
