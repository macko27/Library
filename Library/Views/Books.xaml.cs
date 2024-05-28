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
    /// Interaction logic for Books.xaml
    /// </summary>
    public partial class Books : UserControl
    {
        private readonly DatabaseContext _databaseContext;
        private int _currentDatabaseId;

        public Books()
        {
            InitializeComponent();
            _databaseContext = new DatabaseContext();
        }

        public void LoadBooks(int databaseId)
        {
            _currentDatabaseId = databaseId;
            var books = _databaseContext.GetBooks(databaseId);
            ListBoxBooks.ItemsSource = books;
            AddBook.IsEnabled = true;
        }

        private void MyListViewBooksChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxBooks.SelectedItem != null)
            {
                EditBook.IsEnabled = true;
                DeleteBook.IsEnabled = true;
            }
            else
            {
                EditBook.IsEnabled = false;
                DeleteBook.IsEnabled = false;
            }
        }

        private void Edit_Book(object sender, RoutedEventArgs e)
        {
            if (ListBoxBooks.SelectedItem != null)
            {
                BookWindow bookWindow = new BookWindow();
                var selectedBook = ListBoxBooks.SelectedItem as Book;
                if (selectedBook != null)
                {
                    bookWindow.Title.Text = selectedBook.Title;
                    bookWindow.Author.Text = selectedBook.Author;
                    bookWindow.Year.Text = selectedBook.Year.ToString();
                    bookWindow.Sector.Text = selectedBook.Sector;
                    bookWindow.ISBN.Text = selectedBook.ISBN;
                    if (selectedBook.Picture != null)
                    {
                        bookWindow.SelectedImage.Source = LoadImageFromByteArray(selectedBook.Picture);
                    }
                    ListBoxBooks.SelectedItem = null;
                    var result = bookWindow.ShowDialog();
                    if (result == true)
                    {
                        if (int.TryParse(bookWindow.Year.Text, out int year))
                        {
                            selectedBook.Title = bookWindow.Title.Text;
                            selectedBook.Author = bookWindow.Author.Text;
                            selectedBook.Year = year;
                            selectedBook.Sector = bookWindow.Sector.Text;
                            selectedBook.ISBN = bookWindow.ISBN.Text;
                            if (bookWindow.SelectedImage != null)
                            {
                                selectedBook.Picture = ConvertImageToByteArray(bookWindow.SelectedImage.Source as BitmapImage);
                            }
                            _databaseContext.SaveChanges();
                            LoadBooks(_currentDatabaseId);
                        }
                    }
                }
            }
        }

        private void Delete_Book(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Chcete naozaj vymazať vybranú knihu?", "Vymazanie knihy", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (ListBoxBooks.SelectedItem != null)
                {
                    var book = ListBoxBooks.SelectedItem as Book;
                    if (book != null)
                    {
                        _databaseContext.Books.Remove(book);
                        _databaseContext.SaveChanges();
                        LoadBooks(_currentDatabaseId);
                    }
                }
            }
        }

        private void Add_Book(object sender, RoutedEventArgs e)
        {
            BookWindow bookWindow = new BookWindow();
            var result = bookWindow.ShowDialog();
            if (result == true)
            {
                if (int.TryParse(bookWindow.Year.Text, out int year))
                {
                    var book = new Book
                    {
                        Title = bookWindow.Title.Text,
                        Author = bookWindow.Author.Text,
                        Sector = bookWindow.Sector.Text,
                        Year = year,
                        ISBN = bookWindow.ISBN.Text,
                        DatabaseId = _currentDatabaseId
                    };
                    if (bookWindow.SelectedImage != null)
                    {
                        book.Picture = ConvertImageToByteArray(bookWindow.SelectedImage.Source as BitmapImage);
                    }
                    _databaseContext.Books.Add(book);
                    _databaseContext.SaveChanges();
                    LoadBooks(_currentDatabaseId);
                }
            }
        }

        private byte[]? ConvertImageToByteArray(BitmapImage image)
        {
            byte[] imageBytes = null;
            if (image != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    encoder.Save(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
            }
            return imageBytes;
        }

        private BitmapImage? LoadImageFromByteArray(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            using (MemoryStream memoryStream = new MemoryStream(imageData))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = SearchTextBox.Text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                var books = _databaseContext.GetBooksNameContainsWithId(text, _currentDatabaseId);
                ListBoxBooks.ItemsSource = books;
            }
            else
            {
                LoadBooks(_currentDatabaseId);
            }
        }
    }
}
