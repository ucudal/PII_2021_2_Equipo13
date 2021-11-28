using System;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Interface que representa las operaciones de un persistor de información a través de diferentes medios de persistencia.
    /// </summary>
    public interface IPersistor
    {
        /// <summary>
        /// Guarda el contenido de la cadena de caracteres de información en el destino indicado.
        /// </summary>
        /// <param name="destino">El destino donde se guardará la información.</param>
        /// <param name="objeto">El objeto a guardar.</param>
        void Escribir<T>(string destino, T objeto) where T : IIdentificable;

        /// <summary>
        /// Lee la información contenida en la fuente de información enviada por parámetros.
        /// </summary>
        /// <param name="fuente">La dirección, referencia o ruta hacia la fuente de información.</param>
        /// <returns>Retorna la información encontrada como una instancia de la clase enviada por parámetros de tipo.</returns>
        T Leer<T>(string fuente) where T : new();
    }
}
