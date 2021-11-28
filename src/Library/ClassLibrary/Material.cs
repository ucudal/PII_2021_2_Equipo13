using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// La clase material se encarga de conocer lo relativo al material que consituye el producto
    /// Principios y patrones aplicados:
    /// Principio ISP: no hay objetos forzados a depender de otros objetos que no usan. 
    /// Principio SRP: tiene responsabilidad sobre una única parte de la funcionalidad, quedando completamente encapsulada dentro de la clase.
    /// Procurando que la clase tenga solo una razón para cambiar. 
    /// Patrón OCP: la clase es abierta a la extensión mediante herencia y/o composición, pero cerrada a cambios ya que no es posible y no es necesario realizar cambios en su código.
    /// </summary>
    public class Material
    {
        /// <summary>
        /// Crea una instancia vacía de <see cref="Material"/>.
        /// </summary>
        [JsonConstructor]
        public Material()
        {
        }

        /// <summary>
        /// Crea una instancia de un <see cref="Material"/>.
        /// </summary>
        /// <param name="nombre">Nombre del material.</param>
        /// <param name="unidadEstandar">Unidad estándar con la que se mide el material.</param>
        /// <param name="categorias">Categorias dentro de las que esta el material. (Opcional)</param>
        public Material(string nombre, string unidadEstandar, List<string> categorias = null)
        {
            this.Nombre = nombre;
            this.UnidadEstandar = unidadEstandar;
            if (categorias == null)
            {
                this.Categorias = new List<string>();
            }
        }

        /// <summary>
        /// Nombre del material.
        /// </summary>
        /// <value>Cadena de caracteres conteniendo el nombre del material.</value>
        public string Nombre { get; set; }
        /// <summary>
        /// Categorías en las que esta incluido el material.
        /// </summary>
        /// <value>Lista de cadena de caracteres conteniendo las categorías a las que pertenece el material.</value>
        [JsonInclude]
        public List<string> Categorias { get; set; } = new List<string>();

        /// <summary>
        /// Unidad en la que se mide el material según el Sistema Internacional de Unidades.
        /// </summary>
        /// <value>Cadena de caracteres conteniendo la unidad estándar del material en el Sistema Internacional de Unidades.</value>
        public string UnidadEstandar { get; set; }
    }
}