
using System.Collections.Generic;
using PII_E13.ClassLibrary;
using PII_E13.HandlerLibrary;

namespace LibraryHandler
{

    public class VerOfertasHandler: HandlerBase 
    {
        public IHandler Resolver(IMensaje message, out string response)
        {
            Sistema sistema = Sistema();
            List<Oferta> ofertas = Buscador.Instancia.BuscarOfertas(sistema, sistema.ObtenerEmprendedorPorId(message.IdUsuario));
            string respuesta = "";
            ofertas.ForEach(oferta => {
                respuesta += oferta.RedactarResumen() + "\n";
            });
            response = respuesta;
        }

    }

}