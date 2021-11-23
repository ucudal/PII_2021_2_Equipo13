using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Clase que representa un bot de Telegram.
    /// Se aplica el patrón de Adapter para definir una clase que funcione como adaptador entre nuestra solución y la API de Telegram.
    /// También se aplica el patrón de diseño Singleton para que sólo exista una instancia de la clase.
    /// </summary>
    public class TelegramBot: IEnviador
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
        /// Envia un mensaje a la plataforma específica de este enviador.
        /// </summary>
        /// <param name="respuesta">Respuesta a enviar a la plataforma específica.</param>
        public async void EnviarMensaje(IRespuesta respuesta)
        {
            IReplyMarkup markupDeBotones = new ReplyKeyboardRemove();
            if(respuesta.Botones != null){
                List<List<InlineKeyboardButton>> matrizBotones = new List<List<InlineKeyboardButton>>();

                foreach (List<IBoton> fila in respuesta.Botones)
                {
                    List<InlineKeyboardButton> botonesFila = new List<InlineKeyboardButton>();
                    foreach (IBoton boton in fila)
                    {
                        InlineKeyboardButton botonTelegram = InlineKeyboardButton.WithCallbackData(boton.Texto, boton.Callback);
                        botonesFila.Add(botonTelegram);
                    }
                    matrizBotones.Add(botonesFila);
                }
                markupDeBotones = new InlineKeyboardMarkup(matrizBotones);
            }
            
            await this.Cliente.SendTextMessageAsync(respuesta.IdUsuario, respuesta.Texto, replyMarkup: markupDeBotones, parseMode: ParseMode.Markdown);
        }

        /// <summary>
        /// Instancia de <see cref="InlineKeyboardButton"/> predefinida para representar a un botón con texto y callback "Listo".
        /// </summary>
        /// <value>Instancia de <see cref="InlineKeyboardButton"/> con texto y callback "Listo".</value>
        public readonly InlineKeyboardButton BotonListo = InlineKeyboardButton.WithCallbackData("Listo");

        /// <summary>
        /// Instancia de <see cref="InlineKeyboardButton"/> predefinida para representar a un botón con texto y callback "Cancelar".
        /// </summary>
        /// <value>Instancia de <see cref="InlineKeyboardButton"/> con texto y callback "Cancelar".</value>
        public readonly InlineKeyboardButton BotonCancelar = InlineKeyboardButton.WithCallbackData("Cancelar");

        /// <summary>
        /// Instancia de <see cref="InlineKeyboardButton"/> predefinida para representar a un botón con texto y callback "Siguiente".
        /// </summary>
        /// <value>Instancia de <see cref="InlineKeyboardButton"/> con texto y callback "Siguiente".</value>
        public readonly InlineKeyboardButton BotonSiguiente = InlineKeyboardButton.WithCallbackData("Siguiente");

        /// <summary>
        /// Instancia de <see cref="InlineKeyboardButton"/> predefinida para representar a un botón con texto y callback "Anterior".
        /// </summary>
        /// <value>Instancia de <see cref="InlineKeyboardButton"/> con texto y callback "Anterior".</value>
        public readonly InlineKeyboardButton BotonAnterior = InlineKeyboardButton.WithCallbackData("Anterior");

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
