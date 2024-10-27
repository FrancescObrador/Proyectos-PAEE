using MonitorSistema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonitorSistemaWPF
{
    public class RecursosSistema : INotifyPropertyChanged
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
        private ComandosRecursosSistema commandReset;
        private ComandosRecursosSistema commandStart;
        private ComandosRecursosSistema commandStop;

        LectorRecursosSistema lectorRecursosSistema;
        System.Windows.Threading.DispatcherTimer dispatcherTimer;

        public event PropertyChangedEventHandler PropertyChanged;
        public Boolean Ejecutando { get => ejecutandose(); }

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
                new Medida("Virtual"),
                new Medida("Fisica Maxima"),
                new Medida("Virtual Maxima")
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

            this.commandReset = new CommandReset(this);
            this.commandStart = new CommandStart(this);
            this.commandStop = new CommandStop(this);

            dispatcherTimer.Tick += new EventHandler(manejadorDispatcherTimer);
        }

        public void iniciar()
        {
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            EventoCambioEstado("Ejecutando");
            commandStart.NotifyCanExecuteChanged();
            commandStop.NotifyCanExecuteChanged();
        }

        public void parar()
        {
            dispatcherTimer.Stop();
            EventoCambioEstado("Ejecutando");
            commandStart.NotifyCanExecuteChanged();
            commandStop.NotifyCanExecuteChanged();
        }

        void EventoCambioEstado(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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
        public ComandosRecursosSistema CommandoReset { get => commandReset; }
        public ComandosRecursosSistema CommandoStart { get => commandStart; }
        public ComandosRecursosSistema CommandoStop { get => commandStop; }
    
        private void manejadorDispatcherTimer(object sender, EventArgs e)
        {
            this.cpu.Valor = lectorRecursosSistema.getCPU();
            this.memoria[0].Valor = lectorRecursosSistema.getMemoriaFisica();
            this.memoria[1].Valor = lectorRecursosSistema.getMemoriaVirtual();

            /*
            if (this.memoria[2].Valor < this.memoria[0].Valor)
            {
                this.memoria[2].Valor = this.memoria[0].Valor;
            }

            if (this.memoria[3].Valor < this.memoria[1].Valor)
            {
                this.memoria[3].Valor = this.memoria[1].Valor;
            }
            */

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
