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
        /// <summary>
        /// Constructor de la clase Producto.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="material"></param>
        /// <param name="ubicacion"></param>
        public Producto(string nombre, Material material, Ubicacion ubicacion, double cantidadEnUnidad,double valorUYU,double valorUSD)
        {
            this.Nombre = nombre;
            this.Material = material;
            this.Ubicacion = ubicacion;
            this.CantidadEnUnidad = cantidadEnUnidad;
            this.ValorUYU = valorUYU;
            this.ValorUSD = valorUSD;
        }
        
        /// <summary>
        /// Nombre del producto.
        /// </summary>
        /// <value></value>
        public string Nombre { get; set; }
    
        /// <summary>
        /// Una implementacion de la clase material, con el material del que esta compuesto el producto.
        /// </summary>
        /// <value></value>
       public Material Material{get; set;} 

        /// <summary>
        /// Una implementacion de la clase ubicacion, con la ubicacion del material.
        /// Utilizo el principio de creador porque creo instancias de ubicacion y material. ya que guarda y contiene instancias de sus objetos
        /// </summary>
        /// <value></value>
     
        public Ubicacion Ubicacion{get; set;}      
        
        /// <summary>
        /// Esto representara la cantidad del producto en su propia unidad, la unidad esta en el maetrial
        /// </summary>
        /// <value></value>
        public double CantidadEnUnidad{get; set;}   
        /// <summary>
        /// Valor en Pesos Uruguayos del producto
        /// </summary>
        /// <value></value>
        
        public double ValorUYU{get; set;}           
        
        /// <summary>
        /// Valor en Dolar del producto
        /// </summary>
        /// <value></value>
        public double ValorUSD{get; set;} 
    
    }
}
