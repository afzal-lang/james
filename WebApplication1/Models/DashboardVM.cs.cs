using System.Collections.Generic;
namespace WebApplication1.Models
{
    public class DashboardVM
    {
        public int TotalCategories { get; set; }
        public int TotalRecipes { get; set; }
        public int TotalUsers { get; set; }

        public List<Category> Categories { get; set; }
    }
}