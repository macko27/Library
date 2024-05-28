
using System.Reflection.Metadata;

namespace Library
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public int Year { get; set; }
        public string? ISBN { get; set; }
        public string Sector { get; set; } = "";
        public byte[]? Picture { get; set; }

        public int DatabaseId { get; set; }
    }
}
