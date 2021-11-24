using System.Security.Cryptography;
using System.Text;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Interfaz encargada de generar un código de encriptación recibiendo una entrada.
    /// Se aplica DIP para asignarle esta responsabilidad a una abstracción, en forma de interfaz.
    /// </summary>
    public interface IEncriptador
    {
        /// <summary>
        /// Genera un hash utilizando un algoritmo de encriptación.
        /// </summary>
        /// <param name="entrada">Semilla para la generación del hash.</param>
        /// <returns>Una cadena de caracteres correspondiente a un hash obtenido al codificar la entrada.</returns>
        string ObtenerCodigo(string entrada);
    }
}