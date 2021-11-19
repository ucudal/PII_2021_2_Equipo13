using System;
using PII_E13.ClassLibrary;
using PII_E13.HandlerLibrary;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace Application
{
    /// <summary>
    /// Programa principal.
    /// </summary>
    public static class Program
    {
        private static IHandler handler = new PostularseAOfertaHandler(null);

        /// <summary>
        /// Punto de entrada al programa principal.
        /// </summary>
        public static void Main()
        {
            //Obtengo una instancia de TelegramBot
            TelegramBot telegramBot = TelegramBot.Instancia;
            Console.WriteLine($"[BOT INICIADO] Nombre: {telegramBot.NombreBot} - Id: {telegramBot.IdBot}");

            //Obtengo el cliente de Telegram
            ITelegramBotClient bot = telegramBot.Cliente;

            //Asigno un gestor de mensajes
            bot.OnMessage += MensajeNuevo;

            //Inicio la escucha de mensajes
            bot.StartReceiving();

            Console.WriteLine("Presiona una tecla para terminar");
            Console.ReadKey();

            //Detengo la escucha de mensajes 
            bot.StopReceiving();
        }

        /// <summary>
        /// Manejador de evento de mensaje nuevo.
        /// </summary>
        /// <param name="sender">Objeto que generó el evento.</param>
        /// <param name="messageEventArgs">Argumentos del evento.</param>
        private static async void MensajeNuevo(object sender, MessageEventArgs messageEventArgs)
        {
            IMensaje mensaje = new AdaptadorDeTelegram(messageEventArgs.Message);

            if (mensaje.Texto != null)
            {
                ITelegramBotClient client = TelegramBot.Instancia.Cliente;
                Console.WriteLine($"[NUEVO MENSAJE] - {mensaje.IdUsuario} envió: {mensaje.Texto}");

                RespuestaTelegram respuesta;

                IHandler resultado = handler.Resolver(mensaje, out respuesta);

                if (resultado == null)
                {
                    await client.SendTextMessageAsync(mensaje.IdUsuario, "Lo siento, parece que no puedo resolver esa consulta en este momento.");
                }
                else
                {
                    if (respuesta.TecladoTelegram != null)
                    {
                        await client.SendTextMessageAsync(mensaje.IdUsuario, respuesta.Texto, replyMarkup: respuesta.TecladoTelegram);
                    }
                    else
                    {
                        await client.SendTextMessageAsync(mensaje.IdUsuario, respuesta.Texto);
                    }
                }
            }
        }
    }
}