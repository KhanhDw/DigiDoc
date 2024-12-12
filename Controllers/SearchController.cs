using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;


namespace WebApplication1.Controllers
{
    public class SearchController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;


        public IActionResult Index(string SearchName)
        {
            var username = User.Identity?.Name;
            ViewBag.UserName = username;
            ViewBag.DiemTieuTaiFile = 0;
            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            if (userIdClaim != "")
            {
                var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
                ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;
            }

            // Lấy danh sách sách từ cơ sở dữ liệu
            var booksQuery = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(SearchName))
            {
                // Lọc sách theo tiêu chí tìm kiếm
                booksQuery = booksQuery.Where(b => (b.Title != null && b.Title.Contains(SearchName)) ||
                                                (b.Author != null && b.Author.Contains(SearchName)));
            }

            // Thực hiện truy vấn và chuyển đổi thành danh sách
            var booksList = booksQuery.ToList();

            // Tạo view model và gán kết quả tìm kiếm
            var viewModel = new SearchViewModel
            {
                SearchTerm = SearchName,
                ResultsBook = booksList
            };


            return View(viewModel);
        }
    }
}