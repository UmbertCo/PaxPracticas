using SolucionPruebas.AccesoDatos;
using SolucionPruebas.AccesoDatos.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolucionPruebas.AccesoDatos
{
    public class ADContacto
    {
        public ADContacto()
        {
            AccesoDatos.ADAcceso.CadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["conexion"].ToString();
        }

        public void fnInsertar<T>(T Entidad, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Transactions.IsolationLevel psIsolacion = System.Transactions.IsolationLevel.ReadCommitted)
        {
            AccesoDatos.ADAcceso.fnInsertar(ref Entidad, Procedimientos.usp_cli_Contactos_ins, pbRequiereTransaccion, pbEnTransaccion, pbTerminarTransaccion, psIsolacion);   
        }
    }
}
