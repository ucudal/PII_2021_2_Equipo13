
using System.Collections.Generic;
using PII_E13.ClassLibrary;
using PII_E13.HandlerLibrary;

namespace LibraryHandler
{

    public class MensajeErrorHandler: HandlerBase 
    {
        public MensajeErrorHandler(HandlerBase next) : base(next)
        {
            this.Etiquetas = new string[] { "" };
        }
        public new bool ResolverInterno(IMensaje mensaje, out string respuesta)
        {
            respuesta = "Lo siento no pudimos procesar tu mensaje, escribe 'volver al menu' para retornar al principio";
            return true;
        }

    }

} 