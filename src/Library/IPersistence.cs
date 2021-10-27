using System;
using System.Collections.Generic;
     

namespace ClassLibrary
{
     public interface IPersistence
     {
        void EscribirArchivo(string archivo, string informacion); //Estos representari la informacion que se esta buscando, todavia no tengo claro si sera en forma de string o una clase dedicada//
    
        string LeerArchivo(string archivo);  //Esto recviria una string con la info buscada, y la lista en donde buscarlo//
        
    
     }
}