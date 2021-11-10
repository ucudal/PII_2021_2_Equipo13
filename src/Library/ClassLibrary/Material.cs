using System;
using System.Collections.Generic;

namespace PII_E13.ClassLibrary
{
        /// <summary>
        /// Principios y patrones aplicados:
        /// Principio ISP: no hay objetos forzados a depender de otros objetos que no usan. 
        /// Principio SRP: tiene responsabilidad sobre una única parte de la funcionalidad, quedando completamente encapsulada dentro de la clase.
        /// Procurando que la clase tenga solo una razón para cambiar. 
        /// Patrón OCP: la clase es abierta a la extensión mediante herencia y/o composición, pero cerrada a cambios ya que no es posible y no es necesario realizar cambios en su código.
        /// </summary>
    public class Material
    {
        /// <summary>
        /// La clase material se encarga de conocer lo relativo al material que consituye el producto.
        /// </summary>
        /// <param name="nombre">nombre del material</param>
        /// <param name="categorias">categorias dentro de las que esta el material</param>
        /// <param name="unidadEstandar">unidad Estandar con la que se mide el material</param>
        public Material(string nombre, List <string> categorias, string unidadEstandar)
        {
            this.Nombre = nombre;
            this.Categorias = categorias;
            this.UnidadEstandar = unidadEstandar;
        }

        /// <summary>
        /// Nombre del material.
        /// </summary>
        /// <value></value>
        public string Nombre {get;}
        /// <summary>
        /// Categorías en las que esta incluido el material.
        /// </summary>
        /// <value></value>
        public List <string> Categorias {get;}

        /// <summary>
        /// Unidad en la que se mide el material.
        /// </summary>
        /// <value></value>
        public string UnidadEstandar {get; }
    }
}