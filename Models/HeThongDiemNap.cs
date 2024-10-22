using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class HeThongDiemNap
    {
        [Key]
        public int DiemNapID { get; set; }



        public int SoTien { get; set; }
        public int SoDiem { get; set; }
    }
}
