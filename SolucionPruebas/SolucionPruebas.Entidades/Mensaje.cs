using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SolucionPruebas.Entidades
{
    [DataContract()]
    public class Mensaje
    {
        #region Variables Privadas

        private string _descripcionMensaje;
        private int _tipoMensaje;

        #endregion

        #region Propiedades

        [DataMember]
        public string DescripcionMensaje
        {
            get
            {
                return _descripcionMensaje;
            }
            set
            {
                _descripcionMensaje = value;
            }
        }

        [DataMember]
        public int TipoMensaje
        {
            get
            {
                return _tipoMensaje;
            }
            set
            {
                _tipoMensaje = value;
            }
        }

        #endregion

        #region Constructor

        public Mensaje()
        {
            _descripcionMensaje = string.Empty;
            _tipoMensaje = 0;
        }

        #endregion
    }
}
