using System;
using System.Collections.Generic;
using PII_E13.ClassLibrary;
using PII_E13.HandlerLibrary;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace Application
{
    /// <summary>
    /// Programa principal.
    /// </summary>
    public static class Program
    {
        // INSTANCIAR COMO ALGÚN HANDLER.
        private static IHandler handler = new CrearOfertaHandler(null, "Saludo");
        //private static IHandler handler = new RegistrarEmprendedorHandler(null);

        private static GestorSesiones gestorSesiones = GestorSesiones.Instancia;

        /// <summary>
        /// Punto de entrada al programa principal.
        /// </summary>
        public static void Main()
        {
            // DATOS DE PRUEBA -------------------------------------------------------------------------------------------
            Sistema.Instancia.Materiales.Add(new Material("Madera de roble", new List<string>() { "Madera", "Roble", "Carpintería" }, "Kg"));
            Sistema.Instancia.Materiales.Add(new Material("Madera de abeto", new List<string>() { "Madera", "Abeto", "Carpintería" }, "Kg"));
            Sistema.Instancia.Materiales.Add(new Material("Madera de pino", new List<string>() { "Madera", "Pino", "Carpintería" }, "Kg"));
            Sistema.Instancia.Materiales.Add(new Material("Madera de cedro", new List<string>() { "Madera", "Cedro", "Carpintería" }, "Kg"));
            Sistema.Instancia.Materiales.Add(new Material("Madera de arce", new List<string>() { "Madera", "Arce", "Carpintería" }, "Kg"));
            Sistema.Instancia.Materiales.Add(new Material("Madera de haya", new List<string>() { "Madera", "Haya", "Carpintería" }, "Kg"));
            Sistema.Instancia.Materiales.Add(new Material("Madera de nogal", new List<string>() { "Madera", "Nogal", "Carpintería" }, "Kg"));
            Sistema.Instancia.Materiales.Add(new Material("Madera de cerezo", new List<string>() { "Madera", "Cerezo", "Carpintería" }, "Kg"));
            Sistema.Instancia.Materiales.Add(new Material("Madera de caoba", new List<string>() { "Madera", "Caoba", "Carpintería" }, "Kg"));
            Sistema.Instancia.Materiales.Add(new Material("Cobre", new List<string>() { "Metal", "Resistente", "Conductor" }, "Kg"));


            // DATOS DE PRUEBA -------------------------------------------------------------------------------------------

            //Obtengo una instancia de TelegramBot
            TelegramBot telegramBot = TelegramBot.Instancia;
            Console.WriteLine($"[BOT INICIADO] Nombre: {telegramBot.NombreBot} - Id: {telegramBot.IdBot}");

            //Obtengo el cliente de Telegram
            ITelegramBotClient bot = telegramBot.Cliente;

            //Asigno un gestor de mensajes
            bot.OnMessage += MensajeNuevo;

            bot.OnCallbackQuery += CallBackNuevo;

            //Inicio la escucha de mensajes
            bot.StartReceiving();

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
            EventoNuevo(mensaje);
        }

        /// <summary>
        /// Manejador de evento de callback nuevo.
        /// </summary>
        /// <param name="sender">Objeto que generó el evento.</param>
        /// <param name="callbackEventArgs">Argumentos del evento.</param>
        private static async void CallBackNuevo(object sender, CallbackQueryEventArgs callbackEventArgs)
        {
            IMensaje callback = new AdaptadorDeTelegram(callbackEventArgs.CallbackQuery);
            EventoNuevo(callback);
        }

        /// <summary>
        /// Manejador general de eventos nuevos provenientes del bot.
        /// </summary>
        /// <param name="mensaje">Instancia de una implementación de <see cref="IMensaje"/> con la información de un mensaje en una plataforma de mensajería.</param>
        private static async void EventoNuevo(IMensaje mensaje)
        {
            if (mensaje.Texto != null)
            {
                bool nuevaSesion;
                IRespuesta respuesta;
                IEnviador cliente = TelegramBot.Instancia;
                Sesion sesionUsuario = gestorSesiones.ObtenerSesion(mensaje.IdUsuario, out nuevaSesion);
                if (nuevaSesion)
                {
                    System.Console.WriteLine($"[NUEVA SESIÓN] - ID: {sesionUsuario.IdSesion} - ID USUARIO: {mensaje.IdUsuario}");
                }

                sesionUsuario.PLN.ObtenerIntencion(mensaje.Texto);

                try
                {
                    Console.WriteLine($"[NUEVO MENSAJE] - ID USUARIO: {mensaje.IdUsuario} INTENCIÓN: {sesionUsuario.PLN.UltimaIntencion.Nombre} ({sesionUsuario.PLN.UltimaIntencion.ConfianzaDeteccion}%) - ENVIÓ: {mensaje.Texto}");
                }
                catch (Exception e)
                {
                }

                IHandler resultado = handler.Resolver(sesionUsuario, mensaje, out respuesta);

                try
                {
                    cliente.EnviarMensaje(respuesta);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine($"[EXCEPCIÓN] - {e.ToString()}");
                }
            }
        }
    }
}