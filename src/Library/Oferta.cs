using System;
using System.Text;
using System.Collections.Generic;

namespace ClassLibrary
{
        /// <summary>
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
    
    public class Oferta 
    {   
        /// <summary>
        ///  La clase Oferta se encarga de conocer todo lo relativo a la Oferta.
        /// </summary>
        /// <param name="id"> Un número identificador para referenciar la oferta a lo largo del sistema </param>
        /// <param name="empresa"> Empresa que publica la oferta </param>
        /// <param name="fechaCierre"> Fecha de cierre, límite para postularse a la oferta.</param>
        /// <param name="etiquetas">Etiquetas relacionadas a la oferta que sirven para agruparlas o clasificarlas</param>
        /// <param name="habilitaciones">Habilitaciones requeridas por la empresa para postularse a atender la oferta</param>
        /// <param name="descripcion">Descripcion realizada por la empresa</param>
        /// <param name="titulo">Titulo bajo el cual se publica la oferta</param>
        /// <param name="disponibleConstantemente">Para definir si una oferta es recurrente.</param>
        public Oferta (string id, Empresa empresa, DateTime fechaCierre, List<string> etiquetas, List<Habilitacion> habilitaciones, string descripcion, string titulo, bool disponibleConstantemente)
        {
            this.Id = id;                           //01
            this.Empresa = empresa;                 //02
            this.FechaCreada = DateTime.Now;        //03
            this.FechaCierre = fechaCierre;         //04
            this.Etiquetas = etiquetas;             //05
            this.Estado = Estados.Habilitada;       //06
            this.Habilitaciones = habilitaciones;   //07
            this.Descripcion = descripcion;         //08
            this.Titulo = titulo;                   //09
            this.DisponibleConstantemente = disponibleConstantemente;  //10
            this.EmprendedoresPostulados = new List<Emprendedor>();    //11
        }      

        /// <summary>
        /// Lista de estados posibles en que se puede encontrar una Oferta.
        /// </summary>
        public enum Estados{
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
            /// <value></value>
            Suspendida
        }
        /// <summary>
        /// Es una string que identifica a la oferta y que permite referenciarla a lo largo del sitema.
        /// </summary>
        /// <value>Id es el identificador único de la oferta.</value>
        public string Id {get; }
        /// <summary>
        /// Empresa es quién publica la oferta.
        /// </summary>
        /// <value>Empresa es quién publica la oferta</value>
        public Empresa Empresa {get; }
        /// <summary>
        /// Fecha en que se publica la oferta.
        /// </summary>
        /// <value>Fecha en que se publica la oferta.</value>
        public DateTime FechaCreada {get; }
        /// <summary>
        /// Fecha límite para postularse a la oferta.
        /// </summary>
        /// <value>FechaCierre es la fecha límite para postularse a la oferta.</value>
        public DateTime FechaCierre {get; }
        /// <summary>
        /// Son etiquetas que permiten categorizar la oferta para mostrarla agrupadas junto a otras que compartan la misma etiqueta.
        /// </summary>
        /// <value>Etiquetas permite categorizar la oferta.</value>
        public List<string> Etiquetas {get; }
        /// <summary>
        /// Estado indica cuál es la situación actual de una Oferta.
        /// </summary>
        /// <value>Estado indica si una oferta esta habilitada, cerrada o suspendida.</value>
        public Estados Estado {get; }
        /// <summary>
        /// Indica cuáles son las habilitaciones que exige la empresa para postularse a la oferta.
        /// </summary>
        /// <value>Habilitaciones exigidas por al empresa.</value>
        public List<Habilitacion> Habilitaciones {get; }
        /// <summary>
        /// Valor en dólares USA que la empresa ofresa en pago por la realización de la tarea que implica la oferta.
        /// </summary>
        /// <value> ValorUSD es el valor en dólares USA definido por la empresa que publica la oferta.</value>
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
        public string Descripcion {get; }
        /// <summary>
        /// Titulo bajo el que se publica la oferta.
        /// </summary>
        /// <value>Titulo bajo el que se publica la oferta.</value>
        public string Titulo {get; }
        /// <summary>
        /// Productos es la lista de productos que componen la oferta.
        /// </summary>
        /// <value>Productos es la lista de productos que componen la oferta.</value>
        public List<Producto> Productos {get; }
        /// <summary>
        /// Una propiedad que indica si la oferta es recurrente.
        /// </summary>
        /// <value>Una propiedad que indica si la oferta es recurrente.</value>
        public bool DisponibleConstantemente { get; set; }
        /// <summary>
        /// EmprendedoresPostulados es la lista de los emprendedores que se han postulado para la oferta.
        /// </summary>
        /// <value>EmprendedoresPostulados es la lista de los emprendedores que se han postulado para la oferta.</value>
        public List<Emprendedor> EmprendedoresPostulados { get; set; }
      
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
        public void AgregarProducto (Material material, string ciudad, string direccion, double cantidadEnUnidades, double valorUYU, double valorUSD)
        {
            Producto producto = new Producto ( material,  ciudad, direccion,  cantidadEnUnidades,  valorUYU,  valorUSD);
            this.Productos.Add(producto);
        }
        
        /// <summary>
        /// Quita un producto de la lista.
        /// </summary>
        /// <param name="producto">Detalle del producto a quitar de la lista.</param>
        public void RemoverProducto (Producto producto)
        {
                this.Productos.Remove(producto);      
        }

        /// <summary>
        /// Métodos para publicar los mensajes relativos a las ofertas.
        /// </summary>
        public string Redactar()
        {
            StringBuilder redaccion = new StringBuilder();

            redaccion.Append($"La oferta {Titulo} consiste en {Descripcion}. Publicada el {FechaCreada} por la empresa .");

            redaccion.Append($"Para postularse a esta oferta deberá cumplir con la habilitación: {Habilitaciones}.");
            redaccion.Append($"Tiene tiempo para postularse a esta oferta hasta el día: {this.FechaCierre} inclusive.");
            return redaccion.ToString();
        }

        /// <summary>
        /// Métodos para publicar los mensajes relativos a las ofertas en versión resumida.
        /// </summary>
        public string RedactarResumen()
        {
            StringBuilder redaccionCorta = new StringBuilder();
            
            redaccionCorta.Append($"Oferta {Titulo}: {Descripcion}. Empresa {FechaCreada} Fecha inicio {Empresa.Nombre}, Fecha cierre: {FechaCierre};.");

            redaccionCorta.Append("Habilitaciones: " + this.Habilitaciones);
            return redaccionCorta.ToString();
        }

        /// <summary>
        /// Un método para listar todos los emprendedores postulados a una oferta.
        /// </summary>
        public string RedactarPostulados()
        {
            StringBuilder redaccionPostulados = new StringBuilder();
            redaccionPostulados.Append("Emprendedores postulados:" );
            {
                throw new Exception("A la espera de la definición de la persistencia"); 
            }
            return redaccionPostulados.ToString();
        }
    }
}