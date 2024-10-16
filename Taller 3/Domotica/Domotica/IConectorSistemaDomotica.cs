using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domotica
{
    /// <summary>
    /// La persiana por defecto está cerrada
    /// </summary>
    public interface IConectorSistemaDomotica
    {
        // Retorna el valor de apertura de la persiana entre 0 y 1
        double subirPersiana();

        // Retorna el valor de apertura de la persiana entre 0 y 1
        double bajarPersiana();
    }
}
