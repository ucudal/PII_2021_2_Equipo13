using System;
using NUnit.Framework;
using ClassLibrary;
using System.Collections.Generic;


[TestClass]
public class TestEmpresa
{
    [Test]
    public void TestEmpresa()
    {

        List<Oferta> ofertas = new List<Oferta>();

        Rubro r1 = new Rubro("Reciclaje");

        Ubicacion u1 = new Ubicacion("Montevideo", "Calle 13");
        Empresa e1 = new Empresa("Empresa S.A", ofertas, u1, r1, "Empresa S.A");
        string expected = "Empresa S.A";
        Assert.AreEqual(expected, e1.Nombre);
    }
}