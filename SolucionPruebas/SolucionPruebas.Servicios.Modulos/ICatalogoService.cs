using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SolucionPruebas.Servicios.Modulos
{
    [ServiceContract]
    interface ICatalogoService
    {
        [OperationContract]
        void fnRegistrarPersona(Entidades.Personas ENPersona, Entidades.Contacto ENContacto, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false);

        [OperationContract]
        void fnRegistrarContacto(Entidades.Contacto ENContacto, Boolean pbRequireTransaccion, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false);
    }
}
