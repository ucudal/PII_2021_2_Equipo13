using Telegram.Bot.Types;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Contiene la información relevante de un mensaje de Telegram.
    /// Se aplica el patrón Adapter para crear esta clase y desacoplar la información de la API de Telegram.
    /// </summary>
    public class AdaptadorDeTelegram : IMensaje
    {
        /// <summary>
        /// Envía un mensaje a un usuario de un canal.
        /// </summary>
        /// <param name="message"></param>
        public void EnviarMensaje(string message)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Recibe un mensaje de un usuario de un canal.
        /// </summary>
        public string RecibirMensaje()
        {
            throw new System.NotImplementedException();
        }
    }

}
