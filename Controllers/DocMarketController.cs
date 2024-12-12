using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DocMarketController(ApplicationDbContext context) : Controller
    {

        private readonly ApplicationDbContext _context = context;

        public IActionResult Index()
        {
            if (User.Identity != null)
            {
                var username = User.Identity?.Name;
                ViewBag.UserName = username;

                string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
                if (userIdClaim == "")
                {
                    ViewBag.DiemTieuTaiFile = 0;
                }
                else
                {
                    var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
                    ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;
                }
            }


            var books = _context.Books.ToList();
            return View(books);
        }

    }
}