using System.Collections.Generic;

namespace ClassLibrary{
    // A class that represents a system named Sistema.
    // It has properties that represent a List of Empresas and Emprendedores.
    public class Sistema{

        public List<Empresa> Empresa { get; set; }
        // A List of Emprendedores.
        private List<Emprendedor> emprendedores;

        // A constructor that creates a new Sistema.
        public Sistema(){
            this.empresas = new List<Empresa>();
            this.emprendedores = new List<Emprendedor>();
        }

        // A method that returns the List of Empresas.
        public List<Empresa> GetEmpresas(){
            return this.empresas;
        }

        // A method that returns the List of Emprendedores.
        public List<Emprendedor> GetEmprendedores(){
            return this.emprendedores;
        }

        // A method that creates an instance of Empresa and adds it to the list of Empresas.
        public void RegistrarEmpresa(string id, string ciudad, string direccion, string rubro, string nombre){
            Empresa empresa = new Empresa(id, ciudad, direccion, rubro, nombre);
            this.empresas.Add(empresa);
        }
        // --------------------------------------------------------------------------------------------------
        // REDUCIR CLASE UBICACION.
        // DEFINIR BIEN RUBRO COMO INSTANCIA DE UN OBJETO Y NO COLECCIÓN.
        // --------------------------------------------------------------------------------------------------

        // A method that adds an Emprendedor to the List of Emprendedores.
        public void RegistrarEmprendedor(string id, string ciudad, string direccion, string rubro, string nombre,
            List<Habilitacion> habilitaciones)
        {
            Emprendedor emprendedor = new Emprendedor(id, nombre, habilitaciones, ciudad, direccion, rubro);
            this.emprendedores.Add(emprendedor);
        }
    }
}