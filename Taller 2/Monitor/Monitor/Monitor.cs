using MonitorSistema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Monitor
{
    public partial class Monitor : Form
    {
        LectorRecursosSistema lector = new LectorRecursosSistema();
        public Monitor()
        {
            InitializeComponent();
            this.Actualizar();
        }

        private void Actualizar()
        {
            labelCPU.Text = lector.getCPU();
            labelMemoriaFisica.Text = lector.getMemoriaFisica();
            labelMemoriaVirtual.Text = lector.getMemoriaVirtual();

            labelDiscosLectura.Text = lector.getDatosDisco(LectorRecursosSistema.DiskData.Read).ToString();
            labelDiscosEscritura.Text = lector.getDatosDisco(LectorRecursosSistema.DiskData.Write).ToString();
            labelDiscosLogicos.Text = lector.getDiscosLogicos();
            labelDiscosRW.Text = lector.getDatosDisco(LectorRecursosSistema.DiskData.ReadAndWrite).ToString();
            labelRecibidos.Text = lector.getDatosRed(LectorRecursosSistema.NetData.Received).ToString();
            labelEnviados.Text = lector.getDatosRed(LectorRecursosSistema.NetData.Sent).ToString();
            labelReciEnvi.Text = lector.getDatosRed(LectorRecursosSistema.NetData.ReceivedAndSent).ToString();
            labelModelo.Text = lector.getOrdenadorModelo();
            labelProcesador.Text = lector.getProcesadores()[0];
            labelFabricante.Text = lector.getOrdenadorFabricante();
            labelUsuario.Text = lector.getUsuario();
        }

        private void buttonActualizar_Click(object sender, EventArgs e)
        {
            if(timer1.Enabled) 
            { 
                timer1.Stop();
                buttonActualizar.Text = "Iniciar";
            }
            else
            {
                timer1.Interval = 1000;
                timer1.Start();
                buttonActualizar.Text = "Parar";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Actualizar();
        }
    }
}
