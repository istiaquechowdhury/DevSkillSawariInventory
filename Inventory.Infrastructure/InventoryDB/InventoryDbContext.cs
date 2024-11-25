using Inventory.Domain.Entities;
using Inventory.Infrastructure.RepositoryPatternClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.InventoryDb
{
    public class InventoryDbContext : DbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public InventoryDbContext(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString,
                    x => x.MigrationsAssembly(_migrationAssembly));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

          

            modelBuilder.Entity<Product>()
                .Property(p => p.AverageCost)
                .HasPrecision(18, 2);  // Precision of 18 and scale of 2

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);  // Precision of 18 and scale of 2


            modelBuilder.Entity<Product>()
                .Property(p => p.CostPerUnit)
                .HasPrecision(18, 2);
            modelBuilder.Entity<StockList>()
              .Property(p => p.WeightedAvgCost)
              .HasPrecision(18, 2);

            //modelBuilder.Entity<Product>()
            //    .Property(p => p.Tax)
            //    .HasPrecision(18, 2);
            // Precision of 18 and scale of 2
            modelBuilder.Entity<Taxes>()
               .Property(p => p.Percentage)
               .HasPrecision(18, 1);
            //modelBuilder.Entity<Stocklist>()
            //      .HasOne(s => s.Product)
            //      .WithMany()
            //      .HasForeignKey(s => s.RelatedProductId)
            //      .OnDelete(DeleteBehavior.Cascade);

          

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Taxes> Taxes { get; set; }
        public DbSet<WareHouse> WareHouses { get; set; }
        public DbSet<StockTransfer> Stocktransfers { get; set; }    

        public DbSet<Reason> Reasons { get; set; }  

        public DbSet<StockAdjustment> Stockadjustments { get; set; }

        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        public DbSet<StockList> StockLists { get; set; }






    }
}
