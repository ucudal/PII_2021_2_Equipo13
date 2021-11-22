using System.Collections.Generic;
using System;


namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Clase encargada de administrar todas las sesiones del sistema por lo que cumple con SRP y Expert.
    /// Cumple con Singleton para asegurarnos de no duplicar los datos en varias instancias
    /// y que esta clase actua a forma de servicio de ser el referente de almacenar las sesiones y gestionarlas.
    /// Cumple con creator para agregar más sesiones al listado general de sesiones.
    /// </summary>

    public class GestorSesiones
    {
        private static GestorSesiones instancia = null;

        private GestorSesiones()
        {
            Sesiones = new Dictionary<string, Sesion>();
        }

        /// <summary>
        /// Obtiene una sesión para el identificador único de usuario enviado por parámetros.
        /// Si el usuario ya tenía una sesión activa, se retorna dicha sesión; de lo contrario, se crea y retorna una nueva.
        /// </summary>
        /// <param name="idUsuario">Identificador único del usuario.</param>
        /// <param name="nuevaSesion">Booleano que indica si la sesión es nueva.</param>
        public Sesion ObtenerSesion(string idUsuario, out bool nuevaSesion)
        {
            if (this.Sesiones.ContainsKey(idUsuario))
            {
                Sesion sesionUsuario = this.Sesiones[idUsuario];
                if (sesionUsuario.Activa)
                {
                    nuevaSesion = false;
                    return sesionUsuario;
                }
                else
                {
                    this.Sesiones.Remove(idUsuario);
                }
            }
            nuevaSesion = true;
            Sesion sesion = new Sesion(idUsuario);

            this.Sesiones.Add(idUsuario, sesion);
            return sesion;
        }

        /// <summary>
        /// Diccionario de sesiones existentes actualmente.
        /// </summary>
        /// <value>Un diccionario de string, <see cref="Sesion"/> conteniendo todas las sesiones existentes.</value>
        public Dictionary<string, Sesion> Sesiones { get; }

        /// <summary>
        /// Instancia del gestor de sesiones durante la ejecución. Se aplica el patrón Singleton.
        /// </summary>
        public static GestorSesiones Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new GestorSesiones();
                }
                return instancia;
            }
        }
    }
}