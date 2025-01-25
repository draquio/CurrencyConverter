using CurrencyConverter.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConverter.Context
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<ConversionLog> ConversionLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ConversionLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FromCurrency).IsRequired().HasMaxLength(3);
                entity.Property(e => e.ToCurrency).IsRequired().HasMaxLength(3);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ConvertedAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Timestamp).HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
