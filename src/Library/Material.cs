using System;

namespace ClassLibrary
{
    public class Material
    {
        public float ValorMonetarioPesos;
        public float ValorMonetarioDolares;
        public Material(string nombre, string[] categorias, string unidadMedidaEstandar, float valorMonetarioPesos, float valorMonetarioDolares)
        {
            this.Nombre = nombre;
            this.Categorias = categorias;
            this.UnidadMedidaEstandar = unidadMedidaEstandar;
            this.ValorMonetarioPesos = valorMonetarioPesos;
            this.ValorMonetarioDolares = valorMonetarioDolares;
        }

        public string Nombre { get; set; }
        public string Categorias { get; set; }
        public string UnidadMedidaEstandar { get; set; }
        public string ValorMOnetarioDolares { get; set; }

    }
}