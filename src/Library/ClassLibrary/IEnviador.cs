namespace PII_E13.ClassLibrary
{

    /// <summary>
    /// Interfaz que representa un tipo capaz de enviar un mensaje a una plataforma de mensajería.
    /// Se aplica DIP para asignar la responsabilidad de enviar mensajes a una abstraccion de tipo IEnviador y realizar la implementación
    /// correspondiente a cada plataforma en subtipos de éste.
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