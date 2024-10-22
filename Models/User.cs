using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class User /*: IdentityUser*/
    {
        public int UserID { get; set; }


        public string? Username { get; set; }

        public string? Email { get; set; }// không cần khai báo vì thuộc tính cha đã chứa 

        public string? PasswordHash { get; set; }// không cần khai báo vì thuộc tính cha đã chứa 

        public int UserLevel { get; set; }

        public int PointsDownloadFile { get; set; }
        public string? AnhDaiDien { get; set; }
        public string? AnhBia { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now; // Đặt giá trị mặc định hợp lệ
    }
}