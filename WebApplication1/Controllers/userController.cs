using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
namespace WebApplication1.Controllers

{

    public class userController : Controller

    {

        JamesthewContext _db = new JamesthewContext();





        public IActionResult Index()
        {
            var recipes = _db.Recipes
                .Include(r => r.Category)
                .OrderByDescending(r => r.UploadDate)
                .Take(6)
                .ToList();

            var tips = _db.Tips
                .OrderByDescending(t => t.TipId)
                .Take(3)
                .ToList();

            ViewBag.Categories = _db.Categories.ToList();
            ViewBag.Tips = tips;

            return View(recipes);
        }

        [AllowAnonymous]

        public IActionResult ShowRecipes()

        {

            bool isLoggedIn = HttpContext.Session.GetString("UserId") != null;

            ViewBag.IsLoggedIn = isLoggedIn;



            ViewBag.Categories = _db.Categories.ToList();



            if (isLoggedIn)

            {

                int userId = int.Parse(HttpContext.Session.GetString("UserId"));



                // ✅ ONLY SAVED (Favourite removed)

                var savedIds = _db.Saveds

                .Where(s => s.UserId == userId)

                .Select(s => s.RecipeId)

                .ToList();



                ViewBag.SavedIds = savedIds;



                var allRecipes = _db.Recipes

                .Include(r => r.Category)

                .ToList();



                return View(allRecipes);

            }

            else

            {

                ViewBag.SavedIds = new List<int>();



                var freeRecipes = _db.Recipes

                .Include(r => r.Category)

                .Where(r => r.IsFree == true)

                .ToList();

                ViewBag.Categories = _db.Categories.ToList();

                return View(freeRecipes);

            }

        }



        public async Task<IActionResult> Faqs()

        {

            var faqs = await _db.Faqs.ToListAsync();

            ViewBag.Categories = _db.Categories.ToList();

            return View(faqs);

        }



        public IActionResult Announcements()

        {

            // Yeh line database se categories nikaal kar ViewBag mein daal degi

            ViewBag.Categories = _db.Categories.ToList();



            // Aapka purana code

            var announcements = _db.Announcements.OrderByDescending(a => a.PostDate).ToList();

            return View(announcements);

        }



        public IActionResult Tips()

        {

            // Navbar ke dropdown ke liye categories load karna

            ViewBag.Categories = _db.Categories.ToList();



            List<Tip> tipsList;



            if (User.Identity.IsAuthenticated)

            {

                // Agar user LOGIN hai, toh saari tips dikhao (Free aur Paid dono)

                tipsList = _db.Tips.OrderByDescending(t => t.TipId).ToList();

            }

            else

            {

                // Agar user LOGIN NAHI hai, toh sirf FREE wali tips dikhao

                tipsList = _db.Tips.Where(t => t.IsFree == true).OrderByDescending(t => t.TipId).ToList();

            }

            ViewBag.Categories = _db.Categories.ToList();



            return View(tipsList);

        }



        [HttpGet]
        public IActionResult DetailRecipes(int id)
        {
            ViewBag.Categories = _db.Categories.ToList();

            var recipe = _db.Recipes
                .Include(r => r.Category)
                .FirstOrDefault(r => r.RecipeId == id);

            if (recipe == null) return NotFound();

            // Premium recipe check
            if (recipe.IsFree == false)
            {
                var userIdStr = HttpContext.Session.GetString("UserId");

                // Login nahi hai
                if (string.IsNullOrEmpty(userIdStr))
                    return RedirectToAction("Plans", "Subscription");

                int userId = int.Parse(userIdStr);

                // Active subscription check
                var hasSubscription = _db.Subscriptions
                    .Any(s => s.UserId == userId && s.Status == "Active");

                if (!hasSubscription)
                    return RedirectToAction("Plans", "Subscription");
            }

            ViewBag.Categories = _db.Categories.ToList();
            return View(recipe);
        }



        public IActionResult ShowContests()

        {

            var model = new ContestListViewModel

            {

                Contests = _db.Contests.ToList()

            };

            ViewBag.Categories = _db.Categories.ToList();

            return View("~/Views/user/ShowContests.cshtml", model);
        }

        [HttpGet]
        [Route("user/ContestSubmit/{id:int}")]
        public IActionResult ContestSubmit(int id)
        {
            try
            {
                var contest = _db.Contests.FirstOrDefault(c => c.ContestId == id);
                if (contest == null) return NotFound();
                ViewBag.Contest = contest;
                return View("~/Views/user/ContestSubmit.cshtml");
            }
            catch (Exception ex)
            {
                return Content("Error: " + ex.Message);
            }
        }
        [HttpPost]
        [Route("user/ContestSubmit/{id:int}")]
        public IActionResult ContestSubmit(int id, string type, string title, string description)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "cookies");
            int userId = int.Parse(userIdStr);
            var submission = new ContestSubmission
            {
                UserId = userId,
                ContestId = id,
                Type = type,
                Title = title,
                Description = description,
                SubmissionDate = DateTime.Now
            };
            _db.ContestSubmissions.Add(submission);
            _db.SaveChanges();
            return RedirectToAction("ShowContests", new { submitted = true });
        }

        public IActionResult ByCategory(int id)

        {

            // Navbar ke liye categories load karna zaroori hai

            ViewBag.Categories = _db.Categories.ToList();



            var recipes = _db.Recipes

            .Include(r => r.Category)

            .Where(r => r.CategoryId == id)

            .ToList();

            ViewBag.Categories = _db.Categories.ToList();

            return View(recipes);

        }
      

    }

}