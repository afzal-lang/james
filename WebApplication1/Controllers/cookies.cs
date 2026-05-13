using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class cookies : Controller
    {
        JamesthewContext db = new JamesthewContext();

        // ===================== LOGIN =====================

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User u, string returnUrl = null)
        {
            string hashedPassword = HashPassword(u.Password);
            var res = db.Users.FirstOrDefault(x =>
                x.Email == u.Email &&
                x.Password == hashedPassword);
            if (res == null)
            {
                ViewBag.msg = "Wrong email or password";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Sid, res.UserID.ToString()),
        new Claim(ClaimTypes.Name, res.Name),
        new Claim(ClaimTypes.Email, res.Email),
        new Claim(ClaimTypes.Role, res.Role)
    };
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(
                "MyCookieAuth",
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddHours(2)
                });
            HttpContext.Session.SetString("UserId", res.UserID.ToString());
            HttpContext.Session.SetString("UserName", res.Name);
            HttpContext.Session.SetString("UserRole", res.SubscriptionType ?? "Free");
            if (res.Role == "Admin")
                return RedirectToAction("Index", "admin");

            // ✅ ReturnUrl hai to wahan jao, warna ShowRecipes
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "user");
        }
        // ===================== LOGOUT =====================

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // 1. Cookie khatam karne ke liye (SignOut)
            await HttpContext.SignOutAsync("MyCookieAuth");

            // 2. Agar session use ho raha hai toh usay saaf karne ke liye
            HttpContext.Session.Clear();

            // 3. Logout ke baad Announcements page par wapis bhejne ke liye
            // Agar aap Home page par bhejna chahte hain toh "Announcements", "user" ki jagah "Index", "Home" likh dein
            return RedirectToAction("Index", "user");
        }

        // ===================== REGISTER =====================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User u)
        {
            var existing = db.Users.FirstOrDefault(x => x.Email == u.Email);

            if (existing != null)
            {
                ViewBag.msg = "Email already registered";
                return View();
            }

            u.Role = "User";
            u.Password = HashPassword(u.Password);
            u.RegistrationDate = DateTime.Now;

            db.Users.Add(u);
            db.SaveChanges();

            return RedirectToAction("Login");
        }

        // ===================== HASH =====================

        private static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));

                return builder.ToString();
            }
        }
    }
}