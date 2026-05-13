using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string PaymentType { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
