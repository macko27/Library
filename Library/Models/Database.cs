namespace Library
{
    public class Database
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<Book> Books { get; } = [];
    }
}
