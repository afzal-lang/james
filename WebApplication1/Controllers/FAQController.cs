using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class FAQController : Controller
    {
        JamesthewContext db = new JamesthewContext();

        public IActionResult Faqs()
        {
            var data = db.Faqs.ToList();
            return View("Faqs", data);
        }

        [HttpGet]
        public IActionResult CreateFaq()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateFaq(Faq f)
        {
            if (ModelState.IsValid)
            {
                db.Faqs.Add(f);
                db.SaveChanges();
                return RedirectToAction("Faqs");
            }
            return View(f);
        }

        [HttpGet]
        public IActionResult EditFaq(int id)
        {
            var row = db.Faqs.Find(id);
            return View(row);
        }

        [HttpPost]
        public IActionResult EditFaq(Faq f)
        {
            if (ModelState.IsValid)
            {
                db.Entry(f).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Faqs");
            }
            return View(f);
        }


        public IActionResult DeleteFaq(int id)
        {
            var row = db.Faqs.Find(id);
            db.Faqs.Remove(row);
            db.SaveChanges();
            return RedirectToAction("Faqs");
        }
    }
}