using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? UserId { get; set; }

    public int? RecipeId { get; set; }

    public int? TipId { get; set; }

    public string? Message { get; set; }

    public int? Rating { get; set; }

    public DateTime? FeedbackDate { get; set; }

    public virtual Recipe? Recipe { get; set; }

    public virtual Tip? Tip { get; set; }

    public virtual User? User { get; set; }
}
