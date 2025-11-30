using System;

namespace CreditService.Api.Models;

public class CreditApplication
{
    public Guid Id { get; set; }

    public decimal Plafon { get; set; }

    public decimal Bunga { get; set; } 

    public int Tenor { get; set; } 

    public decimal Angsuran { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
