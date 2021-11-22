using System.Collections.Generic;
using System;


namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Clase que cumple SRP y Expert ya que se encarga de determinar la información necesaria para diferenciar una sesión y los datos de la misma
    /// </summary>
    public class Sesion
    {
        public string SesionId { get; }

        public string UsuarioId { get; }

        public DateTime fechaCreacion { get; }

        public DateTime fechaDeExpiracion { get; }

        public Sesion(string sesionId, string usuarioId)
        {
            this.SesionId = SesionId;
            this.UsuarioId = UsuarioId;
            this.fechaCreacion = DateTime.Now;
            this.fechaDeExpiracion = DateTime.Now.AddMinutes(30);
        }
    }
}