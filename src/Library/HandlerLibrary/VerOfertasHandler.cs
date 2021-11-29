using System;
using System.Collections.Generic;
using System.Text;
using PII_E13.ClassLibrary;

namespace PII_E13.HandlerLibrary
{
    /// <summary>
    /// Handler encargado de procesar la postulación de ofertas, desde el proceso de búsqueda hasta la selección final.
    /// </summary>
    public class VerOfertasHandler : HandlerBase
    {
        private const int COLUMNAS_EMPRENDEDORES = 1;
        private const int FILAS_EMPRENDEDORES = 3;
        private const int COLUMNAS_OFERTAS = 1;
        private const int FILAS_OFERTAS = 3;

        /// <summary>
        /// Diccionario utilizado para contener los estados de las sesiones que se encuentran en este handler.
        /// </summary>
        private Dictionary<Sesion, Estados> EstadoSesion { get; set; } = new Dictionary<Sesion, Estados>();

        /// <summary>
        /// Diccionario utilizado para contener las ofertas seleccionadas de las sesiones que se encuentran en este handler.
        /// </summary>
        private Dictionary<Sesion, Oferta> OfertaSeleccionada { get; set; } = new Dictionary<Sesion, Oferta>();

        /// <summary>
        /// Diccionario utilizado para contener los indices de las listas siendo recorridas por las sesiones que se encuentran en este handler.
        /// </summary>
        private Dictionary<Sesion, int> IndiceActual { get; set; } = new Dictionary<Sesion, int>();

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PostularseAOfertaHandler"/>. 
        /// Esta clase procesa la postulación a una oferta.
        /// </summary>
        /// <param name="siguiente">El próximo "handler".</param>
        public VerOfertasHandler(HandlerBase siguiente) : base(siguiente)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PostularseAOfertaHandler"/>. 
        /// </summary>
        /// <param name="siguiente">El próximo "handler".</param>
        /// <param name="intencion">La intención utilizada para identificar a este handler.</param>
        public VerOfertasHandler(HandlerBase siguiente, string intencion) : base(siguiente, intencion)
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
                this.Cancelar(sesion);
                return false;
            }

            if (!this.EstadoSesion.ContainsKey(sesion))
            {
                this.EstadoSesion.Add(sesion, Estados.ViendoOfertas);
            }

            List<List<IBoton>> tecladoFijoVerOfertas = new List<List<IBoton>>() {
                new List<IBoton>() {TelegramBot.Instancia.BotonAnterior, TelegramBot.Instancia.BotonSiguiente},
                new List<IBoton>() {new Boton("Salir")}
            };

            List<List<IBoton>> tecladoFijoVerEmprendedores = new List<List<IBoton>>() {
                new List<IBoton>() {TelegramBot.Instancia.BotonAnterior, TelegramBot.Instancia.BotonSiguiente},
                new List<IBoton>() {new Boton("Salir")}
            };

            StringBuilder stringBuilder = new StringBuilder();

            bool esEmpresa;
            try
            {
                Sistema.Instancia.ObtenerEmpresaPorId(sesion.IdUsuario);
                esEmpresa = true;
            }
            catch (KeyNotFoundException)
            {
                esEmpresa = false;
            }

            List<Oferta> ofertas = new List<Oferta>();
            if (esEmpresa)
            {
                ofertas = Sistema.Instancia.ObtenerEmpresaPorId(sesion.IdUsuario).Ofertas;
            }
            else
            {
                foreach (string id in Sistema.Instancia.ObtenerEmprendedorPorId(sesion.IdUsuario).OfertasPostuladas)
                {
                    ofertas.Add(Sistema.Instancia.ObtenerOfertaPorId(id));
                }
            }

            List<IBoton> botonesOfertas = new List<IBoton>();
            foreach (Oferta oferta in ofertas)
            {
                botonesOfertas.Add(new Boton(oferta.Titulo.Replace("*", ""), oferta.Id));
            }

            List<IBoton> botonesEmprendedores = new List<IBoton>();

            if (this.OfertaSeleccionada.ContainsKey(sesion))
            {
                foreach (string id in this.OfertaSeleccionada[sesion].EmprendedoresPostulados)
                {
                    Emprendedor emprendedor = Sistema.Instancia.ObtenerEmprendedorPorId(id);
                    botonesEmprendedores.Add(new Boton(emprendedor.Nombre, emprendedor.Id));
                }
            }


            switch (this.EstadoSesion[sesion])
            {
                case Estados.ViendoOfertas:

                    if (!this.IndiceActual.ContainsKey(sesion))
                    {
                        this.IndiceActual.Add(sesion, 0);
                    }

                    switch (mensaje.Texto)
                    {
                        case "Siguiente":
                            if (botonesOfertas.Count <= this.IndiceActual[sesion] + COLUMNAS_OFERTAS * FILAS_OFERTAS)
                            {
                                this.IndiceActual[sesion] = (botonesOfertas.Count - COLUMNAS_OFERTAS * FILAS_OFERTAS) > 0 ? (botonesOfertas.Count - COLUMNAS_OFERTAS * FILAS_OFERTAS) : 0;
                            }
                            else
                            {
                                this.IndiceActual[sesion] += COLUMNAS_OFERTAS * FILAS_OFERTAS;
                            }

                            stringBuilder = new StringBuilder();
                            if (esEmpresa)
                            {
                                stringBuilder.Append("*Estas son tus ofertas publicadas:*");
                            }
                            else
                            {
                                stringBuilder.Append("*Estas son las ofertas a las que te has postulado:*");
                            }
                            for (int i = this.IndiceActual[sesion]; i < (this.IndiceActual[sesion] + FILAS_OFERTAS * COLUMNAS_OFERTAS); i++)
                            {
                                try
                                {
                                    Oferta oferta = ofertas[i];
                                    stringBuilder.Append($"\n\n{oferta.RedactarResumen()}");
                                }
                                catch (ArgumentOutOfRangeException e)
                                {
                                    break;
                                }
                            }

                            respuesta.EditarMensaje = true;
                            respuesta.Botones = this.ObtenerMatrizDeBotones(botonesOfertas, this.IndiceActual[sesion], COLUMNAS_OFERTAS, FILAS_OFERTAS, tecladoFijoVerOfertas);
                            respuesta.Texto = stringBuilder.ToString();
                            return true;

                        case "Anterior":
                            if (this.IndiceActual[sesion] - COLUMNAS_OFERTAS * FILAS_OFERTAS < 0)
                            {
                                this.IndiceActual[sesion] = 0;
                            }
                            else
                            {
                                this.IndiceActual[sesion] -= COLUMNAS_OFERTAS * FILAS_OFERTAS;
                            }

                            stringBuilder = new StringBuilder();
                            if (esEmpresa)
                            {
                                stringBuilder.Append("*Estas son tus ofertas publicadas:*");
                            }
                            else
                            {
                                stringBuilder.Append("*Estas son las ofertas a las que te has postulado:*");
                            }
                            for (int i = this.IndiceActual[sesion]; i < (this.IndiceActual[sesion] + FILAS_OFERTAS * COLUMNAS_OFERTAS); i++)
                            {
                                try
                                {
                                    Oferta oferta = ofertas[i];
                                    stringBuilder.Append($"\n\n{oferta.RedactarResumen()}");
                                }
                                catch (ArgumentOutOfRangeException e)
                                {
                                    break;
                                }
                            }

                            respuesta.EditarMensaje = true;
                            respuesta.Botones = this.ObtenerMatrizDeBotones(botonesOfertas, this.IndiceActual[sesion], COLUMNAS_OFERTAS, FILAS_OFERTAS, tecladoFijoVerOfertas);
                            respuesta.Texto = stringBuilder.ToString();
                            return true;

                        case "Salir":
                            this.Cancelar(sesion);
                            return false;
                    }

                    if (ofertas.Exists(of => of.Id == mensaje.Texto))
                    {
                        if (this.OfertaSeleccionada.ContainsKey(sesion))
                        {
                            this.OfertaSeleccionada[sesion] = ofertas.Find(of => of.Id == mensaje.Texto);
                        }
                        else
                        {
                            this.OfertaSeleccionada.Add(sesion, ofertas.Find(of => of.Id == mensaje.Texto));
                        }

                        respuesta.Texto = this.OfertaSeleccionada[sesion].Redactar();
                        IBoton botonPostulantes = new Boton("Ver postulantes", "Postulantes");
                        IBoton botonVolver = new Boton("Ver otras ofertas", "Volver");
                        respuesta.Botones = new List<List<IBoton>>();
                        if (esEmpresa)
                        {
                            respuesta.Botones.Add(new List<IBoton>() { botonPostulantes });
                        }
                        respuesta.Botones.Add(new List<IBoton>() { botonVolver });

                        this.EstadoSesion[sesion] = Estados.DetalleOferta;
                        return true;
                    }

                    stringBuilder = new StringBuilder();
                    if (esEmpresa)
                    {
                        stringBuilder.Append("*Estas son tus ofertas publicadas:*");
                    }
                    else
                    {
                        stringBuilder.Append("*Estas son las ofertas a las que te has postulado:*");
                    }

                    for (int i = this.IndiceActual[sesion]; i < (this.IndiceActual[sesion] + FILAS_OFERTAS * COLUMNAS_OFERTAS); i++)
                    {
                        try
                        {
                            Oferta oferta = ofertas[i];
                            stringBuilder.Append($"\n\n{oferta.RedactarResumen()}");
                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            break;
                        }
                    }
                    respuesta.Texto = stringBuilder.ToString();
                    respuesta.Botones = this.ObtenerMatrizDeBotones(botonesOfertas, IndiceActual[sesion], FILAS_OFERTAS, COLUMNAS_EMPRENDEDORES, tecladoFijoVerOfertas);
                    return true;

                case Estados.DetalleOferta:
                    if (mensaje.Texto.Equals("Volver"))
                    {
                        stringBuilder = new StringBuilder();
                        if (esEmpresa)
                        {
                            stringBuilder.Append("*Estas son tus ofertas publicadas:*");
                        }
                        else
                        {
                            stringBuilder.Append("*Estas son las ofertas a las que te has postulado:*");
                        }

                        for (int i = this.IndiceActual[sesion]; i < (this.IndiceActual[sesion] + FILAS_OFERTAS * COLUMNAS_OFERTAS); i++)
                        {
                            try
                            {
                                Oferta oferta = ofertas[i];
                                stringBuilder.Append($"\n\n{oferta.RedactarResumen()}");
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                break;
                            }
                        }
                        respuesta.Texto = stringBuilder.ToString();
                        respuesta.Botones = this.ObtenerMatrizDeBotones(botonesOfertas, IndiceActual[sesion], FILAS_OFERTAS, COLUMNAS_OFERTAS, tecladoFijoVerOfertas);
                        this.EstadoSesion[sesion] = Estados.ViendoOfertas;
                        return true;
                    }
                    else if (mensaje.Texto.Equals("Postulantes") && esEmpresa)
                    {
                        this.IndiceActual[sesion] = 0;

                        for (int i = this.IndiceActual[sesion]; i < (this.IndiceActual[sesion] + FILAS_EMPRENDEDORES * COLUMNAS_EMPRENDEDORES); i++)
                        {
                            try
                            {
                                Emprendedor emprendedor = Sistema.Instancia.ObtenerEmprendedorPorId(this.OfertaSeleccionada[sesion].EmprendedoresPostulados[i]);
                                stringBuilder.Append($"\n\n • {emprendedor.RedactarResumen()}");
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                break;
                            }
                        }

                        respuesta.Texto = stringBuilder.ToString();
                        respuesta.Botones = this.ObtenerMatrizDeBotones(botonesEmprendedores, IndiceActual[sesion], FILAS_EMPRENDEDORES, COLUMNAS_EMPRENDEDORES, tecladoFijoVerEmprendedores);
                        this.EstadoSesion[sesion] = Estados.ViendoEmprendedores;
                        return true;
                    }

                    this.Cancelar(sesion);
                    return false;

                case Estados.ViendoEmprendedores:
                    switch (mensaje.Texto)
                    {
                        case "Siguiente":
                            if (botonesEmprendedores.Count <= this.IndiceActual[sesion] + COLUMNAS_EMPRENDEDORES * FILAS_EMPRENDEDORES)
                            {
                                this.IndiceActual[sesion] = (botonesEmprendedores.Count - COLUMNAS_EMPRENDEDORES * FILAS_EMPRENDEDORES) > 0 ? (botonesEmprendedores.Count - COLUMNAS_EMPRENDEDORES * FILAS_EMPRENDEDORES) : 0;
                            }
                            else
                            {
                                this.IndiceActual[sesion] += COLUMNAS_EMPRENDEDORES * FILAS_EMPRENDEDORES;
                            }

                            stringBuilder = new StringBuilder();
                            for (int i = this.IndiceActual[sesion]; i < (this.IndiceActual[sesion] + FILAS_EMPRENDEDORES * COLUMNAS_EMPRENDEDORES); i++)
                            {
                                try
                                {
                                    Emprendedor emprendedor = Sistema.Instancia.ObtenerEmprendedorPorId(this.OfertaSeleccionada[sesion].EmprendedoresPostulados[i]);
                                    stringBuilder.Append($"\n\n • {emprendedor.RedactarResumen()}");
                                }
                                catch (ArgumentOutOfRangeException e)
                                {
                                    break;
                                }
                            }

                            respuesta.Botones = this.ObtenerMatrizDeBotones(botonesEmprendedores, this.IndiceActual[sesion], COLUMNAS_EMPRENDEDORES, FILAS_EMPRENDEDORES, tecladoFijoVerEmprendedores);
                            respuesta.Texto = stringBuilder.ToString();
                            respuesta.EditarMensaje = true;
                            return true;

                        case "Anterior":
                            if (this.IndiceActual[sesion] - COLUMNAS_EMPRENDEDORES * FILAS_EMPRENDEDORES < 0)
                            {
                                this.IndiceActual[sesion] = 0;
                            }
                            else
                            {
                                this.IndiceActual[sesion] -= COLUMNAS_EMPRENDEDORES * FILAS_EMPRENDEDORES;
                            }

                            stringBuilder = new StringBuilder();
                            for (int i = this.IndiceActual[sesion]; i < (this.IndiceActual[sesion] + FILAS_EMPRENDEDORES * COLUMNAS_EMPRENDEDORES); i++)
                            {
                                try
                                {
                                    Emprendedor emprendedor = Sistema.Instancia.ObtenerEmprendedorPorId(this.OfertaSeleccionada[sesion].EmprendedoresPostulados[i]);
                                    stringBuilder.Append($"\n\n • {emprendedor.RedactarResumen()}");
                                }
                                catch (ArgumentOutOfRangeException e)
                                {
                                    break;
                                }
                            }

                            respuesta.Botones = this.ObtenerMatrizDeBotones(botonesEmprendedores, this.IndiceActual[sesion], COLUMNAS_EMPRENDEDORES, FILAS_EMPRENDEDORES, tecladoFijoVerEmprendedores);
                            respuesta.Texto = stringBuilder.ToString();
                            respuesta.EditarMensaje = true;
                            return true;

                        case "Salir":
                            this.Cancelar(sesion);
                            return false;
                    }

                    if (this.OfertaSeleccionada[sesion].EmprendedoresPostulados.Exists(idEmprendedor => idEmprendedor.Equals(mensaje.Texto)))
                    {
                        Emprendedor emprendedor = Sistema.Instancia.ObtenerEmprendedorPorId(mensaje.Texto);
                        IBoton botonConsumir = new Boton("Asignar oferta", emprendedor.Id);
                        IBoton botonVolver = new Boton("Volver", "Volver");
                        respuesta.Texto = emprendedor.Redactar();
                        respuesta.Botones = new List<List<IBoton>>()
                        {
                            new List<IBoton>() { botonConsumir },
                            new List<IBoton>() { botonVolver }
                        };
                        this.EstadoSesion[sesion] = Estados.DetalleEmprendedor;
                        return true;
                    }

                    this.Cancelar(sesion);
                    return false;

                case Estados.DetalleEmprendedor:
                    if (mensaje.Texto.Equals("Volver"))
                    {
                        for (int i = this.IndiceActual[sesion]; i < (this.IndiceActual[sesion] + FILAS_EMPRENDEDORES * COLUMNAS_EMPRENDEDORES); i++)
                        {
                            try
                            {
                                Emprendedor emprendedor = Sistema.Instancia.ObtenerEmprendedorPorId(this.OfertaSeleccionada[sesion].EmprendedoresPostulados[i]);
                                stringBuilder.Append($"\n\n • {emprendedor.RedactarResumen()}");
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                break;
                            }
                        }

                        respuesta.Texto = stringBuilder.ToString();
                        respuesta.Botones = this.ObtenerMatrizDeBotones(botonesEmprendedores, IndiceActual[sesion], FILAS_EMPRENDEDORES, COLUMNAS_EMPRENDEDORES, tecladoFijoVerEmprendedores);
                        this.EstadoSesion[sesion] = Estados.ViendoEmprendedores;
                        return true;
                    }
                    else if (this.OfertaSeleccionada[sesion].EmprendedoresPostulados.Exists(emp => emp.Equals(mensaje.Texto)))
                    {
                        this.OfertaSeleccionada[sesion].Consumir(mensaje.Texto);
                        Emprendedor emprendedor = Sistema.Instancia.ObtenerEmprendedorPorId(mensaje.Texto);
                        respuesta.Texto = $"¡Felicidades! La oferta oficialmente ha sido asignada a _{emprendedor.Nombre}_ exitosamente.";
                        respuesta.Botones = new List<List<IBoton>>()
                        {
                            new List<IBoton>() { new Boton("Volver al menú", "Menu") }
                        };
                        return true;
                    }
                    this.Cancelar(sesion);
                    return false;
            }
            this.Cancelar(sesion);
            return false;
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
        /// Retorna este "handler" al estado inicial.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        protected override void CancelarInterno(Sesion sesion)
        {
            try
            {
                this.EstadoSesion.Remove(sesion);
                this.IndiceActual.Remove(sesion);
                this.OfertaSeleccionada.Remove(sesion);
            }
            catch (Exception e) { }
        }

        /// <summary>
        /// Determina si este "handler" puede procesar el mensaje. En la clase base se utiliza procesado de lenguaje natural
        /// para comprobar que la intención identificada corresponda a la del "handler". Las clases sucesores pueden
        /// sobreescribir este método para proveer otro mecanismo para determina si procesan o no un mensaje.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        /// <returns>true si el mensaje puede ser pocesado; false en caso contrario.</returns>
        protected override bool PuedeResolver(Sesion sesion)
        {
            return sesion.PLN.UltimaIntencion.Nombre.Equals(this.Intencion) ||
                (
                    this.EstadoSesion.ContainsKey(sesion) &&
                    (sesion.PLN.UltimaIntencion.Nombre.Equals("Default") || (sesion.PLN.UltimaIntencion.ConfianzaDeteccion < 80))
                );
        }

        /// <summary>
        /// Representación de los posibles estados de la visualización de ofertas.
        /// </summary>
        private enum Estados
        {
            ViendoOfertas,
            DetalleOferta,
            ViendoEmprendedores,
            DetalleEmprendedor,
            OfertaConsumida
        }
    }
}