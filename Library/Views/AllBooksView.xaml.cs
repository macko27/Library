using System;
using System.Collections.Generic;
using System.IO;
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
