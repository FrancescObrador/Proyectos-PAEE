using MonitorSistema;
using System.Windows;
using System.Windows.Media;

namespace MonitorWPFPersonalizacion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LectorRecursosSistema lector = new LectorRecursosSistema();
        private System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            Config.GetInstance().InitDefaults();
            Config.GetInstance().Load();
            aplicarConfiguracion();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            this.Acutalizar();
        }

        private void Acutalizar()
        {
            lblCPU.Content = lector.getCPU();
            lblMemFisica.Content = lector.getMemoriaFisica();
            lblMemVirtual.Content = lector.getMemoriaVirtual();

            lblLectura.Content = lector.getDatosDisco(LectorRecursosSistema.DiskData.Read).ToString();
            lblEscritura.Content = lector.getDatosDisco(LectorRecursosSistema.DiskData.Write).ToString();
            lblDiscoLogico.Content = lector.getDiscosLogicos();
            lblLecEsc.Content = lector.getDatosDisco(LectorRecursosSistema.DiskData.ReadAndWrite).ToString();
            lblRecibidos.Content = lector.getDatosRed(LectorRecursosSistema.NetData.Received).ToString();
            lblEnviados.Content = lector.getDatosRed(LectorRecursosSistema.NetData.Sent).ToString();
            lblEnviadosRecibidos.Content = lector.getDatosRed(LectorRecursosSistema.NetData.ReceivedAndSent).ToString();
            lblModelo.Content = lector.getOrdenadorModelo();
            lblProcesador.Content = lector.getProcesadores()[0];
            lblFabricante.Content = lector.getOrdenadorFabricante();
            lblUsuario.Content = lector.getUsuario();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.Acutalizar();
        }

        private void btnAcutalizar_Click(object sender, RoutedEventArgs e)
        {
            if (dispatcherTimer.IsEnabled)
            {
                dispatcherTimer.Stop();
                btnAcutalizar.Content = "Parar";
            }
            else
            {
                dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
                dispatcherTimer.Start();
                btnAcutalizar.Content = "Iniciar";
            }
        }

        private void btnPersonalizar_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new WindowConfig();
            dlg.ShowDialog();
            aplicarConfiguracion();
        }

        private void aplicarConfiguracion()
        {
            var color = Config.GetInstance().color;
            if (color is null) color = "Red";
            var bgColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            Memoria.Background = bgColor;
            ordenador.Background = bgColor;
            Disco.Background = bgColor;
            Red.Background = bgColor;
            Fabricante.Background = bgColor;
            Modelo.Background = bgColor;
            CPU.Background = bgColor;
        }
    }
}