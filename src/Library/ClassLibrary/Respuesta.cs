namespace PII_E13.ClassLibrary
{

    /// <summary>
    /// Clase que representa una respuesta a un mensaje.
    /// DOCUMENTAR PATRONES APLICADOS
    /// </summary>
    public class Respuesta : IRespuesta
    {

        /// <summary>
        /// Crea una instancia de <see cref="Respuesta"/> con un texto asignado y la indicación explícita
        /// sobre si se debe editar el último mensaje.
        /// </summary>
        public Respuesta(string texto, bool borrarMensaje)
        {
            this.Texto = texto;
            this.EditarMensaje = borrarMensaje;
        }

        /// <summary>
        /// Crea una instancia de <see cref="Respuesta"/> con un texto asignado.
        /// </summary>
        public Respuesta(string texto)
        {
            this.Texto = texto;
            this.EditarMensaje = false;
        }

        /// <summary>
        /// Texto del mensaje de la <see cref="Respuesta"/>.
        /// </summary>
        public string Texto { get; set; }

        /// <summary>
        /// Indica si el último mensaje debe ser editado, de ser posible.
        /// </summary>
        public bool EditarMensaje { get; set; }
    }
}