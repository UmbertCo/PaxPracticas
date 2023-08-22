using SolucionPruebas.Negocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace SolucionPruebas.Servicios.Modulos
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    class CatalogoService : ICatalogoService
    {
        public void fnRegistrarPersona(Entidades.Personas ENPersona, Entidades.Contacto ENContacto, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false)
        {
            BRPersona BPersona = new BRPersona();
            BPersona.fnRegistrar(ENPersona, ENContacto, pbRequiereTransaccion, pbEnTransaccion);
        }

        public void fnRegistrarContacto(Entidades.Contacto ENContacto, bool pbRequireTransaccion, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false)
        {
            BRContacto BContacto = new BRContacto();
            BContacto.fnRegistrar(ENContacto, pbRequiereTransaccion, pbEnTransaccion);
        }
    }
}
