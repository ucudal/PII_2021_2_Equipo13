
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


        public new bool ResolverInterno(Sesion sesion, IMensaje mensaje, out IRespuesta respuesta)
        {
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


         protected override bool PuedeResolver(Sesion sesion)
        {
            try
            {
                Sistema.Instancia.ObtenerEmprendedorPorId(sesion.IdUsuario);
            }
            catch (KeyNotFoundException e)
            {
                return false;
            }

            return sesion.PLN.UltimaIntencion.Nombre.Equals(this.Intencion) ||
                (
                    this.Busquedas.ContainsKey(sesion.IdUsuario) &&
                    (sesion.PLN.UltimaIntencion.Nombre.Equals("Default") || (sesion.PLN.UltimaIntencion.ConfianzaDeteccion < 80))
                );
        }

    }

}