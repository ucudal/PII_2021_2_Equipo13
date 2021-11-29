using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PII_E13.ClassLibrary;

namespace PII_E13.HandlerLibrary
{
    /// <summary>
    /// Handler encargado de procesar el registro de una empresa.
    /// </summary>
    public class MenuHandler : HandlerBase
    {
        //private StringBuilder stringBuilder;
        private Dictionary<Sesion, StringBuilder> SbSesion = new Dictionary<Sesion, StringBuilder>();

        //Dictionary<string, string> DiccDatosEpresa = new Dictionary<string, string>();
        private Dictionary<Sesion, Dictionary<string, string>> Sesiones = new Dictionary<Sesion, Dictionary<string, string>>();

        //private string accionPrevia;
        private Dictionary<Sesion, string> accionPreviaSesion = new Dictionary<Sesion, string>();
        private const int COLUMNAS_EMPRESA = 1;
        private const int FILAS_EMPRESA = 4;

        private bool banderaEmpresario;
        private bool banderaEmprendedor;


        /// <summary>
        /// Diccionario utilizado para contener todas las búsquedas que se están realizando por los usuarios.
        /// Se identifica al usuario por su id en una plataforma y se guarda una instancia de <see cref="InformacionPostulacion"/>.
        /// </summary>
        /// <value>Diccionario de instancias de <see cref="InformacionPostulacion"/> identificadas por ids de usuarios en string</value>
        private Dictionary<string, InformacionPostulacion> Busquedas { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RegistrarEmpresaHandler"/>. 
        /// </summary>
        public MenuHandler(HandlerBase siguiente, string intencion) : base(siguiente, intencion)
        {
            this.Busquedas = new Dictionary<string, InformacionPostulacion>();
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

            if (!this.SbSesion.ContainsKey(sesion))
            {
                this.SbSesion.Add(sesion, new StringBuilder("Bienvenido a contionuacion te mostrarenmos un menú en base a tu rol!\n\n"));
            }
            StringBuilder stringBuilder = this.SbSesion[sesion];

            if (!this.accionPreviaSesion.ContainsKey(sesion))
            {
                this.accionPreviaSesion.Add(sesion, String.Empty);
            }
            string accionPrevia = this.accionPreviaSesion[sesion];

            if (!this.Sesiones.ContainsKey(sesion))
            {
                this.Sesiones.Add(sesion, new Dictionary<string, string>());
            }
            Dictionary<string, string> DiccDatosEpresa = this.Sesiones[sesion];

            InformacionPostulacion infoPostulacion = new InformacionPostulacion();
            if (this.Busquedas.ContainsKey(mensaje.IdUsuario))
            {
                infoPostulacion = this.Busquedas[mensaje.IdUsuario];
            }
            else
            {
                this.Busquedas.Add(mensaje.IdUsuario, infoPostulacion);

            }
            List<string> menuEmpresario = new List<string>(); //Menu de opciones para las acciones de un empresario


            List<string> menuEmprendedor = new List<string>();

            List<string> menuComun = new List<string>();





            try
            {
                Sistema.Instancia.ObtenerEmpresaPorId(mensaje.IdUsuario); // Intenta buscar si hay empresarios con el id usuario
                this.banderaEmpresario = true; //Encontro empresario
                menuEmpresario.Add("Ver Ofertas");
                menuEmpresario.Add("Crear Oferta");
            }
            catch
            {
                try // Si no lo encuentra intenta buscar emprendedor
                {
                    this.banderaEmpresario = false; //No encontro empresario
                    menuEmpresario.Add("Registro Empresario");
                    Sistema.Instancia.ObtenerEmprendedorPorId(mensaje.IdUsuario);
                    this.banderaEmprendedor = true; //Encontro emprendedor
                    menuEmprendedor.Add("Ver Ofertas");
                    menuEmprendedor.Add("Postularse a Oferta");
                }
                catch
                { // Si no encuentra ni emprendedor ni empresario
                    this.banderaEmprendedor = false; //Encontro emprendedor

                    menuEmprendedor.Add("Registro Emprendedor");
                    menuComun.Add("Registro Emprendedor");
                    menuComun.Add("Registro Empresario");

                }
            }











            if (infoPostulacion.DatosMenuEmpresario == null) //Lista de botones con las opciones del registro
            {
                infoPostulacion.DatosMenuEmpresario = new List<string>();

                foreach (string opcion in menuEmpresario)
                {
                    if (!infoPostulacion.DatosMenuEmpresario.Contains(opcion))
                    {
                        infoPostulacion.DatosMenuEmpresario.Add(opcion);
                    }
                }
            }

            if (infoPostulacion.DatosMenuEmprendedor == null) //Lista de botones con las opciones del registro
            {
                infoPostulacion.DatosMenuEmprendedor = new List<string>();

                foreach (string opcion in menuEmprendedor)
                {
                    if (!infoPostulacion.DatosMenuEmprendedor.Contains(opcion))
                    {
                        infoPostulacion.DatosMenuEmprendedor.Add(opcion);
                    }
                }
            }
            if (infoPostulacion.DatosMenuComun == null) //Lista de botones con las opciones del registro
            {
                infoPostulacion.DatosMenuComun = new List<string>();

                foreach (string opcion in menuComun)
                {
                    if (!infoPostulacion.DatosMenuComun.Contains(opcion))
                    {
                        infoPostulacion.DatosMenuComun.Add(opcion);
                    }
                }
            }


            List<IBoton> botonesDeEmpresario = new List<IBoton>();
            List<IBoton> botonesDeEmprendedor = new List<IBoton>();
            List<IBoton> botonesComun = new List<IBoton>();

            List<List<IBoton>> tecladoFijoCategorias = new List<List<IBoton>>()
            {
                new List<IBoton>() {TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo}

            };



            foreach (string opcion in infoPostulacion.DatosMenuEmpresario)
            {
                botonesDeEmpresario.Add(new Boton(opcion));
            }
            foreach (string opcion in infoPostulacion.DatosMenuEmprendedor)
            {
                botonesDeEmprendedor.Add(new Boton(opcion));
            }
            foreach (string opcion in infoPostulacion.DatosMenuComun)
            {
                botonesComun.Add(new Boton(opcion));
            }





            switch (infoPostulacion.Estado)
            {

                case Estados.Inicio:
                    Console.WriteLine("Estado: " + infoPostulacion.Estado);
                    respuesta.Texto = "Por favor, indícanos detalladamente lo qué necesitas, dentro de un mensaje.";
                    infoPostulacion.Estado = Estados.Categorias;
                    infoPostulacion.tipoMensaje = TipoMensaje.Callback;

                    return true;

                case Estados.Categorias:
                    StringBuilder st = new StringBuilder();

                    Console.WriteLine("Estado: " + infoPostulacion.Estado);

                    List<string> etiquetas = mensaje.Texto.Split(' ').ToList();
                    infoPostulacion.Etiquetas = etiquetas;
                    infoPostulacion.Estado = Estados.DatosEmpresa;


                    if (this.banderaEmprendedor & !this.banderaEmpresario)
                    {
                        st.Append("############   MENU EMPRENDEDOR   ############");
                        st.Append("\n\nBien, ahora necesitamos que selecciones la opiones que deas ejecutar y el bot te rederigirá a automaticamente");
                        respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeEmprendedor, infoPostulacion.IndiceActual, FILAS_EMPRESA, COLUMNAS_EMPRESA, tecladoFijoCategorias);
                    }
                    else if (!this.banderaEmprendedor & this.banderaEmpresario)
                    {
                        st.Append("############   MENU EMPRESARIO   ############");
                        st.Append("\n\nBien, ahora necesitamos que selecciones la opiones que deas ejecutar y el bot te rederigirá a automaticamente");
                        respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeEmpresario, infoPostulacion.IndiceActual, FILAS_EMPRESA, COLUMNAS_EMPRESA, tecladoFijoCategorias);
                    }
                    else if (!this.banderaEmprendedor & !this.banderaEmpresario)
                    {
                        st.Append("############   MENU   ############");
                        st.Append("\n\nBien, ahora necesitamos que selecciones la opiones que deas ejecutar y el bot te rederigirá a automaticamente");
                        respuesta.Botones = this.ObtenerMatrizDeBotones(botonesComun, infoPostulacion.IndiceActual, FILAS_EMPRESA, COLUMNAS_EMPRESA, tecladoFijoCategorias);
                    }
                    respuesta.Texto = st.ToString();
                    return true;



                case Estados.DatosEmpresa:
                    //Deteccion de tipo de mensaje en base a si el mensaje de entrada es igual a algún tipo de boton

                    return false;

            }
            infoPostulacion = new InformacionPostulacion();
            return false;
        }

        /// <summary>
        /// Retorna este "handler" al estado inicial.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        protected override void CancelarInterno(Sesion sesion)
        {
            this.Busquedas.Remove(sesion.IdUsuario);
        }

        /// <summary>
        /// Determina si este "handler" puede procesar el mensaje.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        /// <returns>true si el mensaje puede ser pocesado; false en caso contrario.</returns>
        protected override bool PuedeResolver(Sesion sesion)
        {
            return true;

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

        /// <summary>
        /// Representación de los posibles estados de una postulación a oferta.
        /// </summary>
        private enum Estados
        {
            Inicio,
            Categorias,
            DatosEmpresa,
        }

        /// <summary>
        /// Representación de los posibles tipos de mensajes.
        /// </summary>
        private enum TipoMensaje
        {

            Mensaje,
            Callback
        }
        /// <summary>
        /// Clase privada contenedora de la información de una postulación a una oferta.
        /// </summary>
        private class InformacionPostulacion
        {
            /// <summary>
            /// Lista de etiquetas que está usando un usuario para buscar una oferta.
            /// </summary>
            public List<string> Etiquetas { get; set; } = new List<string>();

            /// <summary>
            /// Lista de categorías que está usando un usuario para buscar una oferta.
            /// </summary>
            public List<string> Categorias { get; set; } = new List<string>();

            /// <summary>
            /// Estado de la búsqueda de ofertas de un usuario.
            /// </summary>
            public Estados Estado { get; set; } = Estados.Inicio;

            public TipoMensaje tipoMensaje { get; set; }


            /// <summary>
            /// Indice actual dentro de la lista de categorías.
            /// </summary>
            public int IndiceActual { get; set; } = 0;

            public List<string> DatosMenuEmpresario { get; set; }
            public List<string> DatosMenuEmprendedor { get; set; }
            public List<string> DatosMenuComun { get; set; }

        }
    }
}