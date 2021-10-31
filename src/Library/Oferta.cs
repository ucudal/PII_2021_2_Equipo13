using System;
using System.Text;

namespace ClassLibrary
{

    public class Oferta
    {
        public Oferta(string id, DateTime fechaCierre, string[] etiquetas, string habilitaciones, double valorUYU, double valorUSD, string descripcion, string[] palabraClave, string titulo)
        {

            this.Id = id;                           //01
            DateTime FechaCreada = DateTime.Now;    //02
            this.FechaCierre = fechaCierre;         //03
            this.Etiquetas = etiquetas;             //04
            this.Estado = "Habilitada";             //05
            this.Habilitaciones = habilitaciones;   //06
            this.ValorUSD = valorUSD;               //07
            this.ValorUYU = valorUYU;               //08
            this.Descripcion = descripcion;         //09
            this.Titulo = titulo;                   //10
        }

        //1. Poner acá como propiedades, las conexiones con las clases en las que se relaciona, ver el diagrama UML
        //2. Cambiar fecha creada, para que tome automaticamente la fecha de hoy con daynow
        //3. Cambiar la propiedad Estado para que arranque por defecto en "habilitada"
        //4. Ver la posibilidad de que al ingresar el valor en pesos/dolares se haga la comberción automáticamente al otro valor.

        public string estado { get; set; }

        public string Id { get; }
        public DateTime FechaCreada { get; }

        public DateTime FechaCierre { get; }
        public string[] Etiquetas { get; }
        public Enum Estado { get; }
        public string Habilitaciones { get; }
        public double ValorUSD { get; }
        public double ValorUYU { get; }
        public string Descripcion { get; }
        public string Titulo { get; }

        public obtenerProducto(Producto producto)
        {

        }
        public obtenerEmpresa(Empresa empresa)
        {

        }
        public obtenerHabilitacion(Habilitacion habilitacion)
        {

        }
        public obtenerEmprendedor(Emprendedor emprendedor)
        {

        }

        public void redactar()
        {
            StringBuilder redaccion = new StringBuilder();

            redaccion.Append($"La oferta {Titulo} consiste en {Descripcion}. Publicada el {FechaCreada} por la empresa {Empresa} con validez hasta el {FechaCierre}.");

            redaccion.Append($"Para postularse a esta oferta deberá cumplir con la habilitación: {Habilitaciones}.");
        }
        public void redactarResumen()
        {
            StringBuilder redaccionCorta = new StringBuilder();

            redaccionCorta.Append($"Oferta {Titulo}: {Descripcion}. Empresa {FechaCreada} Fecha inicio {Empresa}, Fecha cierre: {FechaCierre};.");

            redaccionCorta.Append("Habilitaciones: " + this.Habilitaciones);
        }
        public void redactarPostulados()
        {
            StringBuilder redaccionPostulados = new StringBuilder();
            redaccionPostulados.Append("Emprendedores postulados:");

            foreach (emprendedor item in Emprendedores)
            {
                redaccionEmprendedores.Parse.Append(item + " ");
            }
            redaccionEmprendedores.Append(".");
        }
        //definir info a almacenar.
    }
}
