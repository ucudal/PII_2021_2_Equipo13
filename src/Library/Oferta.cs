using System.Text;

namespace ProyectoFinal.GestionOferta
{
    public class Oferta
    {
             
        public  Oferta (string id, string titulo, string descripcion, Producto Producto, Empresa Empresa, float valorPesos, float valorDolares, string[] habilitaciones, string estado, string[] palabraClave, string fechaCreacion, string fechaCierre)
        {
            this.Id = id;
            this.Titulo = titulo;
            this.Descripcion = descripcion;
            this.Producto = Producto;
            this.Empresa = Empresa;
            this.ValorPesos = valorPesos;
            this.ValorDolares = valorDolares;
            this.Habilitaciones = habilitaciones;
            this.Estado = estado;
            this.PalabraClave = palabraClave;
            this.FechaCreacion = fechaCreacion;
            this.FechaCierre = fechaCierre;
        }

        public string Id {get; }
        public string Titulo {get; }
        public string Descripcion {get; }
        public producto Producto {get; }
        public empresa Empresa {get; }
        public float ValorPesos {get; }
        public float ValorDolares {get; }
        public string[] Habilitaciones {get; }
        public string Estado {get; }
        public string[] PalabraClave {get; }
        public string FechaCreacion {get; }
        public string FechaCierre {get; }
        public emprendedores[] Emprendedores {get; }

        public void redactar()
        {
            StringBuilder redaccion = new StringBuilder($"La oferta {0} consiste en {1}. Publicada el {2} por la empresa {3} con validez hasta el {4}.", this.Titulo, this.Descripcion, this.FechaCreacion, this.Empresa, this.FechaCierre);

            redaccion.Append("Para postularse a esta oferta deberá cumplir con las suigientes habilitaciones: ");

            foreach (string item in Habilitaciones)
            {
                redaccion.Append(item + " ");
            }
            redaccion.Append(".");
        }

        public void redactarResumen()
        {
            StringBuilder redaccionCorta = new StringBuilder($"Oferta {0}: {1}. Empresa {3} Fecha inicio {2}, Fecha cierre: {4};.", this.Titulo, this.Descripcion, this.FechaCreacion, this.Empresa, this.FechaCierre);

            redaccionCorta.Append("Habilitaciones: ");

            foreach (string item in Habilitaciones)
            {
               StringBuilder redaccion = StringBuilder redaccion.Append(item + " ");
            }
            StringBuilder redaccion = StringBuilder redaccion.Append(".");
        }

        public void redactarEmprendedores()
        {
            StringBuilder redaccionEmprendedores = new StringBuilder($"Emprendedores postulados: );

            foreach (emprendedor item in Emprendedores)
            {
                redaccionEmprendedores.Parse.Append(item + " ");
            }
            redaccionEmprendedores.Append(".");
        }

        //definir info a almacenar.

}
