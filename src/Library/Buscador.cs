using System.Collections.Generic;

namespace ClassLibrary
{
    /// <summary>
    /// Esta es una clase auxiliar que permite la búsqueda de ofertas. Se aplica SRP para tomar la decisión de
    /// separar esta responsabilidad de Sistema, ya que la forma en que se realiza esta búsqueda puede cambiar y
    /// Sistema puede cambiar por otros motivos.
    /// Se aplica el patrón Singleton dentro de esta clase para permitir solo la creación y 
    /// utilización de una instancia de Buscador.
    /// </summary>
    public class Buscador
    {
        /// <summary>
        /// Crea una instancia de Buscador.
        /// </summary>
        private Buscador() { }

        private struct OfertaConPuntaje
        {
            public Oferta oferta { get; set; }
            public int Puntaje { get; set; }
        }

        public static List<Oferta> BuscarOfertas(Sistema sistema, Emprendedor emprendedor, List<string> palabrasClave)
        {
            List<OfertaConPuntaje> ofertasEncontradas = new List<OfertaConPuntaje>();
            List<Oferta> ofertasOrdenadas = new List<Oferta>();

            foreach (Empresa empresa in sistema.Empresas)
            {
                foreach (Oferta oferta in empresa.Ofertas)
                {
                    OfertaConPuntaje ofertaConPuntaje = new OfertaConPuntaje();


                }
            }
            return ofertasEncontradas;
        }

        private static Buscador instancia = null;
        /// <summary>
        /// Instancia del buscador durante la ejecución. Se aplica el patrón Singleton.
        /// </summary>
        /// <value>Una única instancia de <c>Buscador</c></value>
        public static Buscador Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new Buscador();
                }
                return instancia;
            }
        }
    }
}