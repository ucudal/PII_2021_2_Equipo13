
using System.Collections.Generic;
using PII_E13.ClassLibrary;
using PII_E13.HandlerLibrary;

namespace LibraryHandler
{

    public class VerOfertasHandler : HandlerBase
    {
        VerOfertasHandler(HandlerBase next) : base(next)
        {
            this.Etiquetas = new string[] { "ver ofertas" };
        }
        public new bool ResolverInterno(IMensaje mensaje, out string respuesta)
        {
            if (this.PuedeResolver(mensaje))
            {
                Sistema sistema = Sistema.Instancia;
                List<Oferta> ofertas = Buscador.Instancia.BuscarOfertas(sistema, sistema.ObtenerEmprendedorPorId(mensaje.IdUsuario));
                string respuestaInterna = "";
                ofertas.ForEach(oferta =>
                {
                    respuestaInterna += oferta.RedactarResumen() + "\n";
                });
                respuesta = respuestaInterna;
                return true;
            }
            else
            {
                respuesta = string.Empty;
                return false;
            }
        }

    }

}