using Microsoft.Win32;
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
using System.Windows.Shapes;

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
            if (string.IsNullOrEmpty(Title.Text) || string.IsNullOrEmpty(Author.Text) || string.IsNullOrEmpty(Year.Text) || string.IsNullOrEmpty(Sector.Text))
            {
                MessageBox.Show("Prosím vyplňte všetky povinné polia. (Označené - *)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int year;
            if (!int.TryParse(Year.Text, out year) || year < 0)
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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                SelectedImage.Source = new BitmapImage(new Uri(imagePath));
            }
        }
    }
}
