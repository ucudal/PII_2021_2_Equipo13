namespace Library
{
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