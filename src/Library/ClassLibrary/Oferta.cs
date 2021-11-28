using System;
using System.Text;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// La clase Oferta se encarga de conocer todo lo relativo a la Oferta.
    ///  Patrones y principios aplicados:
    ///  Principio EXPERT: ya que se le asignaron sus responsabilidades en su calidad
    ///  de experto en la información, por ser la clase que tiene la información necesaria
    ///  para poder cumplir con la tarea, mientras que se mantiene la encapsulación 
    ///  ya que utiliza su propia información para cumplir con las responsabilidades.
    ///  Patrón Low Coupling: ya que al asignársele responsabilidades a 
    ///  la clase se buscó que el acoplamiento se mantuviera bajo al no depender de muchas 
    ///  otras clases.
    ///  Patrón High Cohesión: ya que las responsabilidades de la clase están
    ///  fuertemente relacionadas, creando así una clase robusta y fácil de entender. 
    /// </summary>
    public class Oferta : IIdentificable
    {

        /// <summary>
        /// Crea una instancia de <see cref="Oferta"/> a partir de la deserialización de un objeto en formato JSON.
        /// </summary>
        [JsonConstructor]
        public Oferta(string id, string empresa, DateTime fechaCreada, DateTime fechaCierre, Estados estado, string descripcion, string titulo, bool recurrente,
            List<Habilitacion> habilitaciones = null, List<string> etiquetas = null, List<string> emprendedoresPostulados = null, List<Producto> productos = null)
        {
            this.Id = id;
            this.Empresa = empresa;
            this.FechaCreada = fechaCreada;
            this.FechaCierre = fechaCierre;
            this.Etiquetas = etiquetas != null ? etiquetas : new List<string>();
            this.Estado = estado;
            //this.Estado = Enum.Parse<Estados>(estado);
            this.Habilitaciones = habilitaciones != null ? habilitaciones : new List<Habilitacion>();
            this.Descripcion = descripcion;
            this.Titulo = titulo;
            this.Recurrente = recurrente;
            this.EmprendedoresPostulados = emprendedoresPostulados != null ? emprendedoresPostulados : new List<string>();
            this.Productos = productos != null ? productos : new List<Producto>();
        }

        /// <summary>
        /// Crea una instancia de la clase <see cref="Oferta"/>.
        /// </summary>
        /// <param name="id"> Un número identificador para referenciar la oferta a lo largo del sistema </param>
        /// <param name="empresa"> Empresa que publica la oferta </param>
        /// <param name="fechaCierre"> Fecha de cierre, límite para postularse a la oferta.</param>
        /// <param name="etiquetas">Etiquetas relacionadas a la oferta que sirven para agruparlas o clasificarlas</param>
        /// <param name="habilitaciones">Habilitaciones requeridas por la empresa para postularse a atender la oferta</param>
        /// <param name="descripcion">Descripcion realizada por la empresa</param>
        /// <param name="titulo">Titulo bajo el cual se publica la oferta</param>
        /// <param name="recurrente">Para definir si una oferta es recurrente.</param>
        public Oferta(string id, Empresa empresa, DateTime fechaCierre, List<string> etiquetas, List<Habilitacion> habilitaciones, string descripcion, string titulo, bool recurrente)
        {
            this.Id = id;
            this.Empresa = empresa.Id;
            this.FechaCreada = DateTime.Now;
            this.FechaCierre = fechaCierre;
            this.Etiquetas = etiquetas;
            this.Habilitaciones = habilitaciones;
            this.Descripcion = descripcion;
            this.Titulo = titulo;
            this.Recurrente = recurrente;
        }

        /// <summary>
        /// Lista de estados posibles en que se puede encontrar una Oferta.
        /// </summary>
        public enum Estados
        {
            /// <summary>
            /// Se le asigna a una oferta para indicar que esta vigente.
            /// </summary>
            /// <value>Habilitada indica que esta vigente</value>
            Habilitada,
            /// <summary>
            /// Se le asigna a una oferta para indicar que ya está cerrada y no es posible posularse.
            /// </summary>
            /// <value>Cerrada indica que no esta activa una oferta por haber sido adjudicada o por haber llegado a su fecha límite.</value>
            Cerrada,
            /// <summary>
            /// Se le asigna a una oferta para indicar que por algún motivo no esta disponible la oferta.
            /// </summary>
            Suspendida,
            /// <summary>
            /// Se le asigna a una oferta para indicar que fue entregada a un Emprendedor.
            /// </summary>
            Entregada
        }
        /// <summary>
        /// Es una string que identifica a la oferta y que permite referenciarla a lo largo del sitema.
        /// </summary>
        /// <value>Id es el identificador único de la oferta.</value>
        public string Id { get; }
        /// <summary>
        /// El identificador único de la empresa que publicó la oferta.
        /// </summary>
        /// <value>Cadena de caracteres conteniendo el identificador único de la empresa que publicó la oferta.</value>
        public string Empresa { get; }
        /// <summary>
        /// Fecha en que se publica la oferta.
        /// </summary>
        /// <value>Fecha en que se publica la oferta.</value>
        public DateTime FechaCreada { get; }
        /// <summary>
        /// Fecha límite para postularse a la oferta.
        /// </summary>
        /// <value>FechaCierre es la fecha límite para postularse a la oferta.</value>
        public DateTime FechaCierre { get; }
        /// <summary>
        /// Son etiquetas que permiten categorizar la oferta para mostrarla agrupadas junto a otras que compartan la misma etiqueta.
        /// </summary>
        /// <value>Etiquetas permite categorizar la oferta.</value>
        public List<string> Etiquetas { get; } = new List<string>();
        /// <summary>
        /// Estado indica cuál es la situación actual de una Oferta.
        /// </summary>
        /// <value>Estado indica si una oferta esta habilitada, cerrada o suspendida.</value>
        public Estados Estado { get; set; } = Estados.Habilitada;
        /// <summary>
        /// Indica cuáles son las habilitaciones que exige la empresa para postularse a la oferta.
        /// </summary>
        /// <value>Habilitaciones exigidas por al empresa.</value>
        [JsonInclude]
        public List<Habilitacion> Habilitaciones { get; } = new List<Habilitacion>();
        /// <summary>
        /// Valor en dólares USA que la empresa ofresa en pago por la realización de la tarea que implica la oferta.
        /// </summary>
        /// <value> ValorUSD es el valor en dólares USA definido por la empresa que publica la oferta.</value>
        [JsonIgnore]
        public double ValorUSD
        {
            get
            {
                double valorUSD = 0;
                foreach (Producto producto in this.Productos)
                {
                    valorUSD += producto.ValorUSD;
                }
                return valorUSD;
            }
        }
        /// <summary>
        /// ValorUY es el valor en pesos uruguayos definido por la empresa que publica la oferta.
        /// </summary>
        /// <value>ValorUY es el valor en pesos uruguayos definido por la empresa que publica la oferta.</value>
        [JsonIgnore]
        public double ValorUYU
        {
            get
            {
                double valorUYU = 0;
                foreach (Producto producto in this.Productos)
                {
                    valorUYU += producto.ValorUYU;
                }
                return valorUYU;
            }
        }
        /// <summary>
        /// Es la descripción que hace la empresa para describir la oferta al publicarla.
        /// </summary>
        /// <value>Descripcion que hace la empresa</value>
        public string Descripcion { get; }
        /// <summary>
        /// Titulo bajo el que se publica la oferta.
        /// </summary>
        /// <value>Titulo bajo el que se publica la oferta.</value>
        public string Titulo { get; }
        /// <summary>
        /// Productos es la lista de productos que componen la oferta.
        /// </summary>
        /// <value>Productos es la lista de productos que componen la oferta.</value>
        [JsonInclude]
        public List<Producto> Productos { get; } = new List<Producto>();
        /// <summary>
        /// Una propiedad que indica si la oferta es recurrente.
        /// </summary>
        /// <value>Una propiedad que indica si la oferta es recurrente.</value>
        public bool Recurrente { get; set; }
        /// <summary>
        /// Lista de los emprendedores que se han postulado para la oferta.
        /// </summary>
        /// <value>Lista conteniendo cadenas de caracteres referenciando los idenitificadores únicos de los emprendedores que se han postulado a la oferta.</value>
        [JsonInclude]
        public List<string> EmprendedoresPostulados { get; set; } = new List<string>();

        //aplicando Creator
        /// <summary>
        /// Agrega un producto a la lista de productos.
        /// </summary>
        /// <param name="material">Material que conforma al producto.</param>
        /// <param name="ciudad">Ciudad en la que se encuentra la oferta.</param> 
        /// <param name="direccion">Dirección dentro de la Ciudad en la que se encuentra la oferta.</param>      
        /// <param name="cantidadEnUnidades">Cantidad de unidades</param>
        /// <param name="valorUYU">Valor en pesos uruguayos.</param>
        /// <param name="valorUSD">Valor en dolares usa.</param>
        public void AgregarProducto(Material material, string ciudad, string direccion, double cantidadEnUnidades, double valorUYU, double valorUSD)
        {
            Producto producto = new Producto(material, ciudad, direccion, cantidadEnUnidades, valorUYU, valorUSD);
            this.Productos.Add(producto);
        }

        /// <summary>
        /// Quita un producto de la lista.
        /// </summary>
        /// <param name="producto">Detalle del producto a quitar de la lista.</param>
        public void RemoverProducto(Producto producto)
        {
            this.Productos.Remove(producto);
        }

        /// <summary>
        /// Métodos para publicar los mensajes relativos a las ofertas.
        /// </summary>
        public string Redactar()
        {
            StringBuilder redaccion = new StringBuilder();
            redaccion.Append($"*{this.Titulo}*\n_Por {Sistema.Instancia.ObtenerEmpresaPorId(this.Empresa).Nombre}_");
            if (this.Recurrente)
            {
                redaccion.Append($"\n_Esta oferta está disponible recurrentemente. Consulta la frecuencia con el ofertante._");
            }
            redaccion.Append($"\n\n*Descripción:* {this.Descripcion}");
            if (this.Habilitaciones.Count > 0)
            {
                redaccion.Append($"\n\nEl ofertante indicó que las siguientes habilitaciones son necesarias para ser considerado para la oferta:");
                foreach (Habilitacion habilitacion in this.Habilitaciones)
                {
                    redaccion.Append($"\n*->* _{habilitacion.Nombre}_");
                }
            }
            redaccion.Append($"\n\nFecha de publicación: _{this.FechaCreada.ToShortDateString()}_");
            redaccion.Append($"\nFecha de cierre: _{this.FechaCierre.ToShortDateString()}_");

            return redaccion.ToString();
        }

        /// <summary>
        /// Métodos para publicar los mensajes relativos a las ofertas en versión resumida.
        /// </summary>
        public string RedactarResumen()
        {
            StringBuilder redaccionCorta = new StringBuilder();

            redaccionCorta.Append($"*{this.Titulo}*");
            if (this.Recurrente)
            {
                redaccionCorta.Append(" _(Recurrente)_");
            }
            redaccionCorta.Append($"\n_{Sistema.Instancia.ObtenerEmpresaPorId(this.Empresa).Nombre}_\n");
            redaccionCorta.Append($"_Cierre: {this.FechaCierre.ToShortDateString()}_");

            return redaccionCorta.ToString();
        }

        /// <summary>
        /// Un método para listar todos los emprendedores postulados a una oferta.
        /// </summary>
        public string RedactarPostulados()
        {
            StringBuilder redaccionPostulados = new StringBuilder();
            redaccionPostulados.Append("Emprendedores postulados:");
            {
                //throw new Exception("A la espera de la definición de la persistencia"); 
                foreach (string emprendedor in this.EmprendedoresPostulados)
                {
                    redaccionPostulados.Append($"\n*->* _{Sistema.Instancia.ObtenerEmprendedorPorId(emprendedor).Nombre}_");
                }
            }
            return redaccionPostulados.ToString();
        }
    }
}