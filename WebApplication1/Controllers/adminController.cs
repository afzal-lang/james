using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication1.Controllers
{
    public class adminController : Controller
    {
        JamesthewContext db = new JamesthewContext();

        public IActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        [HttpGet]
        public IActionResult Createcategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Createcategory(Category cat, IFormFile file)
        {
            if (file != null)
            {
                // Unique image name (important)
                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                // Folder path
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                // Folder create agar exist na kare
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Full path
                string fullPath = Path.Combine(folderPath, imageName);

                // Save file
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // DB me path save
                cat.image = "/images/" + imageName;
            }

            db.Categories.Add(cat);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        

        [HttpGet]
        public IActionResult Deletecategory(int id)
        {

            var mydata = db.Categories.Find(id);



            return View(mydata);
        }
        [HttpPost]
        public IActionResult Deletecategory(Category cc)
        {
            if (ModelState.IsValid)
            {
                db.Remove(cc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Updatecategory(int id)
        {

            var mydata = db.Categories.Find(id);



            return View(mydata);
        }
        [HttpPost]
        public IActionResult Updatecategory(Category cc)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Update(cc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
    
        public IActionResult DetailCategory(int id)
        {


            ViewData["CatId"] = new SelectList(db.Categories, "CategoryId", "Name");
            var data = db.Categories.Find(id);
            return View(data);
        }

    }
}
