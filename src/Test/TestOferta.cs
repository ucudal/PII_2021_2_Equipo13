using System;
using NUnit.Framework;
using ClassLibrary;
using System.Collections.Generic;

namespace Tests{

    [TestFixture]
    public class TestOferta
    {
        [Test]
        public void TestOfertaId()
        {
            const string expected = "1234";
            Oferta oferta1 = new Oferta("1234", Empresa1, 31/12/21, Etiqueta1, habilitaciones, 1000, 4400, descripcion1);
            Assert.AreEqual(expected, oferta1.Id);
        }
        
        [Test]
        public void TestOfertaFechaCierre()
        {
        Oferta oferta1 = new Oferta("1234", Empresa1, 31/12/21, Etiqueta1, habilitaciones, 1000, 4400, descripcion1);

        DateTime expected = 31/12/21;
        Assert.AreEqual(expected, oferta1.FechaCierre);

        }

        [Test]
        public void TestOfertaEmpresaId()
        {
        
        Empresa empresa1 = new Empresa("15", oferta1, ubicacion, rubro, "Acme Procesamiento de Residuos");
        Oferta oferta1 = new Oferta("1234", empresa1, 31/12/21, Etiqueta1, habilitaciones1, 1000, 4400, descripcion1);
        string expected = "15";
        Assert.AreEqual(expected, oferta1.empresa1.id);

        }

        [Test]
        public void TestOfertaHabilitaciones()
        {
        
        Habilitacion habilitacion1 = new Habilitacion("Procesamiento Residuos Hospitalarios", "Acme Procesamiento de Resiudos", 01/01/19, 31/12/21);
        Oferta oferta1 = new Oferta("1234", Empresa1, 31/12/21, Etiqueta1, habilitaciones1, 1000, 4400, descripcion1);
        string expected = "Procesamiento Residuos Hospitalarios";
        Assert.AreEqual(expected, oferta1.habilitacion1.descripcion);

        }

    }
}

