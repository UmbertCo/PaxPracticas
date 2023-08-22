using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolucionPruebas.Entidades
{
    public class Contacto
    {
        private int _idContacto;
        private int _idPersona;
        private string _nombre;
        private string _profesion;
        private string _puestoEmpresa;
        private string _email;
        private string _telefono;
        private string _extensionTelefono;
        private string _telefono2;
        private string _extensionTelefono2;
        private string _telefono3;
        private string _extensionTelefono3;
        private string _estatus;

        public int nIdContacto 
        {
            get 
            { 
                return _idContacto; 
            } set 
            { 
                _idContacto = value; 
            } 
        }

        public int nIdPersona 
        { 
            get 
            { 
                return _idPersona; 
            } 
            set 
            { 
                _idPersona = value; 
            } 
        }

        public string sNombre
        { 
            get 
            { 
                return _nombre; 
            } 
            set 
            { 
                _nombre = value; 
            } 
        }

        public string sProfesion 
        { 
            get
            { 
                return _profesion; 
            } 
            set 
            { 
                _profesion = value; 
            } 
        }

        public string sPuestoEmpresa 
        { 
            get 
            {
                return _puestoEmpresa; 
            } 
            set 
            {
                _puestoEmpresa = value; 
            } 
        }

        public string sEmail 
        { 
            get 
            { 
                return _email; 
            } 
            set
            { 
                _email = value; 
            } 
        }

        public string sTelefono 
        { 
            get 
            { 
                return _telefono; 
            } 
            set 
            { 
                _telefono = value; 
            } 
        }

        public string sExtensionTelefono 
        { 
            get 
            { 
                return _extensionTelefono; 
            } 
            set 
            { 
                _extensionTelefono = value; 
            } 
        }

        public string sTelefono2 
        {
            get 
            { 
                return _telefono2; 
            } 
            set 
            { 
                _telefono2 = value; 
            } 
        }

        public string sExtensionTelefono2 
        { 
            get { 
                return _extensionTelefono2; 
            } 
            set
            { 
                _extensionTelefono2 = value; 
            } 
        }

        public string sTelefono3 
        { 
            get 
            { 
                return _telefono3; 
            } 
            set 
            { 
                _telefono3 = value; 
            } 
        }

        public string sExtensionTelefono3 
        { 
            get 
            {
                return _extensionTelefono3; 
            } 
            set 
            { 
                _extensionTelefono3 = value;
            } 
        }

        public string sEstatus 
        { 
            get 
            { 
                return _estatus; 
            } 
                set
            {
                _estatus = value; 
            } 
        }

        public Contacto()
        {
            _idContacto = 0;
            _idPersona = 0;
            _nombre = string.Empty;
            _profesion = string.Empty;
            _puestoEmpresa = string.Empty;
            _email = string.Empty;
            _telefono = string.Empty;
            _extensionTelefono = string.Empty;
            _telefono2 = string.Empty;
            _extensionTelefono2 = string.Empty;
            _telefono3 = string.Empty;
            _extensionTelefono3 = string.Empty;
            _estatus = string.Empty;
        }
    }
}
