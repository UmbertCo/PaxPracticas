using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;

public partial class Consultas_webDescargaConsultaGeneracion : System.Web.UI.Page
{
    private clsOperacionConsulta gDAL;
    private SqlCommand com;
    Thread threadDescarga;
    string sQueryString;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnCerrar.Attributes.Add("onclick", "window.close();");

            fnLimpiarHilos();
        }
    }

    protected void btnDescarga_Click(object sender, EventArgs e)
    {
        try
        {

            //Start the Process
            lock (Session.SyncRoot)
            {
                Session["completadoExcel"] = false;
                Session["estatusExcel"] = "";
            }

           sQueryString = (Request.QueryString["p"] != null) ? Request.QueryString["p"].ToString().Replace(" ", "+") : string.Empty;

            threadDescarga = new Thread(new ParameterizedThreadStart(fnRealizarConsultaAsincrona));
            threadDescarga.Start(HttpContext.Current);

            Session["tDescargaExcel"] = threadDescarga;

            lblProgresoDescarga.Visible = true;
            timerDescarga.Enabled = true;
            btnDescarga.Enabled = false;

            
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            fnLimpiarHilos();
        }
    }

    //Timer actualiza el progreso del proceso de descarga
    protected void timerDescarga_Tick(object sender, EventArgs e)
    {
        lblProgresoDescarga.Text = Session["estatusExcel"].ToString();

        if (Session["completadoExcel"] as bool? == true)
        {
            timerDescarga.Enabled = false;

            fnDescargaArchivo();
        }
    }

    private void fnLimpiarHilos()
    {
        Session["rutaArchivoExcel"] = null;
        Session["completadoExcel"] = false;
        Session["estatusExcel"] = "";

        if (timerDescarga != null)
        {
            timerDescarga.Dispose();
        }

        if (Session["tDescargaExcel"] != null)
        {
            threadDescarga = (Thread)Session["tDescargaComp"];

            if (threadDescarga != null && threadDescarga.IsAlive)
            {
                threadDescarga.Abort();
            }

            Session["tDescargaComp"] = null;
        }
    }


    private void fnRealizarConsultaAsincrona(object data)
    {

        if (string.IsNullOrEmpty(sQueryString)) { return; }

        //string sCsv = Utilerias.Encriptacion.Base64.DesencriptarBase64(sQueryString);
        string sCsv = PAXCrypto.CryptoAES.DesencriptaAES64(sQueryString);

        gDAL = new clsOperacionConsulta();
        try
        {
            //Se recupera contexto 
            HttpContext context = (HttpContext)data;
            HttpContext.Current = context;

            if (Session["Culture"] != null)
            {
                string lang = Session["Culture"].ToString();
                if ((lang != null) || (lang != ""))
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX");
                }
            }
            else
            {
                string language = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            }

            lock (Session.SyncRoot)
            {
                Session["estatusExcel"] = "Iniciando proceso...";
            }

            DataTable dtExcel = new DataTable();


            string cadena = System.Configuration.ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString;



            using (SqlConnection scConexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(cadena)))
            {
                try
                {
                    scConexion.Open();

                    using (SqlCommand com = new SqlCommand("usp_cfd_DescargaExcel_GeneracionNomina", scConexion))
                    {
                        try
                        {
                            com.Parameters.AddWithValue("@sCsv", sCsv);

                            com.CommandType = CommandType.StoredProcedure;

                            using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(com))
                            {
                                try
                                {

                                    sdaAdaptador.Fill(dtExcel);
                                }
                                catch
                                {
                                    sdaAdaptador.Dispose();
                                    throw new Exception();

                                }
                            }

                            lock (Session.SyncRoot)
                            {
                                Session["estatusExcel"] = "Procesando: 20%";
                            }
                        }
                        catch
                        {
                            com.Dispose();
                            throw new Exception();

                        }

                    }
                }
                catch
                {
                    scConexion.Close();
                    return;
                }
                finally 
                {

                    scConexion.Close();
                }
            }
           


            MetodoCallBack(dtExcel);

        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            fnLimpiarHilos();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            fnLimpiarHilos();
        }
    }

    private void MetodoCallBack(DataTable dtAuxiliar)
    {

        int nIndice = 0;

        foreach (DataRow renglon in dtAuxiliar.Rows)
        {


            //Reporta progreso (20% - 90%)
            lock (Session.SyncRoot)
            {
                nIndice++;
                Session["estatusExcel"] = "Procesando: " + (((nIndice * 70) / dtAuxiliar.Rows.Count) + 20) + "%";
            }
        }


        ExportarExc( dtAuxiliar);
    }

    public void ExportarExc( DataTable datos)
    {
        string[] titulos = datos.Columns.Cast<DataColumn>()
                                  .Select(x => x.ColumnName)
                                  .ToArray(); ;

        try
        {
            Guid Giud = Guid.NewGuid();
            String dlDir = @"Exceles/";
            string rutas = Server.MapPath(dlDir + Giud + ".xls");
            FileStream fs = new FileStream(rutas, FileMode.Create,
                                           FileAccess.ReadWrite);
            StreamWriter w = new StreamWriter(fs);
            string comillas = char.ConvertFromUtf32(34);
            StringBuilder html = new StringBuilder();
            html.Append(@"<!DOCTYPE html PUBLIC" + comillas +
            "-//W3C//DTD XHTML 1.0 Transitional//EN" + comillas +
            " " + comillas
            + "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" + comillas + ">");
            html.Append(@"<html xmlns=" + comillas
                         + "http://www.w3.org/1999/xhtml"
                         + comillas + ">");
            html.Append(@"<head>");
            html.Append(@"<meta http-equiv=" + comillas + "Content-Type"
                         + comillas + "content=" + comillas
                         + "text/html; charset=utf-8" + comillas + "/>");
            html.Append(@"<title>Untitled Document</title>");
            html.Append(@"</head>");
            html.Append(@"<body>");


            //Generando encabezados del archivo 
            //(aquí podemos dar el formato como a una tabla de HTML)
            html.Append(@"<table WIDTH=730 CELLSPACING=0 CELLPADDING=10 
                    border=8 BORDERCOLOR=" + comillas + "#333366"
                        + comillas + " bgcolor=" + comillas + "#FFFFFF"
                        + comillas + ">");
            html.Append(@"<tr> <b>");

            foreach (object item in titulos)
            {
                html.Append(@"<th>" + item.ToString() + "</th>");
            }
            html.Append(@"</b> </tr>");

            //Generando datos del archivo
            for (int i = 0; i < datos.Rows.Count; i++)
            {
                html.Append(@"<tr>");
                for (int j = 0; j < datos.Columns.Count; j++)
                {
                    if (datos.Columns[j].ColumnName.Contains("total"))
                    {
                        html.Append(@"<td>" +
                                fnFormatoCurrency(datos.Rows[i][j].ToString()) + "</td>");

                    }
                    else
                    {
                        html.Append(@"<td>" +
                                    datos.Rows[i][j].ToString() + "</td>");
                    }

                }
                html.Append(@"</tr>");
            }
            html.Append(@"</body>");
            html.Append(@"</html>");
            w.Write(html.ToString());
            w.Close();

            //Reporta progreso (20% - 90%)

            Session["rutaArchivoExcel"] = rutas;

            lock (Session.SyncRoot)
            {
                Session["estatusExcel"] = "Completado, ya puede descargar el archivo";
                Session["completadoExcel"] = true;
            }
            //fnDescargaArchivo(rutas);

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            fnLimpiarHilos();
        }

    }

    /// <summary>
    /// Funcion encargada de descargar los archivos del servidor.
    /// </summary>
    /// <param name="filename"></param>
    private void fnDescargaArchivo()
    {
        try
        {
            if (Session["rutaArchivoExcel"] != null)
            {
                string sNombreArchivo = Session["rutaArchivoExcel"].ToString();

                if (!String.IsNullOrEmpty(sNombreArchivo))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "descargarExcel",
                                                        String.Format("<script>window.location.href='{0}?f={1}'</script>",
                                                                       "DescargaExcel.ashx", Path.GetFileNameWithoutExtension(sNombreArchivo)), false);

                    //String path = filename;

                    //System.IO.FileInfo toDownload = new System.IO.FileInfo(path);

                    //if (toDownload.Exists)
                    //{
                    //    btnCerrar.Enabled = true;
                    //    Response.Clear();
                    //    Response.Buffer = true;
                    //    Response.ContentType = "application/vnd.ms-excel";
                    //    Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
                    //    Response.AddHeader("Content-Length", toDownload.Length.ToString());
                    //    Response.Charset = "UTF-8";
                    //    Response.ContentEncoding = Encoding.UTF8;
                    //    Response.WriteFile(path);
                    //    Response.Flush();
                    //    Response.End();
                    //}
                }
            }

            fnLimpiarHilos();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            fnLimpiarHilos();
        }
    }

    public static string fnFormatoCurrency(string valor)
    {
        return string.Format("{0:c2}", Convert.ToDouble(valor));
    }

    /// <summary>
    /// Loads the language specific resources
    /// </summary>
    protected override void InitializeCulture()
    {
        if (Session["Culture"] != null)
        {
            string lang = Session["Culture"].ToString();
            if ((lang != null) || (lang != ""))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX");
            }
        }
        else
        {
            string language = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
        }
    }

    protected void btnCerrar_Click(object sender, EventArgs e)
    {
        fnLimpiarHilos();
    }
}