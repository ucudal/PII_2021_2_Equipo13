using System;
using NUnit.Framework;
using ClassLibrary;
using System.Collections.Generic;

namespace TestLibrary{


[TestFixture]
public class TestHabilitaciones
{
        [Test]
        public void TestHabilitacionesDesc()
        {
            Habilitacion h1 = new Habilitacion("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", new DateTime(2021, 7, 10), new DateTime(2026, 9, 20), true);
            string expected = "Habilitacion Comercial";
            Assert.AreEqual(expected, h1.Descripcion);
        }
        [Test]
        public void TestHabilitacionesInst()
        {
            Habilitacion h1 = new Habilitacion("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", new DateTime(2021, 7, 10), new DateTime(2026, 9, 20), true);
            string expected = "Intendencia Municipal De Montevideo (IMM)";
            Assert.AreEqual(expected, h1.NombreInsitucionHabilitada);
        }
        [Test]
        public void TestHabilitacionesFechaTram()
        {
            Habilitacion h1 = new Habilitacion("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", new DateTime(2021, 7, 10), new DateTime(2026, 9, 20), true);
            DateTime expected = new DateTime(2021, 7, 10);
            Assert.AreEqual(expected, h1.FechaTramite);
        }
        [Test]
        public void TestHabilitacionesFechaVenc()
        {
            Habilitacion h1 = new Habilitacion("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", new DateTime(2021, 7, 10), new DateTime(2026, 9, 20), true);
            DateTime expected = new DateTime(2026, 9, 20);
            Assert.AreEqual(expected, h1.FechaVencimiento);
        }

        [Test]
        public void TestHabilitacionesEstado()
        {
            Habilitacion h1 = new Habilitacion("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", new DateTime(2021, 7, 10), new DateTime(2026, 9, 20), true);
            bool expected = true;
            Assert.AreEqual(expected, h1.Vigente);
        }
    }
}