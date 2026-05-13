using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ContestController : Controller
    {
        private readonly JamesthewContext db;

        public ContestController(JamesthewContext context)
        {
            db = context;
        }

        // ================= INDEX =================
        public IActionResult Index()
        {
            var model = new ContestListViewModel
            {
                Contests = db.Contests
                    .Include(c => c.ContestSubmissions)
                    .ToList()
            };

            return View(model);
        }

        // ================= CREATE =================
        [HttpGet]
        [Route("admin/Contest/Create")]
        public IActionResult Create()
        {
            // Professional Layout set karne ke liye path lazmi dein
            return View("~/Views/Contest/Create.cshtml");
        }

        [HttpPost]
        [Route("admin/Contest/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contest model)
        {
            if (ModelState.IsValid)
            {
                db.Contests.Add(model);
                await db.SaveChangesAsync();
                return RedirectToAction("Index"); // Save hone ke baad seedha cards par jao
            }
            return View("~/Views/Contest/Create.cshtml", model);
        }

        // ================= EDIT =================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Console.WriteLine($"Edit called with id: {id}"); // ← add karo

            var contest = db.Contests.Find(id);
            if (contest == null)
            {
                Console.WriteLine("Contest not found!");
                return NotFound();
            }

            return View(contest);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Contest model)
        {
            var data = db.Contests.Find(model.ContestId);

            if (data == null)
                return NotFound();

            data.Title = model.Title;
            data.Description = model.Description;
            data.StartDate = model.StartDate;
            data.EndDate = model.EndDate;

            // ← PostedByNavigation mat chhuo, sirf primitive fields update karo

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // ================= DETAILS =================
        [HttpGet]
        public IActionResult Details(int id)
        {
            var contest = db.Contests
                .Include(c => c.ContestSubmissions)
                .FirstOrDefault(c => c.ContestId == id);

            if (contest == null)
                return NotFound();

            return View(contest);
        }

        // ================= DELETE (CONFIRM PAGE) =================
        [HttpGet]
        public IActionResult DeleteConfirmed(int id)
        {
            var contest = db.Contests.Find(id);

            if (contest == null)
                return NotFound();

            return View(contest);
        }

        // ================= DELETE (POST ACTION) =================
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmedPost(int id)
        {
            var data = db.Contests.Find(id);

            if (data != null)
            {
                db.Contests.Remove(data);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        // ================= APPLY =================
        [HttpGet]
        [Route("user/Apply")] // Sirf ye ek route rehne dein
        public IActionResult Apply()
        {
            var contests = db.Contests.ToList();
            return View("~/Views/user/Apply.cshtml", contests);
        }
        [HttpPost]
            public IActionResult Apply(int contestId)
            {
                var userIdStr = HttpContext.Session.GetString("UserId");
                if (!int.TryParse(userIdStr, out int userId))
                    return Json(new { success = false, message = "Login required" });

                var existing = db.ContestSubmissions
                    .FirstOrDefault(s => s.ContestId == contestId && s.UserId == userId);

                if (existing != null)
                    return Json(new { success = false, message = "Aap pehle se apply kar chuke hain!" });

                var submission = new ContestSubmission
                {
                    ContestId = contestId,
                    UserId = userId,
                    SubmissionDate = DateTime.Now
                };

                db.ContestSubmissions.Add(submission);
                db.SaveChanges();

                return Json(new { success = true });
            }
        }
    }
