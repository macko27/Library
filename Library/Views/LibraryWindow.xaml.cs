using System.Windows;

namespace Library.Views
{
    /// <summary>
    /// Interaction logic for LibraryWindow.xaml
    /// </summary>
    public partial class LibraryWindow : Window
    {
        public LibraryWindow()
        {
            InitializeComponent();
        }

        private void OK_Button(object sender, RoutedEventArgs e)
        {
            if (LibraryName.Text != "")
            {
                DialogResult = true;
                this.Close();
            }
        }
        private void Cancel_Button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
