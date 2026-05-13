using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class TipsController : Controller
    {
        private readonly JamesthewContext db;

        public TipsController(JamesthewContext context)
        {
            db = context;
        }

        public IActionResult Tips()
        {
            var isLoggedIn = User.Identity.IsAuthenticated;

            var tips = db.Tips.AsQueryable();

            // 👤 Guest → only free tips
            if (!isLoggedIn)
            {
                tips = tips.Where(t => t.IsFree == true);
            }

            // Ensure UploadedByNavigation is loaded for each Tip
            tips = tips.Include(t => t.UploadedByNavigation);

            return View(tips.ToList());
        }

        [HttpGet]
        public IActionResult CreateTip()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTip(Tip t)
        {
            t.UploadDate = DateTime.Now;
            t.UploadedBy = 3;

            db.Tips.Add(t);
            db.SaveChanges();

            // Ye line aapko paka Admin wale page par hi bhejegi
            return Redirect("/Tips/Tips");
        }
        // ==================== EDIT TIP ====================

        [HttpGet]
        public IActionResult EditTip(int id)
        {
            var tip = db.Tips.Find(id);
            if (tip == null)
            {
                return NotFound();
            }
            return View(tip);
        }

        [HttpPost]
        public IActionResult EditTip(Tip Tips)
        {
            if (ModelState.IsValid)
            {
                db.Tips.Update(Tips); // Update the Tip
                db.SaveChanges();
                return RedirectToAction("Tips");
            }
            return View(Tips);
        }

        // ==================== DELETE TIP ====================

        public IActionResult DeleteTip(int id)
        {
            var tip = db.Tips.Find(id); // Check karein '_db' hai ya 'db'
            if (tip != null)
            {
                db.Tips.Remove(tip);
                db.SaveChanges();
            }
            return RedirectToAction("Tips"); // Wapis list par bhej dega
        }
    }
}