using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Blog
    {
        public int BlogID { get; set; }              // ID của blog
        public string? BlogName { get; set; }         // Tên của blog
        public string? BlogContent { get; set; }      // Nội dung của blog
        public int BlogViews { get; set; }               // Số lượt xem

        public int UserIDPost { get; set; }
        public DateTime CreatedDate { get; set; }    // Ngày tạo blog
    }
}
