using System.Collections.Generic;
using System.Text;
using PII_E13.ClassLibrary;

namespace PII_E13.HandlerLibrary
{
    /// <summary>
    /// Handler encargado de la primera interacción con el usuario del sistema.
    /// </summary>
    public class BienvenidaHandler : HandlerBase
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RegistrarEmprendedorHandler"/>. 
        /// </summary>
        public BienvenidaHandler(HandlerBase siguiente, string intencion) : base(siguiente, intencion)
        {
        }

        /// <summary>
        /// La clase procesa el mensaje y retorna true o no lo procesa y retorna false.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <param name="respuesta">La respuesta al mensaje procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario</returns>
        protected override bool ResolverInterno(Sesion sesion, IMensaje mensaje, out IRespuesta respuesta)
        {
            respuesta = new Respuesta(mensaje);
            if (!this.PuedeResolver(sesion))
            {
                return false;
            }

            try
            {
                Sistema.Instancia.ObtenerEmprendedorPorId(mensaje.IdUsuario);
            }
            catch (KeyNotFoundException e)
            {
                try
                {
                    Sistema.Instancia.ObtenerEmpresaPorId(mensaje.IdUsuario);
                }
                catch (KeyNotFoundException e2)
                {
                    StringBuilder st = new StringBuilder();
                    st.Append("¡Bienvenido!");
                    st.Append("\n\nPara interactuar con este Bot 🤖 puedes registrarte como Empresa, lo que te permitirá publicar ofertas de trabajos de reciclaje, o como Emprendedor, para postularse a una oferta existente.");
                    st.Append("\n\nEn todo momento podrás navegar haciendo click en los botones que aparecerán debajo de los mensajes.");
                    st.Append("\n\nCuando desees comenzar con tu registro, haz click en el botón. ¡Es muy fácil!");
                    List<List<IBoton>> tecladoFijo = new List<List<IBoton>>()
                        {
                            new List<IBoton>() {new Boton("¡Regístrate ahora!", "Menu")}
                        };
                    respuesta.Botones = this.ObtenerMatrizDeBotones(null, 0, botonesFijos: tecladoFijo);
                    respuesta.Texto = st.ToString();

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Retorna este "handler" al estado inicial.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        public override void Cancelar(Sesion sesion)
        {
            this.CancelarInterno(sesion);
            if (this.Siguiente != null)
            {
                this.Siguiente.Cancelar(sesion);
            }
        }
    }
}