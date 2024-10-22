using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApplication1.Models;
using System.Security.Claims;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApplication1.Controllers
{
    public class AccountController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public IActionResult PersonalInformation()
        {
            var username = User.Identity?.Name;
            ViewBag.UserName = username;

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;

            return View();
        }
        public IActionResult ManageUploadedDocuments()
        {

            var username = User.Identity?.Name;
            ViewBag.UserName = username;

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;


            //------------------tìm kiếm--------------------------------
            var searchName = Request.Query["searchManageUploadedDocument"].ToString();

            if (!string.IsNullOrEmpty(searchName))
            {
                var bookOfUser = _context.Books.Where(b => b.IdUserUploadedFile == Convert.ToInt32(userIdClaim) && b.Title == searchName || b.Author == searchName).ToList();
                // Thực hiện truy vấn và chuyển đổi thành danh sách
                return View(bookOfUser);
            }

            //------------------mặc định -------------------

            var bookUploaded = _context.Books.Where(c => c.IdUserUploadedFile == Convert.ToInt32(userIdClaim)).ToList();

            bookUploaded.Reverse();


            return View(bookUploaded);
        }

        [HttpPost("/Account/ManageUploadedDocuments/RemoveBookinAccount")]
        // [ActionName("/Account/ManageUploadedDocuments/RemoveBookinAccount")]
        public async Task<IActionResult> RemoveBookinAccount([FromBody] AccountSendRequest data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var book = await _context.Books.FirstOrDefaultAsync(b => b.BookID == data.BookID_remove);
                    var genre = await _context.GenresAndBooks.Where(b => b.BookID == data.BookID_remove).ToListAsync();
                    var bookInCart = await _context.Cart.FirstOrDefaultAsync(b => b.Bookid == data.BookID_remove);
                    var CommentsInbook = await _context.Comments.Where(b => b.Bookid == data.BookID_remove).ToListAsync();


                    if (book != null && genre != null)
                    {

                        if (bookInCart != null)
                        {
                            _context.Comments.RemoveRange(CommentsInbook);
                            await _context.SaveChangesAsync();
                        }

                        if (bookInCart != null)
                        {
                            _context.Cart.Remove(bookInCart);
                            await _context.SaveChangesAsync();
                        }
                        _context.GenresAndBooks.RemoveRange(genre);
                        await _context.SaveChangesAsync();


                        _context.Books.Remove(book);
                        await _context.SaveChangesAsync();

                        return Ok(new { success = true, message = "thanh cong11999" });

                    }
                    else
                    {

                        return Ok(new { success = true, message = "thanh cong11" });
                    }
                }
                else
                {
                    return BadRequest(new { success = false, message = "ex.Message" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        public IActionResult SavedDocument()
        {
            var username = User.Identity?.Name;
            ViewBag.UserName = username;

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;


            // var bookSaved = _context.Carts.Where(c => c.Userid == )

            return View();
        }
        public IActionResult Feedback()
        {
            var username = User.Identity?.Name;
            ViewBag.UserName = username;

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;

            return View();
        }



        public IActionResult BaiVietDaDang()
        {
            var username = User.Identity?.Name;
            ViewBag.UserName = username;

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;

            var blog_community = _context.Blog.Where(b => b.UserIDPost == Convert.ToInt32(userIdClaim)).ToList();

            // var blog_community = _context.Blog.ToList();

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


            return View(blog_community);
        }



        [HttpGet("/account/BaiVietDaDang/Docbaiviet/{idblog}")]
        public async Task<IActionResult> DocBaiViet(int idblog)
        {
            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;


            // Tìm bài viết dựa trên ID, sử dụng phương thức async
            var post = await _context.Blog.Where(p => p.BlogID == idblog).ToListAsync();

            // Nếu không tìm thấy bài viết, trả về lỗi 404
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost("/account/BaiVietDaDang/DeleteBlog/{idblog}")]
        public async Task<IActionResult> DeleteBlog(int idblog)
        {
            // Tìm bài viết dựa trên ID, sử dụng phương thức async
            var post = await _context.Blog.FirstOrDefaultAsync(p => p.BlogID == idblog);

            // Nếu không tìm thấy bài viết, trả về lỗi 404
            if (post == null)
            {
                return NotFound();
            }

            // Xóa item khỏi giỏ hàng
            _context.Blog.Remove(post);
            await _context.SaveChangesAsync();


            return Ok(new { success = true });
        }


        [HttpGet("/account/BaiVietDaDang/ChinhSuaBaiViet/{idblog}")]
        public async Task<IActionResult> ChinhSuaBaiViet(int idblog)
        {
            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;


            // Tìm bài viết dựa trên ID, sử dụng phương thức async
            var post = await _context.Blog.Where(p => p.BlogID == idblog).ToListAsync();

            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost("/account/BaiVietDaDang/XacNhanChinhSuaBaiViet")]
        public async Task<IActionResult> XacNhanChinhSuaBaiViet([FromBody] Blog blog)
        {

            // Tìm bài viết dựa trên ID, sử dụng phương thức async
            var post = await _context.Blog.FirstOrDefaultAsync(p => p.BlogID == blog.BlogID);

            // Nếu không tìm thấy bài viết, trả về lỗi 404
            if (post == null)
            {
                return NotFound();
            }


            post.BlogName = blog.BlogName;
            post.BlogContent = blog.BlogContent;


            _context.Blog.Update(post);
            await _context.SaveChangesAsync();


            return RedirectToAction("BaiVietDaDang");
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}