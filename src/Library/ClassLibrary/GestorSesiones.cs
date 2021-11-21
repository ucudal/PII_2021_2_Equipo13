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
        public Dictionary<string, Sesion> Sesiones { get; }


        private GestorSesiones()
        {
            Sesiones = new Dictionary<string, Sesion>();
        }

        /// <summary>
        /// Identificador del usuario. Es único.
        /// </summary>
        /// <param name="usuarioId"></param>
        public AgregarSesion(string usuarioId)
        {
            Sesion sesion = new Sesion(Encriptador.GetHashCode(usuarioId), usuarioId);
            Sesiones.Add(sesion.Id, sesion);
        }
        private static GestorSesiones instancia = null;
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