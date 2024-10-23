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

namespace Ventanas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            framePrincipal.Navigate(new MonitorPage());
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            framePrincipal.Navigate(new AboutMe());
        }

        private void btnPage1_Click(object sender, RoutedEventArgs e)
        {
            framePrincipal.Navigate(new MonitorPage());
        }

        private void btnPage2_Click(object sender, RoutedEventArgs e)
        {
            framePrincipal.Navigate(new Page2());
        }

        private void btnVoler_Click(object sender, RoutedEventArgs e)
        {
            if (framePrincipal.CanGoBack)
            {
                framePrincipal.GoBack();
            }
        }

        private void btnNuevaVentana_Click(object sender, RoutedEventArgs e)
        {
            Window1 w = new Window1();
            w.Show();
        }

        private void btnDialog_Click(object sender, RoutedEventArgs e)
        {
            Window2 w = new Window2();
            w.ShowDialog();
        }
    }
}