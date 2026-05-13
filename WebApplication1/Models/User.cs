using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class User
{
    public int UserID { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public string? SubscriptionType { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();

    public virtual ICollection<ContestSubmission> ContestSubmissions { get; set; } = new List<ContestSubmission>();

    public virtual ICollection<Contest> Contests { get; set; } = new List<Contest>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual ICollection<Tip> Tips { get; set; } = new List<Tip>();
}
