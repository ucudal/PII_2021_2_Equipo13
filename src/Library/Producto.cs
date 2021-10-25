using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary
{
    abstract class Producto
    {
        string Material="";        //Esto deveria ser un metodod que chequeara la clase material//
        string Ubicacion="";       //Esto deveria ser un metodod que chequeara la clase Ubicacion y determinara la ubicaccion del material//
        int cantidad=new int();
        int Valor_Pesos=new int();
        int Valor_Dolares=new int();
    }






}