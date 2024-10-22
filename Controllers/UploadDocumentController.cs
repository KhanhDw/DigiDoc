using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class UploadDocumentController : Controller
    {

        private readonly ApplicationDbContext _context;

        // Constructor
        public UploadDocumentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            var genres = _context.Genres.ToList();
            var genresAndBooks = _context.GenresAndBooks.ToList();


            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;

            var sachModel = new UpdateBookModel
            {
                GenreLoadData = genres,
                GenreSelectedByBook = genresAndBooks
            };

            var username = User.Identity?.Name;
            ViewBag.UserName = username;

            return View(sachModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string title, string author, string price, string kindofbook, IFormFile imgThumbnail, IFormFile FileDoc)
        {

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";

            try
            {
                string priceString = price.Replace(",", "");
                // Chuyển đổi giá trị price một cách an toàn
                var priceValue = decimal.Parse(priceString, System.Globalization.CultureInfo.InvariantCulture);
                // Cắt bỏ phần thập phân nếu cần (đã có scale trong cơ sở dữ liệu xử lý phần thập phân)

                string namePicFile = DateTime.Now.ToString("ddMMyyyyHHmmss") + imgThumbnail.FileName;
                string nameFilePDF = DateTime.Now.ToString("ddMMyyyyHHmmss") + FileDoc.FileName;


                var sach = new Book()
                {
                    Title = title,
                    Author = author,
                    Price = Math.Truncate(priceValue),
                    // ImgThumbnail = imgThumbnail.FileName,
                    // FileDoc = FileDoc.FileName,
                    ImgThumbnail = namePicFile,
                    FileDoc = nameFilePDF,
                    CreatedDate = DateTime.Now,
                    IdUserUploadedFile = Convert.ToInt32(userIdClaim),
                };

                // Đánh dấu thực thể này là đã thay đổi
                _context.Books.Add(sach);
                await _context.SaveChangesAsync();

                var genreAndBook = new GenresAndBook()
                {
                    GenreID = int.Parse(kindofbook),
                    BookID = sach.BookID
                };

                _context.GenresAndBooks.Add(genreAndBook);

                // Lưu các thay đổi vào database
                await _context.SaveChangesAsync();


                if (FileDoc != null && imgThumbnail != null)
                {
                    // Lấy tên tệp
                    var fileName = Path.GetFileName(nameFilePDF);
                    var fileNameImg = Path.GetFileName(namePicFile);
                    // Tạo đường dẫn lưu tệp
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "storage", "filedoc", fileName);
                    var filePathImg = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "storage", "thumnail", fileNameImg);
                    // Lưu tệp vào thư mục đã chỉ định
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await FileDoc.CopyToAsync(stream);
                    }

                    using (var stream = new FileStream(filePathImg, FileMode.Create))
                    {
                        await imgThumbnail.CopyToAsync(stream);
                    }
                }


                var username = User.Identity?.Name;
                ViewBag.UserName = username;

                return RedirectToAction("Index", "DocumentsManage");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

    }
}