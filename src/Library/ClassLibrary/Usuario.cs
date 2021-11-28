using System.Text.Json;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Representa a las abstracciones de un usuario en el sistema
    /// </summary>
    public abstract class Usuario : IIdentificable
    {

        /// <summary>
        /// Crea una instancia de <see cref="Usuario"/>
        /// </summary>
        /// <param name="id">Id del emprendedor en Telegram.</param>
        /// <param name="nombre">Nombre de la empresa del emprendedor.</param>
        /// <param name="ciudad">La ciudad donde se basa el emprendedor.</param>
        /// <param name="direccion">La direccion de la base de operaciones del emprendedor en la ciudad.</param>
        /// <param name="rubro">El rubro dentro del cual trabaja el emprendedor.</param>
        public Usuario(string id, string nombre, string direccion, string ciudad, string rubro)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Ubicacion = new UbicacionBase(ciudad, direccion);
            this.Rubro = new Rubro(rubro);
        }

        /// <summary>
        /// Crea una instancia vacía de <see cref="Usuario"/>
        /// </summary>
        public Usuario() { }

        /// <summary>
        /// Identificador único de un <see cref="Usuario"/> en el sistema.
        /// </summary>
        /// <value>Una cadena de caracteres conteniendo al identificador del <see cref="Usuario"/> en el sistema.</value>
        public string Id { get; set; }

        /// <summary>
        /// Nombre de registro del <see cref="Usuario"/>.
        /// </summary>
        /// <value>Una cadena de caracteres conteniendo el nombre de registro del <see cref="Usuario"/>.</value>
        public string Nombre { get; set; }

        /// <summary>
        /// Ubicación de registro de un <see cref="Usuario"/>.
        /// </summary>
        /// <value>Una instancia de una implementación de <see cref="IUbicacion"/> correspondiente a la ubicación de registro del <see cref="Usuario"/>.</value>
        public UbicacionBase Ubicacion { get; set; }

        /// <summary>
        /// Rubro al que pertence el <see cref="Usuario"/>, indicado por éste durante su registro.
        /// </summary>
        /// <value>Una instancia de <see cref="Rubro"/> correspondiente al rubro de registro del <see cref="Usuario"/>.</value>
        public Rubro Rubro { get; set; }
    }
}