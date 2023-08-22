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

public partial class Consultas_webDescargaConsulta : System.Web.UI.Page
{
    private clsOperacionConsulta gDAL;
    private SqlCommand com;
    Thread threadDescarga;

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
            #region comentado 26-02-2013

            //   PageAsyncTask task = new PageAsyncTask(
            //    new BeginEventHandler(fnRealizarConsultaAsincrona),
            //    new EndEventHandler(MetodoCallBack),
            //    new EndEventHandler(TimeoutAsyncOperation),
            //    null
            //);
            //   RegisterAsyncTask(task);

            #endregion

            //Start the Process
            lock (Session.SyncRoot)
            {
                Session["completadoExcel"] = false;
                Session["estatusExcel"] = "";
            }

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

    #region comentado 26-02-2013

    //IAsyncResult fnRealizarConsultaAsincrona(object sender, EventArgs e, AsyncCallback cb, object state)
    //{
    //    DataTable dtNew = (DataTable)Session["dtConsultaExc"];

    //    gDAL = new clsOperacionConsulta();
    //    try
    //    {
    //        DataTable dtExcel = new DataTable();


    //        string cadena = System.Configuration.ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString;

    //        SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(cadena));
    //        conexion.Open();

    //        com = new SqlCommand("usp_Cfd_ComprobantesAsincrona_Sel_Cobro", conexion);
    //        foreach (DataRow Renglon in dtNew.Rows)
    //        {

    //            com.Parameters.AddWithValue("nId_Usuario", Convert.ToInt32(Renglon["nId_Usuario"]));
    //            com.Parameters.AddWithValue("dFecha_Inicio", Convert.ToDateTime(Renglon["dFecha_Inicio"]));
    //            com.Parameters.AddWithValue("dFecha_Fin", Convert.ToDateTime(Renglon["dFecha_Fin"]));
    //            if (Convert.ToString(Renglon["nId_Estructura"]) != "0")
    //                com.Parameters.AddWithValue("nId_Estructura", Convert.ToInt32(Renglon["nId_Estructura"]));
    //            if (Convert.ToString(Renglon["nId_Tipo_Documento"]) != "0")
    //                com.Parameters.AddWithValue("nId_Tipo_Documento", Convert.ToInt32(Renglon["nId_Tipo_Documento"]));
    //            if (Convert.ToString(Renglon["sEstatus"]) != "0")
    //                com.Parameters.AddWithValue("sEstatus", Convert.ToString(Renglon["sEstatus"]));
    //            if (Convert.ToString(Renglon["sRfc_Receptor"]) != "0")
    //                com.Parameters.AddWithValue("sRfc_Receptor", Convert.ToString(Renglon["sRfc_Receptor"]));
    //            if (Convert.ToString(Renglon["sSerie"]) != Resources.resCorpusCFDIEs.VarDropTodos)
    //                com.Parameters.AddWithValue("sSerie", Convert.ToString(Renglon["sSerie"]));
    //            if (!string.IsNullOrEmpty(Convert.ToString(Renglon["nFolio_Inicio"])))
    //                com.Parameters.AddWithValue("nFolio_Inicio", Convert.ToString(Renglon["nFolio_Inicio"]));
    //            if (!string.IsNullOrEmpty(Convert.ToString(Renglon["nFolio_Fin"])))
    //                com.Parameters.AddWithValue("nFolio_Fin", Convert.ToString(Renglon["nFolio_Fin"]));
    //            if (!string.IsNullOrEmpty(Convert.ToString(Renglon["nUUID"])))
    //                com.Parameters.AddWithValue("nUUID", Convert.ToString(Renglon["nUUID"]));
    //            if (Convert.ToString(Renglon["nId_Usuario_Filtro"]) != "0")
    //                com.Parameters.AddWithValue("nId_Usuario_Filtro", Convert.ToString(Renglon["nId_Usuario_Filtro"]));

    //        }
    //        com.CommandType = CommandType.StoredProcedure;

    //        return com.BeginExecuteReader(cb, state);

    //    }
    //    catch (SqlException ex)
    //    {
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
    //    }
    //    catch (Exception ex)
    //    {
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
    //    }
    //    return null;
    //}

    //private void MetodoCallBack(IAsyncResult result)
    //{
    //    DataTable dtAuxiliar = new DataTable();
    //    SqlDataReader dr = com.EndExecuteReader(result);
    //    dtAuxiliar.Load(dr);

    //    com.Dispose();

    //    DataTable dtNew = new DataTable();
    //    DataColumn columna1 = new DataColumn();
    //    columna1.DataType = System.Type.GetType("System.String");
    //    columna1.AllowDBNull = true;
    //    columna1.Caption = "UUID";
    //    columna1.ColumnName = "UUID";
    //    columna1.DefaultValue = null;
    //    dtNew.Columns.Add(columna1);

    //    DataColumn columna2 = new DataColumn();
    //    columna2.DataType = System.Type.GetType("System.String");
    //    columna2.AllowDBNull = true;
    //    columna2.Caption = "serie";
    //    columna2.ColumnName = "serie";
    //    columna2.DefaultValue = null;
    //    dtNew.Columns.Add(columna2);

    //    DataColumn columna3 = new DataColumn();
    //    columna3.DataType = System.Type.GetType("System.String");
    //    columna3.AllowDBNull = true;
    //    columna3.Caption = "folio";
    //    columna3.ColumnName = "folio";
    //    columna3.DefaultValue = null;
    //    dtNew.Columns.Add(columna3);

    //    DataColumn columna4 = new DataColumn();
    //    columna4.DataType = System.Type.GetType("System.String");
    //    columna4.AllowDBNull = true;
    //    columna4.Caption = "razon_social";
    //    columna4.ColumnName = "razon_social";
    //    columna4.DefaultValue = null;
    //    dtNew.Columns.Add(columna4);

    //    DataColumn columna5 = new DataColumn();
    //    columna5.DataType = System.Type.GetType("System.String");
    //    columna5.AllowDBNull = true;
    //    columna5.Caption = "sucursal";
    //    columna5.ColumnName = "sucursal";
    //    columna5.DefaultValue = null;
    //    dtNew.Columns.Add(columna5);

    //    DataColumn columna14 = new DataColumn();
    //    columna14.DataType = System.Type.GetType("System.String");
    //    columna14.AllowDBNull = true;
    //    columna14.Caption = "usuario";
    //    columna14.ColumnName = "usuario";
    //    columna14.DefaultValue = null;
    //    dtNew.Columns.Add(columna14);

    //    DataColumn columna6 = new DataColumn();
    //    columna6.DataType = System.Type.GetType("System.String");
    //    columna6.AllowDBNull = true;
    //    columna6.Caption = "documento";
    //    columna6.ColumnName = "documento";
    //    columna6.DefaultValue = null;
    //    dtNew.Columns.Add(columna6);

    //    DataColumn columna7 = new DataColumn();
    //    columna7.DataType = System.Type.GetType("System.Double");
    //    columna7.AllowDBNull = true;
    //    columna7.Caption = "total";
    //    columna7.ColumnName = "total";
    //    columna7.DefaultValue = null;
    //    dtNew.Columns.Add(columna7);

    //    DataColumn columna13 = new DataColumn();
    //    columna13.DataType = System.Type.GetType("System.String");
    //    columna13.AllowDBNull = true;
    //    columna13.Caption = "moneda";
    //    columna13.ColumnName = "moneda";
    //    columna13.DefaultValue = null;
    //    dtNew.Columns.Add(columna13);

    //    DataColumn columna8 = new DataColumn();
    //    columna8.DataType = System.Type.GetType("System.String");
    //    columna8.AllowDBNull = true;
    //    columna8.Caption = "fecha";
    //    columna8.ColumnName = "fecha";
    //    columna8.DefaultValue = null;
    //    dtNew.Columns.Add(columna8);

    //    DataColumn columna9 = new DataColumn();
    //    columna9.DataType = System.Type.GetType("System.String");
    //    columna9.AllowDBNull = true;
    //    columna9.Caption = "estatus";
    //    columna9.ColumnName = "estatus";
    //    columna9.DefaultValue = null;
    //    dtNew.Columns.Add(columna9);

    //    DataColumn columna10 = new DataColumn();
    //    columna10.DataType = System.Type.GetType("System.String");
    //    columna10.AllowDBNull = true;
    //    columna10.Caption = "rfc";
    //    columna10.ColumnName = "rfc";
    //    columna10.DefaultValue = null;
    //    dtNew.Columns.Add(columna10);

    //    DataColumn columna11 = new DataColumn();
    //    columna11.DataType = System.Type.GetType("System.String");
    //    columna11.AllowDBNull = true;
    //    columna11.Caption = "fecha_cancelacion";
    //    columna11.ColumnName = "fecha_cancelacion";
    //    columna11.DefaultValue = null;
    //    dtNew.Columns.Add(columna11);

    //    DataColumn columna12 = new DataColumn();
    //    columna12.DataType = System.Type.GetType("System.String");
    //    columna12.AllowDBNull = true;
    //    columna12.Caption = "comentarios_cancelacion";
    //    columna12.ColumnName = "comentarios_cancelacion";
    //    columna12.DefaultValue = null;
    //    dtNew.Columns.Add(columna12);

    //    foreach (DataRow renglon in dtAuxiliar.Rows)
    //    {
    //        DataRow nuevo = dtNew.NewRow();
    //        nuevo["UUID"] = renglon["UUIDM"];
    //        nuevo["serie"] = renglon["serie"];
    //        nuevo["folio"] = renglon["folio"];
    //        nuevo["razon_social"] = renglon["razon_social"];
    //        nuevo["sucursal"] = renglon["sucursal"];
    //        nuevo["usuario"] = renglon["usuario"];
    //        nuevo["documento"] = renglon["documento"];
    //        nuevo["total"] = renglon["total"];
    //        nuevo["moneda"] = renglon["moneda"];
    //        nuevo["fecha"] = renglon["fecha"];

    //        switch (Convert.ToString(renglon["estatus"]))
    //        {
    //            case "P":
    //                nuevo["estatus"] = "Pendiente";
    //                break;
    //            case "C":
    //                nuevo["estatus"] = "Cancelado";
    //                break;
    //            case "A":
    //                nuevo["estatus"] = "Activo";
    //                break;
    //            default:
    //                nuevo["estatus"] = renglon["estatus"];
    //                break;
    //        }

    //        nuevo["rfc"] = renglon["rfc"];
    //        nuevo["fecha_cancelacion"] = renglon["fecha_cancelacion"];
    //        nuevo["comentarios_cancelacion"] = renglon["comentarios_cancelacion"];
    //        dtNew.Rows.Add(nuevo);
    //    }
    //    ArrayList ejemplo = new ArrayList();
    //    ejemplo.Add("UUID");
    //    ejemplo.Add("Serie");
    //    ejemplo.Add("folio");
    //    ejemplo.Add("razon_social");
    //    ejemplo.Add("sucursal");
    //    ejemplo.Add("usuario");
    //    ejemplo.Add("documento");
    //    ejemplo.Add("total");
    //    ejemplo.Add("moneda");
    //    ejemplo.Add("fecha");
    //    ejemplo.Add("estatus");
    //    ejemplo.Add("rfc");
    //    ejemplo.Add("fecha_cancelacion");
    //    ejemplo.Add("comentarios_cancelacion");

    //    ExportarExc(ejemplo, dtNew);
    //}

    //void TimeoutAsyncOperation(IAsyncResult ar)
    //{

    //}

    #endregion

    private void fnRealizarConsultaAsincrona(object data)
    {
        DataTable dtNew = (DataTable)Session["dtConsultaExc"];

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

            SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(cadena));
            conexion.Open();

            com = new SqlCommand("usp_Cfd_ComprobantesAsincrona_Sel_Cobro", conexion);
            foreach (DataRow Renglon in dtNew.Rows)
            {

                com.Parameters.AddWithValue("nId_Usuario", Convert.ToInt32(Renglon["nId_Usuario"]));
                com.Parameters.AddWithValue("dFecha_Inicio", Convert.ToDateTime(Renglon["dFecha_Inicio"]));
                com.Parameters.AddWithValue("dFecha_Fin", Convert.ToDateTime(Renglon["dFecha_Fin"]));
                if (Convert.ToString(Renglon["nId_Estructura"]) != "0")
                    com.Parameters.AddWithValue("nId_Estructura", Convert.ToInt32(Renglon["nId_Estructura"]));
                if (Convert.ToString(Renglon["nId_Tipo_Documento"]) != "0")
                    com.Parameters.AddWithValue("nId_Tipo_Documento", Convert.ToInt32(Renglon["nId_Tipo_Documento"]));
                if (Convert.ToString(Renglon["sEstatus"]) != "0")
                    com.Parameters.AddWithValue("sEstatus", Convert.ToString(Renglon["sEstatus"]));
                if (Convert.ToString(Renglon["sRfc_Receptor"]) != "0")
                    com.Parameters.AddWithValue("sRfc_Receptor", Convert.ToString(Renglon["sRfc_Receptor"]));
                if (Convert.ToString(Renglon["sSerie"]) != Resources.resCorpusCFDIEs.VarDropTodos)
                    com.Parameters.AddWithValue("sSerie", Convert.ToString(Renglon["sSerie"]));
                if (!string.IsNullOrEmpty(Convert.ToString(Renglon["nFolio_Inicio"])))
                    com.Parameters.AddWithValue("nFolio_Inicio", Convert.ToString(Renglon["nFolio_Inicio"]));
                if (!string.IsNullOrEmpty(Convert.ToString(Renglon["nFolio_Fin"])))
                    com.Parameters.AddWithValue("nFolio_Fin", Convert.ToString(Renglon["nFolio_Fin"]));
                if (!string.IsNullOrEmpty(Convert.ToString(Renglon["nUUID"])))
                    com.Parameters.AddWithValue("nUUID", Convert.ToString(Renglon["nUUID"]));
                if (Convert.ToString(Renglon["nId_Usuario_Filtro"]) != "0")
                    com.Parameters.AddWithValue("nId_Usuario_Filtro", Convert.ToString(Renglon["nId_Usuario_Filtro"]));

            }
            com.CommandType = CommandType.StoredProcedure;

            lock (Session.SyncRoot)
            {
                Session["estatusExcel"] = "Procesando: 20%";
            }

            MetodoCallBack(com.ExecuteReader());

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

    private void MetodoCallBack(SqlDataReader pReader)
    {
        DataTable dtAuxiliar = new DataTable();
        SqlDataReader dr = pReader;
        dtAuxiliar.Load(dr);

        com.Dispose();

        DataTable dtNew = new DataTable();
        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "UUID";
        columna1.ColumnName = "UUID";
        columna1.DefaultValue = null;
        dtNew.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "serie";
        columna2.ColumnName = "serie";
        columna2.DefaultValue = null;
        dtNew.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "folio";
        columna3.ColumnName = "folio";
        columna3.DefaultValue = null;
        dtNew.Columns.Add(columna3);

        DataColumn columna4 = new DataColumn();
        columna4.DataType = System.Type.GetType("System.String");
        columna4.AllowDBNull = true;
        columna4.Caption = "razon_social";
        columna4.ColumnName = "razon_social";
        columna4.DefaultValue = null;
        dtNew.Columns.Add(columna4);

        DataColumn columna5 = new DataColumn();
        columna5.DataType = System.Type.GetType("System.String");
        columna5.AllowDBNull = true;
        columna5.Caption = "sucursal";
        columna5.ColumnName = "sucursal";
        columna5.DefaultValue = null;
        dtNew.Columns.Add(columna5);

        DataColumn columna14 = new DataColumn();
        columna14.DataType = System.Type.GetType("System.String");
        columna14.AllowDBNull = true;
        columna14.Caption = "usuario";
        columna14.ColumnName = "usuario";
        columna14.DefaultValue = null;
        dtNew.Columns.Add(columna14);

        DataColumn columna6 = new DataColumn();
        columna6.DataType = System.Type.GetType("System.String");
        columna6.AllowDBNull = true;
        columna6.Caption = "documento";
        columna6.ColumnName = "documento";
        columna6.DefaultValue = null;
        dtNew.Columns.Add(columna6);

        DataColumn columna7 = new DataColumn();
        columna7.DataType = System.Type.GetType("System.Double");
        columna7.AllowDBNull = true;
        columna7.Caption = "total";
        columna7.ColumnName = "total";
        columna7.DefaultValue = null;
        dtNew.Columns.Add(columna7);

        DataColumn columna13 = new DataColumn();
        columna13.DataType = System.Type.GetType("System.String");
        columna13.AllowDBNull = true;
        columna13.Caption = "moneda";
        columna13.ColumnName = "moneda";
        columna13.DefaultValue = null;
        dtNew.Columns.Add(columna13);

        DataColumn columna8 = new DataColumn();
        columna8.DataType = System.Type.GetType("System.String");
        columna8.AllowDBNull = true;
        columna8.Caption = "fecha";
        columna8.ColumnName = "fecha";
        columna8.DefaultValue = null;
        dtNew.Columns.Add(columna8);

        DataColumn columna9 = new DataColumn();
        columna9.DataType = System.Type.GetType("System.String");
        columna9.AllowDBNull = true;
        columna9.Caption = "estatus";
        columna9.ColumnName = "estatus";
        columna9.DefaultValue = null;
        dtNew.Columns.Add(columna9);

        DataColumn columna10 = new DataColumn();
        columna10.DataType = System.Type.GetType("System.String");
        columna10.AllowDBNull = true;
        columna10.Caption = "rfc";
        columna10.ColumnName = "rfc";
        columna10.DefaultValue = null;
        dtNew.Columns.Add(columna10);

        DataColumn columna11 = new DataColumn();
        columna11.DataType = System.Type.GetType("System.String");
        columna11.AllowDBNull = true;
        columna11.Caption = "fecha_cancelacion";
        columna11.ColumnName = "fecha_cancelacion";
        columna11.DefaultValue = null;
        dtNew.Columns.Add(columna11);

        DataColumn columna12 = new DataColumn();
        columna12.DataType = System.Type.GetType("System.String");
        columna12.AllowDBNull = true;
        columna12.Caption = "comentarios_cancelacion";
        columna12.ColumnName = "comentarios_cancelacion";
        columna12.DefaultValue = null;
        dtNew.Columns.Add(columna12);

        int nIndice = 0;

        foreach (DataRow renglon in dtAuxiliar.Rows)
        {
            DataRow nuevo = dtNew.NewRow();
            nuevo["UUID"] = renglon["UUIDM"];
            nuevo["serie"] = renglon["serie"];
            nuevo["folio"] = renglon["folio"];
            nuevo["razon_social"] = renglon["razon_social"];
            nuevo["sucursal"] = renglon["sucursal"];
            nuevo["usuario"] = renglon["usuario"];
            nuevo["documento"] = renglon["documento"];
            nuevo["total"] = renglon["total"];
            nuevo["moneda"] = renglon["moneda"];
            nuevo["fecha"] = renglon["fecha"];

            switch (Convert.ToString(renglon["estatus"]))
            {
                case "P":
                    nuevo["estatus"] = "Pendiente";
                    break;
                case "C":
                    nuevo["estatus"] = "Cancelado";
                    break;
                case "A":
                    nuevo["estatus"] = "Activo";
                    break;
                default:
                    nuevo["estatus"] = renglon["estatus"];
                    break;
            }

            nuevo["rfc"] = renglon["rfc"];
            nuevo["fecha_cancelacion"] = renglon["fecha_cancelacion"];
            nuevo["comentarios_cancelacion"] = renglon["comentarios_cancelacion"];
            dtNew.Rows.Add(nuevo);

            //Reporta progreso (20% - 90%)
            lock (Session.SyncRoot)
            {
                nIndice++;
                Session["estatusExcel"] = "Procesando: " + (((nIndice * 70) / dtAuxiliar.Rows.Count) + 20) + "%";
            }
        }
        ArrayList ejemplo = new ArrayList();
        ejemplo.Add("UUID");
        ejemplo.Add("Serie");
        ejemplo.Add("folio");
        ejemplo.Add("razon_social");
        ejemplo.Add("sucursal");
        ejemplo.Add("usuario");
        ejemplo.Add("documento");
        ejemplo.Add("total");
        ejemplo.Add("moneda");
        ejemplo.Add("fecha");
        ejemplo.Add("estatus");
        ejemplo.Add("rfc");
        ejemplo.Add("fecha_cancelacion");
        ejemplo.Add("comentarios_cancelacion");

        ExportarExc(ejemplo, dtNew);
    }

    public void ExportarExc(ArrayList titulos, DataTable datos)
    {
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