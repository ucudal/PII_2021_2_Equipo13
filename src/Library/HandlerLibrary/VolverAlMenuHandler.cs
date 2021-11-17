
using System.Collections.Generic;
using PII_E13.ClassLibrary;
using PII_E13.HandlerLibrary;

namespace LibraryHandler
{

    public class VolverAlMenuHandler: HandlerBase 
    {
        VolverAlMenuHandler(HandlerBase next) : base(next)
        {
            this.Etiquetas = new string[] { "volver al menu" };
        }
        public new bool ResolverInterno(IMensaje mensaje, out string respuesta)
        {
            this.Siguiente = new MensajeErrorHandler();
            if (this.PuedeResolver(mensaje)) {
                throw new System.NotImplementedException();
            } else {
                respuesta = string.Empty;
                return true;
            }
        }

    }

} 