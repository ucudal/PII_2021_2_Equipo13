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
    public class CrearOfertaHandler : HandlerBase
    {
        private StringBuilder stringBuilder;
        private StringBuilder S4;
        private Oferta oferta;
        private bool yaMostrado = false;

        Dictionary<string, string> DiccDatosOferta = new Dictionary<string, string>();
        Dictionary<string, string> DiccDatosProducto = new Dictionary<string, string>();
        Dictionary<string, string> DiccDatosHabilitacion = new Dictionary<string, string>();


        List<Producto> productos;

        Material material;
        private string accionPrevia;
        private const int COLUMNAS_OFERTA = 1;
        private const int FILAS_OFERTA = 3;

        private const int COLUMNAS_PRODUCTO = 1;
        private const int FILAS_PRODUCTO = 5;
        private const int COLUMNAS_MATERIAL = 1;
        private const int FILAS_MATERIAL = 4;

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



        public CrearOfertaHandler(HandlerBase siguiente, string intencion) : base(siguiente, intencion)
        {
            this.Busquedas = new Dictionary<string, InformacionPostulacion>();
            this.stringBuilder = new StringBuilder();
            this.S4 = new StringBuilder();
            this.productos = new List<Producto>();
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
            List<string> opcionesOferta = new List<string>(); //Opciones para Oferta
            opcionesOferta.Add("Titulo");
            opcionesOferta.Add("Descripcion");
            opcionesOferta.Add("Fecha Cierre");

            List<string> opcionesEtiqueta = new List<string>(); //Opciones para Etiqueta
            opcionesOferta.Add("Etiqueta");

            List<string> opcionesHabilitacion = new List<string>(); //Opciones para el ingreso de Habilitaciones
            opcionesHabilitacion.Add("Nombre");
            opcionesHabilitacion.Add("Descripcion");
            opcionesHabilitacion.Add("Nombre Insitucion Habilitada");


            List<string> opcionesProducto = new List<string>(); //Opciones para el ingreso de Producto
            opcionesProducto.Add("Ciudad");
            opcionesProducto.Add("Direccion");
            opcionesProducto.Add("Cantidad en unidades");
            opcionesProducto.Add("Valor en UYU");
            opcionesProducto.Add("Valor en USD");



            if (infoPostulacion.DatosOfertaDisponibles == null) //Lista de botones con las opciones del registro de ofertas
            {
                infoPostulacion.DatosOfertaDisponibles = new List<string>();

                foreach (string opcion in opcionesOferta)
                {
                    if (!infoPostulacion.DatosOfertaDisponibles.Contains(opcion))
                    {
                        infoPostulacion.DatosOfertaDisponibles.Add(opcion);
                    }
                }
            }

            if (infoPostulacion.DatosProductoDisponible == null)//Lista de botones con las opciones del registro de productos
            {
                infoPostulacion.DatosProductoDisponible = new List<string>();

                foreach (string opcion in opcionesProducto)
                {
                    if (!infoPostulacion.DatosProductoDisponible.Contains(opcion))
                    {
                        infoPostulacion.DatosProductoDisponible.Add(opcion);
                    }
                }
            }

            if (infoPostulacion.DatosHabilitacionesDisponible == null)//Lista de botones con las opciones del registro de habiltiaciones
            {
                infoPostulacion.DatosHabilitacionesDisponible = new List<string>();

                foreach (string opcion in opcionesHabilitacion)
                {
                    if (!infoPostulacion.DatosHabilitacionesDisponible.Contains(opcion))
                    {
                        infoPostulacion.DatosHabilitacionesDisponible.Add(opcion);
                    }
                }
            }

            if (infoPostulacion.DatosEtiquetasDisponible == null)//Lista de botones con las opciones del registro de habiltiaciones
            {
                infoPostulacion.DatosEtiquetasDisponible = new List<string>();

                foreach (string opcion in opcionesEtiqueta)
                {
                    if (!infoPostulacion.DatosEtiquetasDisponible.Contains(opcion))
                    {
                        infoPostulacion.DatosEtiquetasDisponible.Add(opcion);
                    }
                }
            }
            if (infoPostulacion.DatosMaterialesDisponible == null)//Lista de botones con las opciones del registro de habiltiaciones
            {
                infoPostulacion.DatosMaterialesDisponible = new List<string>();

                foreach (Material material in Sistema.Instancia.Materiales)
                {
                    if (!infoPostulacion.DatosMaterialesDisponible.Contains(material.Nombre))
                    {
                        infoPostulacion.DatosMaterialesDisponible.Add(material.Nombre);
                    }
                }
            }



            List<IBoton> botonesDeOferta = new List<IBoton>();
            List<IBoton> botonesDeProducto = new List<IBoton>();
            List<IBoton> botonesDeHabilitaciones = new List<IBoton>();
            List<IBoton> botonesDeEtiquetas = new List<IBoton>();
            List<IBoton> botonesDeMateriales = new List<IBoton>();

            List<List<IBoton>> tecladoFijoCategorias = new List<List<IBoton>>()
            {
                new List<IBoton>() {TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo}

            };
            List<List<IBoton>> tecladoFijoMaterial = new List<List<IBoton>>()
            {
                new List<IBoton>() {TelegramBot.Instancia.BotonAnterior, TelegramBot.Instancia.BotonSiguiente},
                new List<IBoton>() {TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo}

            };
            List<List<IBoton>> tecladoFijoAvanzar = new List<List<IBoton>>()
            {
                new List<IBoton>() {TelegramBot.Instancia.BotonCancelar,TelegramBot.Instancia.BotonAvanzar, TelegramBot.Instancia.BotonFinalizar}

            };
            List<List<IBoton>> tecladoFijoAgregar = new List<List<IBoton>>()
            {
                new List<IBoton>() {TelegramBot.Instancia.BotonCancelar,TelegramBot.Instancia.BotonAgregar, TelegramBot.Instancia.BotonFinalizar}

            };
            List<List<IBoton>> tecladoFijoCancelar = new List<List<IBoton>>()
            {
                new List<IBoton>() {TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonFinalizar}

            };
            List<List<IBoton>> tecladoFijoSoloCancelar = new List<List<IBoton>>()
            {
                new List<IBoton>() {TelegramBot.Instancia.BotonCancelar}

            };

            List<List<IBoton>> tecladoFijoAgregarOtro = new List<List<IBoton>>()
            {
                new List<IBoton>() {TelegramBot.Instancia.BotonCancelar,TelegramBot.Instancia.BotonAgregarOtro, TelegramBot.Instancia.BotonFinalizar}

            };



            foreach (string opcion in infoPostulacion.DatosMaterialesDisponible)
            {
                botonesDeMateriales.Add(new Boton(opcion));
            }

            foreach (string opcion in infoPostulacion.DatosOfertaDisponibles)
            {
                botonesDeOferta.Add(new Boton(opcion));
            }

            foreach (string opcion in infoPostulacion.DatosProductoDisponible)
            {
                botonesDeProducto.Add(new Boton(opcion));
            }

            foreach (string opcion in infoPostulacion.DatosHabilitacionesDisponible)
            {
                botonesDeHabilitaciones.Add(new Boton(opcion));
            }

            foreach (string opcion in infoPostulacion.DatosEtiquetasDisponible)
            {
                botonesDeEtiquetas.Add(new Boton(opcion));
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
                    infoPostulacion.IndiceActual = 0;
                    respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeOferta, infoPostulacion.IndiceActual, FILAS_OFERTA, COLUMNAS_OFERTA, tecladoFijoSoloCancelar);
                    infoPostulacion.Estado = Estados.DatosOferta;
                    foreach (string nombreBoton in opcionesOferta)
                    {
                        if (mensaje.Texto == nombreBoton)
                        {
                            infoPostulacion.tipoMensaje = TipoMensaje.Callback;

                        }
                    }
                    StringBuilder st = new StringBuilder();
                    st.Append("############   REGISTRO OFERTA   ############");
                    st.Append("\nBien, ahora necesitamos que selecciones los datos que quiere ir ingresando.\n\nPresione el boton referido al dato que desea ingresar y escriba el dato en el chat para que lo tomemos. \n\n\nSelecciona \"Listo\" cuando quieras continuar el registro, o \"Cancelar\" para detenerlo.");
                    respuesta.Texto = st.ToString();
                    return true;



                case Estados.DatosOferta:
                    //Deteccion de tipo de mensaje en base a si el mensaje de entrada es igual a algún tipo de boton
                    foreach (string nombreBoton in opcionesOferta)
                    {
                        if ((mensaje.Texto == nombreBoton) ^ (mensaje.Texto == "Listo") ^ (mensaje.Texto == "Cancelar") ^ (mensaje.Texto == "Avanzar"))
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
                                    StringBuilder s3 = new StringBuilder();
                                    foreach (var item in DiccDatosOferta)
                                    {
                                        s3.Append("\n" + item.Key + ":   " + item.Value);
                                    }
                                    respuesta.Texto = this.stringBuilder.ToString();
                                    infoPostulacion.Estado = Estados.IntermedioOferta;
                                    s3.Append("\n\nPresione  \"Avanzar\" para ingresar el material de su oferta o \"Finalizar\" para crear la oferta");

                                    respuesta.Texto = s3.ToString();
                                    respuesta.Botones = this.ObtenerMatrizDeBotones(null, infoPostulacion.IndiceActual, FILAS_PRODUCTO, COLUMNAS_PRODUCTO, tecladoFijoAvanzar);
                                    return true;
                                case "Cancelar":

                                    return false;

                            }
                            if (mensaje.Texto == "Fecha Cierre")
                            {
                                respuesta.Texto = $"A continuacion se habilito el campo _\"{mensaje.Texto}\"_ para su ingreso, debido a que se trata de una fecha es necesario que se ingrese en el siguiente formato:\n\n *DD-MM-YYYY*";
                            }
                            else
                            {
                                respuesta.Texto = $"A continuacion se habilito el campo _\"{mensaje.Texto}\"_ para su ingreso.\n\n";
                            }
                            this.accionPrevia = mensaje.Texto;
                            if (this.DiccDatosOferta.Count >= 3)
                            {
                                respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeOferta, infoPostulacion.IndiceActual, FILAS_OFERTA, COLUMNAS_OFERTA, tecladoFijoCategorias);
                            }
                            else
                            {
                                respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeOferta, infoPostulacion.IndiceActual, FILAS_OFERTA, COLUMNAS_OFERTA, tecladoFijoSoloCancelar);
                            }
                            respuesta.EditarMensaje = true;
                            return true;



                        case TipoMensaje.Mensaje:
                            Console.WriteLine("ESTADO: " + infoPostulacion.tipoMensaje);
                            respuesta.Texto = $"Se ingresó el dato _\"{mensaje.Texto}\"_ en el campo *{this.accionPrevia}*";
                            DiccDatosOferta[accionPrevia] = mensaje.Texto;

                            respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeOferta, infoPostulacion.IndiceActual, FILAS_OFERTA, COLUMNAS_OFERTA, tecladoFijoCategorias);
                            return true;
                    }
                    return true;

                case Estados.IntermedioOferta:

                    //Deteccion de tipo de mensaje en base a si el mensaje de entrada es igual a algún tipo de boton
                    foreach (string nombreBoton in opcionesProducto)
                    {
                        if ((mensaje.Texto == nombreBoton) ^ (mensaje.Texto == "Listo") ^ (mensaje.Texto == "Cancelar") ^ (mensaje.Texto == "Avanzar"))
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
                                case "Avanzar":

                                    respuesta.Texto = "Ingresando datos de los productos";
                                    infoPostulacion.Estado = Estados.DatosMaterial;

                                    respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeMateriales, infoPostulacion.IndiceActual, FILAS_MATERIAL, COLUMNAS_MATERIAL, tecladoFijoMaterial);
                                    respuesta.EditarMensaje = true;

                                    return true;
                                case "Cancelar":
                                    return false;
                            }
                            return true;



                        case TipoMensaje.Mensaje:
                            return true;
                    }
                    return true;


                case Estados.DatosMaterial:
                    foreach (var nombreBoton in Sistema.Instancia.Materiales)
                    {
                        Console.WriteLine("a: " + nombreBoton.Nombre);
                        if ((mensaje.Texto == nombreBoton.Nombre) ^ (mensaje.Texto == "Siguiente") ^ (mensaje.Texto == "Anterior") ^ (mensaje.Texto == "Listo") ^ (mensaje.Texto == "Cancelar") ^ (mensaje.Texto == "Avanzar"))
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
                            switch (mensaje.Texto)
                            {

                                case "Siguiente":
                                    if (botonesDeMateriales.Count <= infoPostulacion.IndiceActual + FILAS_MATERIAL * COLUMNAS_MATERIAL)
                                    {
                                        infoPostulacion.IndiceActual = botonesDeMateriales.Count - FILAS_MATERIAL * COLUMNAS_MATERIAL;
                                    }
                                    else
                                    {
                                        infoPostulacion.IndiceActual += FILAS_MATERIAL * COLUMNAS_MATERIAL;
                                    }
                                    respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeMateriales, infoPostulacion.IndiceActual, FILAS_MATERIAL, COLUMNAS_MATERIAL, tecladoFijoMaterial);
                                    respuesta.Texto = String.Empty;
                                    respuesta.EditarMensaje = true;
                                    return true;

                                case "Anterior":
                                    if (infoPostulacion.IndiceActual - FILAS_MATERIAL * COLUMNAS_MATERIAL < 0)
                                    {
                                        infoPostulacion.IndiceActual = 0;
                                    }
                                    else
                                    {
                                        infoPostulacion.IndiceActual -= FILAS_MATERIAL * COLUMNAS_MATERIAL;
                                    }
                                    respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeMateriales, infoPostulacion.IndiceActual, FILAS_MATERIAL, COLUMNAS_MATERIAL, tecladoFijoMaterial);
                                    respuesta.Texto = String.Empty;
                                    respuesta.EditarMensaje = true;
                                    return true;

                                case "Listo":
                                    this.S4.Clear();
                                    this.S4.Append("Datos sobre la oferta:\n");

                                    foreach (var item in DiccDatosOferta)
                                    {
                                        this.S4.Append("\n" + item.Key + ":   " + item.Value);
                                    }
                                    this.S4.Append("\n\nMaterial seleccionado: " + this.material.Nombre);

                                    respuesta.Texto = this.stringBuilder.ToString();
                                    this.S4.Append("\n\nPresione  \"Avanzar\" para continuar con los datos de la oferta o \"Finalizar\" para crear la oferta");
                                    respuesta.EditarMensaje = true;
                                    respuesta.Texto = this.S4.ToString();
                                    respuesta.Botones = this.ObtenerMatrizDeBotones(null, infoPostulacion.IndiceActual, FILAS_PRODUCTO, COLUMNAS_PRODUCTO, tecladoFijoAvanzar);
                                    respuesta.EditarMensaje = true;

                                    try
                                    {



                                    }
                                    catch (IndexOutOfRangeException e)
                                    {

                                    }
                                    return true;


                                case "Avanzar":

                                    infoPostulacion.Estado = Estados.DatosProducto;
                                    respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeProducto, infoPostulacion.IndiceActual, FILAS_PRODUCTO, COLUMNAS_PRODUCTO, tecladoFijoSoloCancelar);
                                    respuesta.EditarMensaje = true;
                                    return true;

                                case "Cancelar":
                                    return false;


                            }


                            this.material = Sistema.Instancia.ObtenerMaterialPorNombre(mensaje.Texto);
                            respuesta.EditarMensaje = true;

                            respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeMateriales, infoPostulacion.IndiceActual, FILAS_MATERIAL, COLUMNAS_MATERIAL, tecladoFijoMaterial);
                            respuesta.Texto = $"Usted selecciono el material _\"{mensaje.Texto}\"_\n\nPresione  \"Listo\" para ingresar el material o  \"Finalizar\" para crear la oferta";
                            return true;
                    }

                    return true;

                case Estados.DatosProducto:
                    //Deteccion de tipo de mensaje en base a si el mensaje de entrada es igual a algún tipo de boton
                    foreach (string nombreBoton in opcionesProducto)
                    {
                        if ((mensaje.Texto == nombreBoton) ^ (mensaje.Texto == "Listo") ^ (mensaje.Texto == "Cancelar") ^ (mensaje.Texto == "Avanzar") ^ (mensaje.Texto == "Finalizar") ^ (mensaje.Texto == "Agregar") ^ (mensaje.Texto == "Agregar Otro"))
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
                                case "Agregar Otro":
                                    infoPostulacion.Estado = Estados.DatosMaterial;
                                    respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeMateriales, infoPostulacion.IndiceActual, FILAS_MATERIAL, COLUMNAS_MATERIAL, tecladoFijoMaterial);
                                    respuesta.EditarMensaje = true;
                                    return true;


                                case "Finalizar":

                                    this.oferta = new Oferta("1", null, Convert.ToDateTime(this.DiccDatosOferta["Fecha Cierre"]), null, null, this.DiccDatosOferta["Descripcion"], this.DiccDatosOferta["Titulo"], false);
                                    foreach (Producto p in productos)
                                    {
                                        this.oferta.AgregarProducto(p.Material, p.Ubicacion.Ciudad, p.Ubicacion.Direccion, p.CantidadEnUnidad, p.ValorUYU, p.ValorUSD);
                                        Console.WriteLine("Titulo Oferta: " + this.oferta.Titulo);
                                    }
                                    respuesta.Texto = "A continuacion se ingresaran las habilitaciones de la oferta. \nPresione  \"Listo\" para avanzar \"Cancelar\" no ingresar habilitaciones.";
                                    infoPostulacion.Estado = Estados.DatosHabilitacion;
                                    respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeHabilitaciones, infoPostulacion.IndiceActual, FILAS_PRODUCTO, COLUMNAS_PRODUCTO, tecladoFijoSoloCancelar);

                                    return true;

                                case "Agregar":
                                    if (this.yaMostrado == false)
                                    {
                                        this.stringBuilder.Append("Datos sobre la Oferta");

                                        foreach (var item in DiccDatosOferta)
                                        {
                                            this.stringBuilder.Append("\n" + item.Key + ":   " + item.Value);
                                        }
                                        this.yaMostrado = true;
                                        this.stringBuilder.Append("\n\n\n");
                                    }

                                    this.stringBuilder.Append("Datos sobre el producto:");
                                    this.stringBuilder.Append("\n");
                                    this.stringBuilder.Append("\nMaterial Producto: " + this.material.Nombre);
                                    foreach (var item in DiccDatosProducto)
                                    {
                                        this.stringBuilder.Append("\n" + item.Key + ":   " + item.Value);
                                    }
                                    this.stringBuilder.Append("\n\n");

                                    respuesta.Texto = this.stringBuilder.ToString();
                                    infoPostulacion.Estado = Estados.DatosProducto;
                                    this.productos.Add(new Producto(material, DiccDatosProducto["Ciudad"], DiccDatosProducto["Direccion"], Convert.ToDouble(DiccDatosProducto["Cantidad en unidades"]), Convert.ToDouble(DiccDatosProducto["Valor en UYU"]), Convert.ToDouble(DiccDatosProducto["Valor en USD"])));
                                    this.DiccDatosProducto.Clear();
                                    respuesta.Texto = this.stringBuilder.ToString();
                                    respuesta.Botones = this.ObtenerMatrizDeBotones(null, infoPostulacion.IndiceActual, FILAS_PRODUCTO, COLUMNAS_PRODUCTO, tecladoFijoAgregarOtro);
                                    respuesta.EditarMensaje = true;

                                    return true;

                                case "Cancelar":
                                    if (this.yaMostrado == false)
                                    {
                                        this.stringBuilder.Append("Datos sobre la Oferta");

                                        foreach (var item in DiccDatosOferta)
                                        {
                                            this.stringBuilder.Append("\n" + item.Key + ":   " + item.Value);
                                        }
                                        this.yaMostrado = true;
                                    }

                                    this.stringBuilder.Append("\n\n\n");
                                    this.stringBuilder.Append("Datos sobre el producto:");
                                    this.stringBuilder.Append("\n");
                                    this.stringBuilder.Append("\nMaterial Producto: " + this.material.Nombre);
                                    foreach (var item in DiccDatosProducto)
                                    {
                                        this.stringBuilder.Append("\n" + item.Key + ":   " + item.Value);
                                    }
                                    this.stringBuilder.Append("\n\n");

                                    respuesta.Texto = this.stringBuilder.ToString();
                                    infoPostulacion.Estado = Estados.DatosProducto;
                                    this.productos.Add(new Producto(material, DiccDatosProducto["Ciudad"], DiccDatosProducto["Direccion"], Convert.ToDouble(DiccDatosProducto["Cantidad en unidades"]), Convert.ToDouble(DiccDatosProducto["Valor en UYU"]), Convert.ToDouble(DiccDatosProducto["Valor en USD"])));
                                    this.DiccDatosProducto.Clear();
                                    respuesta.Texto = this.stringBuilder.ToString();
                                    respuesta.Botones = this.ObtenerMatrizDeBotones(null, infoPostulacion.IndiceActual, FILAS_PRODUCTO, COLUMNAS_PRODUCTO, tecladoFijoAgregarOtro);
                                    respuesta.EditarMensaje = true;

                                    return true;
                            }


                            respuesta.Texto = $"A continuacion se habilito el campo _\"{mensaje.Texto}\"_ para su ingreso.\n\n";

                            this.accionPrevia = mensaje.Texto;

                            if (this.DiccDatosProducto.Count >= 5)
                            {
                                respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeProducto, infoPostulacion.IndiceActual, FILAS_PRODUCTO, COLUMNAS_PRODUCTO, tecladoFijoAgregar);
                            }
                            else
                            {
                                respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeProducto, infoPostulacion.IndiceActual, FILAS_PRODUCTO, COLUMNAS_PRODUCTO, tecladoFijoSoloCancelar);
                            }
                            respuesta.EditarMensaje = true;

                            return true;



                        case TipoMensaje.Mensaje:
                            Console.WriteLine("ESTADO: " + infoPostulacion.tipoMensaje);
                            respuesta.Texto = $"Se ingresó el dato _\"{mensaje.Texto}\"_ en el campo *{this.accionPrevia}*";
                            DiccDatosProducto[accionPrevia] = mensaje.Texto;


                            if (this.DiccDatosProducto.Count >= 5)
                            {
                                respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeProducto, infoPostulacion.IndiceActual, FILAS_PRODUCTO, COLUMNAS_PRODUCTO, tecladoFijoAgregar);
                            }
                            else
                            {
                                respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeProducto, infoPostulacion.IndiceActual, FILAS_PRODUCTO, COLUMNAS_PRODUCTO, tecladoFijoCancelar);
                            }
                            return true;
                    }
                    return true;




                case Estados.DatosHabilitacion:

                    //Deteccion de tipo de mensaje en base a si el mensaje de entrada es igual a algún tipo de boton
                    foreach (string nombreBoton in opcionesHabilitacion)
                    {
                        if ((mensaje.Texto == nombreBoton) ^ (mensaje.Texto == "Listo") ^ (mensaje.Texto == "Cancelar") ^ (mensaje.Texto == "Agregar Otro") ^ (mensaje.Texto == "Finalizar"))
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
                                    this.stringBuilder.Append("\n\n\nDatos sobre tu habilitacion: \n");

                                    foreach (var item in DiccDatosHabilitacion)
                                    {
                                        this.stringBuilder.Append("\n" + item.Key + ":   " + item.Value);
                                    }

                                    List<Habilitacion> habilitaciones = new List<Habilitacion>();
                                    habilitaciones.Add(new Habilitacion(this.DiccDatosHabilitacion["Nombre"], this.DiccDatosHabilitacion["Descripcion"], this.DiccDatosHabilitacion["Nombre Insitucion Habilitada"], Convert.ToDateTime(null), Convert.ToDateTime(null), true));

                                    this.oferta.AgregarHabilitacion(this.DiccDatosHabilitacion["Nombre"], this.DiccDatosHabilitacion["Descripcion"], this.DiccDatosHabilitacion["Nombre Insitucion Habilitada"]);
                                    respuesta.Botones = this.ObtenerMatrizDeBotones(null, infoPostulacion.IndiceActual, FILAS_OFERTA, COLUMNAS_OFERTA, tecladoFijoAgregarOtro);
                                    this.DiccDatosHabilitacion.Clear();
                                    respuesta.Texto = this.stringBuilder.ToString();
                                    return true;


                                case "Cancelar":
                                    respuesta.Texto = "Felicidades, su oferta se ingresó correctamente!!";
                                    return true;
                                case "Finalizar":

                                    respuesta.Texto = "Felicidades, su oferta se ingresó correctamente!!";

                                    return false;
                            }
                            respuesta.Texto = $"A continuacion se habilito el campo _\"{mensaje.Texto}\"_ para su ingreso.\n\n";
                            this.accionPrevia = mensaje.Texto;


                            if (this.DiccDatosHabilitacion.Count >= 3)
                            {
                                respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeHabilitaciones, infoPostulacion.IndiceActual, FILAS_PRODUCTO, COLUMNAS_PRODUCTO, tecladoFijoCancelar);
                            }
                            else
                            {
                                respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeHabilitaciones, infoPostulacion.IndiceActual, FILAS_PRODUCTO, COLUMNAS_PRODUCTO, tecladoFijoSoloCancelar);
                            }
                            respuesta.EditarMensaje = true;
                            return true;



                        case TipoMensaje.Mensaje:
                            Console.WriteLine("ESTADO: " + infoPostulacion.tipoMensaje);
                            respuesta.Texto = $"Se ingresó el dato _\"{mensaje.Texto}\"_ en el campo *{this.accionPrevia}*";
                            DiccDatosHabilitacion[accionPrevia] = mensaje.Texto;

                            if (this.DiccDatosHabilitacion.Count >= 3)
                            {
                                respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeHabilitaciones, infoPostulacion.IndiceActual, FILAS_PRODUCTO, COLUMNAS_PRODUCTO, tecladoFijoCategorias);
                            }
                            else
                            {
                                respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeHabilitaciones, infoPostulacion.IndiceActual, FILAS_PRODUCTO, COLUMNAS_PRODUCTO, tecladoFijoSoloCancelar);
                            }

                            return true;
                    }


                    return true;


            }
            infoPostulacion = new InformacionPostulacion();
            return false;
        }

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
            DatosOferta,
            IntermedioOferta,
            DatosMaterial,
            IntermedioMaterial,
            DatosProducto,
            IntermedioProducto,
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

            public List<string> DatosOfertaDisponibles { get; set; }
            public List<string> DatosProductoDisponible { get; set; }
            public List<string> DatosHabilitacionesDisponible { get; set; }
            public List<string> DatosEtiquetasDisponible { get; set; }
            public List<string> DatosMaterialesDisponible { get; set; }




        }
    }
}