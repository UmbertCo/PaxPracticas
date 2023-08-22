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
    public interface ISesionService 
    {
        [OperationContract]
        bool fnIniciarSesion(string psUsuario, string psPassword, ref Entidades.Mensajes ENMensajes);
    }
}
