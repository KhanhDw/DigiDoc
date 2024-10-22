using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class ProfileUserController : Controller
    {

        private readonly ApplicationDbContext _context;
        public ProfileUserController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("ProfileUser/{id}")]
        public IActionResult Index(int id)
        {
            var username = User.Identity?.Name;
            ViewBag.UserName = username;

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;

            var getProfileUser = _context.Users.FirstOrDefault(u => u.UserID == id);

            return View(getProfileUser);
        }

    }
}