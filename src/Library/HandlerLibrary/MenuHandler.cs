/*
using Telegram.Bot.Types;
using PII_E13.ClassLibrary;
using System.Collections.Generic;
using System.Text;
using System;



namespace PII_E13.HandlerLibrary
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "menu".
    /// </summary>
    public class MenuHandler : HandlerBase
    {

        private Empresa empresa;
        private Emprendedor emprendedor;


        public MenuHandler(HandlerBase next) : base(next)
        {

            this.Keywords = new string[] { "menu" };
        }


        protected override bool ResolverInterno(Message message, out string response)
        {

            if (this.PuedeResolver(message))
            {

                try
                {
                    empresa = Sistema.Instancia.ObtenerEmpresaPorId(message.From.Id.ToString());
                }
                catch (Exception e1)
                {
                    try
                    {
                        emprendedor = Sistema.Instancia.ObtenerEmprendedorPorId(message.From.Id.ToString());

                    }
                    catch (Exception e2)
                    {

                        if (e1.Message == "")
                        {
                            Console.WriteLine(e2.Message);
                        }
                        else
                        {
                            Console.WriteLine(e1.Message);
                        }
                    }
                }





                if ((empresa == null) & (emprendedor == null))// El usuario con id unico no tiene rol asignado 
                {
                    StringBuilder stringResponse = new StringBuilder();
                    stringResponse.Append("######## MENU ######## ");
                    stringResponse.Append("\n- registrar empresa");
                    stringResponse.Append("\n- registrar emprendedor");

                    response = stringResponse.ToString();


                    return true;
                }
                else if (emprendedor == null) //El usuario con Id unico NO es emprendedor (es empresa)
                {
                    StringBuilder stringResponse = new StringBuilder();
                    stringResponse.Append("######## MENU EMPRESARIO ######## ");
                    stringResponse.Append("\n- accion 1 empresario");
                    stringResponse.Append("\n- accion 2 empresario");
                    stringResponse.Append("\n- accion 3 empresario");

                    response = stringResponse.ToString();


                    return true;
                }
                else if (empresa == null)
                {
                    StringBuilder stringResponse = new StringBuilder();
                    stringResponse.Append("######## MENU EMPRENDEDOR ######## ");
                    stringResponse.Append("\n- accion 1 emprendedor");
                    stringResponse.Append("\n- accion 2 emprendedor");
                    stringResponse.Append("\n- accion 3 emprendedor");

                    response = stringResponse.ToString();
                }

            }


            response = string.Empty;
            return false;
        }


    }
}
*/