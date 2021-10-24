using System.Collections.Generic;

namespace Library
{
    public class Empresa
    {
        public string Id { get; }
        
        public List<Oferta> Ofertas { get; }

        public Ubicacion Ubicacion { get; }
        
        public string Rubro { get; } 
        
        public string Nombre { get; }
        
        public Empresa(string id, List<Oferta> ofertas, Ubicacion ubicacion, string rubro, string nombre)
        {
            Id = id;
            Ofertas = ofertas;
            Ubicacion = ubicacion;
            Rubro = rubro;
            Nombre = nombre;
        }
        
        

    }
}