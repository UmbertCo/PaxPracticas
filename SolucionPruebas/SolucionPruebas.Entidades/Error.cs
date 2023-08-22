using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolucionPruebas.Entidades
{
    public class Error
    {
        private int _nIdError;
        private string _sTipoError;
        private string _sMensaje;
        private string _sModulo;
        private string _sMetodoOrigen;
        private DateTime _dFecha;
        private int _nIdUsuario;
        private string _sObservaciones;
        private string _sTicket;       

        public int nIdError
        {
            get { return _nIdError; }
            set { _nIdError = value; }
        }

        public string sTipoError
        {
            get { return _sTipoError; }
            set { _sTipoError = value; }
        }        

        public string sMensaje
        {
            get { return _sMensaje; }
            set { _sMensaje = value; }
        }        

        public string sModulo
        {
            get { return _sModulo; }
            set { _sModulo = value; }
        }
        
        public string sMetodoOrigen
        {
            get { return _sMetodoOrigen; }
            set { _sMetodoOrigen = value; }
        }        

        public DateTime dFecha
        {
            get { return _dFecha; }
            set { _dFecha = value; }
        }

        public int nIdUsuario
        {
            get { return _nIdUsuario; }
            set { _nIdUsuario = value; }
        }        

        public string sObservaciones
        {
            get { return _sObservaciones; }
            set { _sObservaciones = value; }
        }

        public string sTicket
        {
            get { return _sTicket; }
            set { _sTicket = value; }
        }
        
        public Error()
        { 
            _nIdError = 0;
            _sTipoError = string.Empty;
            _sMensaje = string.Empty;
            _sModulo = string.Empty;
            _sMetodoOrigen = string.Empty;
            _dFecha = Convert.ToDateTime("01/01/1900");
            _nIdUsuario = 0;
            _sObservaciones = string.Empty;
            _sTicket = string.Empty;
        }
    }
}
