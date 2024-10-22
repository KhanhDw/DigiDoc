using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplication1.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }

        public int UserId { get; set; }
        public int Bookid { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
