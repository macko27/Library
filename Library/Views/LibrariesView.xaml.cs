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

                if (ListBoxLibraries.SelectedItem is Database selectedLibrary)
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
                LibraryWindow libraryWindow = new();
                if (ListBoxLibraries.SelectedItem is Database selectedLibrary)
                {
                    libraryWindow.LibraryName.Text = selectedLibrary.Name;
                    ListBoxLibraries.SelectedItem = null;
                    var result = libraryWindow.ShowDialog();
                    if (result == true)
                    {
                        selectedLibrary.Name = libraryWindow.LibraryName.Text;
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
                    if (ListBoxLibraries.SelectedItem is Database library)
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
            LibraryWindow libraryWindow = new();
            var result = libraryWindow.ShowDialog();
            if (result == true)
            {
                var library = new Database
                {
                    Name = libraryWindow.LibraryName.Text,
                };
                _databaseContext.Databases.Add(library);
                _databaseContext.SaveChanges();
                LoadLibraries();
            }
        }
    }
}
