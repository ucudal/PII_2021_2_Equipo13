using Telegram.Bot.Types;
using PII_E13.ClassLibrary;
using System.Collections.Generic;
using System.Text;
using System;

namespace PII_E13.HandlerLibrary
{
    public class BienvenidaHandler : HandlerBase
    {
        private const int COLUMNAS_CATEGORIAS = 2;
        private const int FILAS_CATEGORIAS = 1;
        private const int COLUMNAS_REGISTRO = 2;
        private const int FILAS_REGISTRO = 1;

        BienvenidaHandler(HandlerBase siguiente) : base(siguiente)
        {
            this.Siguiente = siguiente;
        }
        public new bool ResolverInterno(IMensaje mensaje, out IRespuesta respuesta)
        {
            respuesta = new Respuesta(mensaje);
            respuesta.Texto = "Bienvenido al sistema : Pantalla de inicio";
            return true;
        }
        
            List<InlineKeyboardButton> botonesDeBienvenida = TelegramBot.Instancia.ObtenerBotones(); //Botones registro
            List<List<InlineKeyboardButton>> tecladoFijoCategorias = new List<List<InlineKeyboardButton>>() {
            new List<InlineKeyboardButton>() {TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo}
            };
            List<List<InlineKeyboardButton>> tecladoFijoBienvenida = new List<List<InlineKeyboardButton>>()
            {
            };
    }
}