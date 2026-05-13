using System;
using System.Collections.Generic;
namespace WebApplication1.Models;

public partial class ContestSubmission
{
    public int SubmissionId { get; set; }
    public int? UserId { get; set; }
    public int? ContestId { get; set; }
    public string? Type { get; set; }       // "Recipe" ya "Tip"
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? SubmissionDate { get; set; } = DateTime.Now;

    // Navigation Properties
    public virtual Contest? Contest { get; set; }
    public virtual User? User { get; set; }
}