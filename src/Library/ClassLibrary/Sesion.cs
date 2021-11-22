using System.Collections.Generic;
using System;


namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Representa a una sesion de un usuario en el bot.
    /// Cumple SRP y Expert ya que se encarga de determinar la información necesaria para diferenciar una sesión y los datos de la misma.
    /// </summary>
    public class Sesion: IJsonConvertible
    {
        /// <summary>
        /// Crea una instancia de Sesion
        /// </summary>
        /// <param name="idUsuario">Identificador único de un usuario.</param>
        public Sesion(string idUsuario)
        {
            this.IdSesion = Encriptador.GetHashCode(idUsuario + DateTimeOffset.Now.ToUnixTimeSeconds().ToString());
            this.IdUsuario = idUsuario;
            this.FechaCreacion = DateTime.Now;
            this.UltimaActividad = this.FechaCreacion;
            this.PLN = new LenguajeNatural(this.IdSesion);
        }

        /// <summary>
        /// Identificador único de la sesión.
        /// </summary>
        /// <value>Una cadena de caracteres con el identificador único de una sesión de un usuario.</value>
        public string IdSesion { get; }

        /// <summary>
        /// Identificador único del usuario de esta sesión.
        /// </summary>
        /// <value>Una cadena de caracteres con el identificador único de un usuario de una sesión.</value>
        public string IdUsuario { get; }

        /// <summary>
        /// Fecha de creación de la sesión.
        /// </summary>
        /// <value><see cref="DateTime"/> conteniendo el valor temporal del momento en que fue creada la sesión.</value>
        public DateTime FechaCreacion { get; }

        /// <summary>
        /// Fecha de expiración de la sesión.
        /// </summary>
        /// <value><see cref="DateTime"/> conteniendo el valor temporal del momento en que la sesión expirará.</value>
        public DateTime UltimaActividad { get; set; }

        /// <summary>
        /// Indica si la sesión sigue activa. Una sesión es considerad activa si su última actividad ocurrió hace menos de 30 minutos.
        /// </summary>
        /// <value>true si la sesión está activa y false en caso contrario.</value>
        public bool Activa
        {
            get
            {
                TimeSpan diferenciaTemporal = DateTime.Now - this.UltimaActividad;
                return diferenciaTemporal.TotalMinutes < 30;
            }
        }

        /// <summary>
        /// Instancia de <see cref="LenguajeNatural"/> utilizada para procesar con procesamiento de lenguaje natural (PLN) los mensajes de los usuarios.
        /// </summary>
        /// <value>Una instancia de <see cref="LenguajeNatural"/> para una sesión.</value>
        public LenguajeNatural PLN { get; }
    }
}