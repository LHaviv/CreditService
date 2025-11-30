using CreditService.Api.Data;
using CreditService.Api.Dtos;
using CreditService.Api.Models;
using CreditService.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CreditService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]  // all endpoints require JWT
public class CreditApplicationsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IInstallmentCalculator _calculator;
    private readonly ILogger<CreditApplicationsController> _logger;

    public CreditApplicationsController(
        AppDbContext db,
        IInstallmentCalculator calculator,
        ILogger<CreditApplicationsController> logger)
    {
        _db = db;
        _calculator = calculator;
        _logger = logger;
    }

    // Create
    [HttpPost]
    public async Task<ActionResult<CreditApplicationResponse>> Create(
        [FromBody] CreateCreditApplicationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var angsuran = _calculator.CalculateMonthlyInstallment(
            request.Plafon, request.Bunga, request.Tenor);

        var entity = new CreditApplication
        {
            Id = Guid.NewGuid(),
            Plafon = request.Plafon,
            Bunga = request.Bunga,
            Tenor = request.Tenor,
            Angsuran = angsuran,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.CreditApplications.Add(entity);
        await _db.SaveChangesAsync();

        var response = ToResponse(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, response);
    }

    // Read by id
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CreditApplicationResponse>> GetById(Guid id)
    {
        var entity = await _db.CreditApplications.FindAsync(id);

        if (entity == null)
        {
            return NotFound(new { message = "Pengajuan kredit tidak ditemukan" });
        }

        return Ok(ToResponse(entity));
    }

    // List all (could add paging later)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CreditApplicationResponse>>> GetAll()
    {
        var entities = await _db.CreditApplications
                                .OrderByDescending(x => x.CreatedAt)
                                .ToListAsync();

        return Ok(entities.Select(ToResponse));
    }

    // Update
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CreditApplicationResponse>> Update(
        Guid id,
        [FromBody] UpdateCreditApplicationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var entity = await _db.CreditApplications.FindAsync(id);
        if (entity == null)
        {
            return NotFound(new { message = "Pengajuan kredit tidak ditemukan" });
        }

        entity.Plafon = request.Plafon;
        entity.Bunga = request.Bunga;
        entity.Tenor = request.Tenor;
        entity.Angsuran = _calculator.CalculateMonthlyInstallment(
            request.Plafon, request.Bunga, request.Tenor);
        entity.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(ToResponse(entity));
    }

    // Delete
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var entity = await _db.CreditApplications.FindAsync(id);
        if (entity == null)
        {
            return NotFound(new { message = "Pengajuan kredit tidak ditemukan" });
        }

        _db.CreditApplications.Remove(entity);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost("calculate")]
    public ActionResult<decimal> Calculate([FromBody] CreateCreditApplicationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var angsuran = _calculator.CalculateMonthlyInstallment(
            request.Plafon, request.Bunga, request.Tenor);

        return Ok(angsuran);
    }

    private static CreditApplicationResponse ToResponse(CreditApplication c)
        => new()
        {
            Id = c.Id,
            Plafon = c.Plafon,
            Bunga = c.Bunga,
            Tenor = c.Tenor,
            Angsuran = c.Angsuran,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        };
}
