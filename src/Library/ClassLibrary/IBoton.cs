namespace PII_E13.ClassLibrary
{

    /// <summary>
    /// Interfaz que representa un botón adjuntado a un mensaje.
    /// Se aplica el patrón adapter y se define una interfaz genérica de botón para disminuir el acoplamiento con las APIs de mensajería.
    /// </summary>
    public interface IBoton
    {
        /// <summary>
        /// Texto del botón.
        /// </summary>
        string Texto { get; }

        /// <summary>
        /// Texto recibido por el sistema cuando este botón es seleccionado.
        /// </summary>
        string Callback { get; }
    }
}