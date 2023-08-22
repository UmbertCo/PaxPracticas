using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SolucionPruebas.Entidades
{
    [DataContract()]
    public class Mensajes
    {
        #region Variables Privadas

        private List<Mensaje> _listaMensajes;
        private bool _tieneError;

        #endregion

        #region Propiedades

        [DataMember]
        public List<Mensaje> ListaMensajes
        {
            get
            {
                return _listaMensajes;
            }
            set
            {
                _listaMensajes = value;
            }
        }

        [DataMember]
        public bool TieneError
        {
            get
            {
                return _tieneError;
            }
            set
            {
                _tieneError = value;
            }
        }

        #endregion

        #region Constructor

        public Mensajes()
        {
            _listaMensajes = new List<Mensaje>();
            _tieneError = false;
        }

        #endregion
    }
}
