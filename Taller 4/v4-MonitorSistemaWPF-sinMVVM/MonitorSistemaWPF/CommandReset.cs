using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonitorSistemaWPF
{
    public class CommandReset : ICommand
    {
        RecursosSistema destinatario;
        public event EventHandler CanExecuteChanged;

        public CommandReset(RecursosSistema r)
        {
            this.destinatario = r;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            // Valores maximos de Memoria fisica y virtual
            destinatario.Memoria[2].Valor = 0;
            destinatario.Memoria[3].Valor = 0;
        }
    }
}
