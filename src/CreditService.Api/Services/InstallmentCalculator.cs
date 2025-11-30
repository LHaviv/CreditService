using System;

namespace CreditService.Api.Services
{
    public interface IInstallmentCalculator
    {
        decimal CalculateMonthlyInstallment(decimal plafon, decimal bungaPercentPerYear, int tenorMonths);
    }

    public class InstallmentCalculator : IInstallmentCalculator
    {
        public decimal CalculateMonthlyInstallment(decimal plafon, decimal bungaPercentPerYear, int tenorMonths)
        {
            if (plafon <= 0) throw new ArgumentException("Plafon must be > 0");
            if (bungaPercentPerYear < 0 || bungaPercentPerYear > 100)
                throw new ArgumentException("Bunga must be between 0 and 100");
            if (tenorMonths <= 0) throw new ArgumentException("Tenor must be > 0");

            var monthlyRate = (double)bungaPercentPerYear / 100.0 / 12.0;
            var principal = (double)plafon;
            var n = tenorMonths;

            if (monthlyRate == 0)
            {
                return Math.Round((decimal)(principal / n), 2);
            }

            var factor = Math.Pow(1 + monthlyRate, n);
            var payment = principal * monthlyRate * factor / (factor - 1);

            return Math.Round((decimal)payment, 2);
        }
    }
}
