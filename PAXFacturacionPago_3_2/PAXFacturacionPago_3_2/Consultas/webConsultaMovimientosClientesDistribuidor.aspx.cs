using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using System.Globalization;


public partial class Consultas_webConsultaMovimientosClientesDistribuidor : System.Web.UI.Page
{
    private clsInicioSesionUsuario datosUsuario;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                fnCargaUsuarios();
                txtFechaInicio.Text = Convert.ToString(DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year);
                txtFechaFin.Text = Convert.ToString(DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year);                
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            }
        }
    }

    public void fnCargaUsuarios()
    {
        clsOperacionDistribuidores distribuidores = new clsOperacionDistribuidores();
        datosUsuario = clsComun.fnUsuarioEnSesion();
        DataTable dist1 = distribuidores.fnObtieneDistribuidoresporidUsuario(datosUsuario.id_usuario);
        if (dist1.Rows.Count > 0)
        {
            DataTable UsuariosDist = distribuidores.fnObtieneUsuariosporDistribuidor(Convert.ToInt32(dist1.Rows[0]["id_distribuidor"]));
            ddlUsuario.DataSource = UsuariosDist;
            ddlUsuario.DataValueField = "id_usuario";
            ddlUsuario.DataTextField = "clave_usuario";

            DataTable DRUser = distribuidores.fnObtenerDatosUsuario();

            DataRow drFila = UsuariosDist.NewRow();
            drFila["id_usuario"] = datosUsuario.id_usuario;
            drFila["clave_usuario"] = DRUser.Rows[0]["clave_usuario"].ToString();

            txtUDistrib.Text = datosUsuario.userName;
            ddlUsuario.DataBind();
        }
    }
    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        try
        {
            clsOperacionDistribuidores dist = new clsOperacionDistribuidores();
            DataSet dsAuxiliar = new DataSet();

            if (ddlUsuario.Enabled == true)
            {
                dsAuxiliar = dist.fnObtieneReporteDistribuidor(Convert.ToInt32(ddlUsuario.SelectedValue), Convert.ToDateTime(txtFechaInicio.Text), Convert.ToDateTime(txtFechaFin.Text));
            }
            else 
            {
                datosUsuario = clsComun.fnUsuarioEnSesion();
                dsAuxiliar = dist.fnObtieneReporteDistribuidor(Convert.ToInt32(datosUsuario.id_usuario), Convert.ToDateTime(txtFechaInicio.Text), Convert.ToDateTime(txtFechaFin.Text));
            }
            
            if (dsAuxiliar.Tables[0].Rows.Count > 0)
            {
                gvdDetalleDoc1.DataSource = dsAuxiliar.Tables[0];
                gvdDetalleDoc1.DataBind();
                Session["dsAuxiliar"] = dsAuxiliar;
                btnExportar.Visible = true;
                PanelTimbrados.Visible = true;

            }
            else
            {
                Session["dsAuxiliar"] = null;
                btnExportar.Visible = false;
            }
           
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    protected void btnExportar_Click(object sender, EventArgs e)
    {
        try
        {

            string sIDS = "";
            
            if (ddlUsuario.Enabled == true)
            {
                sIDS += "" + ddlUsuario.SelectedValue + "," + txtFechaInicio.Text + "," + txtFechaFin.Text;
            }
            else
            {
                datosUsuario = clsComun.fnUsuarioEnSesion();
                sIDS += "" + datosUsuario.id_usuario + "," + txtFechaInicio.Text + "," + txtFechaFin.Text;
            }

            //sIDS = Utilerias.Encriptacion.Base64.EncriptarBase64(sIDS);
            sIDS = PAXCrypto.CryptoAES.EncriptarAES64(sIDS);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "webDescargaMovClientesDistribuidor.aspx",
                                                        String.Format("<script>window.open('{0}?p={1}','123','height=160, width= 355, status=yes, resizable= no, scrollbars= no, toolbar= no,menubar= no');</script>",
                                                                       "webDescargaMovClientesDistribuidor.aspx", sIDS), false);
        }
        catch (ThreadAbortException)
        {
            //No se registra algun error por la descarga del archivo de excel
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    protected void gdvTimbrados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gdvTimbrados.PageIndex = e.NewPageIndex;
            clsOperacionDistribuidores dist = new clsOperacionDistribuidores();
            DataSet dsAuxiliar = new DataSet();
            dsAuxiliar = dist.fnObtieneReporteDistribuidor(Convert.ToInt32(ddlUsuario.SelectedValue), Convert.ToDateTime(txtFechaInicio.Text), Convert.ToDateTime(txtFechaFin.Text));
            if (dsAuxiliar.Tables[1].Rows.Count > 0)
            {
                gdvTimbrados.DataSource = dsAuxiliar.Tables[1];
                gdvTimbrados.DataBind();
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    
    public void ExportarExc(DataSet datos)
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


            //Generando encabezados del archivo de Timbrado
            //(aquí podemos dar el formato como a una tabla de HTML)
            html.Append(@"<table WIDTH=730 CELLSPACING=0 CELLPADDING=10 
                    border=8 BORDERCOLOR=" + comillas + "#333366"
                        + comillas + " bgcolor=" + comillas + "#FFFFFF"
                        + comillas + ">");
            html.Append(@"<tr> <b>");
           
            for (int j = 0; j < datos.Tables[0].Columns.Count; j++)
            {
                html.Append(@"<th>" +
                    datos.Tables[0].Columns[j].ToString() + "</th>");
            }
            html.Append(@" </b> </tr>");
            
            //Generando datos del archivo
            for (int i = 0; i < datos.Tables[0].Rows.Count; i++)
            {
                html.Append(@"<tr>");
                for (int j = 0; j < datos.Tables[0].Columns.Count; j++)
                {
                    html.Append(@"<td>" +
                                datos.Tables[0].Rows[i][j].ToString() + "</td>");
                }
                html.Append(@"</tr>");
            }

            html.Append(@"<table WIDTH=730 CELLSPACING=0 CELLPADDING=10 
                    border=8 BORDERCOLOR=" + comillas + "White"
           + comillas + " bgcolor=" + comillas + "#FFFFFF"
           + comillas + ">");
            html.Append(@"<tr> <b>");

           // html.Append(@"<th>" + "</th>");

            ////////////////7
            //Generando encabezados del archivo  
            //(aquí podemos dar el formato como a una tabla de HTML)
            html.Append(@"<table WIDTH=730 CELLSPACING=0 CELLPADDING=10 
                    border=8 BORDERCOLOR=" + comillas + "White"
                        + comillas + " bgcolor=" + comillas + "FFFFFF"
                        + comillas + ">");
            html.Append(@"<tr> <b>");

            html.Append(@"</body>");
            html.Append(@"</html>");
            w.Write(html.ToString());
            w.Close();
            fnDescargaArchivo(rutas);

        }
        catch (Exception ex)
        {

        }

    }

    /// <summary>
    /// Funcion encargada de descargar los archivos del servidor.
    /// </summary>
    /// <param name="filename"></param>
    private void fnDescargaArchivo(string filename)
    {
        try
        {
            if (!String.IsNullOrEmpty(filename))
            {
                String path = filename;

                System.IO.FileInfo toDownload = new System.IO.FileInfo(path);

                if (toDownload.Exists)
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
                    Response.AddHeader("Content-Length", toDownload.Length.ToString());
                    Response.Charset = "UTF-8";
                    Response.ContentEncoding = Encoding.Default;
                    Response.WriteFile(path);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
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

    protected void Uchkbox_CheckedChanged(object sender, EventArgs e)
    {
        if (Uchkbox.Checked == true)
        {
            ddlUsuario.Enabled = false;
        }
        else 
        {
            ddlUsuario.Enabled = true;
        }
    }
}