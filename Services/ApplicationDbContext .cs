using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {

        // tên biến đặt trong file databaseContext cần phải giống với tên bảng trong sql server 

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenresAndBook> GenresAndBooks { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<LichSuNapDiem> LichSuNapDiem { get; set; }
        public DbSet<HeThongDiemNap> HeThongDiemNap { get; set; }



    }
}