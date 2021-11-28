using System;
using System.Collections.Generic;
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
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        /// <returns>true si el mensaje puede ser pocesado; false en caso contrario.</returns>
        protected virtual bool PuedeResolver(Sesion sesion)
        {
            return sesion.PLN.UltimaIntencion.Nombre.Equals(this.Intencion) && sesion.PLN.UltimaIntencion.ConfianzaDeteccion > 75;
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

        /// <summary>
        /// Genera una matriz de botones en base a una lista de instancias de implementaciones de <see cref="IBoton"/>, un índice del cual empezar, 
        /// las filas y columnas en las que agrupar los botones y una matriz de botones fijos incluidos al final.
        /// </summary>
        /// <param name="botones">Lista de instancias de <see cref="IBoton"/> a utilizar para generar la matriz.</param>
        /// <param name="indiceInicial">Índice de la lista desde el cual empezar. (Opcional, 0 por defecto)</param>
        /// <param name="filas">Cantidad de filas de botones para la matriz. (Opcional, 1 por defecto)</param>
        /// <param name="columnas">Cantidad de botones a incluir por fila de la matriz. (Opcional, 1 por defecto)</param>
        /// <param name="botonesFijos">Matriz de botones fijos incluida al final de la matriz. (Opcional)</param>
        /// <returns>Una matriz de instancias de implementaciones de <see cref="IBoton"/>.</returns>
        protected List<List<IBoton>> ObtenerMatrizDeBotones(List<IBoton> botones, int indiceInicial = 0, int filas = 1, int columnas = 1, List<List<IBoton>> botonesFijos = null)
        {
            List<List<IBoton>> matrizBotones = new List<List<IBoton>>();
            for (int i = 0; i < filas; i++)
            {
                List<IBoton> fila = new List<IBoton>();
                for (int j = 0; j < columnas; j++)
                {
                    try
                    {
                        fila.Add(botones[indiceInicial]);
                        indiceInicial++;
                    }
                    catch (Exception e)
                    {
                        break;
                    }
                }
                if (fila.Count > 0)
                {
                    matrizBotones.Add(fila);
                }
            }
            if (botonesFijos != null)
            {
                matrizBotones.AddRange(botonesFijos);
            }
            return matrizBotones;
        }
    }
}