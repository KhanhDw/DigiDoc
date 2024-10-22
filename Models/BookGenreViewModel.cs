namespace WebApplication1.Models
{

    // model này để truyền và hiển thị thể loại trong các trang chi tiết cho từng cuốn sách/ tài liệu

    public class BookGenreViewModel
    {

        public List<string?>? GenreShow { get; set; }
        public Book? BookShow { get; set; }
        public int bokSave { get; set; }   // 1 = saved 0= not save
        public int idbookSaved { get; set; }   // 1... = saved ; null = not save
        public int userid { get; set; }

        public int DiemTaiFile { get; set; }

        public User? userAuthor { get; set; }

        // commnets
        public List<CommentWithUserViewModel>? CommentsOfBook { get; set; }

    }
}