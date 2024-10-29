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
            set { 
                valor = value;
                OnPropertyChanged("Valor");

                if (ValorMaximo < valor)
                {
                    ValorMaximo = valor;
                }
                else if (valor < valorMinimo)
                {
                    ValorMinimo = valor;
                }
            } 
        }

        double valorMaximo;
        public double ValorMaximo
        {
            get
            {
                return valorMaximo;
            }
            set
            {
                valorMaximo = value;
                OnPropertyChanged("ValorMaximo"); 
            }

        }

        double valorMinimo = 200;
        public double ValorMinimo
        {
            get
            {
                return valorMinimo;
            }
            set
            {
                valorMinimo = value;
                OnPropertyChanged("ValorMinimo");
            }

        }

        void OnPropertyChanged(string propertyName) 
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override string ToString()
        {
            return String.Format("{0,18:00000000.000}", Valor);
        }
    }
}
