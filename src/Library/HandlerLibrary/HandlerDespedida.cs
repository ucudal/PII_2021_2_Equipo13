using System;
using System.Collections.Generic;
using PII_E13.ClassLibrary;
using PII_E13.HandlerLibrary;

namespace LibraryHandler
{
    /// <summary>
    /// Handler para "despedir" al usuario del  bot, implemenata el mensaje "Hasta Pronto!"
    /// </summary>
    public class HandlerDespedida : HandlerBase
    {
        HandlerDespedida(HandlerBase siguiente, string Intencion) : base(siguiente, Intencion)
        {
            this.Siguiente = siguiente;
        }
        public new bool ResolverInterno(IMensaje mensaje, out IRespuesta respuesta)
        {
            respuesta=new Respuesta(mensaje);
            respuesta.Texto = "Hasta pronto!";
            return true;
        }
    }
}