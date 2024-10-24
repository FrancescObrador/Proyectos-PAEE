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
using static MonitorSistema.LectorRecursosSistema;

namespace MonitorSistemaWPF
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LectorRecursosSistema lectorRecursosSistema = new LectorRecursosSistema();
        private RecursosSistema recursosSistemaVM;

        public MainWindow()
        {
            InitializeComponent();
            // Setup del view-model
            recursosSistemaVM = new RecursosSistema();
            this.DataContext = recursosSistemaVM;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (recursosSistemaVM.ejecutandose())
            {
                recursosSistemaVM.parar();
                btnActualizar.Content = "Iniciar";
            }
            else
            {
                recursosSistemaVM.iniciar();
                btnActualizar.Content = "Parar";
            }

        }

        private void actualizaValores()
        {
            this.labelCPU.Content = lectorRecursosSistema.getCPU();
            this.labelDiscoLectura.Content = lectorRecursosSistema.getDatosDisco(DiskData.Read).ToString();
            this.labelDiscoEscritura.Content = lectorRecursosSistema.getDatosDisco(DiskData.Write).ToString();
            this.labelDiscoLecturaEscritura.Content = lectorRecursosSistema.getDatosDisco(DiskData.ReadAndWrite).ToString();
            this.labelRedRecibidos.Content = lectorRecursosSistema.getDatosRed(NetData.Received).ToString(); ;
            this.labelRedEnviados.Content = lectorRecursosSistema.getDatosRed(NetData.Sent).ToString(); ;
            this.labelRedRecibidosEnviados.Content = lectorRecursosSistema.getDatosRed(NetData.ReceivedAndSent).ToString();
            this.labelDiscosLogicos.Content = lectorRecursosSistema.getDiscosLogicos();
            this.labelMemoriaFisica.Content = lectorRecursosSistema.getMemoriaFisica();
            this.labelMemoriaVirtual.Content = lectorRecursosSistema.getMemoriaVirtual();
            this.labelOrdenadorFabricante.Content = lectorRecursosSistema.getOrdenadorFabricante();
            this.labelOrdenadorModelo.Content = lectorRecursosSistema.getOrdenadorModelo();
            this.labelOrdenadorProcesador.Content = lectorRecursosSistema.getProcesadores()[0];
            this.labelUsuario.Content = lectorRecursosSistema.getUsuario();
        }

        private void manejadorDispatcherTimer(object sender, EventArgs e)
        {
            actualizaValores();
        }
    }
}
