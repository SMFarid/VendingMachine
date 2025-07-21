
using Microsoft.EntityFrameworkCore;
using VendingMachine.Models;

namespace VendingMachine.Data
{
    public class VendingMachineContext : DbContext
    {
        public VendingMachineContext()
        {
        }
        public VendingMachineContext(DbContextOptions<VendingMachineContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=CALIBARN;Database=VendingDB;Trusted_Connection=true;TrustServerCertificate=true;");
            }
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.Property(e => e.Role).HasConversion<string>();
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Balance).HasDefaultValue(0);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAddOrUpdate();  
            });

            builder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(100);
                entity.HasOne(e => e.Seller)
                      .WithMany()
                      .HasForeignKey(e => e.SellerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Buyer)
                      .WithMany()
                      .HasForeignKey(e => e.BuyerId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Seller)
                      .WithMany()
                      .HasForeignKey(e => e.SellerId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Product)
                      .WithMany()
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
