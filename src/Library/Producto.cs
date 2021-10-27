using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary
{
    abstract class Producto
    {
        public Material Material{get; set;}       //Esto deveria ser un metodod que chequeara la clase material//
        public Ubicacion Ubicacion{get; set;}      //Esto deveria ser un metodod que chequeara la clase Ubicacion y determinara la ubicaccion del material//
        public double CantidadEnUnidad{get; set;}   //Esto representara la cantidad del producto en su propia unidad, la unidad esta en el maetrial//
        public double ValorUYU{get; set;}
        public double ValorUSD{get; set;}
    }






}