namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Clase de ejemplo para una implementación de la interfaz ICanal utilizada en los tests donde es relevante tener una implementación de un canal.
    /// </summary>
    public class Canal1 : ICanal
    {
        /// <summary>
        /// Envía un mensaje a un usuario de un canal.
        /// </summary>
        /// <param name="message"></param>
        public void EnviarMensaje(string message)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Recibe un mensaje de un usuario de un canal.
        /// </summary>
        public string RecibirMensaje()
        {
            throw new System.NotImplementedException();
        }
    }

}
