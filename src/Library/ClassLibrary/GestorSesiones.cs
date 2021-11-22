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
        /// Identificador del usuario. Es único.
        /// </summary>
        /// <param name="usuarioId"></param>
        public Sesion AgregarSesion(string usuarioId)
        {
            Sesion sesion = new Sesion(Encriptador.GetHashCode(usuarioId + DateTimeOffset.Now.ToUnixTimeSeconds().ToString()), usuarioId);
            Sesiones.Add(sesion.IdSesion, sesion);
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