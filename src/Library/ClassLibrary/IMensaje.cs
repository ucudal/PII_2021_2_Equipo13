namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Representa un mensaje enviado por un usuario a través de una plataforma de mensajería.
    /// Se aplica el patrón Adapter para desacoplar la información recibida a través de las APIs y almacenarla en instancias de clases que implementen a esta interfaz.
    /// </summary>
    public interface IMensaje
    {
        /// <summary>
        /// Id del chat en el que fue enviado el mensaje.
        /// </summary>
        string IdChat { get; }
        /// <summary>
        /// Id del usuario remitente.
        /// </summary>
        string IdUsuario { get; }
        /// <summary>
        /// Contenido en texto del mensaje.
        /// </summary>
        string Texto { get; }
    }
}