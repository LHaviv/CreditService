using CreditService.Api.Services;
using Xunit;

namespace CreditService.Tests;

public class InstallmentCalculatorTests
{
    [Fact]
    public void CalculateMonthlyInstallment_ZeroInterest_ReturnsPrincipalDividedByTenor()
    {
        var calc = new InstallmentCalculator();
        var result = calc.CalculateMonthlyInstallment(1000m, 0m, 10);

        Assert.Equal(100m, result);
    }

    [Fact]
    public void CalculateMonthlyInstallment_NegativePlafon_Throws()
    {
        var calc = new InstallmentCalculator();
        Assert.Throws<ArgumentException>(() =>
            calc.CalculateMonthlyInstallment(-1m, 10m, 10));
    }
}
