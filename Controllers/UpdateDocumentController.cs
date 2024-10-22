using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting; // Đảm bảo thêm namespace này

namespace WebApplication1.Controllers
{
    public class UpdateDocumentController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor
        public UpdateDocumentController(ApplicationDbContext context)
        {
            _context = context;
        }


        [Route("UpdateDocument/Edit/{iddoc}")]
        public IActionResult Index(int iddoc)
        {

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;


            var sach = _context.Books.FirstOrDefault(s => s.BookID == iddoc);

            //IQueryable giúp truy xuất dữ liệu nhanh hơn dùng list hay FirstOrDefault
            var genres = _context.Genres.ToList();
            var genresAndBooks = _context.GenresAndBooks.ToList();


            var sachModel = new UpdateBookModel
            {
                BookEdit = sach,
                GenreLoadData = genres,
                GenreSelectedByBook = genresAndBooks
            };



            var username = User.Identity?.Name;
            ViewBag.UserName = username;

            return View(sachModel);
            // return RedirectToAction("Index", "Login");

        }

        // [HttpPost]
        // public async Task<IActionResult> EditBook(int iddoc)
        // {
        //     string? BookID = Request.Form["BookID"];
        //     string? Title = Request.Form["Title"];
        //     string? Author = Request.Form["Author"];
        //     string? Price = Request.Form["Price"];
        //     string? ImgThumbnail = Request.Form["ImgThumbnail"];
        //     string? FileDoc = Request.Form["FileDoc"];
        //     string? CreatedDate = Request.Form["CreatedDate"];
        //     // Tìm đối tượng sách theo ID
        //     var book = await _context.Books.FindAsync(BookID);
        //     if (book == null)
        //     {
        //         // Trả về NotFound nếu không tìm thấy sách
        //         return NotFound();
        //     }

        //     if (Price != null && CreatedDate != null)
        //     {
        //         book.Title = Title;
        //         book.Author = Author;
        //         book.Price = decimal.Parse(Price);
        //         book.ImgThumbnail = ImgThumbnail;
        //         book.FileDoc = FileDoc;
        //         book.CreatedDate = DateTime.Parse(CreatedDate);
        //     }

        //     // Đánh dấu thực thể này là đã thay đổi
        //     _context.Books.Update(book);

        //     // Lưu các thay đổi vào database
        //     await _context.SaveChangesAsync();

        //     var username = User.Identity?.Name;
        //     ViewBag.UserName = username;

        //     // Chuyển hướng về trang Index sau khi cập nhật thành công
        //     return RedirectToAction(nameof(Index));

        // }


        // public async Task<IActionResult> UpdateFileBookNew([FromBody] AddToCartRequest request, IFormFile file)  UpdateBookModel fileDoc)
        [HttpPost]
        public async Task<IActionResult> UpdateFileBookNew(int Bookid, string title, string author, string Kindofbook, string price, IFormFile FileDocNew_UserUpload, IFormFile imgThumbnail)
        {

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";

            try
            {
                var book = await _context.Books.FirstOrDefaultAsync(c => c.BookID == Bookid);
                var kindofbook = await _context.GenresAndBooks.FirstOrDefaultAsync(c => c.BookID == Bookid);

                if (book == null || kindofbook == null)
                {
                    // return NotFound();
                    return NotFound(new { success = false, message = "Sách không tìm thấy." });
                }

                // Console.WriteLine("--------update---doc---------->>> : " + FileDocNew_UserUpload);



                if (book != null && kindofbook != null)
                {



                    book.Title = title;
                    book.Author = author;

                    string priceString = price.Replace(",", "");
                    // Chuyển đổi giá trị price một cách an toàn
                    var priceValue = decimal.Parse(priceString, System.Globalization.CultureInfo.InvariantCulture);
                    // Cắt bỏ phần thập phân nếu cần (đã có scale trong cơ sở dữ liệu xử lý phần thập phân)
                    book.Price = Math.Truncate(priceValue);

                    if (FileDocNew_UserUpload != null && FileDocNew_UserUpload.Length > 0
                        && imgThumbnail != null && imgThumbnail.Length > 0)
                    {

                        // cập nhật lưu dữ liệu cho tài liệu
                        string namePicFile = DateTime.Now.ToString("ddMMyyyyHHmmss") + imgThumbnail.FileName;
                        string nameFilePDF = DateTime.Now.ToString("ddMMyyyyHHmmss") + FileDocNew_UserUpload.FileName;

                        book.FileDoc = nameFilePDF;
                        book.ImgThumbnail = namePicFile;

                        // Lấy tên tệp
                        var fileName = Path.GetFileName(nameFilePDF);
                        var fileNameImg = Path.GetFileName(namePicFile);
                        // Tạo đường dẫn lưu tệp
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "storage", "filedoc", fileName);
                        var filePathImg = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "storage", "thumnail", fileNameImg);
                        // Lưu tệp vào thư mục đã chỉ định
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await FileDocNew_UserUpload.CopyToAsync(stream);
                        }

                        using (var stream = new FileStream(filePathImg, FileMode.Create))
                        {
                            await imgThumbnail.CopyToAsync(stream);
                        }
                    }
                    else if (FileDocNew_UserUpload != null && FileDocNew_UserUpload.Length > 0
                        || imgThumbnail != null && imgThumbnail.Length > 0)
                    {
                        if (FileDocNew_UserUpload != null && FileDocNew_UserUpload.Length > 0)
                        {
                            // cập nhật lưu dữ liệu cho tài liệu
                            string nameFilePDF = DateTime.Now.ToString("ddMMyyyyHHmmss") + FileDocNew_UserUpload.FileName;


                            book.FileDoc = nameFilePDF;

                            // Lấy tên tệp
                            var fileName = Path.GetFileName(nameFilePDF);
                            // Tạo đường dẫn lưu tệp
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "storage", "filedoc", fileName);
                            // Lưu tệp vào thư mục đã chỉ định
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await FileDocNew_UserUpload.CopyToAsync(stream);
                            }
                        }
                        if (imgThumbnail != null && imgThumbnail.Length > 0)
                        {
                            string namePicFile = DateTime.Now.ToString("ddMMyyyyHHmmss") + imgThumbnail.FileName;

                            book.ImgThumbnail = namePicFile;
                            // Lấy tên tệp
                            var fileNameImg = Path.GetFileName(namePicFile);
                            // Tạo đường dẫn lưu tệp
                            var filePathImg = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "storage", "thumnail", fileNameImg);
                            // Lưu tệp vào thư mục đã chỉ định
                            using (var stream = new FileStream(filePathImg, FileMode.Create))
                            {
                                await imgThumbnail.CopyToAsync(stream);
                            }
                        }
                    }
                    book.CreatedDate = DateTime.Now;

                    // cập nhật lưu dữ liệu cho thể loại và sách

                    kindofbook.GenreID = int.Parse(Kindofbook);

                    //chạy cập nhật
                    _context.GenresAndBooks.Update(kindofbook);
                    _context.Books.Update(book);
                    await _context.SaveChangesAsync();
                }

                //return Ok(new { success = true });
                return RedirectToAction("Index", "DocumentsManage");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }




    }
}