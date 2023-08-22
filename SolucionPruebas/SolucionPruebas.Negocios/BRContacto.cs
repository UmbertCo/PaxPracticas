using FrameWork.ExceptionHandling;
using FrameWork.ExceptionHandling.CustomException;
using SolucionPruebas.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolucionPruebas.Negocios
{
    public class BRContacto
    {
        private AccesoDatos.ADContacto ADContacto;

        public void fnRegistrar(Entidades.Contacto ENContacto, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Transactions.IsolationLevel psIsolacion = System.Transactions.IsolationLevel.ReadCommitted)
        {
            try
            {
                ADContacto = new AccesoDatos.ADContacto();
                ADContacto.fnInsertar(ENContacto, pbRequiereTransaccion, pbEnTransaccion, pbTerminarTransaccion, psIsolacion);
            }
            catch (FrameWork.ExceptionHandling.CustomException.BussinessLayerCustomException ex)
            {
                throw ex;
            }
            catch(Exception ex)
            {
                BRError.fnRegistrar("Datos", "BRContacto", "fnRegistrar", ex);
                throw new FrameWork.ExceptionHandling.CustomException.BussinessLayerCustomException(ex.Message);
            }
        }
    }
}
