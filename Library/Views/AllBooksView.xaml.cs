using System.Windows.Controls;

namespace Library.Views
{
    /// <summary>
    /// Interaction logic for AllBooksView.xaml
    /// </summary>
    public partial class AllBooksView : UserControl
    {
        private readonly DatabaseContext _databaseContext;
        public AllBooksView()
        {
            InitializeComponent();
            _databaseContext = new DatabaseContext();
            LoadBooks();
        }

        public void LoadBooks()
        {
            var books = _databaseContext.GetAllBooks();
            ListBoxBooks.ItemsSource = books;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = SearchTextBox.Text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                var books = _databaseContext.GetBooksNameContains(text);
                ListBoxBooks.ItemsSource = books;
            }
            else
            {
                LoadBooks();
            }
        }
    }
}
