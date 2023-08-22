using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolucionPruebas.Entidades
{
    public class Personas
    {
        private int _idPersona;
        private string _empresa;
        private int _idContacto;
        private string _telefono;
        private string _telefono2;
        private string _telefono3;
        private string _email;
        private int _idCiudad;
        private DateTime _fechaCaptura;
        private int _idEstatus;
        private int _idTipoCliente;
        private int _idTipoCompra;
        private int _idUsuario;
        private string _rfc;
        private string _razonSocial;
        private string _usuarioCobro;

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
        
        public string sEmpresa 
        {
            get
            { 
                return _empresa;
            }
            set
            { 
                _empresa = value;
            } 
        }

        public int nIdContacto
        {
            get
            {
                return _idContacto;
            }
            set
            {
                _idContacto = value;
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

        public int nIdCiudad
        {
            get
            {
                return _idCiudad;
            }
            set
            {
                _idCiudad = value;
            }
        }

        public DateTime dFechaCaptura
        {
            get
            {
                return _fechaCaptura;
            }
            set
            {
                _fechaCaptura = value;
            }
        }
        
        public int nIdEstatus
        {
            get
            {
                return _idEstatus;
            }
            set
            {
                _idEstatus = value;
            }
        }
        
        public int nIdTipoCliente
        {
            get
            {
                return _idTipoCliente;
            }
            set
            {
                _idTipoCliente = value;
            }
        }

        public int nIdTipoCompra
        {
            get
            {
                return _idTipoCompra;
            }
            set
            {
                _idTipoCompra = value;
            }
        }

        public int nIdUsuario
        {
            get
            {
                return _idUsuario;
            }
            set
            {
                _idUsuario = value;
            }
        }

        public string sRfc
        {
            get
            {
                return _rfc;
            }
            set
            {
                _rfc = value;
            }
        }
        
        public string sRazonSocial
        {
            get
            {
                return _razonSocial;
            }
            set
            {
                _razonSocial = value;
            }
        }

        public string sUsuarioCobro
        {
            get
            {
                return _usuarioCobro;
            }
            set
            {
                _usuarioCobro = value;
            }
        }

        public Personas()
        {
            _idPersona = 0;
            _empresa = string.Empty;
            _idContacto = 0;
            _telefono = string.Empty;
            _telefono2 = string.Empty;
            _telefono3 = string.Empty;
            _email = string.Empty;
            _idCiudad = 0;
            _fechaCaptura = DateTime.Now;
            _idEstatus = 0;
            _idTipoCliente = 0;
            _idTipoCompra = 0;
            _idUsuario = 0;
            _rfc = string.Empty;
            _razonSocial = string.Empty;
            _usuarioCobro = string.Empty;
        }
    }
}
