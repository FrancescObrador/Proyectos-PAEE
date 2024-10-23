using MonitorSistema;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ventanas
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class MonitorPage : Page
    {

        private LectorRecursosSistema lector = new LectorRecursosSistema();
        private System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public MonitorPage()
        {
            InitializeComponent();
            Actualizar();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Actualizar();
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

        private void Actualizar()
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
    }
}
