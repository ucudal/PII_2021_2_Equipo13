using System;
using System.Text;
using System.Collections.Generic;

namespace ProyectoFinal.GestionOferta
{
    public class Oferta 
    {   
        /// <summary>
        ///  La clase Oferta se encarga de conocer todo lo relativo a la Oferta.
        ///  Patrones y principios aplicados:
        ///  Cumple con EXPERT ya que se le asignaron sus responsabilidades en su calidad
        ///  de experto en la información, por ser la clase que tiene la información necesaria
        ///  para poder cumplir con la tarea, mientras que se mantiene la encapsulación 
        ///  ya que utiliza su propia información para cumplir con las responsabilidades.
        ///  Se cumple con el patrón Low Coupling ya que al asignársele responsabilidades a 
        ///  la clase se buscó que el acoplamiento se mantuviera bajo al no depender de muchas 
        ///  otras clases.
        ///  Cumple con el patrón High Cohesión ya que las responsabilidades de la clase están
        ///  fuertemente relacionadas, creando así una clase robusta y fácil de entender. 
        /// </summary>
        /// <param name="id"> id </param>
        /// <param name="fechaCierre"> fechaCierre </param>
        /// <param name="etiquetas">etiquetas</param>
        /// <param name="habilitaciones">habilitaciones</param>
        /// <param name="valorUYU">valorUYU</param>
        /// <param name="valorUSD">valorUSD</param>
        /// <param name="descripcion">descripcion</param>
        /// <param name="palabraClave">palabraClave</param>
        /// <param name="titulo">titulo</param>
        public Oferta (string id, DateTime fechaCierre, List<string> etiquetas, string habilitaciones, double valorUYU, double valorUSD, string descripcion, string titulo)
        {
            this.Id = id;                           //01
            this.FechaCreada = DateTime.Now;        //02
            this.FechaCierre = fechaCierre;         //03
            this.Etiquetas = etiquetas;             //04
            this.Estado = Estados.Habilitada;       //05
            this.Habilitaciones = habilitaciones;   //06
            this.ValorUSD = valorUSD;               //07
            this.ValorUYU = valorUYU;               //08
            this.Descripcion = descripcion;         //09
            this.Titulo = titulo;                   //10
        }
        
        public enum Estados{
            Habilitada,
            Cerrada,
            Suspendida
        }

        //1. Poner acá como propiedades, las conexiones con las clases en las que se relaciona, ver el diagrama UML
        //2. Cambiar fecha creada, para que tome automaticamente la fecha de hoy con daynow
        //3. Cambiar la propiedad Estado para que arranque por defecto en "habilitada"
        //4. Ver la posibilidad de que al ingresar el valor en pesos/dolares se haga la comberción automáticamente al otro valor.

        public string Id {get; }
        public DateTime FechaCreada {get; }
        public DateTime FechaCierre {get; }
        public List<string> Etiquetas {get; }
        public Estados Estado {get; }
        public string Habilitaciones {get; }
        public double ValorUSD {get; }
        public double ValorUYU {get; }
        public string Descripcion {get; }
        public string Titulo {get; }

        /// <summary>
        /// Método para comunicarse con la clase Producto.
        /// </summary>
        /// <param name="producto">producto</param>
        public ObtenerProducto (Producto producto)
        {
            throw new Exception("A la espera de la definición de la persistencia");
        }
        /// <summary>
        /// Método para comunicarse con la clase Empresa.
        /// </summary>
        /// <param name="empresa">empresa</param>
        public ObtenerEmpresa (Empresa empresa)
        {
            throw new Exception("A la espera de la definición de la persistencia");
        }
        public ObtenerHabilitacion (Habilitacion habilitacion)
        {
            throw new Exception("A la espera de la definición de la persistencia");
        }
         /// <summary>
        /// Método para comunicarse con la clase Emprendedor.
        /// </summary>
        /// <param name="emprendedor">emprendedor</param>
        /// <returns></returns>
        public ObtenerEmprendedor (Emprendedor emprendedor)
        {
           throw new Exception("A la espera de la definición de la persistencia");
        }

        /// <summary>
        /// Métodos para publicar los mensajes relativos a las ofertas.
        /// </summary>
        public string Redactar()
        {
            StringBuilder redaccion = new StringBuilder();

            redaccion.Append($"La oferta {Titulo} consiste en {Descripcion}. Publicada el {FechaCreada} por la empresa {Empresa} con validez hasta el {FechaCierre}.");

            redaccion.Append($"Para postularse a esta oferta deberá cumplir con la habilitación: {Habilitaciones}.");
            return redaccion.ToString();
        }

        /// <summary>
        /// Métodos para publicar los mensajes relativos a las ofertas en versión resumida.
        /// </summary>
        public string RedactarResumen()
        {
            StringBuilder redaccionCorta = new StringBuilder();
            
            redaccionCorta.Append($"Oferta {Titulo}: {Descripcion}. Empresa {FechaCreada} Fecha inicio {Empresa}, Fecha cierre: {FechaCierre};.");

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
        //definir info a almacenar.
    }
}
