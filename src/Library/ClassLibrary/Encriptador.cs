using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ucu.Poo.Locations.Client;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public static class Encriptador
    {
        /// <summary>
        /// Clase encargada de generar un hash identificador para los objetos del sistema.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetHashCode(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                string hash = GetHash(sha256Hash, input);
                return hash;
            }
        }
        
        /// <summary>
        /// Metodo privado que se encarga de crear el hash en base un input determinado.
        /// fuente de documentacion de .NET
        /// https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.computehash?view=net-5.0
        /// </summary>
        /// <param name="hashAlgorithm"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }


    }
}