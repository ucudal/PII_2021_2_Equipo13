using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary
{
    abstract class Producto
    {
        public Material Material{get; set;}       //Esto deberia ser un metodo que chequeara la clase material//
        public Ubicacion Ubicacion{get; set;}      //Esto deberia ser un metodo que chequeara la clase Ubicacion y determinara la ubicacion del material//
        public double CantidadEnUnidad{get; set;}   //Esto representara la cantidad del producto en su propia unidad, la unidad esta en el maetrial//
        public double ValorUYU{get; set;}           //Valor en Pesos Uruguayos del producto//
        public double ValorUSD{get; set;}           //Valor en Dolares del producto//
    }






}