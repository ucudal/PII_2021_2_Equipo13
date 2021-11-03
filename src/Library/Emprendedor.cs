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
        /// Se indica el <value>Id</value> del emprendedor en la plataforma de registro
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Se indica el <value>Nombre</value> del emprendedor
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Mediante una lista de <value>Habilitaciones</value> indicaremos todas las habiltiaciones con las que el emprendedor cuenta.
        /// </summary>
        public List<Habilitacion> Habilitaciones { get; set; }

        /// <summary>
        /// Se indica la <value>Ubicacion</value> del emprendedor
        /// </summary>
        public Ubicacion Ubicacion { get; set; }

        /// <summary>
        /// Mediante una lista de <value>Rubo</value> indicaremos los rubros en el que el emprendedor se maneja.
        /// </summary>
        public Rubro Rubro { get; set; }

        /// <summary>
        /// Mediante una lista de <value>Ofertas</value> indicaremos las ofertas a las cual se postuló .
        /// </summary>
        public List<Oferta> ofertasPostuladas { get; set; }

        /// <summary>
        /// Mediante una lista de <value>Ofertas</value> indicaremos las ofertas consumidas a las cual se postuló .
        /// </summary>
        public List<Oferta> ofertasConsumidas { get; set; }

        public Emprendedor(string id, string nombre, List<Habilitacion> habilitaciones, string ciudad, string direccion, Rubro rubro)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Habilitaciones = habilitaciones;
            this.Ubicacion = new Ubicacion(ciudad, direccion);
            this.Rubro = rubro;
            this.ofertasConsumidas = new List<Oferta>();
            this.ofertasPostuladas = new List<Oferta>();
        }

        /// <summary>
        /// Mediante una oferta esté se postulará a ella.
        /// </summary>
        public void PostularseAOferta(Oferta ofertas)
        {
            // TODO
        }


        /// <summary>
        /// Mediante una una fecha de inicio y de fin, ademas del un canal se obtendrán todas las ofertas postuladas en ese periodo de tiempo
        /// y se guardará en la Lista <value>ofertasPostuladas</value> .
        /// </summary>
        public void obtenerOfertaspostuladas(string fechaInicio, string fechaFin, ICanal canal)
        {
            // TODO     return buscador.ofertasPostuladas();
        }

        /// <summary>
        /// </summary>
        public List<Oferta> VerOfertasConsumidas(DateTime inicio, DateTime fin)
        {
            List<Oferta> ofertasConsumidas = new List<Oferta>();
            foreach (Oferta oferta in this.ofertasConsumidas)
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
