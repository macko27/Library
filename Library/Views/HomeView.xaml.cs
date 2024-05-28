using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
