using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonitorSistemaWPF
{
    public abstract class ComandosRecursosSistema : ICommand
    {
        protected RecursosSistema destinatario;

        public event EventHandler CanExecuteChanged;

        public ComandosRecursosSistema(RecursosSistema d) 
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

    public class CommandReset : ComandosRecursosSistema
    {
        public CommandReset(RecursosSistema r) : base(r) 
        {
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            // Valores maximos de Memoria fisica y virtual
            destinatario.Memoria[2].Valor = 0;
            destinatario.Memoria[3].Valor = 0;
        }
    }

    public class CommandStart : ComandosRecursosSistema
    {
        public CommandStart(RecursosSistema r) : base(r)
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

    public class CommandStop : ComandosRecursosSistema
    {
        public CommandStop(RecursosSistema r) : base(r)
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
