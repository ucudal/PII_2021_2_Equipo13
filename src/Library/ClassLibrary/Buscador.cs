using System;
using System.Collections.Generic;
using System.Linq;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Esta es una clase auxiliar que permite la búsqueda de ofertas. Se aplica SRP para tomar la decisión de separar esta responsabilidad de Sistema,
    /// ya que la forma en que se realiza esta búsqueda puede cambiar y Sistema puede tener otros motivos para ser modificada.
    /// Se aplica el patrón Singleton dentro de esta clase para permitir solo la creación y utilización de una instancia de Buscador.
    /// </summary>
    public class Buscador
    {
        /// <summary>
        /// Crea una instancia de Buscador.
        /// </summary>
        private Buscador() { }

        /// <summary>
        /// Realiza una búsqueda de ofertas dentro de una instancia de Sistema, utilizando la información de un Emprendedor dado,
        /// junto a una lista de etiquetas y categorías, para filtrar dentro de las ofertas.
        /// </summary>
        /// <param name="sistema">Instancia de Sistema.</param>
        /// <param name="emprendedor">Emprendedor cuyas propiedades se utilizan para filtrar.</param>
        /// <param name="etiquetas">Lista de etiquetas a utilizar para filtrar.</param>
        /// <param name="categorias">Lista de categorías a utilizar para filtrar.</param>
        /// <returns>Lista de ofertas ordenadas según la relevancia calculada para la búsqueda.</returns>
        public List<Oferta> BuscarOfertas(Sistema sistema, Emprendedor emprendedor, List<string> etiquetas = null,
            List<string> categorias = null)
        {
            Dictionary<Oferta, int> ofertasEncontradas = new Dictionary<Oferta, int>();
            etiquetas = etiquetas == null ? new List<string>() : etiquetas;
            categorias = categorias == null ? new List<string>() : categorias;

            foreach (Empresa empresa in sistema.Empresas)
            {
                foreach (Oferta oferta in empresa.Ofertas)
                {
                    if (oferta.Estado != Oferta.Estados.Habilitada || emprendedor.OfertasPostuladas.Contains(oferta.Id)
                        || emprendedor.OfertasConsumidas.Contains(oferta.Id)) continue;

                    double distanciaMedia = 0;
                    int puntaje = 0;
                    foreach (string etiqueta in etiquetas)
                    {
                        if (oferta.Etiquetas.Any(s => s.Contains(etiqueta, StringComparison.InvariantCultureIgnoreCase)))
                            puntaje++;
                    }
                    foreach (Producto producto in oferta.Productos)
                    {
                        foreach (string categoria in categorias)
                        {
                            if (producto.Material.Categorias.Any(s => s.Contains(categoria, StringComparison.InvariantCultureIgnoreCase)))
                                puntaje += 5;
                        }
                        distanciaMedia += emprendedor.Ubicacion.ObtenerDistancia(producto.Ubicacion);
                    }
                    distanciaMedia /= oferta.Productos.Count;
                    puntaje += this.DistanciaAPuntaje(distanciaMedia, 300, 30);

                    ofertasEncontradas.Add(oferta, puntaje);
                }
            }
            return this.OrdenarOfertasPorPuntaje(ofertasEncontradas);
        }

        /// <summary>
        /// Ordena un diccionario de par <c>Oferta, int</c> de mayor a menor según el valor asignado a cada clave.
        /// </summary>
        /// <param name="ofertas">Dictionary de par <c>Oferta, int</c> a ordenar.</param>
        /// <returns><c>List de Ofertas</c> ordenadas de mayor a menor.</returns>
        private List<Oferta> OrdenarOfertasPorPuntaje(Dictionary<Oferta, int> ofertas)
        {
            IEnumerable<Oferta> ofertasOrdenadas = from pair in ofertas
                                                   orderby pair.Value descending
                                                   select pair.Key;

            return ofertasOrdenadas.ToList<Oferta>();
        }

        /// <summary>
        /// Calcula el puntaje asignado a una oferta según una distancia dada, con la fórmula:
        /// <c>puntaje = 30 - Math.Ceiling(distancia / (distanciaMáxima / puntajeMáximo))</c>
        /// </summary>
        /// <param name="distancia">Distancia hacia la oferta.</param>
        /// <param name="distanciaMax">Distancia máxima para asignar un puntaje. Si la distancia supera (o es igual) a la distancia máxima, el puntaje será 0.</param>
        /// <param name="puntajeMax">Puntaje máximo asignable.</param>
        /// <returns><c>Puntaje en int</c> para la distancia según las condiciones definidas.</returns>
        private int DistanciaAPuntaje(double distancia, int distanciaMax, int puntajeMax)
        {
            if (distancia >= distanciaMax)
            {
                return 0;
            }
            return puntajeMax - (int)Math.Ceiling((distancia / (distanciaMax / puntajeMax)));
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
