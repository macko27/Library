using System.Windows;
using System.Windows.Controls;

namespace Library.Views
{
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

        private void MyListViewChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxLibraries.SelectedItem != null)
            {
                EditButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;

                var selectedLibrary = ListBoxLibraries.SelectedItem as Database;
                if (selectedLibrary != null)
                {
                    Books.LoadBooks(selectedLibrary.Id);
                }
            }
            else
            {
                EditButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
                Books.LoadBooks(-1);
            }
        }

        private void Edit_Library(object sender, RoutedEventArgs e)
        {
            if (ListBoxLibraries.SelectedItem != null)
            {
                LibraryWindow libraryWindow = new LibraryWindow();
                var selectedLibrary = ListBoxLibraries.SelectedItem as Database;
                if (selectedLibrary != null)
                {
                    libraryWindow.Name.Text = selectedLibrary.Name;
                    ListBoxLibraries.SelectedItem = null;
                    var result = libraryWindow.ShowDialog();
                    if (result == true)
                    {
                        selectedLibrary.Name = libraryWindow.Name.Text;
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
                    var library = ListBoxLibraries.SelectedItem as Database;
                    if (library != null)
                    {
                        _databaseContext.Databases.Remove(library);
                        _databaseContext.SaveChanges();
                        LoadLibraries();
                    }
                }
            }
        }

        private void Add_Library(object sender, RoutedEventArgs e)
        {
            LibraryWindow libraryWindow = new LibraryWindow();
            var result = libraryWindow.ShowDialog();
            if (result == true)
            {
                var library = new Database
                {
                    Name = libraryWindow.Name.Text,
                };
                _databaseContext.Databases.Add(library);
                _databaseContext.SaveChanges();
                LoadLibraries();
            }
        }
    }
}
