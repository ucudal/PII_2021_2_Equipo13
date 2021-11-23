namespace PII_E13.ClassLibrary
{

    /// <summary>
    /// Interfaz que representa un botón adjuntado a un mensaje.
    /// Se aplica el patrón adapter y se define una interfaz genérica de botón para disminuir el acoplamiento con las APIs de mensajería.
    /// </summary>
    public interface IEnviador
    {

        /// <summary>
        /// Envia un mensaje a la plataforma específica de este enviador.
        /// </summary>
        /// <param name="respuesta">Respuesta a enviar a la plataforma específica.</param>
        void EnviarMensaje(IRespuesta respuesta);
    }
}