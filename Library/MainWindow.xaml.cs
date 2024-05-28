using System.Windows;
using System.Windows.Input;

namespace Library
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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