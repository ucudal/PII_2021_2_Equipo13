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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Identificación de la estructura de datos</param>
        /// <param name="ofertas">listado de ofertas propias</param>
        /// <param name="ubicacion">ubicación de la empresa</param>
        /// <param name="rubro">rubro al que pertenece</param>
        /// <param name="nombre">Nombre comercial</param>
        public Empresa(string id, List<Oferta> ofertas, string ciudad, string direccion, Rubro rubro, string nombre)
        {
            this.Id = id;
            this.Ofertas = ofertas;
            this.Ubicacion = new Ubicacion(ciudad, direccion);
            this.Rubro = rubro;
            this.Nombre = nombre;
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
        /// <param name="disponibleConstantemente">Indica si la oferta está disponible constantemente o puntualmente</param>
        public Oferta PublicarOferta(string id, Empresa empresa, DateTime fechaCierre, List<string> etiquetas,
            List<Habilitacion> habilitaciones, double valorUYU, double valorUSD, string descripcion,
            string titulo, bool disponibleConstantemente)
        {
            Oferta oferta = new Oferta(id, empresa, fechaCierre, etiquetas, habilitaciones, valorUYU, valorUSD,
                descripcion, titulo, disponibleConstantemente);
            Ofertas.Add(oferta);
            return oferta;
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
        /// Muestra las ofertas publicadas por la empresa en una lista filtrada por fecha.
        /// </summary>
        /// <param name="inicio">Fecha de inicio</param>
        /// <param name="fin">Fecha de fin</param>
        /// <returns></returns>
        public List<Oferta> VerOfertasPropias(DateTime inicio, DateTime fin)
        {
            List<Oferta> ofertasPropias = new List<Oferta>();
            foreach (Oferta oferta in this.Ofertas)
            {
                if (oferta.FechaCreada >= inicio && oferta.FechaCierre <= fin)
                {
                    ofertasPropias.Add(oferta);
                }
            }
            return ofertasPropias;
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