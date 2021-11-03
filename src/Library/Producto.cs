using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary
{
    /// <summary>
    /// Esta es la clase Producto. se encarga de almacenar los datos de un producto.
    /// Implementa el principio de responsabilidad única. su unica funcion es el crear un producto y almacenar sus datos.
    /// Esta clase tiene acoplaje con Material y Ubicacion ya que depende significativamente de ellas.
    /// </summary>
    public class Producto
    {      
        public Producto(Material material, Ubicacion ubicacion, double cantidadEnUnidades, double valorUYU, double valorUSD)
        {
            this.Material = material;
            this.Ubicacion = ubicacion;
            this.CantidadEnUnidades = cantidadEnUnidades;
            this.ValorUYU = valorUYU;
            this.ValorUSD = valorUSD;
        } 
       public Material Material{get; set;} 
        /// <summary>
        /// Una implementacion de la clase material, con el material del que esta compuesto el producto.
        /// </summary>
        /// <value></value>
     
        public Ubicacion Ubicacion{get; set;}      
        /// <summary>
        /// Una implementacion de la clase ubicacion, con la ubicacion del material.
        /// Utilizo el principio de creador porque creo instancias de ubicacion y material. ya que guarda y contiene instancias de sus objetos
        /// </summary>
        /// <value></value>
        
        public double CantidadEnUnidades{get; set;}   
        /// <summary>
        /// Esto representara la cantidad del producto en su propia unidad, la unidad esta en el maetrial
        /// </summary>
        /// <value></value>
        
        public double ValorUYU{get; set;}           
        /// <summary>
        /// Valor en Pesos Uruguayos del producto
        /// </summary>
        /// <value></value>
        
        public double ValorUSD{get; set;} 
        /// <summary>
        /// Valor en Dolar del producto
        /// </summary>
        /// <value></value>

        public string Redaccion
        {
            get{
                return ($"{this.CantidadEnUnidades} {this.Material.UnidadEstandar} de {this.Material.Nombre}");
            }
        }
    }
}
