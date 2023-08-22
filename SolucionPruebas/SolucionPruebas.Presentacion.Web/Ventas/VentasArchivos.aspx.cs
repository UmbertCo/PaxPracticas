using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolucionPruebas.Presentacion.Web.Ventas
{
    public partial class VentasArchivos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnCargarArchivo_Click(object sender, EventArgs e)
        {
            byte[] archivo;
            FileStream MiArchivo;
            string cadena_conexion = string.Empty;
            string directorio = "C:/Users/Desarrollo1/Documents";
            string direccion_archivo = string.Empty;
            if (!fuArchivo.HasFile)
            {
                return;
            }

            try
            {
                File.WriteAllBytes(directorio + fuArchivo.FileName, fuArchivo.FileBytes);
                direccion_archivo = directorio + fuArchivo.FileName;

                MiArchivo = File.OpenRead(direccion_archivo);

                archivo = new byte[MiArchivo.Length];

                MiArchivo.Read(archivo, 0, Convert.ToInt32(MiArchivo.Length));
                //cadena_conexion = string.Format("Data source={0}; Initial catalog={1};User ID={2}; Password={3};", "192.168.2.13", "PAXSistemaVentas", "sa", "F4cturax10nn");
                cadena_conexion = string.Format("Data source={0}; Initial catalog={1};Trusted_Connection=True;", "localhost", "Prueba");

                Utilerias.SQL.InterfazSQL conexion = new Utilerias.SQL.InterfazSQL(cadena_conexion);
                conexion.AgregarParametro("sNombre", fuArchivo.FileName);
                conexion.AgregarParametro("sArchivo", archivo);
                conexion.AgregarParametro("nId_Persona", 2);
                conexion.AgregarParametro("dFecha", DateTime.Now);
                conexion.AgregarParametro("sTipo", "X");
                conexion.AgregarParametro("sExtension", Path.GetExtension(fuArchivo.FileName));

                int nResultado = conexion.NoQuery("usp_cli_Archivos_ins", true);

            }
            catch (Exception ex)
            {

            }
        }
        protected void btnVisualizar_Click(object sender, EventArgs e)
        {
            byte[] archivo;
            DataTable dtArchivo = new DataTable();
            BinaryWriter bw;
            FileStream fs;
            Process comando;
            string cadena_conexion = string.Empty;
            string nombre = string.Empty;
            string nombreDocumento = string.Empty;
            string extension = string.Empty;
            SqlDataReader lector;
            try
            {
                cadena_conexion = string.Format("Data source={0}; Initial catalog={1};User ID={2}; Password={3};", "192.168.2.13", "PAXSistemaVentas", "sa", "F4cturax10n");

                Utilerias.SQL.InterfazSQL conexion = new Utilerias.SQL.InterfazSQL(cadena_conexion);

                conexion.AgregarParametro("nId_Archivo", 2);
                lector = conexion.Query("usp_cli_Archivos_Existe_sel", true);
                lector.Read();

                if (lector == null && !lector.HasRows && !lector.Read())
                {
                    return;
                }

                archivo = new byte[lector.GetBytes(2, 0, null, 0, int.MaxValue) - 1];

                lector.GetBytes(2, 0, archivo, 0, archivo.Length);

                extension = lector[6].ToString();
                nombre = lector[1].ToString();

                nombreDocumento = "C:/Users/Desarrollo1/Documents/" + nombre;

                fs = new FileStream(nombreDocumento, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                bw = new System.IO.BinaryWriter(fs);
                bw.Write(archivo);
                bw.Flush();
                bw.Close();
                fs.Close();
                lector.Close();

                comando = new Process();
                comando.StartInfo.FileName = nombreDocumento;
                comando.StartInfo.UseShellExecute = true;
                comando.StartInfo.CreateNoWindow = true;
                comando.Start();

            }
            catch (Exception ex)
            {

            }
        }
    }
}