using System.Windows.Controls;

namespace Library.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        private readonly DatabaseContext _databaseContext;
        public HomeView()
        {
            InitializeComponent();
            _databaseContext = new DatabaseContext();
            LoadNumberOfLibraries();
            LoadNumberOfBooks();
        }

        public void LoadNumberOfLibraries() 
        {
            int num = _databaseContext.Databases.Count();
            NumberOfLibraries.Text = num.ToString();
        }

        public void LoadNumberOfBooks()
        {
            int num = _databaseContext.Books.Count();
            NumberOfBooks.Text = num.ToString();
        }
    }
}
