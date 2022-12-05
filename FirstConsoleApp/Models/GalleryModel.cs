using FirstConsoleApp.Data;

namespace FirstConsoleApp.Models
{
    public class GalleryModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public Book Book { get; set; }
    }
}
