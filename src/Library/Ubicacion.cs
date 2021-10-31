namespace Library
{
    /// <summary>
    /// Patrones y principios utilizados en esta clase:
    /// ISP ya que segmenta las operaciones de la interface en la persistencia.
    /// Expert ya que se le da la responsabilidad de gestionar las coordenas de una ubicacion, debido a que es la clase más experta de la información.
    /// Polymorphism porque utiliza dos métodos polimorficos de persistencia.
    /// </summary>
    public class Ubicacion
    {
        public string Departamento { get; }
        public string Ciudad { get; }
        public string NumPuerta { get; }
        public string Calle { get; }
        public string Esquina { get; }

        public Ubicacion(string departamento, string ciudad, string numPuerta, string calle, string esquina)
        {
            Departamento = departamento;
            Ciudad = ciudad;
            NumPuerta = numPuerta;
            Calle = calle;
            Esquina = esquina;
        }

        public string Redactar()
        {
            return $"{this.Departamento} {this.Ciudad} { NumPuerta } {Calle} {Esquina}";
        }

    }
}
