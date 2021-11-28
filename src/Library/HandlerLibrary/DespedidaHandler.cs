using PII_E13.ClassLibrary;
using PII_E13.HandlerLibrary;

namespace LibraryHandler
{
    /// <summary>
    /// Handler encargado de despedir a los usuarios al final de una conversación.
    /// </summary>
    public class DespedidaHandler : HandlerBase
    {

        /// <summary>
        /// Crea una instancia de <see cref="DespedidaHandler"/>.
        /// </summary>
        /// <param name="siguiente">El próximo "handler".</param>
        /// <param name="intencion">La intención utilizada para identificar a este handler.</param>
        /// <returns></returns>
        public DespedidaHandler(HandlerBase siguiente, string intencion) : base(siguiente, intencion)
        {
        }

        /// <summary>
        /// La clase procesa el mensaje y retorna true o no lo procesa y retorna false.        
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <param name="respuesta">La respuesta al mensaje procesado.</param>
        /// <returns></returns>
        protected override bool ResolverInterno(Sesion sesion, IMensaje mensaje, out IRespuesta respuesta)
        {
            respuesta = new Respuesta(mensaje);
            respuesta.Texto = "Estaré disponible siempre que necesites gestionar tus ofertas.\n\n¡Hasta pronto! 👋";
            return true;
        }
    }
}