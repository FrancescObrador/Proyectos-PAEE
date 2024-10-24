using MonitorSistema;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorSistemaWPF
{
    public class RecursosSistema
    {
        private Medida cpu;
        private Medida[] disco;
        private Medida[] memoria;
        private Medida[] red;
        private string fabricante;
        private string usuario;
        private string procesador;

        LectorRecursosSistema lectorRecursosSistema;
        System.Windows.Threading.DispatcherTimer dispatcherTimer;

        public RecursosSistema() 
        {
            lectorRecursosSistema = new LectorRecursosSistema();
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            cpu = new Medida("CPU");
            disco = new Medida[]
            {
                new Medida("Lectura"),
                new Medida("Escritura"),
                new Medida("Escritura/Lectura")
            };

            memoria = new Medida[]
            {
                new Medida("Fisica"),
                new Medida("Virtual")
            };

            red = new Medida[]
            {
                new Medida("Paquetes enviados"),
                new Medida("Paquetes recibidos"),
                new Medida("Paquetes enviados/recibidos")
            };

        }

        public void iniciar()
        {
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        public void parar()
        {
            dispatcherTimer.Stop();
        }

        public bool ejecutandose()
        {
            return dispatcherTimer.IsEnabled;
        }

        public Medida Cpu { get => cpu; set => cpu = value; }
        public Medida[] Disco {  get => disco; set => disco = value; }
        public Medida[] Memoria { get => memoria; set => memoria = value; }
        public Medida[] Red { get => red; set => red = value; }
        public Medida DiscoLectura { get => disco[0]; }
        public string Fabricante { get => fabricante; set => fabricante = value; }
        public string Usuario { get => usuario; set => usuario = value; }
        public string Procesador { get => procesador; set => procesador = value; }
    
        private void manejadorDispatcherTimer(object sender, EventArgs e)
        {
            
            this.cpu.Valor = lectorRecursosSistema.getCPU();
            this.usuario = lectorRecursosSistema.getUsuario();
            this.fabricante = lectorRecursosSistema.getOrdenadorFabricante();
            this.memoria[0].Valor = lectorRecursosSistema.getMemoriaFisica();
            this.memoria[1].Valor = lectorRecursosSistema.getMemoriaVirtual();
            this.disco[0].Valor = lectorRecursosSistema.getDatosDisco(LectorRecursosSistema.DiskData.Read);
            this.disco[1].Valor = lectorRecursosSistema.getDatosDisco(LectorRecursosSistema.DiskData.Write);
            this.disco[2].Valor = lectorRecursosSistema.getDatosDisco(LectorRecursosSistema.DiskData.ReadAndWrite);
            this.memoria[0].Valor = lectorRecursosSistema.getMemoriaFisica();
            this.memoria[1].Valor = lectorRecursosSistema.getMemoriaVirtual();
            this.red[0].Valor = lectorRecursosSistema.getDatosRed(LectorRecursosSistema.NetData.Sent);
            this.red[1].Valor = lectorRecursosSistema.getDatosRed(LectorRecursosSistema.NetData.Received);
            this.red[2].Valor = lectorRecursosSistema.getDatosRed(LectorRecursosSistema.NetData.ReceivedAndSent);
        }

    }
}
