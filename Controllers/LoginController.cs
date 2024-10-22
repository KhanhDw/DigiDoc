using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models;
using System.Diagnostics;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        // để sử dụng được sql server trong controller cần gọi db vào và thêm contruster
        private readonly ApplicationDbContext _context;

        public const string SessionKeyUsername = "_Username";
        public const string SessionKeyEmail = "_Email";
        public const string SessionKeyPassword = "_Password";
        public const string SessionKeyUserID = "_UserID";

        public static List<Claim>? claims1;

        public LoginController(ApplicationDbContext context)
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
        public async Task<IActionResult> Index(string email, string password)
        {
            if (ModelState.IsValid)
            {
                //var user = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
                // Tìm người dùng theo địa chỉ email
                var user = _context.Users.SingleOrDefault(u => u.Email == email && u.PasswordHash == Helper.MD5Convert.GetMd5Hash(password));
                if (user != null)
                {
                    // Đăng nhập thành công, lấy username
                    string? username = user.Username;
                    string? userID = user.UserID.ToString();
                    string? roleUser = user.UserLevel.ToString();
                    if (username != null)
                    {
                        HttpContext.Session.SetString(SessionKeyUsername, username);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, username),
                            new Claim(ClaimTypes.Email, email),
                            new Claim(ClaimTypes.Role, roleUser),
                            new Claim("UserID", userID),
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        // var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true //xác thực được duy trì qua các phiên làm việc
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);


                        Debug.WriteLine("----roleUser--->" + roleUser);

                    }

                    HttpContext.Session.SetString(SessionKeyEmail, email);
                    HttpContext.Session.SetString(SessionKeyUserID, userID);
                    // HttpContext.Session.SetString(SessionKeyPassword, password);



                    // Đăng nhập thành công
                    return RedirectToAction("Index", "Docmarket");
                }
                else
                {
                    // Đăng nhập thất bại
                    ViewBag.ErrorMessage = "Sai Email hoặc mật khẩu!";
                }
            }

            // var ssemail = HttpContext.Session.GetString(SessionKeyPassword);

            return View();
        }
    }
}