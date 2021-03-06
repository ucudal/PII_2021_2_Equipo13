using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PII_E13.ClassLibrary
{
    /// <summary> 
    /// Patrones y principios utilizados en esta clase:
    /// ISP ya que segmenta las operaciones de la interface en la persistencia.
    /// Creator ya que crea una instancia de oferta en esta clase con los parametros necesarios para ello.
    /// Expert ya que se le da la responsabilidad de generar las publicaciones, debido a que es la clase más experta de la información.
    /// </summary>
    public class Empresa : Usuario
    {
        /// <summary>
        /// Id del usuario en el canal de registro.
        /// </summary>
        [JsonInclude]
        public List<Oferta> Ofertas { get; set; } = new List<Oferta>();

        /// <summary>
        /// Crea una instancia vacía de <see cref="Empresa"/>.
        /// </summary>
        [JsonConstructor]
        public Empresa() { }

        /// <summary>
        /// Crea una instancia de Empresa.
        /// </summary>
        /// <param name="id">Id del usuario en el canal de registro.</param>
        /// <param name="ciudad">Ciudad donde está basada la empresa.</param>
        /// <param name="direccion">Direccion de la empresa.</param>
        /// <param name="rubro">Rubro al que pertenece la empresa.</param>
        /// <param name="nombre">Nombre comercial de la empresa.</param>
        public Empresa(string id, string ciudad, string direccion, string rubro, string nombre) : base(id, nombre, direccion, ciudad, rubro)
        {
        }

        /// <summary>
        /// Crea una instancia de <see cref="Empresa"/> a través de deserialización.
        /// </summary>
        /// <param name="id">Id del usuario en el canal de registro.</param>
        /// <param name="ubicacion">Instancia de <see cref="UbicacionBase"/> correspondiente a la ubicacion del usuario.</param>
        /// <param name="rubro">Instancia de <see cref="Rubro"/> correspondiente al rubro del usuario.</param>
        /// <param name="nombre">Nombre comercial de la empresa.</param>
        /// <param name="ofertas">Lista de instancias de <see cref="Oferta"/> correspondiente a las ofertas creadas por la empresa.</param>
        /// <returns></returns>
        public Empresa(string id, UbicacionBase ubicacion, Rubro rubro, string nombre, List<Oferta> ofertas = null) : base(id, nombre, ubicacion.Direccion, ubicacion.Ciudad, rubro.Nombre)
        {
        }

        /// <summary>
        /// Crea una nueva Oferta y la añade a la lista de ofertas de la empresa.
        /// </summary>
        /// <param name="id">El id único de la oferta generada.</param>
        /// <param name="fechaCierre">La fecha de cierre de la oferta.</param>
        /// <param name="etiquetas">Las etiquetas o palabras clave de la oferta.</param>
        /// <param name="habilitaciones">Las habilitaciones requeridas para tomar la oferta.</param>
        /// <param name="descripcion">La descripción de la oferta.</param>
        /// <param name="titulo">El titulo de la oferta.</param>
        /// <param name="disponibleConstantemente">Indica si la oferta está disponible constantemente o puntualmente.</param>
        /// <exception cref="ArgumentException">Si el id de la <see cref="Oferta"/> ya existe.</exception>
        public Oferta PublicarOferta(string id, string titulo, string descripcion, DateTime fechaCierre, bool disponibleConstantemente = false,
            List<string> etiquetas = null, List<Habilitacion> habilitaciones = null)
        {
            try
            {
                Sistema.Instancia.ObtenerOfertaPorId(id);
                throw new ArgumentException("La id proveída no es válida ya que está repetida.");
            }
            catch (KeyNotFoundException e) { }

            if (etiquetas == null)
            {
                etiquetas = new List<string>();
            }
            if (habilitaciones == null)
            {
                habilitaciones = new List<Habilitacion>();
            }
            Oferta oferta = new Oferta(id, this, fechaCierre, etiquetas, habilitaciones, descripcion, titulo, disponibleConstantemente);
            Ofertas.Add(oferta);
            IPersistor persistor = new PersistorDeJson();
            persistor.Escribir<Empresa>("Empresas.json", this);
            return oferta;
        }
        /// <summary>
        /// </summary>
        /// <param name="oferta">la oferta que se desea modificar.</param>
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
        /// <param name="inicio">Fecha de inicio del periodo en el que se quiere buscar.</param>
        /// <param name="fin">Fecha de fin del periodo en el que se quiere buscar.</param>
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
        /// <exception cref="KeyNotFoundException">Si no encuentra una <see cref="Oferta"/></exception>
        public Oferta ObtenerOfertaPorId(string id)
        {
            foreach (Oferta oferta in this.Ofertas)
            {
                if (oferta.Id == id)
                    return oferta;
            }
            throw new KeyNotFoundException("No se encontró la oferta con el id dado.");
        }
    }
}