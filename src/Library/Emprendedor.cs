using System.Collections;
using System.Collections.Generic;


namespace ClassLibrary
{
    public class Emprendedor
    {
        public string Nombre { get; set; }
        public List<Habilitacion> Habilitaciones { get; set; }
        public Ubicacion Ubicacion { get; set; }
        public Rubro Rubro { get; set; }
        public List<Oferta> Ofertas { get; set; }
        public string Id { get; set; }

        public Emprendedor(string id, string nombre, List<Habilitacion> habilitaciones, string ciudad, 
            string direccion, string rubro)
        {
            this.Id = id; 
            this.Nombre = nombre;
            this.Habilitaciones = habilitaciones;
            this.Ubicacion = new Ubicacion(ciudad, direccion);
            this.Rubro = new Rubro(rubro);
            this.Ofertas = new List<Oferta>();
        }


        public void buscarOfertas(List<Rubro> rubro, List<Habilitacion> habilitaciones){

            // TODO

        }

        public void postularseAOfertas(List<Oferta> ofertas){

            // TODO

        }



    }

}
