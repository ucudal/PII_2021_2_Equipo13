using System;
using System.Collections.Generic;

namespace ClassLibrary
{
    /// <summary>
       /*  
       Patrones y principios utilizados en esta clase:
        ISP ya que segmenta las operaciones de la interface en la persistencia.
        Expert ya que se le da la responsabilidad de generar las publicaciones, debido a que es la clase más experta de la información.
        Polymorphism porque utiliza dos métodos polimorficos de persistencia. */
    /// </summary>
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

        public Oferta PublicarOferta(Oferta oferta)
        {
            throw new NotImplementedException();
        }

        public void ActualizarOferta(Oferta oferta)
        {
            throw new NotImplementedException();
        }

        public List<Oferta> VerOfertasPropias(DateTime inicio, DateTime fin, ICanal canal)
        {
            throw new NotImplementedException();
        }
    }
}