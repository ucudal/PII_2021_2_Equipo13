using System;
using System.Collections.Generic;
using System.Text.Json;
using Recipes;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Representa a las abstracciones de un usuario en el sistema
    /// </summary>
    public abstract class Usuario: IJsonConvertible
    {

        /// <summary>
        /// Crea una instancia de Usuario
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
        /// Identificador único de un usuario en el sistema.
        /// </summary>
        /// <value>Una cadena de caracteres conteniendo al identificador del usuario en el sistema.</value>
        public string Id { get; set; }

        /// <summary>
        /// Nombre de registro del usuario.
        /// </summary>
        /// <value>Una cadena de caracteres conteniendo el nombre de registro del usuario.</value>
        public string Nombre { get; set; }

        /// <summary>
        /// Ubicación de registro de un usuario.
        /// </summary>
        /// <value>Una instancia de una implementación de <see cref="IUbicacion"/> correspondiente a la ubicación de registro del usuario.</value>
        public IUbicacion Ubicacion { get; set; }

        /// <summary>
        /// Rubro al que pertence el usuario, indicado por éste durante su registro.
        /// </summary>
        /// <value>Una instancia de <see cref="Rubro"/> correspondiente al rubro de registro del usuario.</value>
        public Rubro Rubro { get; set; }

        public string ConvertToJson()
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = MyReferenceHandler.Instance,
                WriteIndented = true
            };

            return JsonSerializer.Serialize(this, options);
        }
    }
}