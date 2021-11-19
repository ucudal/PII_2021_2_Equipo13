using Telegram.Bot.Types;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Contiene la información relevante de un mensaje de Telegram.
    /// Se aplica el patrón Adapter para crear esta clase y desacoplar la información de la API de Telegram.
    /// </summary>
    public class AdaptadorDeTelegram : IMensaje, ICallBack
    {
        private Message _mensaje;
        private CallbackQuery _callback;

        ///<summary>
        /// Constructor de la clase utilizando un <see cref="Message"/> de Telegram.
        /// <param name="mensaje">Mensaje de Telegram.</param>
        /// </summary>
        public AdaptadorDeTelegram(Message mensaje)
        {
            _mensaje = mensaje;
        }

        ///<summary>
        /// Constructor de la clase utilizando una <see cref="CallbackQuery"/> de Telegram.
        /// <param name="callbackQuery">CallbackQuery de Telegram.</param>
        /// </summary>
        public AdaptadorDeTelegram(CallbackQuery callbackQuery)
        {
            _callback = callbackQuery;
        }

        /// <summary>
        /// Obtiene el texto del mensaje.
        /// </summary>
        /// <value>Texto de un mensaje de Telegram</value>
        public string Texto { get => _mensaje != null ? _mensaje.Text : _callback.Data; }

        /// <summary>
        /// Obtiene la Id en Telegram del chat donde se envió mensaje.
        /// </summary>
        /// <value>Id en Telegram de un chat</value>
        public string IdChat { get => _mensaje != null ? _mensaje.Chat.Id.ToString() : _callback.Message.Chat.Id.ToString(); }

        /// <summary>
        /// Obtiene la Id en Telegram del usuario que envió el mensaje.
        /// </summary>
        /// <value>Id en Telegram de un usuario</value>
        public string IdUsuario { get => _mensaje != null ? _mensaje.From.Id.ToString() : _callback.From.Id.ToString(); }

        /// <summary>
        /// Obtiene la Id en Telegram del usuario que envió el mensaje.
        /// </summary>
        /// <value>Id en Telegram de un usuario</value>
        public int IdMensaje { get => _mensaje != null ? _mensaje.MessageId : _callback.Message.MessageId; }
    }

}
