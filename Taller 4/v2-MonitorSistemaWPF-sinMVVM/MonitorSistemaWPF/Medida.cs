using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorSistemaWPF
{
    public class Medida : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string titulo = null;

        public Medida(string titulo)
        {
            this.titulo = titulo;
        }

        public string Titulo { get { return titulo; } set { titulo = value; } }

        double valor;
        public double Valor { 
            get { 
                return valor; 
            } 
            set 
            { 
                if(valor != 0)
                {
                    // Evitamos valores de 0
                    valor = value;
                }
                OnPropertyChanged("Valor");
            } 
        }

        void OnPropertyChanged(string propertyName) 
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
