using System.Collections.Generic;
using System.Linq;

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

        public static List<Oferta> BuscarOfertas(Sistema sistema, Emprendedor emprendedor, List<string> etiquetas,
            List<string> categorias=null)
        {
            Dictionary<Oferta, int> ofertasEncontradas = new Dictionary<Oferta, int>();

            foreach (Empresa empresa in sistema.Empresas)
            {
                foreach (Oferta oferta in empresa.Ofertas)
                {
                    int puntaje = 0;
                    foreach(string etiqueta in etiquetas){
                        if (oferta.Etiquetas.Contains(etiqueta))
                            puntaje++;
                    }
                    foreach (Producto producto in oferta)
                    {
                        foreach(string categoria in categorias){
                            if (producto.Material.Categorias.Contains(categoria))
                                puntaje += 5;
                        }
                    }
                    /* TODO - Comparar ubicación de Oferta con Emprendedor y asignar un valor en un rango
                        * de 0 a 30. */
                    
                    ofertasEncontradas.Add(oferta, puntaje);
                }
            }
            return OrdenarOfertasPorPuntaje(ofertasEncontradas);
        }

        /// <summary>
        /// Ordena un diccionario de par <c>Oferta, int</c> de mayor a menor según el valor
        /// asignado a cada clave.
        /// </summary>
        /// <param name="ofertas">Dictionary de par <c>Oferta, int</c> a ordenar.</param>
        /// <returns><c>List de Ofertas</c> ordenadas de mayor a menor.</returns>
        private static List<Oferta> OrdenarOfertasPorPuntaje(Dictionary<Oferta, int> ofertas)
        {
            IEnumerable<Oferta> ofertasOrdenadas = from pair in ofertas
                                orderby pair.Value descending
                                select pair.Key;

            return ofertasOrdenadas.ToList<Oferta>();
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