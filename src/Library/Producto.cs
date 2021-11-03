using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary
{
    /// <summary>
    /// Clase que representa un Producto dentro del dominio del problema. Un Producto representa una
    /// instancia de venta de un Material, conteniendo su cantidad en su unidad específica, un valor
    /// en UYU y otro en dólares asignados por su vendedor y la ubicación donde se almacena.
    /// Se aplica el patrón de SRP y se obtiene una alta cohesión dentro de la clase Producto, al
    /// asignarle únicamente las responsabilidades de contener información sobre un producto y
    /// redactarla adecuadamente y enfocando sus propiedades y métodos para estos objetivos.
    /// </summary>
    public class Producto
    {

        /// <summary>
        /// Crea una instancia de Producto
        /// </summary>
        public Producto(Material material, string ciudad, string direccion, double cantidadEnUnidad, double valorUYU, double valorUSD)
        {
            this.Material = material;
            this.Ubicacion = new Ubicacion(ciudad, direccion);
            this.CantidadEnUnidad = cantidadEnUnidad;
            this.ValorUYU = valorUYU;
            this.ValorUSD = valorUSD;
        }

        /// <summary>
        /// Instancia de material correspondiente al material ofrecido dentro de un producto
        /// </summary>
        public Material Material { get; set; }

        /// <summary>
        /// Instancia de ubicacion correspondiente a la ubicacion donde se almacena el producto
        /// </summary>
        public Ubicacion Ubicacion { get; set; }

        /// <summary>
        /// Cantidad del material en su unidad especifica
        /// </summary>
        public double CantidadEnUnidad { get; set; }

        /// <summary>
        /// Valor en pesos uruguayos del producto
        /// </summary>
        public double ValorUYU { get; set; }

        /// <summary>
        /// Valor en dólares estadounidenses del producto
        /// </summary>
        public double ValorUSD { get; set; }
        
        /// <summary>
        /// Redaccion del producto formateada
        /// </summary>
        public string Redaccion
        {
            get{
                return ($"{this.CantidadEnUnidades} {this.Material.UnidadEstandar} de {this.Material.Nombre}");
            }
        }
    }
}
