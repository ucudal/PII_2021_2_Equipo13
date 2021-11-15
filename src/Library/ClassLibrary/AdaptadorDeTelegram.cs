using Telegram.Bot.Types;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Contiene la información relevante de un mensaje de Telegram.
    /// Se aplica el patrón Adapter para crear esta clase y desacoplar la información de la API de Telegram.
    /// </summary>
    public class AdaptadorDeTelegram : IMensaje
    {
        private Message _mensaje;

        ///<summary>
        /// Constructor de la clase.
        /// <param name="mensaje">Mensaje de Telegram.</param>
        /// </summary>
        public AdaptadorDeTelegram(Message mensaje)
        {
            _mensaje = mensaje;
        }

        /// <summary>
        /// Obtiene el texto del mensaje.
        /// </summary>
        /// <value>Texto de un mensaje de Telegram</value>
        public string Texto { get => _mensaje.Text; }

        /// <summary>
        /// Obtiene la Id en Telegram del chat donde se envió mensaje.
        /// </summary>
        /// <value>Id en Telegram de un chat</value>
        public string IdChat { get => _mensaje.Chat.Id.ToString(); }

        /// <summary>
        /// Obtiene la Id en Telegram del usuario que envió el mensaje.
        /// </summary>
        /// <value>Id en Telegram de un usuario</value>
        public string IdUsuario { get => _mensaje.From.Id.ToString(); }
    }

}
