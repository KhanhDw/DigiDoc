using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{


    [Route("Coins")]
    public class CoinsController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public IActionResult Index()
        {

            var username = User.Identity?.Name;
            ViewBag.UserName = username;

            string userIdClaim = User.FindFirst("UserID")?.Value ?? "";
            ViewBag.UserId = userIdClaim;
            var Diem = _context.Users.FirstOrDefault(b => b.UserID == Convert.ToInt32(userIdClaim));
            ViewBag.DiemTieuTaiFile = Diem?.PointsDownloadFile;

            var lichSuNapVaHeThongNap = new LichSuNapvaHeThongNap
            {
                ListHeThongDiemNap = _context.HeThongDiemNap.ToList(),
                ListLichSuNapDiem = _context.LichSuNapDiem.Where(b => b.UserId == Convert.ToInt32(userIdClaim)).OrderByDescending(x => x.CreatedDate).ToList(),
                UserId = Convert.ToInt32(userIdClaim),
            };



            return View(lichSuNapVaHeThongNap);
        }



        [HttpPost("CapNhatHeThongNapDiem")]
        public async Task<IActionResult> Index([FromBody] SendRequestPost data)
        {
            try
            {

                if (ModelState.IsValid)
                {

                    if (data.Data == null)
                    {
                        return NotFound();
                    }


                    foreach (var item in data.Data)
                    {
                        var NapDiem = new HeThongDiemNap
                        {
                            SoDiem = item.SoDiem,
                            SoTien = item.SoTien
                        };

                        _context.HeThongDiemNap.Add(NapDiem);
                        await _context.SaveChangesAsync();
                    }

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


        [HttpPost("RemoveMucNap")]
        public async Task<IActionResult> RemoveMucNap([FromBody] SendDiemMucNap DiemNapID)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var NapDiem = await _context.HeThongDiemNap.FirstOrDefaultAsync(b => b.DiemNapID == DiemNapID.DiemNapID);

                    if (NapDiem == null) return NotFound();

                    _context.HeThongDiemNap.Remove(NapDiem);
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


    }
}