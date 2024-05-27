using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Library
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DatabaseContext DatabaseContext { get; } = new();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Add_Book(object sender, RoutedEventArgs e)
        {
            var book = new Book
            {
                Title = "Example Book",
                Author = "John Doe",
                Year = 2022,
                // Pridajte ID existujúcej databázy, kam chcete knihu pridať
                DatabaseId = DatabaseContext.Databases.First().Id
            };
            DatabaseContext.Books.Add(book);
            DatabaseContext.SaveChanges();
            var books = DatabaseContext.Books.OrderBy(b => b.Id).ToList();
            //listView.ItemsSource = books;
            Console.WriteLine("ddd");
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Close_App(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Chcete naozaj ukončiť aplikáciu?", "Ukončenie aplikácie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}