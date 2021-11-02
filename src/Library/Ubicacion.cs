namespace ClassLibrary
{
    /// <summary>
    /// Patrones y principios utilizados en esta clase:
    /// ISP ya que segmenta las operaciones de la interface en la persistencia.
    /// Expert ya que se le da la responsabilidad de gestionar las coordenas de una ubicacion, debido a que es la clase más experta de la información.
    /// Polymorphism porque utiliza dos métodos polimorficos de persistencia.
    /// </summary>
    public class Ubicacion
    {
        public string Ciudad { get; }
        public string Direccion { get; }

        public Ubicacion(string ciudad, string direccion)
        {
            this.Ciudad = ciudad;
            this.Direccion = direccion;
        }

        public string Redactar()
        {
            return $"{this.Departamento} {this.Ciudad} { NumPuerta } {Calle} {Esquina}";
        }

    }
}
