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
        /// <param name="valorUYU">Valor en pesos uruguayos definido por la empresa que publica la oferta como retribución para la tarea.</param>
        /// <param name="valorUSD">Valor en dólares USA definido  por la empresa que publica la oferta como retribución para la tarea.</param>
        /// <param name="descripcion">Descripcion realizada por la empresa</param>
        /// <param name="titulo">Titulo bajo el cual se publica la oferta</param>
        /// <param name="disponibleConstantemente">Para definir si una oferta es recurrente.</param>
        public Oferta (string id, Empresa empresa, DateTime fechaCierre, List<string> etiquetas, string habilitaciones, double valorUYU, double valorUSD, string descripcion, string titulo, bool disponibleConstantemente)
        {
            this.Id = id;                           //01
            this.Empresa = empresa;                 //02
            this.FechaCreada = DateTime.Now;        //03
            this.FechaCierre = fechaCierre;         //04
            this.Etiquetas = etiquetas;             //05
            this.Estado = Estados.Habilitada;       //06
            this.Habilitaciones = habilitaciones;   //07
            this.ValorUYU = valorUYU;               //08
            this.ValorUSD = valorUSD;               //09
            this.Descripcion = descripcion;         //10
            this.Titulo = titulo;                   //11  
            this.DisponibleConstantemente = disponibleConstantemente;  //12
        }      

        /// <summary>
        /// Lista de estados posibles en que se puede encontrar una Oferta.
        /// </summary>
        public enum Estados{
            Habilitada,
            Cerrada,
            Suspendida
        }
        public string Id {get; }
        public Empresa Empresa {get; }
        public DateTime FechaCreada {get; }
        public DateTime FechaCierre {get; }
        public List<string> Etiquetas {get; }
        public Estados Estado {get; }
        public string Habilitaciones {get; }
        public double ValorUSD {get; }
        public double ValorUYU {get; }
        public string Descripcion {get; }
        public string Titulo {get; }
        public List<Producto> Productos {get; }
        public bool DisponibleConstantemente { get; set; }
      
        //aplicando Creator
        /// <summary>
        /// Agrega un producto a la lista de productos.
        /// </summary>
        /// <param name="material">Material que conforma al producto.</param>
        /// <param name="ubicacion">Ubicación geográfica del producto.</param>
        /// <param name="cantidadEnUnidades">Cantidad de unidades/param>
        /// <param name="valorUYU">Valor en pesos uruguayos.</param>
        /// <param name="valorUSD">Valor en dolares usa.</param>
        public void AgregarProducto (Material material, Ubicacion ubicacion, double cantidadEnUnidades, double valorUYU, double valorUSD)
        {
            Producto producto = new Producto ( material,  ubicacion,  cantidadEnUnidades,  valorUYU,  valorUSD);
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

            redaccion.Append($"La oferta {Titulo} consiste en {Descripcion}. Publicada el {FechaCreada} por la empresa {Empresa} con validez hasta el {FechaCierre}.");

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
