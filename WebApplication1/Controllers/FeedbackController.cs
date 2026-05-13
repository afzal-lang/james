using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.Net.Mail;
using System.Net;

namespace WebApplication1.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly JamesthewContext _db;

        public FeedbackController(JamesthewContext db)
        {
            _db = db;
        }

        // ===== USER: Feedback Form =====
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _db.Categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(string name, string message, int rating)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "cookies");

            int userId = int.Parse(userIdStr);

            var feedback = new Feedback
            {
                UserId = userId,
                Message = message,
                Rating = rating,
                FeedbackDate = DateTime.Now
            };

            _db.Feedbacks.Add(feedback);
            _db.SaveChanges();

            // Email to admin
            SendEmailToAdmin(name, message, rating);

            TempData["Success"] = "Feedback submitted successfully!";
            return RedirectToAction("Create");
        }

        private void SendEmailToAdmin(string name, string message, int rating)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(
                        "abdulhadikhann897@gmail.com",   // apna email
                        "kjja buag jgwg sole"        // Gmail app password
                    ),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("your_email@gmail.com"),
                    Subject = "New Feedback - James Kitchen",
                    Body = $@"
                        <h2>New Feedback Received!</h2>
<p><strong>Name:</strong> {name}</p>
                        <p><strong>Rating:</strong> {rating}/5 ⭐</p>
                        <p><strong>Message:</strong> {message}</p>
                        <p><strong>Date:</strong> {DateTime.Now}</p>
                    ",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add("abdulhadikhann897@gmail.com"); // admin email
                smtpClient.Send(mailMessage);
            }
            catch { }
        }

        // ===== ADMIN: View All Feedbacks =====
        public IActionResult AdminFeedbacks()
        {
            var feedbacks = _db.Feedbacks
                .OrderByDescending(f => f.FeedbackDate)
                .ToList();

            ViewBag.Categories = _db.Categories.ToList();
            return View(feedbacks);
        }
    }
}