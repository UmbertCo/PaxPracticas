using SolucionPruebas.Negocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SolucionPruebas.Servicios.Modulos
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class SesionService : ISesionService
    {
        public bool fnIniciarSesion(string psUsuario, string psPassword, ref Entidades.Mensajes ENMensajes)
        {
            BRSesionAD BRSesion = new BRSesionAD();
            return BRSesion.fnIniciar(psUsuario, psPassword, ref ENMensajes);
        }
    }
}
