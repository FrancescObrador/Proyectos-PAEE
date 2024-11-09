using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TallerEF.Modelo
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Identificacion { get; set; }
        public byte[]? Fotografia { get; set; }
        public virtual ICollection<CuentaCliente> Cuentas { get; set; } = new ObservableCollection<CuentaCliente>();


        public Cliente() { }

        public Cliente(Cliente cliente) 
        {
            this.Id = cliente.Id;
            this.Nombre = cliente.Nombre;
            this.Identificacion = cliente.Identificacion;
            this.Fotografia = cliente.Fotografia;
            this.Cuentas = cliente.Cuentas;
        }

    }
}
