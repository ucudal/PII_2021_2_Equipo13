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
    }
}