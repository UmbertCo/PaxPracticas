using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PaxConectorRetenciones
{
    [DataContract]
    class Serial
    {
        [DataMember]
        private string valor;

        public string Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        
    }
}
