using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApplication1.Controllers
{
    public class CommunityController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CommunityController(ApplicationDbContext context)
        {
            _context = context;
        }



        // hiển thị tất cả các bài viết 
        public IActionResult Index()
        {

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;

            // var email= HttpContext.Session.GetString(LoginController.SessionKeyEmail);
            // var username= HttpContext.Session.GetString(LoginController.SessionKeyUsername);

            var blog_community = _context.Blog.ToList();

            // Kiểm tra xem có dữ liệu trong TempData không
            if (TempData["SearchResults"] is string searchResultsJson)
            {
                // Deserialize dữ liệu từ JSON thành List<Blog>
                blog_community = JsonSerializer.Deserialize<List<Blog>>(searchResultsJson);

                // Bạn có thể xử lý searchResults ở đây nếu cần
                // Ví dụ: truyền nó đến View
                ViewData["SearchResults"] = blog_community;
            }

            blog_community?.Reverse();

            //-------------default--------------------
            var username = User.Identity?.Name;
            ViewBag.UserName = username;

            return View(blog_community);
        }


        // lấy dữ liệu bài viết cần xem và hiển thị bài viết
        [HttpGet("/Community/Blog/{postId}")]
        public async Task<IActionResult> Blog(int postId)
        {

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;


            // Tìm bài viết dựa trên ID, sử dụng phương thức async
            var post = await _context.Blog.FirstOrDefaultAsync(p => p.BlogID == postId);

            // Nếu không tìm thấy bài viết, trả về lỗi 404
            if (post == null)
            {
                return NotFound();
            }

            // Trả về dữ liệu bài viết dạng JSON
            return Json(post);

        }


        //tìm kiếm bài viết 
        [HttpPost]
        public async Task<IActionResult> SearchBlog(string BlogName)
        {
            string? BlogNameRequest = BlogName;
            ViewBag.search_blog = null;

            List<Blog> blog_communityQuery = new List<Blog>();

            if (BlogNameRequest != null)
            {

                var search_blog = await _context.Blog.Where(p => p.BlogName != null && p.BlogName.Contains(BlogNameRequest)).ToListAsync();
                blog_communityQuery = search_blog;


                ViewBag.search_blog = search_blog;
                TempData["SearchResults"] = JsonSerializer.Serialize(blog_communityQuery, new JsonSerializerOptions());
            }

            // return Json(new { success = true, ViewBag.search_blog });
            return RedirectToAction("Index");
            // return RedirectToAction("Index", "Community");
        }



        // đăng bài viết mới
        [HttpPost]
        public async Task<IActionResult> Index(string titleBlog, string contentBlog)
        {

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;


            // Tạo một đối tượng Blog mới
            Blog blog = new()
            {
                BlogName = titleBlog,
                BlogContent = contentBlog,
                CreatedDate = DateTime.Now,
                UserIDPost = Convert.ToInt32(userIdClaim),
            };

            // Thêm bài viết vào CSDL
            _context.Blog.Add(blog);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }





        // tăng view khi người dùng click vào bài viết để xem
        [HttpPost("/Community/increaseView/{data_post_id}")]
        public async Task<IActionResult> IncreaseViewOfBlog(int data_post_id, [FromBody] Blog blogViewUpdate)
        {

            try
            {
                // Xử lý dữ liệu nhận được
                if (blogViewUpdate == null)
                {
                    return BadRequest("Invalid data.");
                }

                // var blog = await _context.Blog.FindAsync(blogViewUpdate.BlogID);
                var blog = await _context.Blog.FindAsync(data_post_id);

                if (blog == null)
                {
                    return NotFound("Blog not found.");

                }

                blog.BlogViews += 1;


                // Cập nhật số lượt xem cho Blog
                _context.Blog.Update(blog);
                await _context.SaveChangesAsync();

                // Thực hiện cập nhật số lượt xem cho Blog
                // Bạn có thể gọi một service để cập nhật dữ liệu trong cơ sở dữ liệu

                // Giả sử thành công
                return Ok(new { message = "View count updated successfully!", newViewCount = blogViewUpdate.BlogViews });


            }
            catch (Exception ex)
            {
                // Ghi log lỗi (nếu cần)
                System.Diagnostics.Debug.WriteLine("---------<>" + ex.Message);
                return StatusCode(500, "Internal server error");
            }

        }

    }
}