using System;
using NUnit.Framework;
using ClassLibrary;

using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class EmprendedorYHabilitacionTests
    {
        // Lucas Giffuni
        [Test]
        public void TestHabilitacionDesc()
        {
            Habilitacion h1 = new Habilitacion("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", "10-07-2021", "20-09-2026", true);
            string expected = "Habilitacion Comercial";
            Assert.AreEqual(expected, h1.descripcion);
        }

        [Test]
        public void TestHabilitacionInst()
        {
            Habilitacion h1 = new Habilitacion("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", "10-07-2021", "20-09-2026", true);
            string expected = "Intendencia Municipal De Montevideo (IMM)";
            Assert.AreEqual(expected, h1.nombreInsitucionHabilitada);
        }

        [Test]
        public void TestHabilitacionFechaTram()
        {
            Habilitacion h1 = new Habilitacion("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", "10-07-2021", "20-09-2026", true);
            string expected = "10-07-2021";
            Assert.AreEqual(expected, h1.fechaTramite);
        }

        [Test]
        public void TestHabilitacionFechaVenc()
        {
            Habilitacion h1 = new Habilitacion("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", "10-07-2021", "20-09-2026", true);
            string expected = "10-07-2021";
            Assert.AreEqual(expected, h1.fechaTramite);
        }

        [Test]
        public void TestHabilitacionEstado()
        {
            Habilitacion h1 = new Habilitacion("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", "10-07-2021", "20-09-2026", true);
            bool expected = true;
            Assert.AreEqual(expected, h1.estado);
        }

        [Test]
        public void TestEmprendedor()
        {
            Habilitacion h1 = new Habilitacion("Habilitacion Comercial", "Intendencia Municipal De Montevideo (IMM)", "10-07-2021", "20-09-2026", true);
            Habilitacion h2 = new Habilitacion("Habilitacion Comercial 2 ", "Intendencia Municipal De Montevideo (IMM)", "10-07-2021", "20-09-2026", true);

            List<Habilitacion> Habilitaciones = new List<Habilitacion>();
            Habilitaciones.Add(h1);
            Habilitaciones.Add(h2);

            Rubro r1 = new Rubro("Reciclaje");

            Emprendedor e1 = new Emprendedor("123", "Emprendedor S.A ", Habilitaciones, "Ciudad", "Calle 1 1892", r1);
            bool expected = true;
            Assert.AreEqual(expected, expected);
        }
    }
}