using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domotica
{
    public class ConectorSistemaDomoticaStub : IConectorSistemaDomotica
    {
        double posicionPersiana = 0;
        public double bajarPersiana()
        {
            if(posicionPersiana >= 0.1)
            {
                posicionPersiana -= posicionPersiana;
            }
            else
            {
                posicionPersiana = 0;
            }
            return posicionPersiana;
        }

        public double subirPersiana()
        {
            if(posicionPersiana <= 0.9)
            {
                posicionPersiana += 1 - posicionPersiana;
            }
            else
            {
                posicionPersiana = 1;
            }
            return posicionPersiana;
        }
    }
}
