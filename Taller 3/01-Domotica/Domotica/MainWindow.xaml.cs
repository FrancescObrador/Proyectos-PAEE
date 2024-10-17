using System.Windows;

namespace Domotica
{
    public partial class MainWindow : Window
    {
        private ConectorSistemaDomoticaStub conectorSistema;

        public MainWindow()
        {
            InitializeComponent();
            conectorSistema = new ConectorSistemaDomoticaStub();
        }

        private void BajarPersiana_Click(object sender, RoutedEventArgs e)
        {
            double posicion = conectorSistema.bajarPersiana();
            CambiarColorPersianas(posicion);
        }

        private void SubirPersiana_Click(object sender, RoutedEventArgs e)
        {
            double posicion = conectorSistema.subirPersiana();
            CambiarColorPersianas(posicion);
        }

        private void CambiarColorPersianas(double posicion)
        {
            if (posicion == 1)  // Persiana subida
            {
                panel.Background = System.Windows.Media.Brushes.Green;
            }
            else if (posicion == 0)  // Persiana bajada
            {
                panel.Background = System.Windows.Media.Brushes.Red;
            }
            else  // Persiana en otra posición
            {
                panel.Background = System.Windows.Media.Brushes.White;
            }
        }
    }
}
