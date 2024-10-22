using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("BillManage")]
    public class BillManageController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public IActionResult Index()
        {
            var username = User.Identity?.Name;
            ViewBag.UserName = username;

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;


            var tongSoTienNap = _context.LichSuNapDiem
                .Where(nap => nap.TrangThai == "Xác nhận")
                .Sum(nap => nap.SotienNap);

            ViewBag.TongSoTienNap = tongSoTienNap;

            // var LichSuNap = _context.LichSuNapDiem.ToList();

            // var LichSuNapUserID = _context.LichSuNapDiem.Select(l => l.UserId).ToList();
            // var userNap = _context.Users.Where(u => LichSuNapUserID.Contains(u.UserID)).ToList();

            var query = from u in _context.Users
                        join l in _context.LichSuNapDiem
                        on u.UserID equals l.UserId
                        select new User_LichSuNapDiem
                        {
                            LichSuNapId = l.LichSuNapDiemID,
                            Username = u.Username,
                            Email = u.Email,
                            DiemNap = l.DiemNap,
                            SotienNap = l.SotienNap,
                            TrangThai = l.TrangThai,
                            CreatedDate = l.CreatedDate
                        };


            var model = query.OrderByDescending(x => x.CreatedDate).ToList();

            // var LichSuNapUser = new User_LichSuNapDiem();
            // LichSuNapUser.List_LichSuNapDiem = LichSuNap;
            // LichSuNapUser.List_User_LichSuNapDiem = userNap;


            return View(model);
        }



        [HttpPost("XacNhanThanhToan")]
        public async Task<IActionResult> XacNhanThanhToan([FromBody] SendRequestPost data)
        {
            try
            {

                if (ModelState.IsValid)
                {

                    var LichSuNap = _context.LichSuNapDiem.FirstOrDefault(b => b.LichSuNapDiemID == data.IdLichSuNap);

                    if (LichSuNap == null)
                    {
                        return NotFound();
                    }

                    LichSuNap.TrangThai = "Xác nhận";

                    var tangDiem = _context.Users.FirstOrDefault(b => b.UserID == LichSuNap.UserId);
                    if (tangDiem == null) return NotFound();
                    tangDiem.PointsDownloadFile += LichSuNap.DiemNap;

                    _context.LichSuNapDiem.Update(LichSuNap);
                    await _context.SaveChangesAsync();

                    return Ok(new { success = true });
                }

                return Ok(new { success = false });
            }

            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        [HttpPost("TuChoiThanhToan")]
        public async Task<IActionResult> TuChoiThanhToan([FromBody] SendRequestPost data)
        {
            try
            {

                if (ModelState.IsValid)
                {

                    var LichSuNap = _context.LichSuNapDiem.FirstOrDefault(b => b.LichSuNapDiemID == data.IdLichSuNap);

                    if (LichSuNap == null)
                    {
                        return NotFound();
                    }

                    LichSuNap.TrangThai = "Từ chối";

                    _context.LichSuNapDiem.Update(LichSuNap);
                    await _context.SaveChangesAsync();

                    return Ok(new { success = true });
                }

                return Ok(new { success = false });
            }

            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(new { success = false, message = ex.Message });
            }
        }



        [HttpPost("GuiYeuCauXacThucNap")]
        public async Task<IActionResult> GuiYeuCauXacThucNap([FromBody] SendRequestPostNapDiem data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LichSuNap = new LichSuNapDiem
                    {
                        UserId = data.UserId,
                        DiemNap = data.DiemNap,
                        SotienNap = data.SotienNap,
                        TrangThai = "Chờ xác nhận",
                        CreatedDate = DateTime.Now
                    };

                    _context.LichSuNapDiem.Add(LichSuNap);
                    await _context.SaveChangesAsync();

                    return Ok(new { success = true });
                }

                return Ok(new { success = false });
            }

            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }



        [HttpPost("XoaYeuCauXacThucNap")]
        public async Task<IActionResult> XoaYeuCauXacThucNap([FromBody] SendRequestPost data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var xoaLichSuNap = await _context.LichSuNapDiem.FirstOrDefaultAsync(u => u.LichSuNapDiemID == data.IdLichSuNap);

                    if (xoaLichSuNap == null) return NotFound();

                    _context.LichSuNapDiem.Remove(xoaLichSuNap);
                    await _context.SaveChangesAsync();

                    return Ok(new { success = true });
                }

                return Ok(new { success = false });
            }

            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}