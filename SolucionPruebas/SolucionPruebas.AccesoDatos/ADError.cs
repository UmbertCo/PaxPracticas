using SolucionPruebas.AccesoDatos.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolucionPruebas.AccesoDatos
{
    public class ADError
    {
        public ADError()
        {
            AccesoDatos.ADAcceso.CadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["conexion"].ToString();
        }

        public void fnInsertar<T>(T Entidad, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Data.IsolationLevel psIsolacion = System.Data.IsolationLevel.ReadCommitted)
        {
            AccesoDatos.ADAcceso.fnInsertar(ref Entidad, Procedimientos.usp_Ctp_Registrar_Error_ins, pbEnTransaccion, pbEnTransaccion, pbTerminarTransaccion, psIsolacion);
        }
    }
}
