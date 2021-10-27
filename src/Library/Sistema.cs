using System.Collections.Generic;
using System;

namespace ClassLibrary
{
    /// <summary>
    /// Esta clase representa al sistema principal de la aplicación. Permite registrar usuarios como
    /// Empresas o Emprendedores y actúa como un contenedor de los mismos.
    /// Se aplica el patrón Singleton dentro de esta clase para permitir solo la creación y 
    /// utilización de una instancia de Sistema.
    /// </summary>
    public class Sistema
    {
        /// <summary>
        /// Crea una instancia de Sistema.
        /// </summary>
        private Sistema()
        {
            this.Empresas = new List<Empresa>();
            this.Emprendedores = new List<Emprendedor>();
        }

        /// <summary>
        /// Registra una nueva empresa en el sistema.
        /// </summary>
        public void RegistrarEmpresa(string id, string ciudad, string direccion, string rubro, string nombre)
        {
            Empresa empresa = new Empresa(id, ciudad, direccion, rubro, nombre);
            this.Empresas.Add(empresa);
        }

        /// <summary>
        /// Registra un nuevo emprendedor en el sistema.
        /// </summary>
        public void RegistrarEmprendedor(string id, string ciudad, string direccion, string rubro, string nombre,
            List<Habilitacion> habilitaciones)
        {
            Emprendedor emprendedor = new Emprendedor(id, nombre, habilitaciones, ciudad, direccion, rubro);
            this.Emprendedores.Add(emprendedor);
        }

        /// <summary>
        /// Lista de empresas registradas en el sistema.
        /// </summary>
        /// <value>Colección de instancias de <c>Empresa</c></value>
        public List<Empresa> Empresas { get; set; }

        /// <summary>
        /// Lista de emprendedores registrados en el sistema.
        /// </summary>
        /// <value>Colección de instancias de <c>Emprendedor</c></value>
        public List<Emprendedor> Emprendedores { get; set; }

        private static Sistema instancia = null;
        /// <summary>
        /// Instancia del sistema durante la ejecución. Se aplica el patrón Singleton.
        /// </summary>
        /// <value>Una única instancia de <c>Sistema</c></value>
        public static Sistema Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new Sistema();
                }
                return instancia;
            }
        }
    }
}