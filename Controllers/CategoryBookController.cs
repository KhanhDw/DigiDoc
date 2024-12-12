using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CategoryBookController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;


        // [HttpGet(")]
        [Route("/CategoryBook/trang/{page?}")]

        public async Task<IActionResult> Index(int? page)
        {
            var username = User.Identity?.Name;
            ViewBag.UserName = username;

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;


            ViewBag.danhmuc = "Tất cả thể loại";
            // var books = _context.Books.ToList();


            try
            {
                // Lấy toàn bộ danh sách book
                var allBooks = await _context.Books
                                           .OrderBy(b => b.BookID)
                                           .ToListAsync();
                allBooks.Reverse();

                // Số items trên mỗi trang
                int pageSize = 20;
                // Trang hiện tại (mặc định là 1)
                int pageNumber = page ?? 1;

                // Chuyển đổi từ List sang PaginatedList
                var paginatedList = PaginatedList<Book>.Create(allBooks, pageNumber, pageSize);

                return View(paginatedList);
            }
            catch (Exception)
            {
                // Log exception
                return View("Error");
            }


            // return View(paginatedItems);
            // return View(books);
        }



        [Route("CategoryBook/{danhmuc}")]
        public IActionResult Index(string danhmuc)
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

            var books1 = _context.Books.ToList();

            if (danhmuc == "ban-hang-va-kinh-doanh")
            {
                ViewBag.danhmuc = "Bán hàng và kinh doanh";
                books1 = GetBookBySubject(ViewBag.danhmuc);
                return View(books1);
            }
            else if (danhmuc == "phat-trien-ban-than")
            {
                ViewBag.danhmuc = "Phát triển bản thân";
                books1 = GetBookBySubject(ViewBag.danhmuc);
                return View(books1);
            }
            else if (danhmuc == "IT-va-lap-trinh")
            {
                ViewBag.danhmuc = "IT và lập trình";
                books1 = GetBookBySubject(ViewBag.danhmuc);
                return View(books1);
            }
            else if (danhmuc == "ke-toan-thue-tai-chinh")
            {
                ViewBag.danhmuc = "Kế toán, Thuế và Tài chính";
                books1 = GetBookBySubject(ViewBag.danhmuc);
                return View(books1);
            }
            else if (danhmuc == "kien-truc-va-xay-dung")
            {
                ViewBag.danhmuc = "Kiến trúc và xây dựng";
                books1 = GetBookBySubject(ViewBag.danhmuc);
                return View(books1);
            }
            else if (danhmuc == "luat-phap-ly")
            {
                ViewBag.danhmuc = "Luật và Pháp lý";
                books1 = GetBookBySubject(ViewBag.danhmuc);
                return View(books1);
            }
            else if (danhmuc == "Marketing-truyen-thong")
            {
                ViewBag.danhmuc = "Marketing và Truyền thông";
                books1 = GetBookBySubject(ViewBag.danhmuc);
                return View(books1);
            }
            else if (danhmuc == "thiet-ke")
            {
                ViewBag.danhmuc = "Thiết kế";
                books1 = GetBookBySubject(ViewBag.danhmuc);
                return View(books1);
            }
            else if (danhmuc == "khac")
            {
                ViewBag.danhmuc = "Khác";
                books1 = GetBookBySubject(ViewBag.danhmuc);
                return View(books1);
            }

            try
            {
                // Lấy toàn bộ danh sách book
                var allBooks = books1;
                allBooks.Reverse();

                // Số items trên mỗi trang
                int pageSize = 6;
                // Trang hiện tại (mặc định là 1)
                int pageNumber = 1;

                // Chuyển đổi từ List sang PaginatedList
                var paginatedList = PaginatedList<Book>.Create(allBooks, pageNumber, pageSize);

                return View(paginatedList);
            }
            catch (Exception)
            {
                // Log exception
                return View("Error");
            }

            //return View(books1);
        }

        public List<Book> GetBookBySubject(string linhvuc)
        {
            var genreIds = _context.Genres.Where(g => g.TenTheLoai == linhvuc).Select(g => g.GenreID);
            var bookid = _context.GenresAndBooks.Where(bg => genreIds.Contains(bg.GenreID)).Select(bg => bg.BookID).ToList();
            var books = _context.Books.Where(b => bookid.Contains(b.BookID)).ToList();

            //var books = _context.Books.ToList();
            return books;
        }
    }
}