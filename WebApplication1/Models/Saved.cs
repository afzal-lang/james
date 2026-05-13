using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models;

public partial class Saved
{
    public int SavedId { get; set; }
    public int UserId { get; set; }
    public int RecipeId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public virtual User? User { get; set; }
    public virtual Recipe? Recipe { get; set; }
}