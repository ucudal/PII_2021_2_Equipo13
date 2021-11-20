using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

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
        /// Genera y retorna una lista de botones de Telegram a partir de una lista de opciones.
        /// </summary>
        /// <param name="opciones">La lista de opciones con las cuales crear los botones.</param>
        /// <returns>Una lista de <see cref="KeyboardButton"/> conteniendo botones con las opciones recibidas por parámetros.</returns>
        public List<InlineKeyboardButton> ObtenerBotones(List<string> opciones)
        {
            List<InlineKeyboardButton> botones = new List<InlineKeyboardButton>();
            List<string> opcionesAuxiliar = new List<string>();
            foreach (string opcion in opciones)
            {
                if (!opcionesAuxiliar.Contains(opcion))
                {
                    InlineKeyboardButton boton = InlineKeyboardButton.WithCallbackData(opcion);
                    botones.Add(boton);
                    //botones.Add(new InlineKeyboardButton.(opcion));
                    opcionesAuxiliar.Add(opcion);
                }
            }
            return botones;
        }

        /// <summary>
        /// Genera y retorna un teclado de Telegram (<see cref="InlineKeyboardMarkup"/>) con una lista de botones, un índice
        /// índice de la lista a partir del cual iniciar y la cantidad de columnas y filas de botones a mostrar.
        /// </summary>
        /// <param name="botones">La lista de instancias de <see cref="InlineKeyboardButton"/> con la cual se quiere generar un teclado.</param>
        /// <param name="indice">El índice de la lista desde el cual iniciar. Se define a 0 por defecto.</param>
        /// <param name="columnas">La cantidad de columnas de botones a incluir. Se define a 1 por defecto.</param>
        /// <param name="filas">La cantidad de filas de botones a incluir. Se define a 1 por defecto.</param>
        /// <param name="botonesFijos">Matriz de <see cref="InlineKeyboardButton"/> fijos opcional para mostrar al final del teclado.</param>
        /// <returns></returns>
        public InlineKeyboardMarkup ObtenerKeyboard(List<InlineKeyboardButton> botones, int indice = 0, int filas = 1, int columnas = 1,
            List<List<InlineKeyboardButton>> botonesFijos = null)
        {
            List<List<InlineKeyboardButton>> matrizBotones = new List<List<InlineKeyboardButton>>();
            for (int i = 0; i < filas; i++)
            {
                List<InlineKeyboardButton> fila = new List<InlineKeyboardButton>();
                for (int j = 0; j < columnas; j++)
                {
                    try
                    {
                        fila.Add(botones[indice]);
                        indice++;
                    }
                    catch (Exception e)
                    {
                        break;
                    }
                }
                if (fila.Count > 0)
                {
                    matrizBotones.Add(fila);
                }
            }
            if (botonesFijos != null)
            {
                matrizBotones.AddRange(botonesFijos);
            }
            return new InlineKeyboardMarkup(matrizBotones);
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