using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class SendRequestPost
    {

        // coins - thêm mục nạp 
        public List<HeThongDiemNap>? Data { get; set; }
        public int IdLichSuNap { get; set; }



    }
}
