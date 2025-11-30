using System.ComponentModel.DataAnnotations;

namespace CreditService.Api.Dtos;

public class CreateCreditApplicationRequest
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Plafon must be > 0")]
    public decimal Plafon { get; set; }

    [Required]
    [Range(0.01, 100.0, ErrorMessage = "Bunga must be between 0 and 100")]
    public decimal Bunga { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Tenor must be >= 1 month")]
    public int Tenor { get; set; }
}

public class UpdateCreditApplicationRequest : CreateCreditApplicationRequest
{
    // same fields for now
}

public class CreditApplicationResponse
{
    public Guid Id { get; set; }
    public decimal Plafon { get; set; }
    public decimal Bunga { get; set; }
    public int Tenor { get; set; }
    public decimal Angsuran { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
