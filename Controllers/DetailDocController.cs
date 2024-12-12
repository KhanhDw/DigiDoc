using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.Diagnostics;
using static Azure.Core.HttpHeader;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class DetailDocController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;


        [Route("DetailDoc/{iddoc}")]
        public IActionResult Index(int iddoc)
        {
            //ViewBag.Title1111 = iddoc;
            int userIdClaimINT = 0;

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            if (userIdClaim == "")
            {
                ViewBag.DiemTieuTaiFile = 0;
            }
            else
            {
                var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
                ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;
            }

            if (!userIdClaim.IsNullOrEmpty())
            {
                userIdClaimINT = Int32.Parse(userIdClaim);
            }
            else
            {
                userIdClaimINT = -1;
            }
            // var books = _context.Books.ToList();

            // var bookShow = new Book();

            var sach = _context.Books.FirstOrDefault(s => s.BookID == iddoc);
            var idtheLoaiList = _context.GenresAndBooks.Where(tl => tl.BookID == iddoc).Select(tl => tl.GenreID).ToList();
            var theloai = _context.Genres.Where(g => idtheLoaiList.Contains(g.GenreID)).Select(g => g.TenTheLoai).ToList();
            var author = _context.Users.FirstOrDefault(a => sach != null && a.UserID == sach.IdUserUploadedFile);


            // tìm dữ liệu người dùng đã lưu sách (nếu chưa lưu sẽ trả về 0)
            var bookSave = _context.Cart.Where(c => c.Bookid == iddoc && c.Userid == userIdClaimINT).ToList();

            int checkSaveBook = 0;
            int idbookSaved = -1;

            if (bookSave.Count == 0)
            {
                idbookSaved = iddoc;
            }
            foreach (var id in bookSave)
            {
                idbookSaved = id.Bookid;
            }


            if (sach == null)
            {
                return RedirectToAction("index", "NotFound");
            }

            if (bookSave.Count == 1)
            {
                checkSaveBook = 1;
            }


            //----show comment

            var commentsWithUsers = from c in _context.Comments
                                    join u in _context.Users on c.UserId equals u.UserID
                                    where c.Bookid == iddoc
                                    select new CommentWithUserViewModel
                                    {
                                        // CommentID = c.CommentID,
                                        UserName = u.Username,
                                        Content = c.Content,
                                        CommentDate = c.CreatedDate,
                                        // UserLevel = u.UserLevel,
                                        // UserCreatedDate = u.CreatedDate
                                    };

            var listCommentsWithUsers = commentsWithUsers.ToList();


            // Tạo ViewModel và truyền dữ liệu vào
            var viewModel = new BookGenreViewModel
            {
                BookShow = sach,
                GenreShow = theloai,
                bokSave = checkSaveBook,
                idbookSaved = idbookSaved,
                userid = userIdClaimINT,
                userAuthor = author,
                DiemTaiFile = ViewBag.DiemTieuTaiFile,
                //commnet
                CommentsOfBook = listCommentsWithUsers,

            };


            var username = User.Identity?.Name;
            ViewBag.UserName = username;


            return View(viewModel);
        }



        [HttpGet("DetailDoc/download/{iddoc}")]
        public async Task<IActionResult> DownloadPdf(int iddoc)
        {

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            if (userIdClaim == "")
            {
                ViewBag.DiemTieuTaiFile = 0;

            }
            else
            {
                var sach = await _context.Books.FirstOrDefaultAsync(b => b.BookID == iddoc);
                var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
                ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;



                // System.Diagnostics.Debug.WriteLine("--->>>>>--->>--->" + Diem?.UserID);

                if (sach != null && Diem != null)
                {

                    if (Diem.PointsDownloadFile < Convert.ToInt32(sach.Price))
                    {
                        // return RedirectToAction("ChanTaiFile_KhongDuDiem");
                        // return RedirectToAction("Index", new { iddoc = iddoc });
                    }
                    else
                    {
                        // System.Diagnostics.Debug.WriteLine("-gia->>>>>->>->" + Convert.ToInt32(sach.Price));
                        sach.DownloadFile += 1;
                        Diem.PointsDownloadFile -= Convert.ToInt32(sach.Price);

                        ViewBag.DiemTieuTaiFile = Diem.PointsDownloadFile;

                        _context.Books.Update(sach);
                        _context.Users.Update(Diem);
                        await _context.SaveChangesAsync();


                        /// <summary>
                        /// file download
                        /// </summary>
                        string? filePath = "";

                        // Đường dẫn tới file PDF trên hệ thống
                        if (sach?.FileDoc != null)
                        {
                            filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/storage/filedoc/{sach?.FileDoc}");
                        }

                        var fileBytes = System.IO.File.ReadAllBytes(filePath);

                        if (!System.IO.File.Exists(filePath))
                        {
                            return NotFound(); // Trả về 404 nếu file không tồn tại
                        }

                        var fileBytesName = System.IO.File.ReadAllBytes(filePath);

                        // Đổi tên file khi tải xuống
                        string newFileNameChanged = sach?.Title + ".pdf"; // Thay đổi tên file tại đây


                        // Trả về file để tải xuống
                        return File(fileBytesName, "application/pdf", newFileNameChanged);
                    }
                }
            }
            return NotFound();

        }


        [HttpGet("/DetailDoc/download/KhongDuDiem")]
        public IActionResult KhongDuDiem()
        {
            // Trả về thông báo từ server

            return Json(new { message = "Không đủ điểm!" });
        }


        [HttpPost("/PostComment")]
        [ValidateAntiForgeryToken]// chỉ chấp nhận dữ liệu hợp lệ
        public async Task<IActionResult> PostComment()
        {


            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            if (userIdClaim == "")
            {
                ViewBag.DiemTieuTaiFile = 0;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
                ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;

                // dùng request để lấy dữ liệu từ form
                string? BookIDComment = Request.Form["BookIDComment"];
                string? contentOfCommentFromUser = Request.Form["contentOfCommentFromUser"];



                if (BookIDComment != null && contentOfCommentFromUser != null)
                {
                    var newComment = new Comment
                    {
                        Bookid = Int32.Parse(BookIDComment),
                        Content = contentOfCommentFromUser,
                        UserId = Int32.Parse(User.FindFirst("UserID")?.Value ?? ""),
                        CreatedDate = DateTime.Now,
                    };

                    // Thêm comment vào database
                    _context.Comments.Add(newComment);
                    await _context.SaveChangesAsync(); // Lưu thay đổi vào database

                    return Redirect(Request.Headers.Referer.ToString());// reload page current

                }
                return View();
            }
        }

    }
}