using System.Threading.Tasks;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Representa a la implementación básica del tipo <see cref="IUbicacion"/>, incluyendo implementaciones para todas sus operaciones.
    /// Aplica Expert ya que se le da la responsabilidad de gestionar las coordenas de una ubicacion, debido a que es la clase más experta de la información.
    /// </summary>
    public class UbicacionBase : IUbicacion
    {
        /// <summary>
        /// Nombre de la ciudad de la ubicación
        /// </summary>
        public string Ciudad { get; }

        /// <summary>
        /// Dirección correspondiente a la ubicación
        /// </summary>
        /// <value></value>
        public string Direccion { get; }

        private IAdaptadorLocacion AdaptadorLocacion = new AdaptadorLocacion();

        /// <summary>
        /// Crea una instancia de la clase Ubicacion
        /// </summary>
        /// <param name="ciudad">Ciudad</param>
        /// <param name="direccion">Dirección</param>
        public UbicacionBase(string ciudad, string direccion)
        {
            this.Ciudad = ciudad;
            this.Direccion = direccion;
        }
        /// <summary>
        /// Genera una cadena de texto con la información de la ubicación
        /// </summary>
        /// <returns>Retorna un string con la ubicación formateada</returns>
        public string Redactar()
        {
            return $"{this.Direccion}, {this.Ciudad}";
        }

        /// <summary>
        /// Calcula la distancia entre dos instancias de implementaciones de <see cref="IUbicacion"/>
        /// Delega la responsabilidad de calcular la distancia al gestor.
        /// </summary>
        /// <param name="otraUbicacion">Otra ubicación, cuya distancia hacia esta instancia se quiere obtener.</param>
        /// <returns>Un double conteniendo la distancia entre las ubicaciones</returns>
        public double ObtenerDistancia(IUbicacion otraUbicacion)
        {
            return AdaptadorLocacion.ObtenerDistancia(this, otraUbicacion);
        }
    }
}