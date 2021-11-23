using System.Collections.Generic;

namespace PII_E13.ClassLibrary
{

    /// <summary>
    /// Interfaz que representa una respuesta a un mensaje.
    /// DOCUMENTAR PATRONES APLICADOS
    /// </summary>
    public interface IRespuesta
    {
        /// <summary>
        /// Texto del mensaje de la respuesta.
        /// </summary>
        string Texto { get; }

        /// <summary>
        /// Identificador único de un usuario en una plataforma de mensajería.
        /// </summary>
        string IdUsuario { get; }

        /// <summary>
        /// Indica si el último mensaje debe ser editado, de ser posible.
        /// </summary>
        bool EditarMensaje { get; }

        /// <summary>
        /// Matriz de botones enviados junto al mensaje
        /// </summary>
        List<List<IBoton>> Botones { get; }
    }
}