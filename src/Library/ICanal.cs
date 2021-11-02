namespace ClassLibrary
{
    /// <summary>
    /// Esta clase respresenta la interfaz de los canales
    /// </summary>
    public interface ICanal
    {
        /// <summary>
        /// Este método se encargará de recibir los mensajes
        /// </summary>
        string recibirMensaje();

        /// <summary>
        /// Este método se encargará de enviar los mensajes
        /// </summary>
        /// <param name="message">message</param>        
        void enviarMensaje(string message);

    }

}
