using Library.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Library.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _selectedView;

        public object SelectedView
        {
            get => _selectedView;
            set
            {
                _selectedView = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowHomeCommand { get; }
        public ICommand ShowLibrariesCommand { get; }
        public ICommand ShowAllBooksCommand { get; }

        public MainViewModel()
        {
            ShowHomeCommand = new RelayCommand(ShowHome);
            ShowLibrariesCommand = new RelayCommand(ShowLibraries);
            ShowAllBooksCommand = new RelayCommand(ShowAllBooks);

            SelectedView = new HomeView();
        }

        private void ShowHome() => SelectedView = new HomeView();
        private void ShowLibraries() => SelectedView = new LibrariesView();
        private void ShowAllBooks() => SelectedView = new AllBooksView();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
