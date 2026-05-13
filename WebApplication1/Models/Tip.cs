using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Tip
{
    public int TipId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool? IsFree { get; set; }

    public int? UploadedBy { get; set; }

    public DateTime? UploadDate { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual User? UploadedByNavigation { get; set; }
}
