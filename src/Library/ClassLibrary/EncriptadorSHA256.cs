using System.Security.Cryptography;
using System.Text;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Clase encargada de generar un hash identificador para los objetos del sistema.
    /// </summary>
    public class EncriptadorSHA256 : IEncriptador
    {
        /// <summary>
        /// Genera un hash utilizando SHA256 con una entrada
        /// </summary>
        /// <param name="entrada">Semilla para la generación del hash.</param>
        /// <returns>Una cadena de caracteres correspondiente a un hash obtenido de codificar la entrada con SHA256.</returns>
        public string ObtenerCodigo(string entrada)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                string hash = this.ObtenerHash(sha256Hash, entrada);
                return hash;
            }
        }

        /// <summary>
        /// Metodo privado que se encarga de crear el hash en base un input determinado.
        /// Extraído de la documentación de .NET:
        /// https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.computehash?view=net-5.0
        /// </summary>
        /// <param name="algoritmoDeHash">Algoritmo de hash a utilizar.</param>
        /// <param name="entrada">Semilla para la generación del hash.</param>
        /// <returns></returns>
        private string ObtenerHash(HashAlgorithm algoritmoDeHash, string entrada)
        {

            byte[] data = algoritmoDeHash.ComputeHash(Encoding.UTF8.GetBytes(entrada));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }


    }
}