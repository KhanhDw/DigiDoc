using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace WebApplication1.Controllers
{

    // [Route("ManageUsers")]
    public class ManageUsersController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;


        public IActionResult Index()
        {
            var username = User.Identity?.Name;
            ViewBag.UserName = username;

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;


            var searchName = Request.Query["SearchUserNameOrEmail"].ToString();


            if (!string.IsNullOrEmpty(searchName))
            {
                //Lọc sách theo tiêu chí tìm kiếm
                var searchItem = _context.Users.Where(u => u.Username != null && u.Username.Contains(searchName) || u.Email != null && u.Email.Contains(searchName)).ToList();

                return View(searchItem);
            }



            //-------------------mặc định-----------------------------
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            IEnumerable<User> users = [];


            // Chuyển đổi các vai trò từ chuỗi sang số
            var numericRoles = roles.Select(role => int.Parse(role)).ToList();
            int roleToCompare = 3; // Số để so sánh
            bool Admin3 = numericRoles.Contains(roleToCompare);
            if (Admin3)
            {
                users = _context.Users.Where(u => u.UserLevel != 3);

                if (users != null)
                {
                    // Lấy dữ liệu thành công, 
                    return View(users);
                }
                else
                {
                    return View();

                }
            }
            else
            {
                var user = _context.Users.Where(u => u.UserLevel == 1).ToList();

                if (user != null)
                {
                    // Lấy dữ liệu thành công, 
                    return View(user);
                }
                else
                {
                    return View();

                }
            }

        }

        [HttpPost]
        public IActionResult Index(int newRole)
        {
            // Thay đổi giá trị roleChange, ví dụ: gán giá trị mới là 2

            // Cập nhật giá trị roleChange dựa trên giá trị từ form
            ViewBag.RoleChange = newRole;

            var user = _context.Users.Where(u => u.UserLevel == newRole).ToList();

            if (user != null)
            {
                // Lấy dữ liệu thành công, 
                return View(user);
            }
            else
            {
                return View();

            }
        }


        [HttpPost("/ManageUsers/RemoveUser")]
        public async Task<IActionResult> RemoveUser([FromBody] AddToCartRequest userId)  // AddToCartRequest mượn model để truyền dữ liệu từ js đến controller
        {
            try
            {
                var userItem = await _context.Users.FirstOrDefaultAsync(c => c.UserID == userId.Userid);


                if (userItem == null)
                {
                    return NotFound(new { success = false, message = "Không11 tìm thấy người dùng." });
                }

                // Xóa item khỏi giỏ hàng
                _context.Users.Remove(userItem);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}