
using System;
using System.ComponentModel;

namespace MonitorSistemaWPF
{
    public class TensiometroVM : INotifyPropertyChanged
    {
        private Medida tension;

        private Comandos commandReset;
        private Comandos commandStart;
        private Comandos commandStop;

        private Random rnd = new Random();
        private int targetSistolica;
        private int targetDiastolica;

        System.Windows.Threading.DispatcherTimer dispatcherTimer;

        public event PropertyChangedEventHandler PropertyChanged;
        public Boolean Ejecutando { get => ejecutandose(); }

        private DateTime startTime;

        public TensiometroVM()
        {
            tension = new Medida("Tensión");

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

            this.commandReset = new CommandReset(this);
            this.commandStart = new CommandStart(this);
            this.commandStop = new CommandStop(this);

            dispatcherTimer.Tick += new EventHandler(manejadorDispatcherTimer);
        }

        public void iniciar()
        {
            startTime = DateTime.Now;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
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
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool ejecutandose()
        {
            return dispatcherTimer.IsEnabled;
        }

        public Medida Tension { get => tension; set => tension = value; }

        public Comandos CommandoReset { get => commandReset; }
        public Comandos CommandoStart { get => commandStart; }
        public Comandos CommandoStop { get => commandStop; }

        private void manejadorDispatcherTimer(object sender, EventArgs e)
        {
            // Mock data 

            if ((DateTime.Now - startTime).TotalSeconds >= 30)
            {
                parar(); 
                return;
            }

            tension.Valor = rnd.Next(60, 120);
        }
    }
}
