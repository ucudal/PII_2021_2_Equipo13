namespace ClassLibrary
{
    /// <summary>
    /// Patrones y principios utilizados en esta clase:
    /// ISP ya que segmenta las operaciones de la interface en la persistencia.
    /// Expert ya que se le da la responsabilidad de gestionar las coordenas de una ubicacion, debido a que es la clase más experta de la información.
    /// </summary>
    public class Ubicacion
    {
        public string Ciudad { get; }
        public string Direccion { get; }

        public GestorLocacion GestorLocacion = new GestorLocacion();

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

    }
}
