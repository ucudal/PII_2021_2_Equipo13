using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary
{
    /// <summary>
    /// Esta es la clase Producto. se encarga de almacenar los datos de un producto.
    /// Implementa la clase Material y Ubicacion para almacenar los datos de ubicacion y material, y contiene la cantidad de producto en unidades corresopondientes.
    /// Tambien tien el precios del producto en pesos y en dolares.
    /// </summary>
    public class Producto
    {
        public Material Material { get; set; }       //Esto deberia ser un metodo que chequeara la clase material//
        public Ubicacion Ubicacion { get; set; }      //Esto deberia ser un metodo que chequeara la clase Ubicacion y determinara la ubicacion del material//
        public double CantidadEnUnidad { get; set; }   //Esto representara la cantidad del producto en su propia unidad, la unidad esta en el maetrial//
        public double ValorUYU { get; set; }           //Valor en Pesos Uruguayos del producto//
        public double ValorUSD { get; set; }           //Valor en Dolares del producto//
    }
}