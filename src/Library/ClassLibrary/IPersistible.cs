using System.Collections.Generic;

namespace PII_E13.ClassLibrary
{

    /// <summary>
    /// Interfaz comun para clases que se encargen de persistir informacion del sistema
    /// </summary>
    public abstract class Persistible
    {
        /// <summary>
        /// Operacion que se encarga de persistir la informacion en un formato dado
        /// </summary>
        /// 
        public static void GuardarEn() {}

        /// <summary>
        /// Operacion que se encarga de recuperar la informacion de un formato dado
        /// </summary>
        public static string CargarDesde() {}


    }
}