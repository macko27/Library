using Microsoft.Win32;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Library.Views
{
    /// <summary>
    /// Interaction logic for BookWindow.xaml
    /// </summary>
    public partial class BookWindow : Window
    {
        public BookWindow()
        {
            InitializeComponent();
        }

        private void OK_Button(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(BookTitle.Text) || string.IsNullOrEmpty(Author.Text) || string.IsNullOrEmpty(Year.Text) || string.IsNullOrEmpty(Sector.Text))
            {
                MessageBox.Show("Prosím vyplňte všetky povinné polia. (Označené - *)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(Year.Text, out int year) || year < 0)
            {
                MessageBox.Show("Zle zadaný rok", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
            this.Close();
        }
        private void Cancel_Button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                SelectedImage.Source = new BitmapImage(new Uri(imagePath));
            }
        }
    }
}
