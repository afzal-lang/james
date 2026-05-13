using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

public class SavedController : Controller
{
    JamesthewContext _db = new JamesthewContext();

    public IActionResult ToggleSaved(int recipeId)
    {
        // ✅ String se read karo — GetInt32 null deta tha
        string userIdStr = HttpContext.Session.GetString("UserId");
        if (userIdStr == null)
            return Json(new { success = false });

        int userId = int.Parse(userIdStr); // ✅ Parse karo

        var existing = _db.Saveds
            .FirstOrDefault(s => s.UserId == userId && s.RecipeId == recipeId);

        if (existing != null)
        {
            _db.Saveds.Remove(existing);
            _db.SaveChanges();
            return Json(new { success = true, action = "removed" });
        }
        else
        {
            _db.Saveds.Add(new Saved
            {
                UserId = userId,
                RecipeId = recipeId,
                CreatedAt = DateTime.Now
            });
            _db.SaveChanges();
            return Json(new { success = true, action = "added" });
        }


    }

    public IActionResult MySaved()
    {
        string userIdStr = HttpContext.Session.GetString("UserId");
        if (userIdStr == null)
        return RedirectToAction("Login", "cookies", new { returnUrl = "/Saved/MySaved" });

        int userId = int.Parse(userIdStr);

        var savedRecipes = _db.Saveds
            .Where(s => s.UserId == userId)
            .Include(s => s.Recipe)
                .ThenInclude(r => r.Category)
            .Select(s => s.Recipe)
            .ToList();
        ViewBag.Categories = _db.Categories.ToList();
        return View(savedRecipes); // ✅ Recipe list return ho rahi hai
    }
}