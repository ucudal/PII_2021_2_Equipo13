using System;
using System.Collections.Generic;

namespace Library
{
    /// <summary> 
    /// Patrones y principios utilizados en esta clase:
    /// ISP ya que segmenta las operaciones de la interface en la persistencia.
    /// Expert ya que se le da la responsabilidad de generar las publicaciones, debido a que es la clase más experta de la información.
    /// Polymorphism porque utiliza dos métodos polimorficos de persistencia.
    /// </summary>
    public class Empresa
    {
        public string Id { get; }
        
        public List<Oferta> Ofertas { get; }

        public Ubicacion Ubicacion { get; }
        
        public Rubro Rubro { get; } 
        
        public string Nombre { get; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Identificación de la estructura de datos</param>
        /// <param name="ofertas">listado de ofertas propias</param>
        /// <param name="ubicacion">ubicación de la empresa</param>
        /// <param name="rubro">rubro al que pertenece</param>
        /// <param name="nombre">Nombre comercial</param>
        public Empresa(string id, List<Oferta> ofertas, Ubicacion ubicacion, Rubro rubro, string nombre)
        {
            Id = id;
            Ofertas = ofertas;
            Ubicacion = ubicacion;
            Rubro = rubro;
            Nombre = nombre;
        }
        /// <summary>
        /// </summary>
        /// <param name="oferta">la oferta que se desea publicar</param>
        /// <returns></returns>
        public Oferta PublicarOferta(Oferta oferta)
        {
            this.Ofertas.Add(oferta);
        }
        /// <summary>
        /// </summary>
        /// <param name="oferta">la oferta que se desea modificar</param>
        public void ActualizarOferta(Oferta oferta)
        {
            foreach (Oferta _oferta in this.Ofertas)
            {
                if (_oferta.Id == oferta.Id)
                {
                    this.Ofertas.Remove(_oferta);
                    this.Ofertas.Add(oferta);
                }
            }
        }
        /// <summary>
        ///    Muestra las ofertas publicadas por la empresa en una lista filtrada por fecha y canal.
        /// </summary>
        /// <param name="inicio">Fecha de inicio</param>
        /// <param name="fin">Fecha de fin</param>
        /// <returns></returns>
        public List<Oferta> VerOfertasPropias(DateTime inicio, DateTime fin, ICanal canal)
        {
            List<Oferta> ofertasPropias = new List<Oferta>();
            foreach (Oferta oferta in this.Ofertas)
            {
                if (oferta.FechaCreada >= inicio && oferta.FechaCierre <= fin)
                {
                    ofertas.Add(oferta);
                }
            }
            return ofertasPropias;
        }
    }
}
