
namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Representa una intención obtenida a través de procesamiento de lenguaje natural (PLN) a partir de un texto.
    /// Es una clase contenedora de información.
    /// Se aplica el patrón Adapter para desacoplar las APIs de Google para PLN a través de esta clase y su clase constructora.
    /// <see cref="LenguajeNatural"/>, que representa a un procesador de lenguaje natural.
    /// </summary>
    public class Intencion
    {
        /// <summary>
        /// Crea una instancia de una <see cref="Intencion"/> obtenida a partir de analizar un texto con PLN.
        /// </summary>
        /// <param name="nombre">El Display Name de la <see cref="Intencion"/> en Dialogflow.</param>
        /// <param name="confianzaDeteccion">El porcentaje, de 0 al 100, de confianza en la detección de la <see cref="Intencion"/>.</param>
        /// <param name="entrada">La entrada de texto que se analizó para encontrar esta <see cref="Intencion"/>.</param>
        public Intencion(string nombre, float confianzaDeteccion, string entrada)
        {
            this.Nombre = nombre;
            this.ConfianzaDeteccion = confianzaDeteccion;
            this.Entrada = entrada;
        }

        /// <summary>
        /// La entrada de texto que se analizó para encontrar esta <see cref="Intencion"/>.
        /// </summary>
        /// <value>Una cadena de caracteres conteniendo la entrada de texto analizada para encontrar esta <see cref="Intencion"/>.</value>
        public string Entrada { get; }

        /// <summary>
        /// El Display Name de la <see cref="Intencion"/> en Dialogflow.
        /// </summary>
        /// <value>Una cadena de caracteres conteniendo el nombre de la <see cref="Intencion"/>.</value>
        public string Nombre { get; }

        /// <summary>
        /// El porcentaje, de 0 al 100, de confianza en la detección de la <see cref="Intencion"/>.
        /// </summary>
        /// <value>Un valor float del 0 al 100 representando el porcentaje de confianza en la identificación de la <see cref="Intencion"/>.</value>
        public float ConfianzaDeteccion { get; }
    }
}