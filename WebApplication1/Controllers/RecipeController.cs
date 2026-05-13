using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class RecipeController : Controller
    {
        JamesthewContext db = new JamesthewContext();

    // ===================== LIST PAGE =====================
    public IActionResult Recipes()
        {
            bool isLoggedIn = HttpContext.Session.GetString("UserId") != null;
            ViewBag.IsLoggedIn = isLoggedIn;

            ViewBag.Categories = db.Categories.ToList();

            var data = db.Recipes
                .Include(r => r.Category)
                .Include(r => r.UploadedByNavigation);

            if (isLoggedIn)
            {
                return View(data.ToList());
            }
            else
            {
                var freeRecipes = data.Where(r => r.IsFree == true).ToList();
                return View(freeRecipes);
            }
        }

        // ===================== ADD =====================
        [HttpGet]
        public IActionResult Add()
        {
            ViewData["CategoryId"] = new SelectList(db.Categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Add(Recipe model, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);

                string folderPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = System.IO.Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                model.image = "/images/" + fileName;
            }

            // ✅ IMPORTANT FIX: Category attach
            model.Category = db.Categories.Find(model.CategoryId);

            model.UploadDate = DateTime.Now;

            db.Recipes.Add(model);
            db.SaveChanges();

            return RedirectToAction("Recipes");
        }

        // ===================== DELETE =====================
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var data = db.Recipes.Include(r => r.Category).FirstOrDefault(r => r.RecipeId == id);

            if (data == null) return NotFound();

            return View(data);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            // Pehle related data delete karo
           

            var saved = db.Saveds.Where(s => s.RecipeId == id).ToList();
            db.Saveds.RemoveRange(saved);

            var feedbacks = db.Feedbacks.Where(f => f.RecipeId == id).ToList();
            db.Feedbacks.RemoveRange(feedbacks);

            // Ab recipe delete karo
            var data = db.Recipes.Find(id);
            if (data != null)
            {
                db.Recipes.Remove(data);
                db.SaveChanges();
            }

            return RedirectToAction("Recipes");
        }

        // ===================== UPDATE =====================
        [HttpGet]
        public IActionResult Update(int id)
        {
            ViewData["CategoryId"] = new SelectList(db.Categories, "CategoryId", "Name");

            var data = db.Recipes.Find(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult Update(Recipe model, IFormFile file, string hid)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);

                string folderPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = System.IO.Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                model.image = "/images/" + fileName;
            }
            else
            {
                model.image = hid;
            }

            // ✅ FIX CATEGORY RELATION
            model.Category = db.Categories.Find(model.CategoryId);

            db.Recipes.Update(model);
            db.SaveChanges();

            return RedirectToAction("Recipes");
        }

        // ===================== DETAIL =====================
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var data = db.Recipes
                .Include(r => r.Category)
                .Include(r => r.UploadedByNavigation)
                .FirstOrDefault(x => x.RecipeId == id);

            return View(data);
        }
    }

}
