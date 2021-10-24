using System.Collections;
using System.Collections.Generic;


namespace ClassLibrary
{
    public class Emprendedor
    {
        public string nombre { get; set; }
        public List<Habilitacion> habilitaciones { get; set; }
        public string ubicacion { get; set; }
        public List<Rubro> rubro { get; set; }
        public List<Oferta> ofertas { get; set; }



        public Emprendedor(string nombre, List<Habilitacion> habilitaciones, string ubicacion, List<Rubro> rubro, List<Oferta> ofertas)
        {
            this.nombre = nombre;
            this.habilitaciones = habilitaciones;
            this.ubicacion = ubicacion;
            this.rubro = rubro;
            this.ofertas = ofertas;
        }


        public void buscarOfertas(List<Rubro> rubro, List<Habilitacion> habilitaciones){

            // TODO

        }

        public void postularseAOfertas(List<Oferta> ofertas){

            // TODO

        }



    }

}
