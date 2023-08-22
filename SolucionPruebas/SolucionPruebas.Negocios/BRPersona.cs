using FrameWork.ExceptionHandling;
using SolucionPruebas.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolucionPruebas.Negocios
{
    public class BRPersona
    {
        private SolucionPruebas.AccesoDatos.ADPersona ADPersona;
        BRContacto BRContacto;

        public void fnRegistrar(Entidades.Personas ENPersona, Entidades.Contacto ENContacto, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Transactions.IsolationLevel psIsolacion = System.Transactions.IsolationLevel.ReadCommitted)
        {
            try
            {
                ADPersona = new AccesoDatos.ADPersona();
                ADPersona.fnInsertar(ENPersona, true, true, false, System.Transactions.IsolationLevel.ReadUncommitted);

                ENContacto.nIdPersona = ENPersona.nIdPersona;

                BRContacto = new BRContacto();
                BRContacto.fnRegistrar(ENContacto, false, true, true, System.Transactions.IsolationLevel.ReadUncommitted);
            }
            catch (FrameWork.ExceptionHandling.CustomException.BussinessLayerCustomException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                BRError.fnRegistrar("Datos", "BRPersona", "fnRegistrar", ex);
                FrameWork.ExceptionHandling.BussinessExceptionHandler.HandleException(ref ex);
            }
        }
    }
}
