using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth")]
    public class SubscriptionController : Controller
    {
        private readonly JamesthewContext db;

        public SubscriptionController(JamesthewContext context)
        {
            db = context;
        }

        // ─── Step 1: Plans Page ───────────────────────────────────
        [HttpGet]
        public IActionResult Plans()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.Sid)!.Value);

            var active = db.Subscriptions
                .FirstOrDefault(s => s.UserId == userId &&
                                     s.Status == "Active" &&
                                     s.EndDate > DateTime.Now);

            if (active != null)
                ViewBag.ActiveSub = active;

            return View();
        }

        // ─── Step 2: Payment Page ───────────────────────────────────
        [HttpGet]
        public IActionResult Payment(string plan)
        {
            if (plan != "Monthly" && plan != "Yearly")
                return RedirectToAction("Plans");

            ViewBag.Plan = plan;
            ViewBag.Amount = plan == "Monthly" ? 10 : 100;

            return View();
        }

        // ─── Step 3: Process Payment & Save ────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(string plan, string paymentMethod, string phoneNumber)
        {
            // ✅ Validation
            if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length != 11)
            {
                TempData["Error"] = "Sahih 11 digit phone number darain.";
                return RedirectToAction("Payment", new { plan });
            }

            if (plan != "Monthly" && plan != "Yearly")
                return RedirectToAction("Plans");

            int userId = int.Parse(User.FindFirst(ClaimTypes.Sid)!.Value);
            decimal amount = plan == "Monthly" ? 10m : 100m;

            DateTime startDate = DateTime.Now;
            DateTime endDate = (plan == "Monthly")
                                ? startDate.AddMonths(1)
                                : startDate.AddYears(1);

            // ✅ Old active subscriptions expire karo
            var existing = db.Subscriptions
                .Where(s => s.UserId == userId && s.Status == "Active")
                .ToList();

            foreach (var s in existing)
                s.Status = "Expired";

            // ✅ New subscription
            var sub = new Subscription
            {
                UserId = userId,
                PlanType = plan,
                Amount = amount,
                PaymentMethod = paymentMethod,
                PhoneNumber = phoneNumber,
                Status = "Active",
                StartDate = startDate,
                EndDate = endDate,
                CreatedAt = DateTime.Now
            };

            db.Subscriptions.Add(sub);
            await db.SaveChangesAsync();

            // ✅ TempData for Success Page
            TempData["Plan"] = plan;
            TempData["Amount"] = amount.ToString();
            TempData["PaymentMethod"] = paymentMethod;
            TempData["EndDate"] = endDate.ToString("dd MMMM yyyy");

            return RedirectToAction("Success");
        }

        // ─── Step 4: Success Page ───────────────────────────────────
        [HttpGet]
        public IActionResult Success()
        {
            // ❗ Agar direct URL hit ho to redirect
            if (TempData["Plan"] == null)
                return RedirectToAction("Plans");

            // 👇 IMPORTANT: TempData ko preserve karna (refresh ke liye)
            TempData.Keep();

            return View();
        }
    }
}