using System.Threading.Tasks;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Patrones y principios utilizados en esta clase:
    /// ISP ya que segmenta las operaciones de la interface en la persistencia.
    /// Expert ya que se le da la responsabilidad de gestionar las coordenas de una ubicacion, debido a que es la clase más experta de la información.
    /// </summary>
    public class Ubicacion: IJsonConvertible
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
        public Ubicacion(string ciudad, string direccion)
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
        /// Delega la responsabilidad de calcular la distancia al gestor.
        /// </summary>
        /// <param name="target">Distancia objectivo</param>
        /// <returns></returns>
        public double ObtenerDistancia(Ubicacion target)
        {
            return AdaptadorLocacion.ObtenerDistancia(this, target);
        }

    }
}