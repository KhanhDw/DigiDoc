using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Cart
    {
        public int CartID { get; set; }

        public int Bookid { get; set; }

        public int Userid { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}