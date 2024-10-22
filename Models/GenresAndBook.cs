using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class GenresAndBook
    {

        [Key]
        public int GenresAndBookID { get; set; }


        public int GenreID { get; set; }
        public int BookID { get; set; }
    }
}