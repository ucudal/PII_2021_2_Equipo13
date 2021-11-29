using System.Collections.Generic;
using System;
using System.Text;
using PII_E13.ClassLibrary;

namespace PII_E13.HandlerLibrary
{
    /// <summary>
    /// Handler encargado de procesar el registro de una empresa.
    /// </summary>
    public class MenuHandler : HandlerBase
    {
        private const int COLUMNAS_EMPRESA = 1;
        private const int FILAS_EMPRESA = 4;

        private bool banderaEmpresario;
        private bool banderaEmprendedor;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RegistrarEmpresaHandler"/>. 
        /// </summary>
        public MenuHandler(HandlerBase siguiente, string intencion) : base(siguiente, intencion)
        {
        }

        /// <summary>
        /// La clase procesa el mensaje y retorna true o no lo procesa y retorna false.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <param name="respuesta">La respuesta al mensaje procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario</returns>
        protected override bool ResolverInterno(Sesion sesion, IMensaje mensaje, out IRespuesta respuesta)
        {
            respuesta = new Respuesta(mensaje);
            if (!this.PuedeResolver(sesion))
            {
                return false;
            }
            this.Cancelar(sesion);

            try
            {
                Sistema.Instancia.ObtenerEmpresaPorId(mensaje.IdUsuario); // Intenta buscar si hay empresarios con el id usuario
                this.banderaEmpresario = true; //Encontro empresario
            }
            catch
            {
                try
                { // Si no lo encuentra intenta buscar emprendedor
                    this.banderaEmpresario = false; //No encontro empresario
                    Sistema.Instancia.ObtenerEmprendedorPorId(mensaje.IdUsuario);
                    this.banderaEmprendedor = true; //Encontro emprendedor
                }
                catch
                { // Si no encuentra ni emprendedor ni empresario
                    this.banderaEmprendedor = false; //Encontro emprendedor
                }
            }

            List<IBoton> botonesDeEmpresario = new List<IBoton>()
            {
                new Boton("Publicar una Oferta", "Quiero publicar una oferta"),
                new Boton("Ver mis Ofertas", "Quiero ver mis ofertas publicadas")
            };
            List<IBoton> botonesDeEmprendedor = new List<IBoton>()
            {
                new Boton("Buscar Ofertas", "Quiero buscar una oferta"),
                new Boton("Ver mis Ofertas", "Quiero ver las ofertas a las que estoy postulado")
            };
            List<IBoton> botonesComun = new List<IBoton>()
            {
                new Boton("Registrarse como Empresa", "Empresa"),
                new Boton("Registrarse como Emprendedor", "Emprendedor")
            };

            StringBuilder st = new StringBuilder();

            if (this.banderaEmprendedor & !this.banderaEmpresario)
            {
                st.Append("*¡Bienvenido!*\n\n¿Qué deseas hacer?\n_Selecciona una opción_");
                respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeEmprendedor, 0, FILAS_EMPRESA, COLUMNAS_EMPRESA);
            }
            else if (!this.banderaEmprendedor & this.banderaEmpresario)
            {
                st.Append("*¡Bienvenido!*\n\n¿Qué deseas hacer?\n_Selecciona una opción_");
                respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeEmpresario, 0, FILAS_EMPRESA, COLUMNAS_EMPRESA);
            }
            else if (!this.banderaEmprendedor & !this.banderaEmpresario)
            {
                st.Append("¡Bien! Primero indícanos: ¿Con qué tipo de usuario quieres registrarte?");
                respuesta.Botones = this.ObtenerMatrizDeBotones(botonesComun, 0, FILAS_EMPRESA, COLUMNAS_EMPRESA);
            }
            respuesta.Texto = st.ToString();
            return true;
        }

        /// <summary>
        /// Retorna este "handler" al estado inicial.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        protected override void CancelarInterno(Sesion sesion)
        {
        }

        /// <summary>
        /// Determina si este "handler" puede procesar el mensaje.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        /// <returns>true si el mensaje puede ser pocesado; false en caso contrario.</returns>
        protected override bool PuedeResolver(Sesion sesion)
        {
            if (this.Intencion.Equals(String.Empty))
            {
                return true;
            }
            return sesion.PLN.UltimaIntencion.Nombre.Equals(this.Intencion) && sesion.PLN.UltimaIntencion.ConfianzaDeteccion >= 95;
        }

        /// <summary>
        /// Retorna este "handler" al estado inicial.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        public override void Cancelar(Sesion sesion)
        {
            this.CancelarInterno(sesion);
            if (this.Siguiente != null)
            {
                this.Siguiente.Cancelar(sesion);
            }
        }
    }
}