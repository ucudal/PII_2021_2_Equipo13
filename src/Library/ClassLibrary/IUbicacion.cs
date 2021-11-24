using System.Threading.Tasks;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Representa a la información y operaciones básicas de una ubicación.
    /// Se aplica DIP al aplicar dependencias en una abstracción de una ubicación, en lugar de una clase específica.
    /// </summary>
    public interface IUbicacion
    {
        /// <summary>
        /// Nombre de la ciudad de la ubicación
        /// </summary>
        string Ciudad { get; }

        /// <summary>
        /// Dirección correspondiente a la ubicación
        /// </summary>
        /// <value></value>
        string Direccion { get; }

        /// <summary>
        /// Genera una cadena de texto con la información de la ubicación
        /// </summary>
        /// <returns>Retorna un string con la ubicación formateada</returns>
        string Redactar();

        /// <summary>
        /// Calcula la distancia entre dos instancias de implementaciones de <see cref="IUbicacion"/>
        /// Delega la responsabilidad de calcular la distancia al gestor.
        /// </summary>
        /// <param name="otraUbicacion">Otra ubicación, cuya distancia hacia esta instancia se quiere obtener.</param>
        /// <returns>Un double conteniendo la distancia entre las ubicaciones</returns>
        double ObtenerDistancia(IUbicacion otraUbicacion);
    }
}