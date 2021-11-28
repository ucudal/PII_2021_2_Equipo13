using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PII_E13.ClassLibrary;

namespace PII_E13.HandlerLibrary
{
    /// <summary>
    /// Handler encargado de procesar el registro de un emprendedor.
    /// </summary>
    public class RegistrarEmprendedorHandler : HandlerBase
    {
        //private StringBuilder stringBuilder;
        private Dictionary<Sesion, StringBuilder> SbSesion = new Dictionary<Sesion, StringBuilder>();

        // Dictionary<string, string> DiccDatosEmprendedor = new Dictionary<string, string>();
        // Dictionary<string, string> DiccDatosHabilitacion = new Dictionary<string, string>();
        private Dictionary<Sesion, Dictionary<string, Dictionary<string, string>>> Sesiones = new Dictionary<Sesion, Dictionary<string, Dictionary<string, string>>>();

        //private string accionPrevia;
        private Dictionary<Sesion, string> accionPreviaSesion = new Dictionary<Sesion, string>();
        private const int COLUMNAS_EMPRENDEDOR = 1;
        private const int FILAS_EMPRENDEDOR = 4;
        private const int COLUMNAS_HABILTIACIONES = 1;
        private const int FILAS_HABILTIACIONES = 5;
        private const int COLUMNAS_OFERTAS = 1;

        /// <summary>
        /// Diccionario utilizado para contener todas las búsquedas que se están realizando por los usuarios.
        /// Se identifica al usuario por su id en una plataforma y se guarda una instancia de <see cref="InformacionPostulacion"/>.
        /// </summary>
        /// <value>Diccionario de instancias de <see cref="InformacionPostulacion"/> identificadas por ids de usuarios en string</value>
        private Dictionary<string, InformacionPostulacion> Busquedas { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RegistrarEmprendedorHandler"/>. 
        /// </summary>
        public RegistrarEmprendedorHandler(HandlerBase siguiente, string intencion) : base(siguiente, intencion)
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
                this.SbSesion.Add(sesion, new StringBuilder("Datos sobre ti: \n"));
            }
            StringBuilder stringBuilder = this.SbSesion[sesion];

            if (!this.accionPreviaSesion.ContainsKey(sesion))
            {
                this.accionPreviaSesion.Add(sesion, String.Empty);
            }
            string accionPrevia = this.accionPreviaSesion[sesion];

            if (!this.Sesiones.ContainsKey(sesion))
            {
                Dictionary<string, Dictionary<string, string>> datos = new Dictionary<string, Dictionary<string, string>>();
                datos.Add("DiccDatosEmprendedor", new Dictionary<string, string>());
                datos.Add("DiccDatosHabilitacion", new Dictionary<string, string>());
                this.Sesiones.Add(sesion, datos);
            }
            Dictionary<string, string> DiccDatosEmprendedor = Sesiones[sesion]["DiccDatosEmprendedor"];
            Dictionary<string, string> DiccDatosHabilitacion = Sesiones[sesion]["DiccDatosHabilitacion"];

            InformacionPostulacion infoPostulacion = new InformacionPostulacion();
            if (this.Busquedas.ContainsKey(mensaje.IdUsuario))
            {
                infoPostulacion = this.Busquedas[mensaje.IdUsuario];
            }
            else
            {
                this.Busquedas.Add(mensaje.IdUsuario, infoPostulacion);
            }
            List<string> titulosOfertas = new List<string>();
            List<string> opcionesRegistro = new List<string>(); //Opciones para registro
            opcionesRegistro.Add("Nombre");
            opcionesRegistro.Add("Ciudad");
            opcionesRegistro.Add("Direccion");
            opcionesRegistro.Add("Rubro");


            List<string> opcionesHabilitacion = new List<string>(); //Opciones para el ingreso de Habiltiaciones
            opcionesHabilitacion.Add("Nombre");
            opcionesHabilitacion.Add("Descripcion");
            opcionesHabilitacion.Add("Nombre Insitucion Habilitada");
            opcionesHabilitacion.Add("Fecha Tramite");
            opcionesHabilitacion.Add("Fecha Vencimiento");


            if (infoPostulacion.DatosEmprendedorDisponibles == null) //Lista de botones con las opciones del registro
            {
                infoPostulacion.DatosEmprendedorDisponibles = new List<string>();

                foreach (string opcion in opcionesRegistro)
                {
                    if (!infoPostulacion.DatosEmprendedorDisponibles.Contains(opcion))
                    {
                        infoPostulacion.DatosEmprendedorDisponibles.Add(opcion);
                    }
                }
            }

            if (infoPostulacion.HabilitacionesDisponibles == null)//Lista de botones con las opciones del registro de habiltiaciones
            {
                infoPostulacion.HabilitacionesDisponibles = new List<string>();

                foreach (string opcion in opcionesHabilitacion)
                {
                    if (!infoPostulacion.HabilitacionesDisponibles.Contains(opcion))
                    {
                        infoPostulacion.HabilitacionesDisponibles.Add(opcion);
                    }
                }
            }
            List<IBoton> botonesDeEmprendedor = new List<IBoton>();
            List<IBoton> botonesDeHabilitacion = new List<IBoton>();
            List<List<IBoton>> tecladoFijoCategorias = new List<List<IBoton>>()
            {
                new List<IBoton>() {TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo}

            };

            foreach (string opcion in infoPostulacion.DatosEmprendedorDisponibles)
            {
                botonesDeEmprendedor.Add(new Boton(opcion));
            }

            foreach (string opcion in infoPostulacion.HabilitacionesDisponibles)
            {
                botonesDeHabilitacion.Add(new Boton(opcion));
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
                    Console.WriteLine("Estado: " + infoPostulacion.Estado);

                    List<string> etiquetas = mensaje.Texto.Split(' ').ToList();
                    infoPostulacion.Etiquetas = etiquetas;
                    respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeEmprendedor, infoPostulacion.IndiceActual, FILAS_EMPRENDEDOR, COLUMNAS_EMPRENDEDOR, tecladoFijoCategorias);
                    infoPostulacion.Estado = Estados.DatosEmprendedor;
                    foreach (string nombreBoton in opcionesRegistro)
                    {
                        if (mensaje.Texto == nombreBoton)
                        {
                            infoPostulacion.tipoMensaje = TipoMensaje.Callback;

                        }
                    }
                    StringBuilder st = new StringBuilder();
                    st.Append("############   REGISTRO EMPRENDEDOR   ############");
                    st.Append("\nBien, ahora necesitamos que selecciones los datos que quiere ir ingresando.\n\nPresione el boton referido al dato que desea ingresar y escriba el dato en el chat para que lo tomemos. \n\n\nSelecciona \"Listo\" cuando quieras continuar el registro, o \"Cancelar\" para detenerlo.");
                    respuesta.Texto = st.ToString();
                    return true;



                case Estados.DatosEmprendedor:
                    //Deteccion de tipo de mensaje en base a si el mensaje de entrada es igual a algún tipo de boton
                    foreach (string nombreBoton in opcionesRegistro)
                    {
                        if ((mensaje.Texto == nombreBoton) ^ (mensaje.Texto == "Listo") ^ (mensaje.Texto == "Cancelar"))
                        {
                            infoPostulacion.tipoMensaje = TipoMensaje.Callback;
                            break;
                        }
                        else
                        {
                            infoPostulacion.tipoMensaje = TipoMensaje.Mensaje;
                        }
                    }
                    switch (infoPostulacion.tipoMensaje)
                    {
                        case TipoMensaje.Callback:
                            Console.WriteLine("ESTADO: " + infoPostulacion.tipoMensaje);

                            switch (mensaje.Texto)
                            {
                                case "Listo":
                                    foreach (var item in DiccDatosEmprendedor)
                                    {
                                        stringBuilder.Append("\n" + item.Key + ":   " + item.Value);
                                    }
                                    respuesta.Texto = stringBuilder.ToString();
                                    infoPostulacion.Estado = Estados.DatosHabilitacion;
                                    respuesta.Texto = $"A continuación repetiremos el proceso para ingresar sus habiltiaciones.\n\nSelecciona _\"Listo\"_ cuando quieras finalziar con el registro, o _\"Cancelar\"_ para no ingresar habilitaciones.";
                                    respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeHabilitacion, infoPostulacion.IndiceActual, FILAS_HABILTIACIONES, COLUMNAS_HABILTIACIONES, tecladoFijoCategorias);
                                    return true;
                                case "Cancelar":
                                    return false;
                            }
                            respuesta.Texto = $"A continuacion se habilito el campo _\"{mensaje.Texto}\"_ para su ingreso.\n\n";
                            accionPrevia = mensaje.Texto;
                            respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeEmprendedor, infoPostulacion.IndiceActual, FILAS_EMPRENDEDOR, COLUMNAS_EMPRENDEDOR, tecladoFijoCategorias);
                            respuesta.EditarMensaje = true;
                            return true;



                        case TipoMensaje.Mensaje:
                            Console.WriteLine("ESTADO: " + infoPostulacion.tipoMensaje);
                            respuesta.Texto = $"Se ingresó el dato _\"{mensaje.Texto}\"_ en el campo *{accionPrevia}*";
                            DiccDatosEmprendedor[accionPrevia] = mensaje.Texto;
                            respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeEmprendedor, infoPostulacion.IndiceActual, FILAS_EMPRENDEDOR, COLUMNAS_EMPRENDEDOR, tecladoFijoCategorias);
                            return true;
                    }
                    return true;



                case Estados.DatosHabilitacion:
                    //Deteccion de tipo de mensaje en base a si el mensaje de entrada es igual a algún tipo de boton
                    foreach (string nombreBoton in opcionesHabilitacion)
                    {
                        if ((mensaje.Texto == nombreBoton) ^ (mensaje.Texto == "Listo") ^ (mensaje.Texto == "Cancelar"))
                        {
                            infoPostulacion.tipoMensaje = TipoMensaje.Callback;
                            break;
                        }
                        else
                        {
                            infoPostulacion.tipoMensaje = TipoMensaje.Mensaje;
                        }
                    }

                    switch (infoPostulacion.tipoMensaje)
                    {
                        case TipoMensaje.Callback:
                            Console.WriteLine("ESTADO: " + infoPostulacion.tipoMensaje);
                            switch (mensaje.Texto)
                            {
                                case "Listo":
                                    stringBuilder.Append("\n\n\nDatos sobre tus habilitaciones: \n");

                                    foreach (var item in DiccDatosHabilitacion)
                                    {
                                        stringBuilder.Append("\n" + item.Key + ":   " + item.Value);
                                    }

                                    List<Habilitacion> habilitaciones = new List<Habilitacion>();
                                    habilitaciones.Add(new Habilitacion(DiccDatosHabilitacion["Nombre"], DiccDatosHabilitacion["Descripcion"], DiccDatosHabilitacion["Nombre Insitucion Habilitada"], Convert.ToDateTime(DiccDatosHabilitacion["Fecha Tramite"]), Convert.ToDateTime(DiccDatosHabilitacion["Fecha Vencimiento"]), true));
                                    Sistema.Instancia.RegistrarEmprendedor(mensaje.IdUsuario.ToString(), DiccDatosEmprendedor["Ciudad"], DiccDatosEmprendedor["Direccion"], DiccDatosEmprendedor["Rubro"], DiccDatosEmprendedor["Nombre"], habilitaciones);

                                    Console.WriteLine("id empresa registrado: " + mensaje.IdUsuario.ToString());

                                    if (Sistema.Instancia.ObtenerEmprendedorPorId(mensaje.IdUsuario.ToString()).Nombre == DiccDatosEmprendedor["Nombre"])
                                    {
                                        stringBuilder.Append("\n\nUsted ha sido ingresado en el sistema exitosamente. Bienvenido.");
                                    }
                                    respuesta.Texto = stringBuilder.ToString();
                                    return true;



                                case "Cancelar":
                                    Sistema.Instancia.RegistrarEmprendedor(mensaje.IdUsuario.ToString(), DiccDatosEmprendedor["Ciudad"], DiccDatosEmprendedor["Direccion"], DiccDatosEmprendedor["Rubro"], DiccDatosEmprendedor["Nombre"], null);
                                    Console.WriteLine("id empresa registrado: " + mensaje.IdUsuario.ToString());
                                    if (Sistema.Instancia.ObtenerEmprendedorPorId(mensaje.IdUsuario.ToString()).Nombre == DiccDatosEmprendedor["Nombre"])
                                    {
                                        stringBuilder.Append("\n\nUsted ha sido ingresado en el sistema exitosamente. Bienvenido.");
                                    }
                                    respuesta.Texto = stringBuilder.ToString();

                                    return true;
                            }
                            if (mensaje.Texto == "Fecha Tramite" ^ mensaje.Texto == "Fecha Vencimiento")
                            {
                                respuesta.Texto = $"A continuacion se habilito el campo _\"{mensaje.Texto}\"_ para su ingreso, debido a que se trata de una fecha es necesario que se ingrese en el siguiente formato:\n\n *DD-MM-YYYY*";
                            }
                            else
                            {
                                respuesta.Texto = $"A continuacion se habilito el campo _\"{mensaje.Texto}\"_ para su ingreso.\n\n";
                            }
                            accionPrevia = mensaje.Texto;
                            respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeHabilitacion, infoPostulacion.IndiceActual, FILAS_HABILTIACIONES, COLUMNAS_HABILTIACIONES, tecladoFijoCategorias);
                            respuesta.EditarMensaje = true;
                            return true;



                        case TipoMensaje.Mensaje:
                            Console.WriteLine("ESTADO: " + infoPostulacion.tipoMensaje);
                            respuesta.Texto = $"Se ingresó el dato _\"{mensaje.Texto}\"_ en el campo *{accionPrevia}*";
                            DiccDatosHabilitacion[accionPrevia] = mensaje.Texto;
                            respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeHabilitacion, infoPostulacion.IndiceActual, FILAS_HABILTIACIONES, COLUMNAS_HABILTIACIONES, tecladoFijoCategorias);
                            return true;
                    }
                    return true;


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
            try
            {
                Sistema.Instancia.ObtenerEmprendedorPorId(sesion.IdUsuario);
                return false;
            }
            catch (KeyNotFoundException e)
            {
                try
                {
                    Sistema.Instancia.ObtenerEmprendedorPorId(sesion.IdUsuario);
                    return false;
                }
                catch (KeyNotFoundException e2)
                {
                    Intencion intencion = sesion.PLN.UltimaIntencion;
                    if (intencion.Entrada.Equals("Emprendedor"))
                    {
                        return true;
                    }
                    else if (intencion.Entrada.Equals("Empresa"))
                    {
                        return false;
                    }

                    return (this.Busquedas.ContainsKey(sesion.IdUsuario) &&
                        (intencion.Nombre.Equals("Default") || sesion.PLN.UltimaIntencion.ConfianzaDeteccion < 90)
                    );
                }
            }
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

        private List<List<IBoton>> ObtenerMatrizDeBotones(List<IBoton> botones, int indiceInicial = 0, int filas = 1, int columnas = 1, List<List<IBoton>> botonesFijos = null)
        {
            List<List<IBoton>> matrizBotones = new List<List<IBoton>>();
            for (int i = 0; i < filas; i++)
            {
                List<IBoton> fila = new List<IBoton>();
                for (int j = 0; j < columnas; j++)
                {
                    try
                    {
                        fila.Add(botones[indiceInicial]);
                        indiceInicial++;
                    }
                    catch (Exception e)
                    {
                        break;
                    }
                }
                if (fila.Count > 0)
                {
                    matrizBotones.Add(fila);
                }
            }
            if (botonesFijos != null)
            {
                matrizBotones.AddRange(botonesFijos);
            }
            return matrizBotones;
        }

        /// <summary>
        /// Representación de los posibles estados de una postulación a oferta.
        /// </summary>
        private enum Estados
        {
            Inicio,
            Categorias,
            DatosEmprendedor,
            DatosHabilitacion

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
            /// Lista de ofertas encontradas en la búsqueda de ofertas.
            /// </summary>
            public List<Oferta> OfertasEncontradas { get; set; } = new List<Oferta>();
            /// <summary>
            /// Lista de categorías que está usando un usuario para buscar una oferta.
            /// </summary>
            public List<string> Categorias { get; set; } = new List<string>();
            public List<string> Habilitaciones { get; set; } = new List<string>();

            /// <summary>
            /// Estado de la búsqueda de ofertas de un usuario.
            /// </summary>
            public Estados Estado { get; set; } = Estados.Inicio;

            public TipoMensaje tipoMensaje { get; set; }

            /// <summary>
            /// Oferta seleccionada por un usuario entre la lista de ofertas encontradas.
            /// </summary>
            public Oferta ofertaSeleccionada { get; set; }

            /// <summary>
            /// Indice actual dentro de la lista de categorías.
            /// </summary>
            public int IndiceActual { get; set; } = 0;

            public List<string> DatosEmprendedorDisponibles { get; set; }
            public List<string> HabilitacionesDisponibles { get; set; }

        }
    }
}