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
using static System.Net.Mime.MediaTypeNames;
using static System.Reflection.Metadata.BlobBuilder;

namespace Library.Views
{
    /// <summary>
    /// Interaction logic for LibrariesView.xaml
    /// </summary>
    public partial class LibrariesView : UserControl
    {
        private readonly DatabaseContext _databaseContext;

        public LibrariesView()
        {
            InitializeComponent();
            _databaseContext = new DatabaseContext();
            LoadLibraries();
        }

        private void LoadLibraries()
        {
            var libraries = _databaseContext.GetAllDatabases();

            ListBoxLibraries.ItemsSource = libraries;
        }

        private void LoadBooks(int databaseId)
        {
            var books = _databaseContext.GetBooks(databaseId);

            ListBoxBooks.ItemsSource = books;
        }

        private void Edit_Library(object sender, RoutedEventArgs e)
        {
            if (ListBoxLibraries.SelectedItem != null)
            {
                LibraryWindow libraryWindow = new();
                var selectedLib = ListBoxLibraries.SelectedItem as Database;
                if (selectedLib != null)
                {
                    libraryWindow.Name.Text = selectedLib.Name;
                    ListBoxLibraries.SelectedItem = null;
                    var result = libraryWindow.ShowDialog();
                    if (result == true)
                    {
                        selectedLib.Name = libraryWindow.Name.Text;
                        _databaseContext.SaveChanges();
                        LoadLibraries();
                    }
                }

            }
        }

        private void Delete_Library(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Chcete naozaj vymazať vybranú knižnicu?", "Vymazanie knižnice", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (ListBoxLibraries.SelectedItem != null)
                {
                    var db = ListBoxLibraries.SelectedItem as Database;
                    if (db is not null)
                    {
                        if (db.Books.Count == 0)
                        {
                            _databaseContext.Databases.Remove(db);
                            _databaseContext.SaveChanges();
                            LoadLibraries();
                        }
                    }
                }
            }
        }

        private void Add_Library(object sender, RoutedEventArgs e)
        {
            LibraryWindow libraryWindow = new();
            var result = libraryWindow.ShowDialog();
            if (result == true)
            {
                var db = new Database
                {
                    Name = libraryWindow.Name.Text
                };
                _databaseContext.Databases.Add(db);
                _databaseContext.SaveChanges();
                LoadLibraries();
            }
        }

        private void MyListViewLibrariesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxLibraries.SelectedItem != null)
            {
                EditButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
                var db = ListBoxLibraries.SelectedItem as Database;
                if (db is not null)
                {
                    LoadBooks(db.Id);
                }
                AddBook.IsEnabled = true;
            }
            else
            {
                EditButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
                AddBook.IsEnabled = false;
                ListBoxBooks.ItemsSource = null;
            }
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


        private byte[] ConvertImageToByteArray(BitmapImage image)
        {
            byte[] imageBytes = null;
            if (image != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    BitmapEncoder encoder = new PngBitmapEncoder(); // Môžete zmeniť na iný typ obrázka podľa vašich potrieb
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    encoder.Save(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
            }
            return imageBytes;
        }

        private BitmapImage LoadImageFromByteArray(byte[] imageData)
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

        private void Edit_Book(object sender, RoutedEventArgs e)
        {
            if (ListBoxBooks.SelectedItem != null)
            {
                BookWindow bookWindow = new BookWindow();
                var selectedBook = ListBoxBooks.SelectedItem as Book;
                var db = ListBoxLibraries.SelectedItem as Database;
                if (selectedBook != null)
                {
                    bookWindow.Title.Text = selectedBook.Title;
                    bookWindow.Author.Text = selectedBook.Author;
                    bookWindow.Year.Text = selectedBook.Year.ToString();
                    bookWindow.Sector.Text = selectedBook.Sector;
                    bookWindow.ISBN.Text = selectedBook.ISBN;
                    if (selectedBook.Picture is not null)
                    {
                        bookWindow.SelectedImage.Source = LoadImageFromByteArray(selectedBook.Picture);
                    }
                    ListBoxBooks.SelectedItem = null;
                    var result = bookWindow.ShowDialog();
                    if (result == true && db != null)
                    {
                        int year;
                        if (int.TryParse(bookWindow.Year.Text, out year))
                        {
                            selectedBook.Title = bookWindow.Title.Text;
                            selectedBook.Author = bookWindow.Author.Text;
                            selectedBook.Year = year;
                            selectedBook.Sector = bookWindow.Sector.Text;
                            selectedBook.ISBN = bookWindow.ISBN.Text;
                            if (bookWindow.SelectedImage is not null)
                            {
                                selectedBook.Picture = ConvertImageToByteArray(bookWindow.SelectedImage.Source as BitmapImage);
                                _databaseContext.SaveChanges();
                                LoadBooks(db.Id);
                            }
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
                if (ListBoxBooks.SelectedItem != null && ListBoxLibraries.SelectedItem != null)
                {
                    var book = ListBoxBooks.SelectedItem as Book;
                    var db = ListBoxLibraries.SelectedItem as Database;
                    if (book is not null && db is not null)
                    {
                        _databaseContext.Books.Remove(book);
                        _databaseContext.SaveChanges();
                        LoadBooks(db.Id);
                    }
                }
            }
        }

        private void Add_Book(object sender, RoutedEventArgs e)
        {
            BookWindow bookWindow = new BookWindow();
            var db = ListBoxLibraries.SelectedItem as Database;
            var result = bookWindow.ShowDialog();
            if (result == true && db is not null)
            {
                int year;
                if (int.TryParse(bookWindow.Year.Text, out year))
                {
                    var book = new Book
                    {
                        Title = bookWindow.Title.Text,
                        Author = bookWindow.Author.Text,
                        Sector = bookWindow.Sector.Text,
                        Year = year,
                        ISBN = bookWindow.ISBN.Text,
                        DatabaseId = db.Id
                    };
                    if (bookWindow.SelectedImage is not null)
                    {
                        book.Picture = ConvertImageToByteArray(bookWindow.SelectedImage.Source as BitmapImage);
                    }
                    _databaseContext.Books.Add(book);
                    _databaseContext.SaveChanges();
                    LoadBooks(db.Id);
                }
            }
        }
    }
}
