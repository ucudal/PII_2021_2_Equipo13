using System;
using NUnit.Framework;
using ClassLibrary;
using System.Collections.Generic;


[TestClass]
public class Tests
{

    // Lucas Giffuni
    [Test]
    public void TestHabilitacionesDesc()
    {
        Habilitaciones h1 = new Habilitaciones("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", "10-07-2021", "20-09-2026", true);
        string expected = "Habilitacion Comercial";
        Assert.AreEqual(expected, h1.descripcion);
    }
    [Test]
    public void TestHabilitacionesInst()
    {
        Habilitaciones h1 = new Habilitaciones("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", "10-07-2021", "20-09-2026", true);
        string expected = "Intendencia Municipal De Montevideo (IMM)";
        Assert.AreEqual(expected, h1.nombreInsitucionHabilitada);
    }
    [Test]
    public void TestHabilitacionesFechaTram()
    {
        Habilitaciones h1 = new Habilitaciones("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", "10-07-2021", "20-09-2026",true);
        string expected = "10-07-2021";
        Assert.AreEqual(expected, h1.fechaTramite);
    }
    [Test]
    public void TestHabilitacionesFechaVenc()
    {
        Habilitaciones h1 = new Habilitaciones("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", "10-07-2021", "20-09-2026", true);
        string expected = "10-07-2021";
        Assert.AreEqual(expected, h1.fechaTramite);
    }

    [Test]
    public void TestHabilitacionesEstado()
    {
        Habilitaciones h1 = new Habilitaciones("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", "10-07-2021", "20-09-2026", true);
        string expected = true;
        Assert.AreEqual(expected, h1.estado);
    }



  

    [Test]
    public void TestEmprendedor()
    {
        Habilitaciones h1 = new Habilitaciones("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", "10-07-2021", "20-09-2026", true);
        Habilitaciones h2 = new Habilitaciones("Habilitacion Comercial 2 ", "Intendencia Municipal De Montevideo (IMM)", "10-07-2021", "20-09-2026", true);

        List<Habilitacion> habilitaciones = new List<Habilitacion>();
        habilitaciones.Add(h1);
        habilitaciones.Add(h2);


        Rubro r1 = new Rubro("Reciclaje");
        Rubro r2 = new Rubro("Compostaje");

        List<Rubro> rubros = new List<Rubro>();
        rubros.Add(r1);
        rubros.Add(r2);



        Emprendedor e1 = new Emprendedor("Emprendedor S.A ", habilitaciones, "Calle 1 1892", rubros);
        string expected = true;
        Assert.AreEqual(expected, expected);
    }

}
