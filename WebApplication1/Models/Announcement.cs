using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

public partial class Announcement
{
    public int AnnouncementId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? ContestId { get; set; }

    public int? PostedBy { get; set; }

    public DateTime? PostDate { get; set; }

    public virtual Contest? Contest { get; set; }

    [ForeignKey("PostedBy")] // Yeh line add karein
    public virtual User? PostedByNavigation { get; set; }
}
