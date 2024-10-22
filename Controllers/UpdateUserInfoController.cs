using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("UpdateUserInfo")]
    public class UpdateUserInfoController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;


        [HttpGet("{UserID}")]
        public IActionResult Index(int UserID)
        {
            var username = User.Identity?.Name;
            ViewBag.UserName = username;

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;

            var user = _context.Users.SingleOrDefault(u => u.UserID == UserID);

            if (user != null)
            {
                ViewBag.UserID_UpdateUser = user.UserID;
                ViewBag.UserName_UpdateUser = user.Username;
                ViewBag.Email_UpdateUser = user.Email;
                ViewBag.Role_UpdateUser = user.UserLevel;
            }

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> EditUser(int UserID, int UserLevel)
        {

            var userr = _context.Users.SingleOrDefault(u => u.UserID == UserID);

            if (userr == null)
            {
                return RedirectToAction("Index", "Home");
            }


            var existingUser = await _context.Users.FindAsync(UserID);
            if (existingUser == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin
            existingUser.UserLevel = UserLevel;
            // Cập nhật các thuộc tính khác...

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();


            // thông báo hiển thị 3 giây rồi ẩn -> chuyển qua giao diện quản ly

            return RedirectToAction("Index", "ManageUsers");
        }

    }
}