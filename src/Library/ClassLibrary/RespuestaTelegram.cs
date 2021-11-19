using Telegram.Bot.Types.ReplyMarkups;

namespace PII_E13.ClassLibrary
{

    /// <summary>
    /// Clase que representa una respuesta a un mensaje de Telegram.
    /// DOCUMENTAR PATRONES APLICADOS
    /// </summary>
    public class RespuestaTelegram : Respuesta
    {
        /// <summary>
        /// Crea una instancia de <see cref="RespuestaTelegram"/> con un texto y un teclado de tipo
        /// <see cref="InlineKeyboardMarkup"/> asignados.
        /// </summary>
        public RespuestaTelegram(string texto, InlineKeyboardMarkup teclado) : base(texto)
        {
            this.TecladoTelegram = teclado;
        }

        /// <summary>
        /// Crea una instancia de <see cref="RespuestaTelegram"/> con un texto asignado.
        /// </summary>
        public RespuestaTelegram(string texto) : base(texto)
        {
        }

        /// <summary>
        /// Teclado de botones predefinidos para Telegram.
        /// </summary>
        /// <value> Instancia de <see cref="InlineKeyboardMarkup"/> con botones definidos</value>
        public InlineKeyboardMarkup TecladoTelegram { get; set; }
    }
}