using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;  // ← yeh add karo
namespace WebApplication1.Models;
public partial class Contest
{
    public int ContestId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? PostedBy { get; set; }
    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
    public virtual ICollection<ContestSubmission> ContestSubmissions { get; set; } = new List<ContestSubmission>();

    [ForeignKey("PostedBy")]  // ← yeh add karo
    public virtual User? PostedByNavigation { get; set; }
}