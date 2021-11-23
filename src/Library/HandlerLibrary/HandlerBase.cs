using System;
using System.Linq;
using PII_E13.ClassLibrary;

namespace PII_E13.HandlerLibrary
{
    /// <summary>
    /// Clase base para implementar el patrón Chain of Responsibility. En ese patrón se pasa un mensaje a través de una
    /// cadena de "handlers" que pueden procesar o no el mensaje. Cada "handler" decide si procesa el mensaje, o si se lo
    /// pasa al siguiente. Esta clase base implmementa la responsabilidad de recibir el mensaje y pasarlo al siguiente
    /// "handler" en caso que el mensaje no sea procesado. La responsabilidad de decidir si el mensaje se procesa o no, y
    /// de procesarlo, se delega a las clases sucesoras de esta clase base.
    /// </summary>
    public abstract class HandlerBase : IHandler
    {
        /// <summary>
        /// Obtiene el próximo "handler".
        /// </summary>
        /// <value>El "handler" que será invocado si este "handler" no procesa el mensaje.</value>
        public IHandler Siguiente { get; set; }

        /// <summary>
        /// Obtiene o asigna el conjunto de palabras clave que este "handler" puede procesar.
        /// </summary>
        /// <value>Un array de palabras clave.</value>
        public string Intencion { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="HandlerBase"/>.
        /// </summary>
        /// <param name="siguiente">El próximo "handler".</param>
        public HandlerBase(IHandler siguiente)
        {
            this.Siguiente = siguiente;
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="HandlerBase"/> con una lista de comandos.
        /// </summary>
        /// <param name="intencion">La intención utilizada para identificar a este handler.</param>
        /// <param name="siguiente">El próximo "handler".</param>
        public HandlerBase(IHandler siguiente, string intencion)
        {
            this.Intencion = intencion;
            this.Siguiente = siguiente;
        }

        /// <summary>
        /// Este método debe ser sobreescrito por las clases sucesores. La clase sucesora procesa el mensaje y retorna
        /// true o no lo procesa y retorna false.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <param name="respuesta">La respuesta al mensaje procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario</returns>
        protected virtual bool ResolverInterno(Sesion sesion, IMensaje mensaje, out IRespuesta respuesta)
        {
            throw new InvalidOperationException("Este método debe ser sobrescrito");
        }

        /// <summary>
        /// Este método puede ser sobreescrito en las clases sucesores que procesan varios mensajes cambiando de estado
        /// entre mensajes deben sobreescribir este método para volver al estado inicial. En la clase base no hace nada.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        protected virtual void CancelarInterno(Sesion sesion)
        {
            // Intencionalmente en blanco.
        }

        /// <summary>
        /// Determina si este "handler" puede procesar el mensaje. En la clase base se utiliza procesado de lenguaje natural
        /// para comprobar que la intención identificada corresponda a la del "handler". Las clases sucesores pueden
        /// sobreescribir este método para proveer otro mecanismo para determina si procesan o no un mensaje.
        /// </summary>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <returns>true si el mensaje puede ser pocesado; false en caso contrario.</returns>
        protected virtual bool PuedeResolver(IMensaje mensaje)
        {
            // Cuando no hay palabras clave este método debe ser sobreescrito por las clases sucesoras y por lo tanto
            // este método no debería haberse invocado.
            if (this.Etiquetas == null || this.Etiquetas.Length == 0)
            {
                throw new InvalidOperationException("No hay palabras clave que puedan ser procesadas");
            }
            if (Sistema.Instancia.ObtenerEmprendedorPorId(mensaje.IdUsuario) != null)
            {
                throw new InvalidOperationException("Usted ya está registrado.");
            }

            return this.Etiquetas.Any(s => mensaje.Texto.Equals(s, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Determina si este "handler" puede procesar el mensaje. En la clase base se utiliza el array
        /// <see cref="HandlerBase.Etiquetas"/> para buscar el texto en el mensaje ignorando mayúsculas y minúsculas. Las
        /// clases sucesores pueden sobreescribir este método para proveer otro mecanismo para determina si procesan o no
        /// un mensaje.
        /// </summary>
        /// <param name="callback">El mensaje a procesar.</param>
        /// <returns>true si el mensaje puede ser pocesado; false en caso contrario.</returns>
        protected virtual bool PuedeResolver(Sesion sesion)
        {
            // Cuando no hay palabras clave este método debe ser sobreescrito por las clases sucesoras y por lo tanto
            // este método no debería haberse invocado.
            if (this.Etiquetas == null || this.Etiquetas.Length == 0)
            {
                throw new InvalidOperationException("No hay palabras clave que puedan ser procesadas");
            }
              if (Sistema.Instancia.ObtenerEmprendedorPorId(callback.IdUsuario) != null)
            {
                throw new InvalidOperationException("Usted ya está registrado.");
            }

            return sesion.PLN.UltimaIntencion.Nombre.Equals(this.Intencion);
        }

        /// <summary>
        /// Procesa el mensaje o la pasa al siguiente "handler" si existe.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <param name="respuesta">La respuesta al mensaje procesado.</param>
        /// <returns>El "handler" que procesó el mensaje si el mensaje fue procesado; null en caso contrario.</returns>
        public IHandler Resolver(Sesion sesion, IMensaje mensaje, out IRespuesta respuesta)
        {
            if (this.ResolverInterno(sesion, mensaje, out respuesta))
            {
                return this;
            }
            else if (this.Siguiente != null)
            {
                return this.Siguiente.Resolver(sesion, mensaje, out respuesta);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retorna este "handler" al estado inicial. En los "handler" sin estado no hace nada. Los "handlers" que
        /// procesan varios mensajes cambiando de estado entre mensajes deben sobreescribir este método para volver al
        /// estado inicial.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        public virtual void Cancelar(Sesion sesion)
        {
            this.CancelarInterno(sesion);
            if (this.Siguiente != null)
            {
                this.Siguiente.Cancelar(sesion);
            }
        }
    }
}