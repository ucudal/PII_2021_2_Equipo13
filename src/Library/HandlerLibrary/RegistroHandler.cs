using System;
using System.Collections.Generic;
using System.Linq;
using PII_E13.ClassLibrary;
using Telegram.Bot.Types.ReplyMarkups;

namespace PII_E13.HandlerLibrary
{
    public class RegistroHandler: HandlerBase
    {
        public IHandler Siguiente { get; set; }

        public string ID { get; set; }
        public string Roll {get;set;}

        public string respuesta{get;set;}
        
        public RegistroHandler(HandlerBase siguiente):base(siguiente)
        {
            this.Siguiente = siguiente;
        }
              
        protected virtual bool ResolverInterno(IMensaje mensaje, out string respuesta)
        {
            this.Roll = mensaje.Texto;
            this.ID =mensaje.IdUsuario;
            
            if (this.Roll=="Empresa")
            {
                respuesta="Escriba su nombre para el registro";
                string nombre = mensaje.Texto;
                respuesta="Escriba su ciudad";
                string ciudad = mensaje.Texto;
                respuesta="Escriba su direccion";
                string direccion = mensaje.Texto;
                respuesta="Escriba su rubro";
                string rubro   = mensaje.Texto;
                respuesta=($" Son estos datos correctos?:/n nombre: {nombre}/n ciudad: {ciudad}/n direccion: {direccion}/n rubro: {rubro}/n =====Si los siguientes datos son correctos presione le boton 1, si alguno de los datos NO es correcto precion el boton 2====== ");
                if (mensaje.Texto=="1")
                {
                    Sistema.Instancia.RegistrarEmpresa(ID,ciudad,direccion,rubro,nombre);
                    respuesta="Registro exitoso";
                    return true;
                }
                else
                {
                    respuesta="Registro fallido";
                    return false;
                }
                
               
            }

           else if(this.Roll=="Emprendedor")
           {
                List <Habilitacion> habilitaciones=new List<Habilitacion>();
                respuesta="Escriba su nombre para el registro";
                string nombre = mensaje.Texto;
                respuesta="Escriba su ciudad";
                string ciudad = mensaje.Texto;
                respuesta="Escriba su direccion";
                string direccion = mensaje.Texto;
                respuesta="Escriba su rubro";
                string rubro   = mensaje.Texto;
                respuesta=($" Son estos datos correctos?:/n nombre: {nombre}/n ciudad: {ciudad}/n direccion: {direccion}/n rubro: {rubro}/n =====Si los siguientes datos son correctos presione le boton 1, si alguno de los datos NO es correcto precion el boton 2====== ");
                if (mensaje.Texto=="1")
                { 
                    string loop="0";
                    respuesta="Escriba su habilitacion";
                    while(loop!="1")
                    {
                        respuesta="Escriba la descrimpcion de su habilitcion";
                        string descripcion = mensaje.Texto;
                        respuesta="Escriba el nombre de la  institucion habilitadora";
                        string nombreHabilitacion = mensaje.Texto;
                        respuesta="Escriba la fecha en la que realiso el tramite";
                        DateTime fecha_tramite = DateTime.Parse(mensaje.Texto);
                        respuesta="Escriba la fecha de vencimiento";
                        DateTime fecha_vencimiento = DateTime.Parse(mensaje.Texto);
                        respuesta="Si la habilitcion esta vijente precione el boton 1, si no es asi precione el boton 2";
                        bool vigente=false;
                        if (mensaje.Texto=="1")
                        {
                            vigente=true;
                        }
                        else
                        {
                            vigente=false;
                        }
                        Habilitacion habilitacion=new Habilitacion(descripcion,nombreHabilitacion,fecha_tramite,fecha_vencimiento,vigente);
                        habilitaciones.Add(habilitacion);
                        respuesta="Si desea agregar otra habilitacion precione el boton 1, si no es asi precione el boton 2";
                        loop=mensaje.Texto;
                        
                    }
                    Sistema.Instancia.RegistrarEmprendedor(ID,ciudad,direccion,rubro,nombre);
                    respuesta="Registro exitoso";
                    return true;
                }
                else
                {
                    respuesta="Registro fallido";
                    return false;
                }
                
           }
           else
           {
            respuesta="No se reconoce su rol";
            this.Cancelar();
            return false;
              
           }
        }
        
        public IHandler Resolver(IMensaje mensaje, out string respuesta)
        {
            if (this.ResolverInterno(mensaje, out respuesta))
            {
                return this;
            }
            else if (this.Siguiente != null)
            {
                return this.Siguiente.Resolver(mensaje, out respuesta);
            }
            else
            {
                return null;
            }
        }

        public void Cancelar()
        {
            this.CancelarInterno();
            if (this.Siguiente != null)
            {
                this.Siguiente.Cancelar();
            }
        }
    }
}