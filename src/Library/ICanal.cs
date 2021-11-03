namespace ClassLibrary
{
    /// <summary>
    /// Esta clase respresenta la interfaz de los canales
    /// </summary>
    public interface ICanal
    {
        /// <summary>
        /// Recibe un mensaje de un usuario
        /// </summary>
        string RecibirMensaje();

        /// <summary>
        /// Envía un mensaje a un usuario
        /// </summary>
        /// <param name="message">message</param>        
        void EnviarMensaje(string message);

    }

}