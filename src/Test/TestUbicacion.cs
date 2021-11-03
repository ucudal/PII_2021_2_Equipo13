using System;
using NUnit.Framework;
using ClassLibrary;
using System.Collections.Generic;


[TestClass]
public class TestUbicacion
{

    [Test]
    public void TestUbicacionDireccion()
    {
        Ubicacion u1 = new Ubicacion("Montevideo", "Calle 1 1892");
        string expected = "Calle 1 1892";
        Assert.AreEqual(expected, u1.Direccion);
    }

    [Test]
    public void TestUbicacionCiudad()
    {
        Ubicacion u1 = new Ubicacion("Montevideo", "Calle 13");
        string expected = "Montevideo";
        Assert.AreEqual(expected, u1.Ciudad);
    }
}