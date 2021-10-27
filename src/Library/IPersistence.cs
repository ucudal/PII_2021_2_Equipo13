using System;
using System.Collections.Generic;
     

namespace ClassLibrary
{
     public interface IPersistence
     {
        void recivirPedido(string pedido); //Estos representari la informacion que se esta buscando, todavia no tengo claro si sera en forma de string o una clase dedicada//
    
        void checkInformacion(string informacion,string objetivo); //Esto recviria una string con la info buscada, y la lista en donde buscarlo//
        
    
     }
}