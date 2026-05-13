using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    // ===================== Contest List (Admin + User) =====================
    public class ContestListViewModel
    {
        public List<Contest> Contests { get; set; } = new List<Contest>();
        public int TotalContests { get; set; }
        public int ActiveContests { get; set; }
    }

    // ===================== Admin: View Submissions =====================
    public class ContestSubmissionsViewModel
    {
        public Contest Contest { get; set; }
        public List<ContestSubmission> Submissions { get; set; } = new List<ContestSubmission>();
        public string FilterType { get; set; } = "All"; // "All", "Recipe", "Tip"
    }

    // ===================== User: Submit to Contest =====================
    public class SubmitToContestViewModel
    {
        public int ContestId { get; set; }           // ContestId (small d)
        public string ContestTitle { get; set; }
        public string ContestDescription { get; set; }
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Type select karo")]
        [Display(Name = "Submission Type")]
        public string Type { get; set; }             // "Recipe" ya "Tip"

        [Required(ErrorMessage = "Title zaroor bharo")]
        [StringLength(200)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content zaroor bharo")]
        [Display(Name = "Recipe / Tip Content")]
        public string Description { get; set; }
    }
}