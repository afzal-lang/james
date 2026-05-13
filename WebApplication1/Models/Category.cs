using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    [Display(Name = "Category Picture")]
    public string? image { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

}
