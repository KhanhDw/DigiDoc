
namespace WebApplication1.Models
{
    public class UpdateBookModel
    {
        public Book? BookEdit { get; set; }
        public List<Genre>? GenreLoadData { get; set; }

        public List<GenresAndBook>? GenreSelectedByBook { get; set; }


        //not using file under


        // làm đường truyền dữ liệu cho chức năng cập nhật thông tin sách trong trang quản lý
        public IFormFile? FileDocNew_UserUpload { get; set; }
        public IFormFile? FileImgNew_UserUpload { get; set; }

        // ánh xạ biến qua cshtml để nhận và truyến dữ liệu
        // <label asp-for="FileDocNew_UserUpload">Cập nhật File tài liệu mới - định dạng PDF</label>
        // <input type="file" id="fileDoc" asp-for="FileDocNew_UserUpload" accept=".pdf">
    }
}