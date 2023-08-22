using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;

namespace WebServiceSQL
{
    /// <summary>
    /// Conexion a sql server, Insertar, borrar y actualizar registros. 
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
   
    public class Service1 : System.Web.Services.WebService
    {

        [WebMethod]
        public bool Inserta(string nom, string apepat, string apemat, string dir, string tel)
        {
            SqlConnection conex = new SqlConnection("data source = localhost; initial catalog = JCMTPrueba; Integrated Security = SSPI;");
            string msj = string.Empty;
            string nombre = nom;
            string apellidoPat = apepat;
            string apellidoMat = apemat;
            string direccion = dir;
            string telefono = tel;
            SqlCommand sqlcm;        
                       
            try
            {   conex.Open();
                sqlcm = new SqlCommand("insert into dbo.datosGenerales (Nombre,ApellidoPat,ApellidoMat,Direccion,Telefono) values('" + nombre + "','" + apellidoPat + "','" + apellidoMat + "','" + direccion + "','" + telefono + "')", conex);
                sqlcm.ExecuteNonQuery();
               
            }
            catch (Exception)
            {
                
                throw;
            }
            
            conex.Close();
            return true;
            
        }


        [WebMethod]
        public string elimina(int id)
        {
            int idEliminar = id;
            SqlConnection conex = new SqlConnection("data source = localhost; initial catalog = JCMTPrueba; Integrated Security = SSPI;");
            SqlCommand sqlcm;
            try
            {
                conex.Open();
                sqlcm = new SqlCommand("delete from dbo.datosGenerales where datosGenerales.id = '"+ idEliminar +"'",conex);
                sqlcm.ExecuteNonQuery();

            }
            catch (Exception)
            {
                
                throw;
            }
            conex.Close();
            return "Registro eliminado con exito.";
        }
    }
}