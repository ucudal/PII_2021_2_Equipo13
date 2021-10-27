using System;
using System.Text;

namespace ProyectoFinal.GestionOferta
{
    public class Oferta
    {   
        public Oferta (string id, DateTime fechaCreada, DateTime fechaCierre, string[] etiquetas, Enum estado, string habilitaciones, double valorUYU, double valorUSD, string descripcion, string[] palabraClave, string titulo)
        {
            this.Id = id;                           //01
            this.FechaCreada = fechaCreada;         //02
            this.FechaCierre = fechaCierre;         //03
            this.Etiquetas = etiquetas;             //04
            this.Estado = estado;                   //05
            this.Habilitaciones = habilitaciones;   //06
            this.ValorUSD = valorUSD;               //07
            this.ValorUYU = valorUYU;               //08
            this.Descripcion = descripcion;         //09
            this.Titulo = titulo;                   //10
        }

        //1. poner acá como propiedades, las conexiones con las clases en las que se relaciona, ver el diagrama UML
        //2. cambiar fecha creada, para que tome automaticamente la fecha de hoy con daynow
        //3.cambiar la propiedad Estado para que arranque por defecto en "habilitada"
        //4.ver la posibilidad de que al ingresar el valor en pesos/dolares se haga la comberción automáticamente al otro valor.
        public string Id {get; }
        public DateTime FechaCreada {get; }
        public DateTime FechaCierre {get; }
        public string[] Etiquetas {get; }
        public Enum Estado {get; }
        public string Habilitaciones {get; }
        public double ValorUSD {get; }
        public double ValorUYU {get; }
        public string Descripcion {get; }
        public string Titulo {get; }

        public void redactar()
        {
            StringBuilder redaccion = new StringBuilder();

            redaccion.Append($"La oferta {0} consiste en {1}. Publicada el {2} por la empresa {3} con validez hasta el {4}.", this.Titulo, this.Descripcion, this.FechaCreada, this.Empresa, this.FechaCierre);

            redaccion.Append($"Para postularse a esta oferta deberá cumplir con la habilitación: {0}.", this.Habilitaciones);
        }
        public void redactarResumen()
        {
            StringBuilder redaccionCorta = new StringBuilder();
            
            redaccionCorta.Append($"Oferta {0}: {1}. Empresa {3} Fecha inicio {2}, Fecha cierre: {4};.", this.Titulo, this.Descripcion, this.FechaCreada, this.Empresa, this.FechaCierre);

            redaccionCorta.Append("Habilitaciones: " + this.Habilitaciones);
        }
        public void redactarPostulados()
        {
            StringBuilder redaccionPostulados = new StringBuilder();
            redaccionPostulados.Append("Emprendedores postulados:" +  );

            foreach (emprendedor item in Emprendedores)
            {
                redaccionEmprendedores.Parse.Append(item + " ");
            }
            redaccionEmprendedores.Append(".");
        }
        //definir info a almacenar.
}
