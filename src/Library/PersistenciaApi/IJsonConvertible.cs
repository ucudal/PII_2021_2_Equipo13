using System.Text.Json;

namespace Recipes
{
    /// <summary>
    /// Define el tipo que tienen los objetos que pueden ser serializados en y deserializados desde texto en formato
    /// Json.
    /// </summary>
    public interface IJsonConvertible
    {
        /// <summary>
        /// Convierte el objeto a texto en formato Json. El objeto puede ser reconstruido a partir del texto en formato
        /// Json utilizando JsonSerializer.Deserialize.
        /// </summary>
        /// <returns>El objeto convertido a texto en formato Json.</returns>
        string ConvertToJson();
    }
}