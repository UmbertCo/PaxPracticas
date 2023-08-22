using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolucionPruebas.Entidades
{
    public class Sesion
    {
        private int _nIdUsuario;
        private string _sClaveUsuario;
        private string _sError;
        private bool _bTieneError;

        public int nIdUsuario
        {
            get { return _nIdUsuario; }
            set { _nIdUsuario = value; }
        }

        public string sClaveUsuario
        {
            get { return _sClaveUsuario; }
            set { _sClaveUsuario = value; }
        }

        public string sError
        {
            get { return _sError; }
            set { _sError = value; }
        }

        public bool bTieneError
        {
            get { return _bTieneError; }
            set { _bTieneError = value; }
        }

        public Sesion()
        {
            _nIdUsuario = 0;
            _sClaveUsuario = string.Empty;
            _sError = string.Empty;
            _bTieneError = false;
        }
    }
}
