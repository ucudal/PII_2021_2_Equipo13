using System;
using NUnit.Framework;
using ClassLibrary;


using System.Collections.Generic;
[TestFixture]
public class EmprendedorTests
{
    [Test]
    public void TestEmprendedor()
    {
        Habilitacion h1 = new Habilitacion("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", new DateTime(2021, 10, 10), new DateTime(2026, 10, 10), true);
        Habilitacion h2 = new Habilitacion("Habilitacion Comercial 2 ", "Intendencia Municipal De Montevideo (IMM)", new DateTime(2021, 10, 10), new DateTime(2026, 10, 10), true);

        List<Habilitacion> habilitaciones = new List<Habilitacion>();
        habilitaciones.Add(h1);
        habilitaciones.Add(h2);

        Ubicacion u1 = new Ubicacion("montevideo", "Calle 1 1892");

        Emprendedor e1 = new Emprendedor("1","Emprendedor S.A", habilitaciones, "montevideo","Calle 1 1892", "reciclaje");
        string expectedName = "Emprendedor S.A";
        Assert.AreEqual(expectedName, e1.Nombre);
    }


    public void testPostularOferta()
    {
         Habilitacion h1 = new Habilitacion("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", new DateTime(2021, 10, 10), new DateTime(2026, 10, 10), true);
        Habilitacion h2 = new Habilitacion("Habilitacion Comercial 2 ", "Intendencia Municipal De Montevideo (IMM)", new DateTime(2021, 10, 10), new DateTime(2026, 10, 10), true);

        List<Habilitacion> habilitaciones = new List<Habilitacion>();
        habilitaciones.Add(h1);
        habilitaciones.Add(h2);

        Ubicacion u1 = new Ubicacion("montevideo", "Calle 1 1892");

        Emprendedor e1 = new Emprendedor("1","Emprendedor S.A", habilitaciones, "montevideo","Calle 1 1892", "reciclaje");
      
        Empresa em1 = new Empresa("1","ciudad 1", "calle 1" , "reciclaje", "Empresa S.A");

        List<Oferta> ofertas = new List<Oferta>();
        List<string> etiquetas = new List<string>();
        etiquetas.Add("etiqueta 1");

        ofertas.Add(new Oferta("1234", em1, new DateTime(2021,10,10), etiquetas, habilitaciones, "descripcion1", "titulo1", true));
        ofertas.Add(new Oferta("1234", em1, new DateTime(2021,10,10), etiquetas, habilitaciones, "descripcion2", "titulo2", false));
        e1.PostularseAOferta(ofertas);

        List<Oferta> expected = ofertas;
        Assert.AreEqual(expected, e1.OfertasPostuladas);

    }
}