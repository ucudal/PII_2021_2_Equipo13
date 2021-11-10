using PII_E13.ClassLibrary;
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
        /// Prueba para las historias de usuario 1 y 2: registro de una empresa en el sistema.
        /// </summary>
        [Test]
        public void RegistroDeEmpresaTest()
        {
            string id = "123456";                                   // Se planea obtener el ID de usuario de Telegram para los IDs de las empresas.
            string nombre = "Aserradero El Madero";
            string ciudad = "Montevideo";
            string direccion = "Av. Luis Albert de Herrera 2890";
            string rubro = "Maderera";

            int cantidadEmpresasEsperada = Sistema.Instancia.Empresas.Count + 1;
            Sistema.Instancia.RegistrarEmpresa(id, ciudad, direccion, rubro, nombre);

            Assert.AreEqual(cantidadEmpresasEsperada, Sistema.Instancia.Empresas.Count);

            Empresa empresa = Sistema.Instancia.Empresas[0];       // Empresa registrada en el sistema.
            Assert.AreEqual(empresa, Sistema.Instancia.ObtenerEmpresaPorId(id));
            Assert.AreEqual(empresa.Ubicacion, Sistema.Instancia.ObtenerEmpresaPorId(id).Ubicacion);
        }

        /// <summary>
        /// Prueba para las historias de usuario 3, 4, 5 y 6:
        ///  - Publicación de ofertas
        ///  - Información de materiales y materiales en oferta (productos)
        ///  - Información de habilitaciones requeridas
        ///  - Inclusión de palabras clave en ofertas
        /// </summary>
        [Test]
        public void PublicarOfertaTest()
        {
            string id = "123456";                                   // Se planea obtener el ID de usuario de Telegram para los IDs de las empresas.
            string nombre = "Aserradero El Madero";
            string ciudad = "Montevideo";
            string direccion = "Av. Luis Albert de Herrera 2890";
            string rubro = "Maderera";
            Sistema.Instancia.RegistrarEmpresa(id, ciudad, direccion, rubro, nombre);
            Empresa empresa = Sistema.Instancia.ObtenerEmpresaPorId("123456");
            string idOferta = empresa.Id + "-123";

            int cantidadOfertasEsperada = empresa.Ofertas.Count + 1;

            Oferta oferta = empresa.PublicarOferta(idOferta, new DateTime(2021, 12, 10), new List<string>() {"madera barata", "madera húmeda",
                "en buen estado", "barato", "reutilizable" }, new List<Habilitacion>(),
                "Roble de muy buena calidad que estuvo expuesto a la humedad por extendidos periodos de tiempo. Se mantiene en buen estado",
                "Roble húmedo", false);

            Assert.AreEqual(cantidadOfertasEsperada, empresa.Ofertas.Count);
            Assert.AreSame(oferta, empresa.ObtenerOfertaPorId(idOferta));

            oferta.AgregarProducto(
                new Material("Roble", new List<string>() { "madera", "inflamable", "carpintería" }, "Kg"),
                "Montevideo", "Av. Luis Albert de Herrera 2890", 3000, 40000, 800);

            int cantidadProductos = 1;
            Assert.AreEqual(cantidadProductos, oferta.Productos.Count);
        }

        /// <summary>
        /// Prueba para las historias de usuario 1 y 7: registro de un emprendedor en el sistema.
        /// </summary>
        [Test]
        public void RegistroDeEmprendedorTest()
        {
            string id = "123457";                                   // Se planea obtener el ID de usuario de Telegram para los IDs de las empresas.
            string nombre = "Carpintería Gilberto e Hijos";
            string ciudad = "Montevideo";
            string direccion = "Av. Gral Flores 1560";
            string rubro = "Carpintería";

            int cantidadEmprendedoresEsperada = Sistema.Instancia.Emprendedores.Count + 1;
            Sistema.Instancia.RegistrarEmprendedor(id, ciudad, direccion, rubro, nombre, new List<Habilitacion>());

            Assert.AreEqual(cantidadEmprendedoresEsperada, Sistema.Instancia.Emprendedores.Count);

            Emprendedor emprendedor = Sistema.Instancia.Emprendedores[0];   // Emprendedor registrado en el sistema.
            Assert.AreSame(emprendedor, Sistema.Instancia.ObtenerEmprendedorPorId(id));
            Assert.AreSame(emprendedor.Ubicacion, Sistema.Instancia.ObtenerEmprendedorPorId(id).Ubicacion);
        }

        /// <summary>
        /// Prueba para la historia de usuario 8: Búsqueda de ofertas.
        /// </summary>
        [Test]
        public void BuscarOfertasTest()
        {
            string id = "123457";                                   // Se planea obtener el ID de usuario de Telegram para los IDs de las empresas.
            string nombre = "Carpintería Gilberto e Hijos";
            string ciudad = "Montevideo";
            string direccion = "Av. Gral Flores 1560";
            string rubro = "Carpintería";
            Sistema.Instancia.RegistrarEmprendedor(id, ciudad, direccion, rubro, nombre, new List<Habilitacion>());

            id = "123456";                                          // Se planea obtener el ID de usuario de Telegram para los IDs de las empresas.
            nombre = "Aserradero El Madero";
            ciudad = "Montevideo";
            direccion = "Av. Luis Albert de Herrera 2890";
            rubro = "Maderera";
            Sistema.Instancia.RegistrarEmpresa(id, ciudad, direccion, rubro, nombre);
            Empresa empresa = Sistema.Instancia.ObtenerEmpresaPorId("123456");

            this.GenerarOfertas(empresa);

            List<string> etiquetas = new List<string>() { "madera", "barato", "reutilizable", "carpintería" };
            List<string> categorias = new List<string>() { "madera" };
            Sistema sistema = Sistema.Instancia;
            List<Oferta> ofertas = Buscador.Instancia.BuscarOfertas(sistema, sistema.ObtenerEmprendedorPorId("123457"), etiquetas, categorias);

            Oferta ofertaEsperada = sistema.ObtenerEmpresaPorId("123456").ObtenerOfertaPorId("123456-1"); // Esta oferta debería ser la más relevante para el emprendedor.
            Assert.AreSame(ofertaEsperada, ofertas[0]);
        }

        /// <summary>
        /// Método para cargar varias ofertas de ejemplo en una empresa.
        /// </summary>
        private void GenerarOfertas(Empresa empresa)
        {
            string idOferta = empresa.Id + "-1";

            Oferta oferta = empresa.PublicarOferta(idOferta, new DateTime(2021, 12, 10), new List<string>() {"madera barata", "madera húmeda",
                "en buen estado", "barato", "reutilizable" }, new List<Habilitacion>(),
                "Roble de muy buena calidad que estuvo expuesto a la humedad por extendidos periodos de tiempo. Se mantiene en buen estado",
                "Roble húmedo", false);

            oferta.AgregarProducto(
                new Material("Roble", new List<string>() { "madera", "inflamable", "carpintería" }, "Kg"),
                "Montevideo", "Av. Luis Albert de Herrera 2890", 3000, 40000, 800);


            idOferta = empresa.Id + "-2";

            oferta = empresa.PublicarOferta(idOferta, new DateTime(2021, 11, 30), new List<string>(), new List<Habilitacion>(),
                "Prendas y uniformes utilizados en un aserradero durante varios años.",
                "Prendas gastadas", true);

            oferta.AgregarProducto(
                new Material("Tela", new List<string>() { "prendas", "inflamable" }, "Kg"),
                "Montevideo", "Av. Luis Albert de Herrera 2890", 50, 20000, 450);


            idOferta = empresa.Id + "-3";
            oferta = empresa.PublicarOferta(idOferta, new DateTime(2021, 11, 25), new List<string>(), new List<Habilitacion>(),
                "Oferta de materiales.",
                "Oferta", false);

            oferta.AgregarProducto(
                new Material("Una cosa", new List<string>() { }, "m³"),
                "Montevideo", "Av. Luis Albert de Herrera 2890", 200, 30000, 600);
        }

        /// <summary>
        /// Prueba para la historia de usuario 9 y 10: Recurrencia de ofertas
        /// </summary>
        [Test]
        public void OfertaRecurrenteTest()
        {
            string id = "123456";                                   // Se planea obtener el ID de usuario de Telegram para los IDs de las empresas.
            string nombre = "Aserradero El Madero";
            string ciudad = "Montevideo";
            string direccion = "Av. Luis Albert de Herrera 2890";
            string rubro = "Maderera";
            Sistema.Instancia.RegistrarEmpresa(id, ciudad, direccion, rubro, nombre);
            Empresa empresa = Sistema.Instancia.ObtenerEmpresaPorId("123456");
            string idOferta = empresa.Id + "-1234";

            Oferta oferta = empresa.PublicarOferta(idOferta, new DateTime(2021, 12, 10), new List<string>() {"madera barata", "madera húmeda",
                "en buen estado", "barato", "reutilizable" }, new List<Habilitacion>(),
                "Roble de muy buena calidad que estuvo expuesto a la humedad por extendidos periodos de tiempo. Se mantiene en buen estado",
                "Roble húmedo", false);

            oferta.AgregarProducto(
                new Material("Roble", new List<string>() { "madera", "inflamable", "carpintería" }, "Kg"),
                "Montevideo", "Av. Luis Albert de Herrera 2890", 3000, 40000, 800);

            Assert.AreEqual(oferta.Estado, Oferta.Estados.Habilitada); // La oferta debería estar habilitada por determinado.
        }

        /// <summary>
        /// Prueba para la historia de usuario 11: Ofertas entregadas
        /// </summary>
        [Test]
        public void OfertasEntregadasTest()
        {
            string id = "123456";                                          // Se planea obtener el ID de usuario de Telegram para los IDs de las empresas.
            string nombre = "Aserradero El Madero";
            string ciudad = "Montevideo";
            string direccion = "Av. Luis Albert de Herrera 2890";
            string rubro = "Maderera";
            Sistema.Instancia.RegistrarEmpresa(id, ciudad, direccion, rubro, nombre);
            Empresa empresa = Sistema.Instancia.ObtenerEmpresaPorId("123456");

            this.GenerarOfertas(empresa);
            string idOferta = empresa.Id + "-3";
            empresa.ObtenerOfertaPorId(idOferta).Estado = Oferta.Estados.Entregada;
            List<Oferta> ofertasEntregadas = new List<Oferta>();
            List<Oferta> ofertasPropias = empresa.VerOfertasPropias(new DateTime(2021, 11, 1), new DateTime(2021, 12, 2));

            foreach (Oferta oferta in ofertasPropias)
            {
                if (oferta.Estado == Oferta.Estados.Entregada)
                {
                    ofertasEntregadas.Add(oferta);
                }
            }
            Assert.AreEqual(idOferta, ofertasEntregadas[0].Id);
        }

        /// <summary>
        /// Prueba para la historia de usuario 12: Ofertas consumidas por emprendedor
        /// </summary>
        [Test]
        public void OfertasConsumidasTest()
        {
            string id = "123456";                                          // Se planea obtener el ID de usuario de Telegram para los IDs de las empresas.
            string nombre = "Aserradero El Madero";
            string ciudad = "Montevideo";
            string direccion = "Av. Luis Albert de Herrera 2890";
            string rubro = "Maderera";
            Sistema.Instancia.RegistrarEmpresa(id, ciudad, direccion, rubro, nombre);
            Empresa empresa = Sistema.Instancia.ObtenerEmpresaPorId("123456");

            this.GenerarOfertas(empresa);

            id = "123457";                                   // Se planea obtener el ID de usuario de Telegram para los IDs de las empresas.
            nombre = "Carpintería Gilberto e Hijos";
            ciudad = "Montevideo";
            direccion = "Av. Gral Flores 1560";
            rubro = "Carpintería";

            Sistema.Instancia.RegistrarEmprendedor(id, ciudad, direccion, rubro, nombre, new List<Habilitacion>());

            Emprendedor emprendedor = Sistema.Instancia.ObtenerEmprendedorPorId(id);

            string idOferta = empresa.Id + "-3";
            emprendedor.OfertasConsumidas.Add(empresa.ObtenerOfertaPorId(idOferta));

            Oferta ofertaConsumida = emprendedor.VerOfertasConsumidas(new DateTime(2021, 11, 1), new DateTime(2021, 12, 2))[0];
            Assert.AreEqual(ofertaConsumida, empresa.ObtenerOfertaPorId(idOferta));
        }
    }
}