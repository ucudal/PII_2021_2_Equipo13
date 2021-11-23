using System;

namespace PII_E13.ClassLibrary
{

    /// <summary>
    /// Interfaz que representa un botón adjuntado a un mensaje.
    /// Se aplica el patrón adapter y se define una interfaz genérica de botón para disminuir el acoplamiento con las APIs de mensajería.
    /// </summary>
    public class Boton : IBoton
    {
        /// <summary>
        /// Constructor de la clase con callback igual al texto del botón.
        /// </summary>
        /// <param name="texto">Texto del botón.</param>
        public Boton(string texto)
        {
            this.Texto = texto;
            this.Callback = texto;
        }

        /// <summary>
        /// Constructor de la clase con callback y texto definidos explícitamente.
        /// </summary>
        /// <param name="texto">Texto del botón.</param>
        /// <param name="callback">Callback con respuesta recibida.</param>
        public Boton(string texto, string callback)
        {
            this.Texto = texto;
            this.Callback = callback;
        }

        /// <summary>
        /// Texto del botón.
        /// </summary>
        public string Texto { get; set; }

        /// <summary>
        /// Texto recibido por el sistema cuando este botón es seleccionado.
        /// </summary>
        public string Callback { get; set; }
    }
}