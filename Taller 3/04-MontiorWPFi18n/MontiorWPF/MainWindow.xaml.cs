using MonitorSistema;
using System.Windows;
using System.Windows.Media;

namespace MonitorWPFi18n
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
            Config.GetInstance().InitDefaults();
            Config.GetInstance().Load();
            cambiarIdioma();
            InitializeComponent();  
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
            cambiarIdioma();
        }
        
        private void cambiarIdioma()
        {
            var lang = Config.GetInstance().lang;
            if(lang != null)
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(lang);
            }
        }

        private void aplicarConfiguracion()
        {
            var color = Config.GetInstance().color;
            if (color == "") color = "Red";
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