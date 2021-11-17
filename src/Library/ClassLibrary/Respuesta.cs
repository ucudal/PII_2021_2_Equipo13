namespace PII_E13.ClassLibrary
{

    /// <summary>
    /// Clase que representa una respuesta a un mensaje.
    /// DOCUMENTAR PATRONES APLICADOS
    /// </summary>
    public class Respuesta : IRespuesta
    {

        /// <summary>
        /// Crea una instancia de Respuesta con un texto asignado.
        /// </summary>
        public Respuesta(string texto){
            this.Texto = texto;
        }

        /// <summary>
        /// Texto del mensaje de la respuesta.
        /// </summary>
        public string Texto { get; set; }
    }
}