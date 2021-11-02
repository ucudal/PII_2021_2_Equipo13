using System;

namespace ClassLibrary
{
        /// <summary>
        /// Principios y patrones aplicados:
        /// Cumple con el principio ISP ya que no hay objetos forzados a depender de otros
        ///  objetos que no usan.
        /// Cumple con el patrón OCP pues la clase es abierta a la extensión mediante herencia
        ///  y/o composición, pero cerrada a cambios ya que no es posible y no es necesario realizar
        ///  cambios en su código.
        /// La clase cumple con el Principio SRP ya que tiene responsabilidad sobre una única
        /// parte de la funcionalidad, quedando completamente encapsulada dentro de la clase. 
        /// Procurando que la clase tenga solo una razón para cambiar. 
        /// </summary>
    public class Material
    {
        /// <summary>
        /// La clase material se encarga de conocer lo relativo al material que consituye el producto.
        /// </summary>
        /// <param name="nombre">nombre</param>
        /// <param name="categorias">categorias</param>
        /// <param name="unidadEstandar">unidadEstandar</param>
        public Material(string nombre, List <string> categorias, string unidadEstandar)
        {
            this.Nombre = nombre;
            this.Categorias = categorias;
            this.UnidadEstandar = unidadEstandar;
        }
        public string Nombre {get;}
        public List <string> Categorias {get;}
        public string UnidadEstandar {get; }
    }
}
