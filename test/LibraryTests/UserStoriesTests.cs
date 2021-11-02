using ClassLibrary;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests
{
    /// <summary>
    /// Pruebas para demostrar el cumplimiento de las historias de usuario.
    /// </summary>
    [TestFixture]
    public class UserStoriesTests
    {
        /// <summary>
        /// Prueba para las historias de usuario 1 y 2; registro de una empresa en el sistema.
        /// </summary>
        [Test]
        public void RegistroDeEmpresaTest()
        {
            string id = "123456";                                   // Id de una empresa, se planea obtener el ID de usuario de Telegram.
            string nombre = "Empresa";                              // Nombre de la empresa.
            string ciudad = "Montevideo";                           // Ciudad de la empresa.
            string direccion = "Av. Luis Albert de Herrera 2890";   // Calle y número de puerta de la empresa.
            Rubro rubro = new Rubro("Tecnología");                  // Rubro de la empresa.
            Sistema.Instancia.RegistrarEmpresa(id, ciudad, direccion, rubro, nombre);
            
            int cantidadEmpresas = 1;                               // Cantidad de empresas registradas en el sistema.
            Assert.AreEqual(cantidadEmpresas, Sistema.Instancia.Empresas.Count);

            Empresa empresa = Sistema.Instancia.Empresas[0];       // Empresa registrada en el sistema.
            Assert.AreEqual(empresa, Sistema.Instancia.ObtenerEmpresaPorId(id));
            Assert.AreEqual(empresa.Ubicacion, Sistema.Instancia.ObtenerEmpresaPorId(id).Ubicacion);
        }

        /// <summary>
        /// Prueba para la historia de usuario 3; publicación de oferta.
        /// </summary>
        [Test]
        public void PublicarOfertaTest()
        {
            Empresa empresa = Sistema.Instancia.ObtenerEmpresaPorId("123456");
            string idOferta = empresa.Id + "-1";

            Oferta oferta = empresa.PublicarOferta(idOferta, new DateTime(2021, 12, 10), new List<string>(), new List<Habilitacion>(), 
                "Una oferta de cosas", "Oferta");

            int cantidadOfertas = 1;
            Assert.AreEqual(cantidadOfertas, empresa.Ofertas.Count);
            Assert.AreEqual(oferta, empresa.ObtenerOfertaPorId(oferta.Id));

            
        }
           
    }
}