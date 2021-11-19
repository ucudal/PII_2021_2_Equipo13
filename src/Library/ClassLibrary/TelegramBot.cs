using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Clase que representa un bot de Telegram.
    /// Se aplica el patrón de Adapter para definir una clase que funcione como adaptador entre nuestra solución y la API de Telegram.
    /// También se aplica el patrón de diseño Singleton para que sólo exista una instancia de la clase.
    /// </summary>
    public class TelegramBot
    {

        private const string TOKEN_BOT_DE_TELEGRAM = "2133543111:AAHtlHAp1B-irzg7ZhfUH2olwG7InxVT9Yw";
        private static TelegramBot instancia;
        private ITelegramBotClient bot;

        private TelegramBot()
        {
            this.bot = new TelegramBotClient(TOKEN_BOT_DE_TELEGRAM);
        }

        /// <summary>
        /// Cliente de la API de Telegram.
        /// </summary>
        /// <value>Un cliente de la API de Telegram que extiende a la interfaz <see cref="ITelegramBotClient"/>.</value>
        public ITelegramBotClient Cliente
        {
            get
            {
                return this.bot;
            }
        }

        private User InfoBot
        {
            get
            {
                return this.Cliente.GetMeAsync().Result;
            }
        }

        /// <summary>
        /// Identificador único del bot en Telegram.
        /// </summary>
        /// <value>Número entero correspondiente al identificador único en Telegram del bot.</value>
        public int IdBot
        {
            get
            {
                return this.InfoBot.Id;
            }
        }

        /// <summary>
        /// Nombre del bot en Telegram.
        /// </summary>
        /// <value>Cadena de caracteres correspondiente al nombre en Telegram del bot.</value>
        public string NombreBot
        {
            get
            {
                return this.InfoBot.FirstName;
            }
        }


        /// <summary>
        /// Obtiene una instancia de la clase <see cref="TelegramBot"/>.
        /// </summary>
        /// <value>Instancia de la clase <see cref="TelegramBot"/>.</value>
        public static TelegramBot Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new TelegramBot();
                }
                return instancia;
            }
        }
    }
}