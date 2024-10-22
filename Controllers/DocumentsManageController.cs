using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DocumentsManageController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public IActionResult Index()
        {


            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;

            var booksWithGenres = from b in _context.Books
                                  join u in _context.Users on b.IdUserUploadedFile equals u.UserID
                                  join gb in _context.GenresAndBooks on b.BookID equals gb.BookID
                                  join g in _context.Genres on gb.GenreID equals g.GenreID
                                  select new BooksWithGenresModel
                                  {
                                      UserID = b.IdUserUploadedFile,
                                      BookID = b.BookID,
                                      Title = b.Title,
                                      Author = b.Author,
                                      Price = b.Price,
                                      FileDoc = b.FileDoc,
                                      ImgThumbnail = b.ImgThumbnail,
                                      CreatedDate = b.CreatedDate,
                                      TenTheLoai = g.TenTheLoai,
                                      UserNameUploadFile = u.Username // Thêm UserNameUploadFile

                                  };

            var listbooksWithGenres = booksWithGenres.ToList();


            var username = User.Identity?.Name;
            ViewBag.UserName = username;

            return View(listbooksWithGenres);
        }


        // [HttpPost("/SearchBookInManagePage")]
        [HttpPost]
        // [ActionName("SearchBookInManagePage")]
        [ValidateAntiForgeryToken]// chỉ chấp nhận dữ liệu hợp lệ
        public IActionResult Index(string? SearchBookInManagePage_content)
        {
            //show danh sach tài liệu 
            if (!string.IsNullOrEmpty(SearchBookInManagePage_content))
            {
                var booksWithGenres = from b in _context.Books
                                      join u in _context.Users on b.IdUserUploadedFile equals u.UserID
                                      join gb in _context.GenresAndBooks on b.BookID equals gb.BookID
                                      join g in _context.Genres on gb.GenreID equals g.GenreID
                                      where b.Title == SearchBookInManagePage_content || b.Author == SearchBookInManagePage_content
                                      select new BooksWithGenresModel
                                      {
                                          BookID = b.BookID,
                                          Title = b.Title,
                                          Author = b.Author,
                                          Price = b.Price,
                                          FileDoc = b.FileDoc,
                                          ImgThumbnail = b.ImgThumbnail,
                                          CreatedDate = b.CreatedDate,
                                          TenTheLoai = g.TenTheLoai,
                                          UserNameUploadFile = u.Username // Thêm UserNameUploadFile
                                      };

                var listbooksWithGenres = booksWithGenres.ToList();


                return View(listbooksWithGenres);
            }

            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        [ActionName("RemoveBook")]
        public async Task<IActionResult> RemoveBook(int idBookDel)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookID == idBookDel);
            var genre = _context.GenresAndBooks.Where(b => b.BookID == idBookDel).ToList();
            if (book != null && genre != null)
            {
                foreach (var item in genre)
                {
                    _context.GenresAndBooks.Remove(item);
                    await _context.SaveChangesAsync();
                }
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "DocumentsManage");

                // return Ok("success");

            }
            else
            {
                //return NotFound();
                return BadRequest("not success");
            }
        }
    }
}