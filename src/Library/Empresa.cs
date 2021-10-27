using System.Collections.Generic;

namespace ClassLibrary
{
    public class Empresa
    {
        public string Id { get; }

        public List<Oferta> Ofertas { get; }

        public Ubicacion Ubicacion { get; }

        public Rubro Rubro { get; }

        public string Nombre { get; }

        public Empresa(string id, string rubro, string nombre, string ciudad, string direccion)
        {
            this.Id = id;
            this.Ofertas = new List<Oferta>();
            this.Ubicacion = new Ubicacion(ciudad, direccion);
            this.Rubro = new Rubro(rubro);
            this.Nombre = nombre;
        }



    }
}