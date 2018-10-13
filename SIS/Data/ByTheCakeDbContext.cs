using ByTheCakeApp.Data.Configuration;
using ByTheCakeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ByTheCakeApp.Data
{
    public class ByTheCakeDbContext : DbContext
    {
        public ByTheCakeDbContext()
        {
        }

        public ByTheCakeDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<OrderProduct> OrderProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(ByTheCakeApp.Data.Configuration.Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new OrderConfig())
                .ApplyConfiguration(new ProductConfig())
                .ApplyConfiguration(new UserConfig())
                .ApplyConfiguration(new OrderProductsConfig());
        }
    }
}
