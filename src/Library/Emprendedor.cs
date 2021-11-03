using System;
using System.Collections.Generic;


namespace ClassLibrary
{
    /// <summary>
    /// Esta clase respresenta los datos basicos y necesarios que todo Emprendedor tiene, además de sus responsabilidades asignadas.
    /// </summary>
    /// Patrones y principios:
    ///    Cumple con SRP porque solo se identifica una razón de cambio: algún cambio en la lógica de las ofertas.
    ///    Cumple con OSP porque está abierta a la extención y cerrada a la modificación.
    ///    Cumple con Expert porque tiene toda la información necesaria para poder cumplir con la responsabilidad de dar de alta un emprendedor y consumir ofertas.


    public class Emprendedor
    {   
        /// <summary>
        /// Se indica el <value>Id</value> en Telegram del emprendedor.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Se indica el <value>Nombre</value> del emprendedor.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Mediante una lista de <value>Habilitaciones</value> indicaremos todas las habiltiaciones con las que el emprendedor cuenta.
        /// </summary>
        public List<Habilitacion> Habilitaciones { get; set; }

        /// <summary>
        /// Se indica la <value>Ubicacion</value> del emprendedor.
        /// </summary>
        public Ubicacion Ubicacion { get; set; }

        /// <summary>
        /// Mediante una lista de <value>Rubo</value> indicaremos los rubros en el que el emprendedor se maneja.
        /// </summary>
        public Rubro Rubro { get; set; }

        /// <summary>
        /// Mediante una lista de <value>Ofertas</value> indicaremos las ofertas a las cual se postuló.
        /// </summary>
        public List<Oferta> OfertasPostuladas { get; set; }
        
        /// <summary>
        /// Mediante una lista de <value>Ofertas</value> indicaremos las ofertas consumidas a las cual se postuló.
        /// </summary>
        public List<Oferta> OfertasConsumidas { get; set; }
        
        /// <summary>
        /// Crea una instancia de Emprendedor
        /// </summary>
        /// <param name="id">Id del emprendedor en Telegram.</param>
        /// <param name="nombre">Nombre de la empresa del emprendedor.</param>
        /// <param name="habilitaciones">Habilitaciones poseídas por el emprendedor.</param>
        /// <param name="ciudad">La ciudad donde se basa el emprendedor.</param>
        /// <param name="direccion">La direccion de la base de operaciones del emprendedor en la ciudad.</param>
        /// <param name="rubro">El rubro dentro del cual trabaja el emprendedor.</param>
        public Emprendedor(string id, string nombre, List<Habilitacion> habilitaciones, string ciudad, string direccion, string rubro)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Habilitaciones = habilitaciones;
            this.Ubicacion = new Ubicacion(ciudad, direccion);
            this.Rubro = new Rubro(rubro);
            this.OfertasConsumidas = new List<Oferta>();
            this.OfertasPostuladas = new List<Oferta>();
        }

        /// <summary>
        /// Mediante una oferta esté se postulará a ella.
        /// </summary>
        /// <param name="ofertas">Lista de ofertas a postularse.</param>
        public void PostularseAOferta(List<Oferta> ofertas){
            foreach(Oferta oferta in ofertas){
                OfertasPostuladas.Add(oferta);
                oferta.EmprendedoresPostulados.Add(this);
            }
        }
      
        /// <summary>
        /// Mediante una oferta esté se postulará a ella.
        /// </summary>
        /// <param name="oferta">Oferta a postularse.</param>
        public void PostularseAOferta(Oferta oferta){
            OfertasPostuladas.Add(oferta);
            oferta.EmprendedoresPostulados.Add(this);
        }

        /// <summary>
        /// Mediante una una fecha de inicio y de fin se obtienen todas las ofertas postuladas en ese periodo de tiempo.
        /// </summary>
        /// <param name="inicio">Fecha de inicio del periodo en el que se quiere buscar.</param>
        /// <param name="fin">Fecha de fin del periodo en el que se quiere buscar.</param>
        /// <returns>Una lista de ofertas a las cuales se postuló el Emprendedor.</returns>
        public List<Oferta> VerOfertasPostuladas(DateTime inicio, DateTime fin)
        {
            List<Oferta> ofertasPostuladas = new List<Oferta>();
            foreach (Oferta oferta in this.OfertasPostuladas)
            {
                if (oferta.FechaCreada >= inicio && oferta.FechaCierre <= fin)
                {
                    ofertasPostuladas.Add(oferta);
                }
            }
            return ofertasPostuladas;
        }

        /// <summary>
        /// Mediante palabras calve, un buscador y un canal se obtienen las ofertas consumidas por el Emprendedor.
        /// </summary>
        /// <param name="inicio">Fecha de inicio del periodo en el que se quiere buscar.</param>
        /// <param name="fin">Fecha de fin del periodo en el que se quiere buscar.</param>
        /// <returns>Una lista de ofertas consumidas por el Emprendedor.</returns>
        public List<Oferta> VerOfertasConsumidas(DateTime inicio, DateTime fin)
        {
            List<Oferta> ofertasConsumidas = new List<Oferta>();
            foreach (Oferta oferta in this.OfertasConsumidas)
            {
                if (oferta.FechaCreada >= inicio && oferta.FechaCierre <= fin)
                {
                    ofertasConsumidas.Add(oferta);
                }
            }
            return ofertasConsumidas;
        }
    }
}