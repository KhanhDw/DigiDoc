using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication1.Models;
using System.Linq;
using System.Security.Claims;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace WebApplication1.Controllers
{
    public class CartController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // [HttpGet]
        public IActionResult Index()
        {
            //----------------------------------------tìm kiếm-------------------------------

            var username = User.Identity?.Name;
            ViewBag.UserName = username;

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;



            var searchName = Request.Query["SearchName"].ToString();
            // Kiểm tra nếu SearchName không có trong URL hoặc là null
            List<Book> booksList = [];

            if (!string.IsNullOrEmpty(searchName))
            {
                //Lọc sách theo tiêu chí tìm kiếm
                var booksQuery = _context.Books.
                     Where(b => b.Title != null && b.Title.Contains(searchName)).
                     Select(b => b.BookID).ToList();

                var itemSearch = _context.Cart.Where(c => booksQuery.Contains(c.Bookid) && c.Userid == Convert.ToInt32(userIdClaim))
                .Select(c => c.Bookid).ToList();

                var bookidincart = _context.Books
                .Where(b => itemSearch.Contains(b.BookID))
                .ToList();

                // Thực hiện truy vấn và chuyển đổi thành danh sách
                booksList = bookidincart;
                return View("Index", booksList);
            }

            //----------------------------------------Mặc định-------------------------------

            //string? userID = HttpContext.Session.GetString(LoginController.SessionKeyUserID);

            var emailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

            if (userIdClaim == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var itemCart = _context.Cart.Where(u => userIdClaim != null && u.Userid == Int32.Parse(userIdClaim)).Select(u => u.Bookid).ToList();

            if (itemCart == null)
            {
                return RedirectToAction("Index", "home");
            }

            var bookSave = _context.Books.Where(b => itemCart.Contains(b.BookID)).ToList();

            ViewBag.UserName = username;
            ViewBag.Userid = userIdClaim;


            return View("Index", bookSave);


        }





        [HttpPost("/Cart/AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                var cartItem = new Cart
                {
                    Bookid = request.BookId,
                    Userid = request.Userid,
                    CreatedDate = DateTime.Now,
                };

                // Debug.WriteLine("--->Debug:controller Cart = : " + cartItem.Bookid + " --- " + cartItem.Userid);

                _context.Cart.Add(cartItem);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPost("/Cart/RemoveToCart")]
        public async Task<IActionResult> RemoveToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                // Tìm item trong giỏ hàng dựa trên BookId và Userid
                var cartItem = await _context.Cart.FirstOrDefaultAsync(c => c.Bookid == request.BookId && c.Userid == request.Userid);

                if (cartItem == null)
                {
                    return NotFound(new { success = false, message = "Sách không có trong giỏ hàng." });
                }

                // Xóa item khỏi giỏ hàng
                _context.Cart.Remove(cartItem);
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