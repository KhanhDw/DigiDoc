using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Book
    {
        public int BookID { get; set; }

        //[MaxLength(200)]
        public string? Title { get; set; }

        //[MaxLength(200)]
        public string? Author { get; set; }

        public string? ImgThumbnail { get; set; }
        public string? FileDoc { get; set; }

        [Precision(15, 0)]
        public decimal Price { get; set; }

        public DateTime CreatedDate { get; set; }
        public int DownloadFile { get; set; }
        public int IdUserUploadedFile { get; set; }


    }
}