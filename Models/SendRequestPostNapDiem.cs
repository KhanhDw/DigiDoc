using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class SendRequestPostNapDiem
    {


        public int LichSuNapDiemID { get; set; }
        public int UserId { get; set; }
        public int DiemNap { get; set; }
        public int SotienNap { get; set; }
    }

}