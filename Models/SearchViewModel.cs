namespace WebApplication1.Models
{
    public class SearchViewModel
    {

        public string? SearchTerm { get; set; }
        public List<Book>? ResultsBook { get; set; } = [];
        public List<Cart> ResultsBookCart { get; set; } = [];
    }
}