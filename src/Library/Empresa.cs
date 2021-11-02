using System;
using System.Collections.Generic;

namespace ClassLibrary
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
        
        public Empresa(string id, string ciudad, string direccion, Rubro rubro, string nombre)
        {
            Id = id;
            Ofertas = new List<Oferta>();
            Ubicacion = new Ubicacion(ciudad, direccion);
            Rubro = rubro;
            Nombre = nombre;
        }

        /// <summary>
        /// Crea una nueva Oferta y la añade a la lista de ofertas de la empresa.
        /// </summary>
        /// <param name="id">El id único de la oferta generada</param>
        /// <param name="fechaCierre">La fecha de cierre de la oferta</param>
        /// <param name="etiquetas">Las etiquetas o palabras clave de la oferta</param>
        /// <param name="habilitaciones">Las habilitaciones requeridas para tomar la oferta</param>
        /// <param name="valorUYU">El valor en UYU de la oferta</param>
        /// <param name="valorUSD">El valor en USD de la oferta</param>
        /// <param name="descripcion">La descripción de la oferta</param>
        /// <param name="titulo">El titulo de la oferta</param>
        public Oferta PublicarOferta(string id, DateTime fechaCierre, List<string> etiquetas,
            List<Habilitacion> habilitaciones, string descripcion, string titulo)
        {
            Oferta oferta = new Oferta(id, fechaCierre, etiquetas, habilitaciones,
                descripcion, titulo, this)
            Ofertas.Add(oferta);
            return oferta;
        }

        public void ActualizarOferta(Oferta oferta)
        {
            throw new NotImplementedException();
        }

        public List<Oferta> VerOfertasPropias(DateTime inicio, DateTime fin, ICanal canal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Recupera una oferta de la lista de ofertas utilizando su id y una id dada.
        /// </summary>
        /// <param name="id">Id de la oferta a recuperar.</param>
        /// <returns>La instancia de Oferta correspondiente a la id dada.</returns>
        public Oferta ObtenerOfertaPorId(string id)
        {
            foreach(Oferta oferta in this.Ofertas){
                if (oferta.Id == id)
                    return oferta;
            }
            throw new KeyNotFoundException("No se encontró la oferta con el id dado.");
        }
    }
}
