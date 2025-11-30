using CreditService.Api.Controllers;
using CreditService.Api.Data;
using CreditService.Api.Dtos;
using CreditService.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class CreditApplicationsControllerTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Create_ValidRequest_ReturnsCreated()
    {
        var db = CreateDbContext();
        var logger = Mock.Of<ILogger<CreditApplicationsController>>();
        var controller = new CreditApplicationsController(
            db, new InstallmentCalculator(), logger);

        var request = new CreateCreditApplicationRequest
        {
            Plafon = 100000000,
            Bunga = 12,
            Tenor = 60
        };

        var result = await controller.Create(request);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.NotNull(createdResult.Value);
    }

    [Fact]
    public async Task GetById_NotFound_ReturnsNotFound()
    {
        var db = CreateDbContext();
        var logger = Mock.Of<ILogger<CreditApplicationsController>>();
        var controller = new CreditApplicationsController(
            db, new InstallmentCalculator(), logger);

        var result = await controller.GetById(Guid.NewGuid());

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
