using CreditService.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CreditService.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<CreditApplication> CreditApplications => Set<CreditApplication>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var entity = modelBuilder.Entity<CreditApplication>();

        entity.ToTable("pengajuan_kredit");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
              .HasColumnName("id");

        entity.Property(x => x.Plafon)
              .HasColumnName("plafon")
              .HasColumnType("numeric");

        entity.Property(x => x.Bunga)
              .HasColumnName("bunga")
              .HasColumnType("decimal(5,2)");

        entity.Property(x => x.Tenor)
              .HasColumnName("tenor");

        entity.Property(x => x.Angsuran)
              .HasColumnName("angsuran")
              .HasColumnType("numeric");

        entity.Property(x => x.CreatedAt)
              .HasColumnName("created_at")
              .HasDefaultValueSql("now()");

        entity.Property(x => x.UpdatedAt)
              .HasColumnName("updated_at")
              .HasDefaultValueSql("now()");

        entity.HasIndex(x => x.Plafon).HasDatabaseName("idx_pengajuan_plafon");
        entity.HasIndex(x => x.Tenor).HasDatabaseName("idx_pengajuan_tenor");
    }
}
