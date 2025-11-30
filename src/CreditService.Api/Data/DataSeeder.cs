using CreditService.Api.Models;
using CreditService.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace CreditService.Api.Data;

public static class DataSeeder
{
    public static void Seed(AppDbContext context, IInstallmentCalculator calculator, ILogger logger)
    {
        // Only seed if table is empty
        if (context.CreditApplications.Any())
        {
            logger.LogInformation("Database already has data, skipping seeding.");
            return;
        }

        logger.LogInformation("Seeding initial credit applications...");

        var now = DateTime.UtcNow;

        var seedData = new List<CreditApplication>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Plafon = 100_000_000m,
                Bunga = 12m,
                Tenor = 60,
                Angsuran = calculator.CalculateMonthlyInstallment(100_000_000m, 12m, 60),
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Plafon = 50_000_000m,
                Bunga = 10m,
                Tenor = 36,
                Angsuran = calculator.CalculateMonthlyInstallment(50_000_000m, 10m, 36),
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Plafon = 200_000_000m,
                Bunga = 14m,
                Tenor = 120,
                Angsuran = calculator.CalculateMonthlyInstallment(200_000_000m, 14m, 120),
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Plafon = 25_000_000m,
                Bunga = 8m,
                Tenor = 24,
                Angsuran = calculator.CalculateMonthlyInstallment(25_000_000m, 8m, 24),
                CreatedAt = now,
                UpdatedAt = now
            }
        };

        context.CreditApplications.AddRange(seedData);
        context.SaveChanges();

        logger.LogInformation("Seeding completed. Inserted {Count} records.", seedData.Count);
    }
}
