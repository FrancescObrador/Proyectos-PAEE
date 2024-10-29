using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonitorSistemaWPF
{
    public abstract class Comandos : ICommand
    {
        protected TensiometroVM destinatario;

        public event EventHandler CanExecuteChanged;

        public Comandos(TensiometroVM d) 
        {
            this.destinatario = d;
        }

        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);

        public void NotifyCanExecuteChanged()
        {
            if(CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }

    public class CommandReset : Comandos
    {
        public CommandReset(TensiometroVM r) : base(r) 
        {
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            destinatario.Tension.Valor = 0;
        }
    }

    public class CommandStart : Comandos
    {
        public CommandStart(TensiometroVM r) : base(r)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return !destinatario.ejecutandose();
        }

        public override void Execute(object parameter)
        {
            destinatario.iniciar();
        }
    }

    public class CommandStop : Comandos
    {
        public CommandStop(TensiometroVM r) : base(r)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return destinatario.ejecutandose();
        }

        public override void Execute(object parameter)
        {
            destinatario.parar();
        }
    }

}
