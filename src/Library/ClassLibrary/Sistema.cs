using System.Collections.Generic;
using System;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Esta clase representa al sistema principal de la aplicación. Permite registrar usuarios como Empresas
    /// o Emprendedores y actúa como un contenedor de los mismos.
    /// Se aplica el patrón Singleton dentro de esta clase para permitir solo la creación y utilización
    /// de una instancia de Sistema. Además, utilizamos el principio Creator para definir a Sistema como una
    /// clase creadora de empresas y emprendedores, ya que está compuesta por ellos. Esto le permite a Sistema
    /// actuar como el contenedor de información principal de la aplicación.
    /// Enfocando de esta forma a la clase de Sistema, buscamos también aumentar la cohesión de la misma y 
    /// lograr el patrón de SRP.
    /// </summary>
    public class Sistema
    {
        /// <summary>
        /// Crea una instancia de <see cref="Sistema"/>.
        /// </summary>
        private Sistema()
        {
            this.Empresas = new List<Empresa>();
            this.Emprendedores = new List<Emprendedor>();
            this.Materiales = new List<Material>();
        }

        /// <summary>
        /// Registra una nueva <see cref="Empresa"/> en el sistema.
        /// </summary>
        public void RegistrarEmpresa(string id, string ciudad, string direccion, string rubro, string nombre)
        {
            Empresa empresa = new Empresa(id, ciudad, direccion, rubro, nombre);
            this.Empresas.Add(empresa);
        }

        /// <summary>
        /// Registra un nuevo <see cref="Emprendedor"/> en el sistema.
        /// </summary>
        public void RegistrarEmprendedor(string id, string ciudad, string direccion, string rubro, string nombre,
            List<Habilitacion> habilitaciones)
        {
            Emprendedor emprendedor = new Emprendedor(id, nombre, habilitaciones, ciudad, direccion, rubro);
            this.Emprendedores.Add(emprendedor);
        }

        /// <summary>
        /// Recupera una empresa de la lista de empresas utilizando su id y una id dada.
        /// </summary>
        /// <param name="id">Id de la empresa a recuperar.</param>
        /// <returns>La instancia de <see cref="Empresa"/> correspondiente a la id dada.</returns>
        public Empresa ObtenerEmpresaPorId(string id)
        {
            foreach (Empresa empresa in this.Empresas)
            {
                if (empresa.Id == id)
                    return empresa;
            }
            throw new KeyNotFoundException("No se encontró la empresa con el id dado.");
        }

        /// <summary>
        /// Recupera un emprendedor de la lista de emprendedores utilizando su id y una id dada.
        /// </summary>
        /// <param name="id">Id del emprendedor a recuperar.</param>
        /// <returns>La instancia de <see cref="Emprendedor"/> correspondiente a la id dada.</returns>
        public Emprendedor ObtenerEmprendedorPorId(string id)
        {
            foreach (Emprendedor emprendedor in this.Emprendedores)
            {
                if (emprendedor.Id == id)
                    return emprendedor;
            }
            throw new KeyNotFoundException("No se encontró el emprendedor con el id dado.");
        }

        public Material ObtenerMaterialPorNombre(string nombre)
        {

            foreach (Material material in this.Materiales)
            {
                if (material.Nombre == nombre)
                {
                    return material;
                }
            }
            throw new KeyNotFoundException("No se encontró el material con el nombre dado.");

        }

        /// <summary>
        /// Lista de empresas registradas en el sistema.
        /// </summary>
        /// <value>Colección de instancias de <see cref="Empresa"/></value>
        public List<Empresa> Empresas { get; set; }

        /// <summary>
        /// Lista de emprendedores registrados en el sistema.
        /// </summary>
        /// <value>Colección de instancias de <see cref="Emprendedor"/></value>
        public List<Emprendedor> Emprendedores { get; set; }

        /// <summary>
        /// Lista de materiales registrados en el sistema.
        /// </summary>
        /// <value>Colección de instancias de <see cref="Material"/></value>
        public List<Material> Materiales { get; set; }

        private static Sistema instancia = null;
        /// <summary>
        /// Instancia del sistema durante la ejecución. Se aplica el patrón Singleton.
        /// </summary>
        /// <value>Una única instancia de <see cref="Sistema"/></value>
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