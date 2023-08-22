using FrameWork.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SolucionPruebas.AccesoDatos
{
    public static class ADAcceso
    {
        public static string CadenaConexion 
        { 
            set
            { 
                fnEstablecerCadenaConexion(value); 
            }
        }

        public static void fnActualizar<T>(ref T Entidad, string psProcedimientoAlmacenado, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Transactions.IsolationLevel psIsolacion = System.Transactions.IsolationLevel.ReadCommitted)
        {
            try
            {
                FrameWork.DataAccess.AccesoDatos FaccesoDatos = new FrameWork.DataAccess.AccesoDatos();
                FaccesoDatos.fnActualizar(ref Entidad, psProcedimientoAlmacenado, pbRequiereTransaccion, pbEnTransaccion, pbTerminarTransaccion, psIsolacion);
            }
            catch (Exception ex)
            {
                FrameWork.ExceptionHandling.DataAccessExceptionHandler.HandleException(ex);
            }
        }

        public static void fnInsertar<T>(ref T Entidad, string psProcedimientoAlmacenado, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Transactions.IsolationLevel psIsolacion = System.Transactions.IsolationLevel.ReadCommitted)
        {
            try
            {
                FrameWork.DataAccess.AccesoDatos FaccesoDatos = new FrameWork.DataAccess.AccesoDatos();
                FaccesoDatos.fnInsertar(ref Entidad, psProcedimientoAlmacenado, pbRequiereTransaccion, pbEnTransaccion, pbTerminarTransaccion, psIsolacion);
            }
            catch (Exception ex)
            {
                FrameWork.ExceptionHandling.DataAccessExceptionHandler.HandleException(ex);
            }
        }

        public static void fnActualizar<T>(ref T Entidad, string psProcedimientoAlmacenado, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Data.IsolationLevel psIsolacion = System.Data.IsolationLevel.ReadCommitted)
        {
            try
            {
                FrameWork.DataAccess.AccesoDatos FaccesoDatos = new FrameWork.DataAccess.AccesoDatos();
                FaccesoDatos.fnActualizar(ref Entidad, psProcedimientoAlmacenado, pbRequiereTransaccion, pbEnTransaccion, pbTerminarTransaccion, psIsolacion);
            }
            catch (Exception ex)
            {
                FrameWork.ExceptionHandling.DataAccessExceptionHandler.HandleException(ex);
            }
        }

        public static void fnInsertar<T>(ref T Entidad, string psProcedimientoAlmacenado, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Data.IsolationLevel psIsolacion = System.Data.IsolationLevel.ReadCommitted)
        {
            try
            {
                FrameWork.DataAccess.AccesoDatos FaccesoDatos = new FrameWork.DataAccess.AccesoDatos();
                FaccesoDatos.fnInsertar(ref Entidad, psProcedimientoAlmacenado, pbRequiereTransaccion, pbEnTransaccion, pbTerminarTransaccion, psIsolacion);
            }
            catch (Exception ex)
            {
                FrameWork.ExceptionHandling.DataAccessExceptionHandler.HandleException(ex);
            }
        }

        public static DataTable fnConsultar<T>(T Entidad, string psProcedimientoAlmacenado, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Transactions.IsolationLevel psIsolacion = System.Transactions.IsolationLevel.ReadCommitted)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                FrameWork.DataAccess.AccesoDatos FaccesoDatos = new FrameWork.DataAccess.AccesoDatos();
                dtResultado = FaccesoDatos.fnConsultar(Entidad, psProcedimientoAlmacenado, pbRequiereTransaccion, pbEnTransaccion, pbTerminarTransaccion, psIsolacion);
            }
            catch (Exception ex)
            {
                FrameWork.ExceptionHandling.DataAccessExceptionHandler.HandleException(ex);
            }
            return dtResultado;
        }

        public static DataTable fnConsultar<T>(T Entidad, string psProcedimientoAlmacenado)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                FrameWork.DataAccess.AccesoDatos FaccesoDatos = new FrameWork.DataAccess.AccesoDatos();
                return FaccesoDatos.fnConsultar(Entidad, psProcedimientoAlmacenado);
            }
            catch (Exception ex)
            {
                FrameWork.ExceptionHandling.DataAccessExceptionHandler.HandleException(ex);
            }
            return dtResultado;
        }

        private static void fnEstablecerCadenaConexion(string psCadena)
        {
            FrameWork.DataAccess.AccesoDatos._cadenaConexion = psCadena;
        }
    }
}
