using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string usernameregister, string emailregister, string passwordregister, string repasswordregister)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra mật khẩu nhập lại
                if (passwordregister == repasswordregister)
                {
                    string md5password = Helper.MD5Convert.GetMd5Hash(repasswordregister);

                    // khởi tạo user mới khi người dùng đăng ký
                    var user = new User
                    {
                        Username = usernameregister,
                        Email = emailregister,
                        PasswordHash = md5password, // Cần mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
                        UserLevel = 1,
                        AnhBia = "AnhBiaMacDinh.png",
                        AnhDaiDien = "AnhDaiDienMacDinh.png"
                    };

                    _context.Users.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Login"); // Chuyển hướng đến trang đăng nhập sau khi đăng ký thành công
                }
                else
                {
                    ViewBag.ErrorMessage = "Mật khẩu nhập lại không khớp.";
                }
            }

            return View();
        }
    }
}