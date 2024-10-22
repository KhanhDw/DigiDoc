namespace WebApplication1.Models
{
    public class LichSuNapvaHeThongNap
    {
        public List<HeThongDiemNap>? ListHeThongDiemNap { get; set; } = [];
        public List<LichSuNapDiem>? ListLichSuNapDiem { get; set; } = [];

        //cấp thông tin người dùng để chuyển khoản
        public int UserId { get; set; }
    }
}