using System;
using NUnit.Framework;
using PII_E13.ClassLibrary;
using System.Collections.Generic;

namespace Tests{

    [TestFixture]
    public class TestOferta
    {
        [Test]
        public void TestOfertaId()
    {
        Habilitacion h1 = new Habilitacion("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", new DateTime(2021, 10, 10), new DateTime(2026, 10, 10), true);
        Habilitacion h2 = new Habilitacion("Habilitacion Comercial 2 ", "Intendencia Municipal De Montevideo (IMM)", new DateTime(2021, 10, 10), new DateTime(2026, 10, 10), true);

        List<Habilitacion> habilitaciones = new List<Habilitacion>();
        habilitaciones.Add(h1);
        habilitaciones.Add(h2);

        Ubicacion u1 = new Ubicacion("montevideo", "Calle 1 1892");

        Emprendedor e1 = new Emprendedor("1","Emprendedor S.A", habilitaciones, "montevideo","Calle 1 1892", "reciclaje");
      
        Empresa em1 = new Empresa("1","ciudad 1", "calle 1" , "reciclaje", "Empresa S.A");

        List<string> etiquetas = new List<string>();
        etiquetas.Add("etiqueta 1");

        Oferta f1 = new Oferta("1234", em1, new DateTime(2021,10,10), etiquetas, habilitaciones, "descripcion2", "titulo2", false);
        string expected = "1234";
        Assert.AreEqual(expected, f1.Id);

    }
        
        [Test]
        public void TestOfertaFechaCierre()
    {
        Habilitacion h1 = new Habilitacion("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", new DateTime(2021, 10, 10), new DateTime(2026, 10, 10), true);
        Habilitacion h2 = new Habilitacion("Habilitacion Comercial 2 ", "Intendencia Municipal De Montevideo (IMM)", new DateTime(2021, 10, 10), new DateTime(2026, 10, 10), true);

        List<Habilitacion> habilitaciones = new List<Habilitacion>();
        habilitaciones.Add(h1);
        habilitaciones.Add(h2);

        Ubicacion u1 = new Ubicacion("montevideo", "Calle 1 1892");

        Emprendedor e1 = new Emprendedor("1","Emprendedor S.A", habilitaciones, "montevideo","Calle 1 1892", "reciclaje");
      
        Empresa em1 = new Empresa("1","ciudad 1", "calle 1" , "reciclaje", "Empresa S.A");

        List<string> etiquetas = new List<string>();
        etiquetas.Add("etiqueta 1");

        Oferta f1 = new Oferta("1234", em1, new DateTime(2021,10,10), etiquetas, habilitaciones, "descripcion2", "titulo2", false);
        DateTime expected = new DateTime(2021,10,10);
        Assert.AreEqual(expected, f1.FechaCierre);
    }

        [Test]
        public void TestOfertaDisponibilidad()
    {
        Habilitacion h1 = new Habilitacion("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", new DateTime(2021, 10, 10), new DateTime(2026, 10, 10), true);
        Habilitacion h2 = new Habilitacion("Habilitacion Comercial 2 ", "Intendencia Municipal De Montevideo (IMM)", new DateTime(2021, 10, 10), new DateTime(2026, 10, 10), true);

        List<Habilitacion> habilitaciones = new List<Habilitacion>();
        habilitaciones.Add(h1);
        habilitaciones.Add(h2);

        Ubicacion u1 = new Ubicacion("montevideo", "Calle 1 1892");

        Emprendedor e1 = new Emprendedor("1","Emprendedor S.A", habilitaciones, "montevideo","Calle 1 1892", "reciclaje");
      
        Empresa em1 = new Empresa("1","ciudad 1", "calle 1" , "reciclaje", "Empresa S.A");

        List<string> etiquetas = new List<string>();
        etiquetas.Add("etiqueta 1");

        Oferta f1 = new Oferta("1234", em1, new DateTime(2021,10,10), etiquetas, habilitaciones, "descripcion2", "titulo2", false);
        bool expected = false;
        Assert.AreEqual(expected, f1.DisponibleConstantemente);
    }

    }
}

