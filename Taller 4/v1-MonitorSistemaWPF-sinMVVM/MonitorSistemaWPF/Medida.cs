using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorSistemaWPF
{
    public class Medida
    {
        string titulo = null;

        public Medida(string titulo)
        {
            this.titulo = titulo;
        }

        public string Titulo { get { return titulo; } set { titulo = value; } }

        double valor;
        public double Valor { get { return valor; } set { valor = value; } }

    }
}
