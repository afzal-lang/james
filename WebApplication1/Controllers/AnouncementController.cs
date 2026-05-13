using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly JamesthewContext _context;

        public AnnouncementController(JamesthewContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            // 1. Categories load karein (Login ho ya na ho, ye chalay ga)
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;

            // 2. Announcements load karein (Aapka existing code)
            var announcements = await _context.Announcements
                .Include(a => a.Contest)
                .Include(a => a.PostedByNavigation)
                .OrderByDescending(a => a.PostDate)
                .ToListAsync();

            return View(announcements);
        }

        // 📌 CREATE (GET)
        public IActionResult CreateAnnouncement()
        {
            return View();
        }

        // 📌 CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnnouncement(Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                announcement.ContestId = null;
                announcement.PostedBy = null;
                announcement.PostDate = DateTime.Now;

                _context.Add(announcement);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(announcement);
        }

        // 📌 EDIT (GET)
        public async Task<IActionResult> EditAnnouncement(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);

            if (announcement == null)
                return NotFound();

            return View(announcement);
        }

        // 📌 EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAnnouncement(int id, Announcement announcement)
        {
            if (id != announcement.AnnouncementId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    announcement.ContestId = null;
                    announcement.PostedBy = null;

                    _context.Update(announcement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(announcement);
        }

        // 📌 DELETE (SIMPLE)
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);

            if (announcement == null)
                return NotFound();

            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}