namespace WebApplication1.Models
{
    public class User_LichSuNapDiem
    {
        // public List<LichSuNapDiem>? List_LichSuNapDiem { get; set; } = [];
        // public List<User>? List_User_LichSuNapDiem { get; set; } = [];


        public int LichSuNapId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public int DiemNap { get; set; }
        public int SotienNap { get; set; }
        public string? TrangThai { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}