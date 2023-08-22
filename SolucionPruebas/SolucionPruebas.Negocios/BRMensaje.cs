using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolucionPruebas.Negocios
{
    public class BRMensaje
    {
        /// <summary>
        /// Método que se encarga de agregar un nuevo mensaje.
        /// </summary>
        /// <param name="psMensaje">Mensaje</param>
        /// <param name="nTipoMensaje">Tipo de mensaje</param>
        /// <param name="EMensajes">Entidad Mensajes</param>
        public void fnAgregarMensaje(string psMensaje, int nTipoMensaje, ref Entidades.Mensajes EMensajes)
        {
            try
            {
                EMensajes.ListaMensajes.Add(new Entidades.Mensaje { DescripcionMensaje = psMensaje, TipoMensaje = nTipoMensaje });

                EMensajes.TieneError = (nTipoMensaje.Equals(1) ? true : false);
            }
            catch (Exception ex)
            {
                BRError.fnRegistrar("Datos", "BRMensaje", "fnAgregarMensaje", ex);
                FrameWork.ExceptionHandling.BussinessExceptionHandler.HandleException(ref ex);
            }
        }
    }
}
