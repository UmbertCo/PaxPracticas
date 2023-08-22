using FrameWork.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolucionPruebas.Negocios
{
    public class BRSesionAD
    {
        public BRSesionAD()
        { 
        
        }

        public bool fnIniciar(string psUsuario, string psPassword, ref Entidades.Mensajes ENMensajes)
        {
            bool bResultado = false;
            BRMensaje BRMensaje = new BRMensaje();
            try
            {
                FrameWork.Logging.ADAccountManagement oAccountManagement = new ADAccountManagement("pax.local", psUsuario, psPassword);
                if (!oAccountManagement.fnValidarCredenciales(psUsuario, psPassword))
                {
                    BRMensaje.fnAgregarMensaje("Usuario o contraseña invalido", 1, ref ENMensajes);
                    return bResultado;
                }

                if(!oAccountManagement.EsUsuarioBloqueado(psUsuario))
                {
                    BRMensaje.fnAgregarMensaje("Usuario bloqueado", 1, ref ENMensajes);
                    return bResultado;
                }

                if (!oAccountManagement.EsUsuarioExpirado(psUsuario))
                {
                    BRMensaje.fnAgregarMensaje("Usuario expirado", 1, ref ENMensajes);
                    return bResultado;
                }
            }
            catch (Exception ex)
            {
                BRError.fnRegistrar("Datos", "BRSesionAD", "fnIniciar", ex);
                FrameWork.ExceptionHandling.BussinessExceptionHandler.HandleException(ref ex);
            }
            return bResultado;
        }
    }
}
