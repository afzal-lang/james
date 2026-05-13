using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Recipe
{
    public int RecipeId { get; set; }

public string? Title { get; set; }

    public string? Ingredients { get; set; }

    public string? Description { get; set; }

    public string? Instructions { get; set; }
    public int? CookingTime { get; set; }

    public int? Servings { get; set; }

    public string? Tags { get; set; }

    public string? Nutrition { get; set; }

    public string? Name { get; set; }  

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public bool? IsFree { get; set; }

    public int? UploadedBy { get; set; }

    public DateTime? UploadDate { get; set; }

    public string? image { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual User? UploadedByNavigation { get; set; }

}
