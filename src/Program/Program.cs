using System;
using System.Collections.Generic;
using PII_E13.ClassLibrary;
using PII_E13.HandlerLibrary;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Application
{
    /// <summary>
    /// Programa principal.
    /// </summary>
    public static class Program
    {
        // INSTANCIAR COMO ALGÚN HANDLER.
        private static IHandler handler = new PostularseAOfertaHandler(null, "Buscar Ofertas");
        //private static IHandler handler = new RegistrarEmprendedorHandler(null);

        private static GestorSesiones gestorSesiones = GestorSesiones.Instancia;

        /// <summary>
        /// Punto de entrada al programa principal.
        /// </summary>
        public static void Main()
        {
            // DATOS DE PRUEBA -------------------------------------------------------------------------------------------
            /*Sistema.Instancia.Materiales.Add(new Material("Madera de roble", "Kg", new List<string>() { "Madera", "Roble", "Carpintería" }));
            Sistema.Instancia.Materiales.Add(new Material("Madera de abeto", "Kg", new List<string>() { "Madera", "Abeto", "Carpintería" }));
            Sistema.Instancia.Materiales.Add(new Material("Madera de pino", "Kg", new List<string>() { "Madera", "Pino", "Carpintería" }));
            Sistema.Instancia.Materiales.Add(new Material("Madera de cedro", "Kg", new List<string>() { "Madera", "Cedro", "Carpintería" }));
            Sistema.Instancia.Materiales.Add(new Material("Madera de arce", "Kg", new List<string>() { "Madera", "Arce", "Carpintería" }));
            Sistema.Instancia.Materiales.Add(new Material("Madera de haya", "Kg", new List<string>() { "Madera", "Haya", "Carpintería" }));
            Sistema.Instancia.Materiales.Add(new Material("Madera de nogal", "Kg", new List<string>() { "Madera", "Nogal", "Carpintería" }));
            Sistema.Instancia.Materiales.Add(new Material("Madera de cerezo", "Kg", new List<string>() { "Madera", "Cerezo", "Carpintería" }));
            Sistema.Instancia.Materiales.Add(new Material("Madera de caoba", "Kg", new List<string>() { "Madera", "Caoba", "Carpintería" }));
            Sistema.Instancia.Materiales.Add(new Material("Cobre", "Kg", new List<string>() { "Metal", "Resistente", "Conductor" }));
            */
            Sistema.Instancia.RegistrarEmprendedor("2101409600", "Montevideo", "Constitución 2450", "Informática", "Walter S.A.", new List<Habilitacion>());
            Sistema.Instancia.RegistrarEmprendedor("2101529600", "Montevideo", "Constitución 2450", "Informática", "Walter S.A.", new List<Habilitacion>());
            Sistema.Instancia.RegistrarEmpresa("123123", "Canelones", "Canelones 1234", "Industria", "Industria S.A.");

            Empresa e = Sistema.Instancia.ObtenerEmpresaPorId("123123");
            e.PublicarOferta("123", "Lorem Ipsum", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum ut ultrices nulla. Praesent eu magna ac tellus fringilla bibendum at ac mauris. Duis id odio quis sem porttitor imperdiet ac et libero. Duis vitae accumsan elit. Sed dictum fermentum fringilla. Etiam sagittis eros urna, aliquam pulvinar lectus pretium sed.",
                DateTime.MaxValue, etiquetas: new List<string> { "Prueba", "Lorem", "Ipsum" });

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