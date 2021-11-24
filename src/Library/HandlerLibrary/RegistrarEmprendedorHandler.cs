using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PII_E13.ClassLibrary;
using Telegram.Bot.Types.ReplyMarkups;

namespace PII_E13.HandlerLibrary
{
    /// <summary>
    /// Clase base para implementar el patrón Chain of Responsibility. En ese patrón se pasa un mensaje a través de una
    /// cadena de "handlers" que pueden procesar o no el mensaje. Cada "handler" decide si procesa el mensaje, o si se lo
    /// pasa al siguiente. Esta clase base implmementa la responsabilidad de recibir el mensaje y pasarlo al siguiente
    /// "handler" en caso que el mensaje no sea procesado. La responsabilidad de decidir si el mensaje se procesa o no, y
    /// de procesarlo, se delega a las clases sucesoras de esta clase base.
    /// </summary>
    public class RegistrarEmprendedorHandler : HandlerBase
    {
        private StringBuilder stringBuilder;


        Dictionary<string, string> DiccDatosEmprendedor = new Dictionary<string, string>();
        Dictionary<string, string> DiccDatosHabilitacion = new Dictionary<string, string>();
        private string accionPrevia;
        private const int COLUMNAS_CATEGORIAS = 1;
        private const int FILAS_CATEGORIAS = 4;
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
        /// Inicializa una nueva instancia de la clase <see cref="PostularseAOfertaHandler"/>. 
        /// Esta clase procesa la postulación a una oferta.
        /// </summary>

     

         public RegistrarEmprendedorHandler(HandlerBase siguiente, string intencion) : base(siguiente, intencion)
        {
            this.Busquedas = new Dictionary<string, InformacionPostulacion>();
            this.stringBuilder = new StringBuilder();
            stringBuilder.Append("Datos sobre ti: \n\n");
        }

        /// <summary>
        /// La clase procesa el mensaje y retorna true o no lo procesa y retorna false.
        /// </summary>
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
                new List<IBoton>() {TelegramBot.Instancia.BotonAnterior, TelegramBot.Instancia.BotonSiguiente},
                new List<IBoton>() {TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo}
            };

            foreach (string opcion in infoPostulacion.DatosEmprendedorDisponibles)
            {
                botonesDeEmprendedor.Add(new Boton(opcion));
            }


            switch (infoPostulacion.Estado)
            {

                case Estados.Inicio:
                    Console.WriteLine("Estado: " + infoPostulacion.Estado);
                    respuesta.Texto = "Por favor, indícanos detalladamente lo qué necesitas, dentro de un mensaje.";
                    infoPostulacion.Estado = Estados.Categorias;
                    return true;

                case Estados.Categorias:
                    Console.WriteLine("Estado: " + infoPostulacion.Estado);

                    List<string> etiquetas = mensaje.Texto.Split(' ').ToList();
                    infoPostulacion.Etiquetas = etiquetas;
                    respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeEmprendedor, infoPostulacion.IndiceActual, FILAS_HABILTIACIONES, COLUMNAS_HABILTIACIONES, tecladoFijoCategorias);
                    infoPostulacion.Estado = Estados.DatosEmprendedor;
                    StringBuilder st = new StringBuilder();
                    st.Append("############   REGISTRO EMPRENDEDOR   ############");
                    st.Append("\nBien, ahora necesitamos que selecciones los datos que quiere ir ingresando.\n\nPresione el boton referido al dato que desea ingresar y escriba el dato en el chat para que lo tomemos. \n\nSelecciona \"Listo\" cuando quieras continuar el registro, o \"Cancelar\" para detenerlo.");
                    respuesta.Texto = st.ToString();
                    return true;

                case Estados.DatosEmprendedor:
                    Console.WriteLine("Estado: " + infoPostulacion.Estado);
                    switch (mensaje.Texto)
                    {
                        case "Listo":
                            infoPostulacion.Estado = Estados.DatosHabilitacion;
                            Console.WriteLine("Estado: " + infoPostulacion.Estado);
                            infoPostulacion.IndiceActual = 0;

                            respuesta.Texto = this.stringBuilder.ToString();
                            respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeHabilitacion, infoPostulacion.IndiceActual, FILAS_HABILTIACIONES, COLUMNAS_HABILTIACIONES, tecladoFijoCategorias);
                            return true;
                        case "Cancelar":
                            this.Cancelar(sesion);
                            return false;
                    }
                    if (!infoPostulacion.DatosEmprendedorDisponibles.Contains(mensaje.Texto))
                    {
                        respuesta.Texto = $"Se ingresó el dato _\"{mensaje.Texto}\"_ en el campo *{this.accionPrevia}*";
                        this.stringBuilder.Append("\n" + this.accionPrevia + ":   " + mensaje.Texto);
                        DiccDatosEmprendedor.Add(this.accionPrevia, mensaje.Texto);


                        respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeEmprendedor, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                        respuesta.EditarMensaje = true;
                        return true;
                    }

                    if (infoPostulacion.IndiceActual >= botonesDeEmprendedor.Count)
                    {
                        infoPostulacion.IndiceActual = botonesDeEmprendedor.Count - FILAS_CATEGORIAS * COLUMNAS_CATEGORIAS;
                    }
                    infoPostulacion.DatosEmprendedorDisponibles.Remove(mensaje.Texto);
                    if (infoPostulacion.DatosEmprendedorDisponibles.Count <= (FILAS_CATEGORIAS * COLUMNAS_CATEGORIAS))
                    {
                        respuesta.Botones = new List<List<IBoton>>()
                                    {
                                        new List<IBoton>() {new Boton("Volver al menú")},
                                        new List<IBoton>() {new Boton("a")}

                                    };
                    }

                    respuesta.Texto = $"A continuacion se habilito el campo _\"{mensaje.Texto}\"_ para su ingreso.\n\nSelecciona _\"Listo\"_ cuando quieras finalizar el registro, o _\"Cancelar\"_ para detenerlo.";
                    respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeEmprendedor, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                    respuesta.EditarMensaje = true;
                    return true;




                case Estados.DatosHabilitacion:

                    Console.WriteLine("Estado: " + infoPostulacion.Estado);

                    if (!infoPostulacion.HabilitacionesDisponibles.Contains(mensaje.Texto))
                    {
                        respuesta.Texto = $"Se ingresó el dato _\"{mensaje.Texto}\"_ en el campo *{this.accionPrevia}*";
                        this.stringBuilder.Append("\n" + this.accionPrevia + ":   " + mensaje.Texto);
                        this.DiccDatosHabilitacion.Add(this.accionPrevia, mensaje.Texto);
                        respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeHabilitacion, infoPostulacion.IndiceActual, FILAS_HABILTIACIONES, COLUMNAS_HABILTIACIONES, tecladoFijoCategorias);
                        return true;
                    }

                    if (botonesDeHabilitacion.Count <= infoPostulacion.IndiceActual + COLUMNAS_HABILTIACIONES * FILAS_HABILTIACIONES)
                    {
                        infoPostulacion.IndiceActual = botonesDeHabilitacion.Count - COLUMNAS_HABILTIACIONES * FILAS_HABILTIACIONES;
                    }
                    infoPostulacion.HabilitacionesDisponibles.Remove(mensaje.Texto);
                    if (infoPostulacion.HabilitacionesDisponibles.Count <= (FILAS_HABILTIACIONES * COLUMNAS_HABILTIACIONES))
                    {
                        respuesta.Botones = new List<List<IBoton>>()
                                    {
                                        new List<IBoton>() {new Boton("Volver al menú")},
                                        new List<IBoton>() {new Boton("a")}

                                    };
                    }


                    respuesta.Texto = $"A continuacion se habilito el campo _\"{mensaje.Texto}\"_ para su ingreso.\n\nSelecciona _\"Listo\"_ cuando quieras finalizar el registro, o _\"Cancelar\"_ para detenerlo.";
                    respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeHabilitacion, infoPostulacion.IndiceActual, FILAS_HABILTIACIONES, COLUMNAS_HABILTIACIONES, tecladoFijoCategorias);
                    respuesta.EditarMensaje = true;
                    return true;

            }
            infoPostulacion = new InformacionPostulacion();
            return false;
        }

        /// <summary>
        /// La clase procesa el mensaje y retorna true o no lo procesa y retorna false.
        /// </summary>
        /// <param name="callback">El callback a procesar.</param>
        /// <param name="respuesta">La respuesta al mensaje procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario</returns>
        /*
        protected override bool ResolverInterno(ICallBack callback, out RespuestaTelegram respuesta)
        {
            respuesta = new RespuestaTelegram(string.Empty);
            if (!this.PuedeResolver(callback))
            {
                return false;
            }

            InformacionPostulacion infoPostulacion = new InformacionPostulacion();
            if (this.Busquedas.ContainsKey(callback.IdUsuario))
            {
                infoPostulacion = this.Busquedas[callback.IdUsuario];
            }
            else
            {
                this.Busquedas.Add(callback.IdUsuario, infoPostulacion);
            }
            List<string> titulosOfertas = new List<string>();

            List<InlineKeyboardButton> botonesDeCategorias = TelegramBot.Instancia.ObtenerBotones(infoPostulacion.CategoriasDisponibles);
            List<InlineKeyboardButton> botonesDehabilitacion = TelegramBot.Instancia.ObtenerBotones(infoPostulacion.HabilitacionesDisponibles);
            List<List<InlineKeyboardButton>> tecladoFijoCategorias = new List<List<InlineKeyboardButton>>() {
                new List<InlineKeyboardButton>() {TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo}
            };
            List<List<InlineKeyboardButton>> tecladoFijoHabilitaciones = new List<List<InlineKeyboardButton>>() {
                new List<InlineKeyboardButton>() {TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo}
            };



            switch (infoPostulacion.Estado)
            {
                case Estados.DatosEmprendedor:
                    switch (callback.Texto)
                    {
                        case "Listo":
                            Console.WriteLine("Estado: " + infoPostulacion.Estado);

                            botonesDehabilitacion = TelegramBot.Instancia.ObtenerBotones(infoPostulacion.HabilitacionesDisponibles);
                            this.stringBuilder.Append("\n-------------------------------------------------");
                            this.stringBuilder.Append("\n\n\nDatos sobre las habilitaciones: ");
                            infoPostulacion.IndiceActual = 0;

                            respuesta.Texto = this.stringBuilder.ToString();
                            infoPostulacion.Estado = Estados.DatosHabilitacion;

                            respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeHabilitacion, infoPostulacion.IndiceActual, FILAS_HABILTIACIONES, COLUMNAS_HABILTIACIONES, tecladoFijoCategorias);
                            return true;

                        case "Cancelar":
                            this.Cancelar();
                            return false;
                    }

                    botonesDeCategorias.Remove(botonesDeCategorias.First(b => b.Text == callback.Texto));
                    if (infoPostulacion.IndiceActual >= botonesDeCategorias.Count)
                    {
                        infoPostulacion.IndiceActual = botonesDeCategorias.Count - FILAS_CATEGORIAS * COLUMNAS_CATEGORIAS;
                    }
                    infoPostulacion.CategoriasDisponibles.Remove(callback.Texto);
                    if (infoPostulacion.CategoriasDisponibles.Count <= (FILAS_CATEGORIAS * COLUMNAS_CATEGORIAS))
                    {
                        tecladoFijoCategorias = new List<List<InlineKeyboardButton>>()
                        {
                            new List<InlineKeyboardButton>() { TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo }
                        };
                    }

                    infoPostulacion.Categorias.Add(callback.Texto);
                    respuesta.Texto = $"A continuacion se habilito el campo _\"{callback.Texto}\"_ para su ingreso.\n\nSelecciona _\"Listo\"_ cuando quieras continuar con el registro, o _\"Cancelar\"_ para detenerlo.";
                    this.accionPrevia = callback.Texto;
                    respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                    respuesta.EditarMensaje = true;
                    return true;


                case Estados.DatosHabilitacion:


                    switch (callback.Texto)
                    {
                        case "Listo":
                            respuesta.Texto = this.stringBuilder.ToString();
                            List<Habilitacion> habilitaciones = new List<Habilitacion>();
                            int id = _random.Next(1000);


                            habilitaciones.Add(new Habilitacion(this.DiccDatosHabilitacion["Nombre"], this.DiccDatosHabilitacion["Descripcion"], this.DiccDatosHabilitacion["Nombre Insitucion Habilitada"], Convert.ToDateTime(this.DiccDatosHabilitacion["Fecha Tramite"]), Convert.ToDateTime(this.DiccDatosHabilitacion["Fecha Vencimiento"]), true));
                            Sistema.Instancia.RegistrarEmprendedor(callback.IdUsuario.ToString(), this.DiccDatosEmprendedor["Ciudad"], this.DiccDatosEmprendedor["Direccion"], this.DiccDatosEmprendedor["Rubro"], this.DiccDatosEmprendedor["Nombre"], habilitaciones);
                            Console.WriteLine("id random: " + callback.IdUsuario.ToString());
                            if (Sistema.Instancia.ObtenerEmprendedorPorId(callback.IdUsuario.ToString()).Nombre == this.datosEmprendedor[0])
                            {
                                this.stringBuilder.Append("\n\nUsted ha sido ingresado en el sistema exitosamente. Bienvenido.");

                                respuesta.Texto = this.stringBuilder.ToString();

                            }
                            return true;
                        case "Cancelar":
                            this.Cancelar();
                            return false;
                    }

                    botonesDehabilitacion.Remove(botonesDehabilitacion.First(b => b.Text == callback.Texto));
                    if (infoPostulacion.IndiceActual >= botonesDehabilitacion.Count)
                    {
                        infoPostulacion.IndiceActual = botonesDehabilitacion.Count - FILAS_HABILTIACIONES * COLUMNAS_HABILTIACIONES;
                    }
                    infoPostulacion.HabilitacionesDisponibles.Remove(callback.Texto);
                    if (infoPostulacion.HabilitacionesDisponibles.Count <= (FILAS_HABILTIACIONES * COLUMNAS_HABILTIACIONES))
                    {
                        tecladoFijoHabilitaciones = new List<List<InlineKeyboardButton>>()
                        {
                            new List<InlineKeyboardButton>() { TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo }
                        };
                    }
                    if (infoPostulacion.HabilitacionesDisponibles.Count == 4)
                    {
                        tecladoFijoHabilitaciones = new List<List<InlineKeyboardButton>>()
                        {
                            new List<InlineKeyboardButton>() { TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo }
                        };
                    }
                    infoPostulacion.Categorias.Add(callback.Texto);
                    if (callback.Texto == "Fecha Tramite" ^ callback.Texto == "Fecha Vencimiento")
                    {
                        respuesta.Texto = $"A continuacion se habilito el campo _\"{callback.Texto}\"_ para su ingreso, debido a que se trata de una fecha es necesario que se ingrese en el siguiente formato:\n\n *DD-MM-YYYY*\n\nSelecciona _\"Listo\"_ cuando quieras continuar con el registro, o _\"Cancelar\"_ para detenerlo.";

                    }
                    else
                    {
                        respuesta.Texto = $"A continuacion se habilito el campo _\"{callback.Texto}\"_ para su ingreso.\n\nSelecciona _\"Listo\"_ cuando quieras continuar con el registro, o _\"Cancelar\"_ para detenerlo.";

                    }
                    this.accionPrevia = callback.Texto;
                    Console.WriteLine("accion previa: " + this.accionPrevia);
                    respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeHabilitacion, infoPostulacion.IndiceActual, FILAS_HABILTIACIONES, COLUMNAS_HABILTIACIONES, tecladoFijoCategorias);
                    respuesta.EditarMensaje = true;
                    return true;


            }
            infoPostulacion = new InformacionPostulacion();
            return false;
        }
        */

        /// <summary>
        /// Este método puede ser sobreescrito en las clases sucesores que procesan varios mensajes cambiando de estado
        /// entre mensajes deben sobreescribir este método para volver al estado inicial. En la clase base no hace nada.
        /// </summary>
        protected void CancelarInterno(string idUsuario)
        {
            this.Busquedas.Remove(idUsuario);
        }

        /// <summary>
        /// Determina si este "handler" puede procesar el mensaje. En la clase base se utiliza el array
        /// <see cref="HandlerBase.Etiquetas"/> para buscar el texto en el mensaje ignorando mayúsculas y minúsculas. Las
        /// clases sucesores pueden sobreescribir este método para proveer otro mecanismo para determina si procesan o no
        /// un mensaje.
        /// </summary>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <returns>true si el mensaje puede ser pocesado; false en caso contrario.</returns>
        protected override bool PuedeResolver(Sesion sesion)
        {
           return true;
        }

        /// <summary>
        /// Determina si este "handler" puede procesar el mensaje. En la clase base se utiliza el array
        /// <see cref="HandlerBase.Etiquetas"/> para buscar el texto en el mensaje ignorando mayúsculas y minúsculas. Las
        /// clases sucesores pueden sobreescribir este método para proveer otro mecanismo para determina si procesan o no
        /// un mensaje.
        /// </summary>
        /// <param name="callback">El callback a procesar.</param>
        /// <returns>true si el mensaje puede ser pocesado; false en caso contrario.</returns>
       

        /// <summary>
        /// Retorna este "handler" al estado inicial. En los "handler" sin estado no hace nada. Los "handlers" que
        /// procesan varios mensajes cambiando de estado entre mensajes deben sobreescribir este método para volver al
        /// estado inicial.
        /// </summary>
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