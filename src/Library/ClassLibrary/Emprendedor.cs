using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Esta clase respresenta los datos basicos y necesarios que todo Emprendedor tiene, además de sus responsabilidades asignadas.
    /// </summary>
    /// Patrones y principios:
    ///    Cumple con SRP porque solo se identifica una razón de cambio: algún cambio en la lógica de las ofertas.
    ///    Cumple con OSP porque está abierta a la extención y cerrada a la modificación.
    ///    Cumple con Expert porque tiene toda la información necesaria para poder cumplir con la responsabilidad de dar de alta un emprendedor y consumir ofertas.


    public class Emprendedor : Usuario
    {

        /// <summary>
        /// Mediante una lista de <value>Habilitaciones</value> indicaremos todas las habiltiaciones con las que el emprendedor cuenta.
        /// </summary>
        [JsonInclude]
        public List<Habilitacion> Habilitaciones { get; set; }

        /// <summary>
        /// Mediante una lista de identificadores únicos de instancias de <see cref="Oferta"/> indicaremos las ofertas a las cual se postuló.
        /// </summary>
        [JsonInclude]
        public List<string> OfertasPostuladas { get; set; }

        /// <summary>
        /// Mediante una lista de identificadores únicos de instancias de <see cref="Oferta"/> indicaremos las ofertas consumidas a las cual se postuló.
        /// </summary>
        [JsonInclude]
        public List<string> OfertasConsumidas { get; set; }

        /// <summary>
        /// Crea una instancia vacía de <see cref="Emprendedor"/>
        /// </summary>
        [JsonConstructor]
        public Emprendedor() { }

        /// <summary>
        /// Crea una instancia de <see cref="Emprendedor"/>
        /// </summary>
        /// <param name="id">Id del emprendedor en Telegram.</param>
        /// <param name="nombre">Nombre de la empresa del emprendedor.</param>
        /// <param name="habilitaciones">Habilitaciones poseídas por el emprendedor.</param>
        /// <param name="ciudad">La ciudad donde se basa el emprendedor.</param>
        /// <param name="direccion">La direccion de la base de operaciones del emprendedor en la ciudad.</param>
        /// <param name="rubro">El rubro dentro del cual trabaja el emprendedor.</param>
        public Emprendedor(string id, string nombre, List<Habilitacion> habilitaciones, string ciudad, string direccion, string rubro) : base(id, nombre, direccion, ciudad, rubro)
        {
            this.Habilitaciones = habilitaciones;
            this.OfertasConsumidas = new List<string>();
            this.OfertasPostuladas = new List<string>();
        }

        /// <summary>
        /// Crea una instancia de <see cref="Emprendedor"/> a través de deserialización.
        /// </summary>
        public Emprendedor(string id, string nombre, UbicacionBase ubicacion, Rubro rubro, List<Habilitacion> habilitaciones = null,
            List<string> ofertasConsumidas = null, List<string> ofertasPostuladas = null) : base(id, nombre, ubicacion.Direccion, ubicacion.Ciudad, rubro.Nombre)
        {
            this.Rubro = rubro;
            this.Habilitaciones = habilitaciones != null ? habilitaciones : new List<Habilitacion>();
            this.OfertasConsumidas = ofertasConsumidas != null ? ofertasConsumidas : new List<string>();
            this.OfertasPostuladas = ofertasPostuladas != null ? ofertasPostuladas : new List<string>();
        }

        /// <summary>
        /// Mediante una oferta, éste emprendedor se postulará a ella.
        /// </summary>
        /// <param name="ofertas">Lista de ofertas a postularse.</param>
        public void PostularseAOferta(List<Oferta> ofertas)
        {
            foreach (Oferta oferta in ofertas)
            {
                this.PostularseAOferta(oferta);
            }
        }

        /// <summary>
        /// Mediante una oferta, éste emprendedor se postulará a ella.
        /// </summary>
        /// <param name="oferta">Oferta a postularse.</param>
        public void PostularseAOferta(Oferta oferta)
        {
            OfertasPostuladas.Add(oferta.Id);
            oferta.EmprendedoresPostulados.Add(this.Id);
            IPersistor persistor = new PersistorDeJson();
            persistor.Escribir<Emprendedor>("Emprendedores.json", this);
            persistor.Escribir<Empresa>("Empresas.json", Sistema.Instancia.ObtenerEmpresaPorId(oferta.Empresa));
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
            foreach (string idOferta in this.OfertasPostuladas)
            {
                Oferta oferta = Sistema.Instancia.ObtenerOfertaPorId(idOferta);
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
            foreach (string idOferta in this.OfertasConsumidas)
            {
                Oferta oferta = Sistema.Instancia.ObtenerOfertaPorId(idOferta);
                if (oferta.FechaCreada >= inicio && oferta.FechaCierre <= fin)
                {
                    ofertasConsumidas.Add(oferta);
                }
            }
            return ofertasConsumidas;
        }

        /// <summary>
        /// Redacta en una cadena de caracteres un resumen incluyendo características básicas del emprendedor.
        /// </summary>
        /// <returns>Una cadena de caracteres incluyendo nombre, rubro y ubicación de un <see cref="Emprendedor"/>.</returns>
        public string RedactarResumen()
        {
            return $"*{this.Nombre}*, {this.Rubro.Nombre}.\n_{this.Ubicacion.Redactar()}_";
        }

        /// <summary>
        /// Redacta en una cadena de caracteres la información de un <see cref="Emprendedor"/>.
        /// </summary>
        /// <returns>Una cadena de caracteres incluyendo nombre, rubro, ubicación y habilitaciones de un <see cref="Emprendedor"/>.</returns>
        public string Redactar()
        {
            StringBuilder stringBuilder = new StringBuilder($"*Nombre:* {this.Nombre}");
            stringBuilder.Append($"\n*Rubro:* {this.Rubro.Nombre}");
            stringBuilder.Append($"\n*Dirección:* {this.Ubicacion.Redactar()}");

            if (this.Habilitaciones.Count > 0)
            {
                stringBuilder.Append("\n\n*Habilitaciones:*");
                foreach (Habilitacion habilitacion in this.Habilitaciones)
                {
                    stringBuilder.Append($"\n\n*{habilitacion.Nombre}:* {habilitacion.Descripcion}");
                    if (habilitacion.Vigente)
                    {
                        stringBuilder.Append($"\nEstado: _Vigente_");
                        stringBuilder.Append($"\nFecha de vencimiento: _{habilitacion.FechaVencimiento}_");
                    }
                    else
                    {
                        stringBuilder.Append($"\nEstado: _Vencida_");
                    }
                    stringBuilder.Append($"\nHabilitado por: _{habilitacion.NombreInsitucionHabilitada}_");
                }
            }

            return stringBuilder.ToString();
        }
    }
}