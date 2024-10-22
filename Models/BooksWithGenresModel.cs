

namespace WebApplication1.Models
{
    public class BooksWithGenresModel
    {
        public int BookID { get; set; }
        public int UserID { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public decimal Price { get; set; }
        public string? FileDoc { get; set; }
        public string? ImgThumbnail { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? TenTheLoai { get; set; }
        public string? UserNameUploadFile { get; set; }
    }
}