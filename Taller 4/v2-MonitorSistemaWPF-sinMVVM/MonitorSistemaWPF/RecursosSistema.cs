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
        private String fabricante;
        private String usuario;
        private String procesador;
        private String modelo;
        private String discosLogicos;

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

            this.modelo = lectorRecursosSistema.getOrdenadorModelo();
            this.usuario = lectorRecursosSistema.getUsuario();
            this.fabricante = lectorRecursosSistema.getOrdenadorFabricante();
            this.procesador = lectorRecursosSistema.getProcesadores()[0];
            this.discosLogicos = lectorRecursosSistema.getDiscosLogicos();

            dispatcherTimer.Tick += new EventHandler(manejadorDispatcherTimer);
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
        public String Fabricante { get => fabricante; set => fabricante = value; }
        public String Usuario { get => usuario; set => usuario = value; }
        public String Procesador { get => procesador; set => procesador = value; }
        public String Modelo { get => modelo; set => modelo = value; }
        public String DiscosLogicos { get => discosLogicos; set => discosLogicos = value; }
    
        private void manejadorDispatcherTimer(object sender, EventArgs e)
        {

            this.cpu.Valor = lectorRecursosSistema.getCPU();

            double totalMemoriaFisica = lectorRecursosSistema.getMemoriaFisicaTotal();
            double memoriaFisicaDisponible = lectorRecursosSistema.getMemoriaFisicaDisponible();
            double memoriaFisicaUsada = totalMemoriaFisica - memoriaFisicaDisponible;
            this.memoria[0].Valor = (memoriaFisicaUsada / totalMemoriaFisica) * 100;

            double memoriaVirtualComprometida = lectorRecursosSistema.getMemoriaVirtualComprometida();
            double memoriaVirtualLimite = lectorRecursosSistema.getMemoriaVirtualLimite();
            this.memoria[1].Valor = (memoriaVirtualComprometida / memoriaVirtualLimite) * 100;

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
