namespace ClassLibrary
{
    public class Ubicacion
    {
        public string Ciudad { get; }
        public string Direccion { get; }

        public Ubicacion(string ciudad, string direccion)
        {
            Ciudad = ciudad;
            Direccion = direccion;
        }

        public string Redactar()
        {
            return $"{this.Direccion}, {this.Ciudad}";
        }

    }
}