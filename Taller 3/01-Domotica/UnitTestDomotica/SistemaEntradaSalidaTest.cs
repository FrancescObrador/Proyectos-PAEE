using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Domotica;

namespace UnitTestDomotica
{
    [TestClass]
    public class SistemaEntradaSalidaTest
    {
        [TestMethod]
        public void TestSubida()
        {
            IConectorSistemaDomotica conector = new ConectorSistemaDomoticaStub();
            double posicion = conector.bajarPersiana();
            Assert.AreEqual(0.0, posicion);

            double nuevaPosicion = conector.subirPersiana();
            Assert.IsTrue(nuevaPosicion > posicion);
        }

        [TestMethod]
        public void TestBajada()
        {
            IConectorSistemaDomotica conector = new ConectorSistemaDomoticaStub();
            double posicion = conector.subirPersiana();
            Assert.AreEqual(1.0, posicion);

            double nuevaPosicion = conector.bajarPersiana();
            Assert.IsTrue(nuevaPosicion < posicion);
        }
    }
}
