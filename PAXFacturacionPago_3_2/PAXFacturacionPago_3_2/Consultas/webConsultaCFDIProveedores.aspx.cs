using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Threading;

public partial class Consultas_webConsultaCFDIProveedores : System.Web.UI.Page
{
    private clsOperacionConsulta gDAL;
    clsInicioSesionUsuario datosUsuario;
    protected DataTable dtCreditos;

    protected void Page_Load(object sender, EventArgs e)
    {
        clsComun.fnPonerTitulo(this);

        if (!IsPostBack)
        {
            fnCargarProveedores();
         

            //Establecemos el filtro de fechas para el día d ehoy
            //txtFechaFin_CalendarExtender.SelectedDate = DateTime.Now;
            //txtFechaIni_CalendarExtender.SelectedDate = DateTime.Now;
            //txtFechaFinRec_CalendarExtender.SelectedDate = DateTime.Now;
            //txtFechaIniRec_CalendarExtender.SelectedDate = DateTime.Now;

            fnRegistrarScript();
            ViewState["GuidPathXMLs"] = Guid.NewGuid().ToString();
            ViewState["GuidPathZIPs"] = Guid.NewGuid().ToString();

            //Directory.CreateDirectory(clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"]);
            //Directory.CreateDirectory(clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"]);
            //LimpiaCarpetas();

            //Revisar los creditos disponibles.
            datosUsuario = clsComun.fnUsuarioEnSesion();
            dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), "Timbrado", 'A', 'N');
            ViewState["dtCreditos"] = dtCreditos;
            //if (dtCreditos.Rows.Count == 0)
            //{

            //    modalRevisaCreditos.Show();
            //}
            if (dtCreditos.Rows.Count > 0)
            {
                double creditos = 0;
                creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                if (creditos == 0)
                {
                    clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                    dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                    ViewState["dtCreditos"] = dtCreditos;
                    double creditos2 = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                    if (creditos2 == 0)
                    {
                        modalRevisaCreditos.Show();
                    }
                }

            }
            else
            {
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                ViewState["dtCreditos"] = dtCreditos;
                double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                if (creditos == 0)
                {
                    modalRevisaCreditos.Show();
                }
            }

            fnActualizarLblCreditos();

        }
    }

    /// <summary>
    /// Carga los receptores activos para usarlos en los filtros
    /// </summary>
    private void fnCargarProveedores()
    {
        try
        {
            gDAL = new clsOperacionConsulta();
             datosUsuario = clsComun.fnUsuarioEnSesion();
            ddlReceptor.DataSource = gDAL.fnObtenerProveedores(datosUsuario.rfc);
            ddlReceptor.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            ddlReceptor.DataSource = null;
            ddlReceptor.DataBind();
        }
    }
    /// <summary>
    /// Registra el script de confirmación para la cancelación de un comprobante
    /// </summary>
    private void fnRegistrarScript()
    {
        string sScript = "function confirmacion(){ return confirm('" + Resources.resCorpusCFDIEs.varConfirmacionCancelacion + "'); }";
        ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "cancelacion", sScript, true);
    }
    protected void cbPaginado_CheckedChanged(object sender, EventArgs e)
    {
        if (cbPaginado.Checked)
        {
            gdvComprobantes.AllowPaging = true;
            gdvComprobantes.PageSize = 10;
            Panel234.ScrollBars = ScrollBars.None;
            fnRealizarConsulta();
        }
        else
        {
            gdvComprobantes.AllowPaging = false;
            Panel234.ScrollBars = ScrollBars.Auto;
            fnRealizarConsulta();
        }
    }
    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        fnRealizarConsulta();
    }

    private void fnRealizarConsulta()
    {
        gDAL = new clsOperacionConsulta();

        try
        {
            string psFechastring = string.Empty;
            string psFechastring2 = string.Empty; 
            string psFechastring3= string.Empty;
            string psFechastring4 = string.Empty;

            datosUsuario = clsComun.fnUsuarioEnSesion();
            if(!(txtFechaIni.Text == string.Empty))
            {
            DateTime fechacut1 = Convert.ToDateTime(txtFechaIni.Text);
             psFechastring= fechacut1.ToString("yyyyMMdd");
            }
            if(!(txtFechaFin.Text == string.Empty))
            {
           DateTime fechacut2 = Convert.ToDateTime(txtFechaFin.Text);
            psFechastring2 = fechacut2.ToString("yyyyMMdd");
            }
            if (!(txtFechaIniRec.Text == string.Empty))
            {
                DateTime fechacut3 = Convert.ToDateTime(txtFechaIniRec.Text);
                 psFechastring3 = fechacut3.ToString("yyyyMMdd");
            }
            if (!(txtFechaFinRec.Text == string.Empty))
            {
                DateTime fechacut4 = Convert.ToDateTime(txtFechaFinRec.Text);
                 psFechastring4 = fechacut4.ToString("yyyyMMdd");
            }
           DataTable Proveedores = new DataTable();
           Proveedores = gDAL.fnObtenerComprobantesProveedor(
               datosUsuario.rfc,
                psFechastring,
                psFechastring2,
               ddlReceptor.SelectedValue, psFechastring3, psFechastring4);
           gdvComprobantes.DataSource = Proveedores;
            gdvComprobantes.DataBind();
            ViewState["ExportarExcel"] = gdvComprobantes.DataSource;
            if (gdvComprobantes.Rows.Count > 0)
            {
              
                btnDescargar.Visible = true;
                cbSeleccionar.Visible = true;
                cbPaginado.Visible = true;
                btnExportar.Visible = true;
            }
            else
            {
               
                btnDescargar.Visible = false;
                cbSeleccionar.Visible = false;
                cbPaginado.Visible = false;
                btnExportar.Visible = false;
            }
            clsComun.fnNuevaPistaAuditoria(
                "webConsultasCFDI",
                "fnObtenerComprobantes",
                "Se consultó con los filtros",
                ddlReceptor.SelectedItem.Text,
                txtFechaIni.Text,
                txtFechaFin.Text
                );
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void btnDescargar_Click(object sender, EventArgs e)
    {
        try
        {
            int bandera = 0;
            byte[] buffer = { };
           
            ArrayList Final = new ArrayList();




            Directory.CreateDirectory(clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"]);
            Directory.CreateDirectory(clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"]);

            //string newPath = clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\";
            //string newPath = clsComun.ObtenerParamentro("RutaDocXmlZips");//+ ViewState["GuidPathXMLs"] + "\\";

            foreach (GridViewRow renglon in gdvComprobantes.Rows)
            {
                CheckBox CbCan;

                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                if (CbCan.Checked)
                {
                    Guid Gid;
                    Gid = Guid.NewGuid();

                    Label sIdCfd = ((Label)renglon.FindControl("lblid_cfd"));

                    gDAL = new clsOperacionConsulta();
                    XmlDocument comprobante = new XmlDocument();
                    string sTipoDocumento = HttpUtility.UrlDecode(Request.QueryString["doc"]);

                    int nIdContribuyente = clsComun.fnUsuarioEnSesion().id_contribuyente;
                    comprobante = gDAL.fnObtenerComprobanteXMLProveedores(Convert.ToInt32(sIdCfd.Text));

                    Byte[] byto = { };


                    byto = gDAL.fnObtenerPDFProveedor(Convert.ToInt32(sIdCfd.Text));
                
                    string pathPDF = clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + Gid + ".pdf";



                    if (!File.Exists(pathPDF))
                    {
                        File.WriteAllBytes(pathPDF, byto);
                    }

                    clsComun.fnNuevaPistaAuditoria(
                        "webConsultasGeneradorPDF",
                        "fnGenerarPDF",
                        "Se generó el PDF para el comprobante con ID " + sIdCfd.Text
                        );

                    bandera = 1;

                    buffer = Encoding.UTF8.GetBytes(comprobante.InnerXml);

                    string path = clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + Gid + ".xml";


                    // Create the text file if it doesn't already exist.
                    if (!File.Exists(path))
                    {
                       // fnActualizaCreditos();
                        File.WriteAllBytes(path, buffer);
                    }
                }
            }
            if (bandera == 1)
            {

                Guid Gid;
                Gid = Guid.NewGuid();

                string Ruta = clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"] + "\\" + Gid + ".zip";

                ICSharpCode.SharpZipLib.Zip.ZipOutputStream zip = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(File.Create(Ruta));
                zip.SetLevel(6);

                string folder = clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"];

                ComprimirCarpeta(folder, folder, zip);

                zip.Finish();
                zip.Close();

                foreach (string file in Directory.GetFiles(clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"]))
                {
                    File.Delete(file);
                }
                Directory.Delete(clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"], true);

                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.CacheControl = "public";
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + Gid + ".zip");
                //Response.Write(Ruta);
                //Response.Flush();
                //Response.TransmitFile(Ruta);
                Response.BinaryWrite(File.ReadAllBytes(Ruta));
                Response.Flush();
                Response.Close();

                //Response.Close();
                LimpiaCarpetas();


            }
        }
        catch (System.Web.Services.Protocols.SoapException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    public static void ComprimirCarpeta(string RootFolder, string CurrentFolder, ICSharpCode.SharpZipLib.Zip.ZipOutputStream zStream)
    {
        string[] SubFolders = Directory.GetDirectories(CurrentFolder);

        //Llama de nuevo al metodo recursivamente para cada carpeta
        foreach (string Folder in SubFolders)
        {
            ComprimirCarpeta(RootFolder, Folder, zStream);
        }

        string relativePath = CurrentFolder.Substring(RootFolder.Length) + "/";

        //the path "/" is not added or a folder will be created
        //at the root of the file
        if (relativePath.Length > 1)
        {
            ICSharpCode.SharpZipLib.Zip.ZipEntry dirEntry;
            dirEntry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(relativePath);

        }

        //Añade todos los ficheros de la carpeta al zip
        foreach (string file in Directory.GetFiles(CurrentFolder))
        {
            AñadirFicheroaZip(zStream, relativePath, file);
        }
    }

    private static void AñadirFicheroaZip(ICSharpCode.SharpZipLib.Zip.ZipOutputStream zStream, string relativePath, string file)
    {
        byte[] buffer = new byte[4096];

        //the relative path is added to the file in order to place the file within
        //this directory in the zip
        string fileRelativePath = (relativePath.Length > 1 ? relativePath : string.Empty)
                                  + Path.GetFileName(file);

        ICSharpCode.SharpZipLib.Zip.ZipEntry entry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(fileRelativePath);

        zStream.PutNextEntry(entry);

        using (FileStream fs = File.OpenRead(file))
        {
            int sourceBytes;
            do
            {
                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                zStream.Write(buffer, 0, sourceBytes);
            } while (sourceBytes > 0);
        }
    }
    protected void btnExportar_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)(ViewState["ExportarExcel"]);
        dataTableAExcel(dt);    
    }

    private void dataTableAExcel(DataTable tabla)
    {
        ScriptManager SM = ScriptManager.GetCurrent(this);
        SM.RegisterPostBackControl(btnExportar);
        if (tabla.Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            System.Web.UI.Page pagina = new System.Web.UI.Page();
            HtmlForm form = new HtmlForm();
            GridView dg = new GridView();
            dg.EnableViewState = false;
            dg.DataSource = tabla;
            dg.DataBind();
            pagina.EnableEventValidation = false;
            pagina.DesignerInitialize();
            pagina.Controls.Add(form);
            form.Controls.Add(dg);
            pagina.RenderControl(htw);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Guid.NewGuid().ToString() + ".xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }
    }

    public void LimpiaCarpetas()
    {

        foreach (string file in Directory.GetFiles(clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"]))
        {
            File.Delete(file);
        }
        Directory.Delete(clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"], true);
        Directory.Delete(clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"], true);
        
    }
    protected void cbSeleccionar_CheckedChanged(object sender, EventArgs e)
    {
        if (cbSeleccionar.Checked)
        {
            foreach (GridViewRow renglon in gdvComprobantes.Rows)
            {
                CheckBox CbCan;

                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                CbCan.Checked = true;

            }
        }
        else
        {
            foreach (GridViewRow renglon in gdvComprobantes.Rows)
            {
                CheckBox CbCan;

                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                CbCan.Checked = false;

            }
        }
    }
  
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        gDAL = new clsOperacionConsulta();
 
    }
    protected void gdvComprobantes_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ImageButton Boton1 = (ImageButton)sender;

        //     string sIdCfd = this.gdvComprobantes.DataKeys(this.gdvComprobantes.SelectedIndex).Values("Descripcion");
    }
    protected void gdvComprobantes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ScriptManager SM = ScriptManager.GetCurrent(this);
        SM.RegisterPostBackControl(gdvComprobantes);

        if (e.CommandName == "xml")
        {
            int index = Convert.ToInt32(e.CommandArgument);
           
            GridView gdvComprobante = (GridView)e.CommandSource;
            GridViewRow row = gdvComprobante.Rows[index];
            Label IdCfd = (Label)row.Cells[9].Controls[1];
            int sIdCfd = Convert.ToInt32(IdCfd.Text);
            gDAL = new clsOperacionConsulta();
            XmlDocument comprobante = new XmlDocument();
            
            try
            {
                comprobante = gDAL.fnObtenerComprobanteXMLProveedor(sIdCfd);

                clsComun.fnNuevaPistaAuditoria(
                    "webConsultasGeneradorXML",
                    "fnGenerarXML",
                    "Se generó el XML para el comprobante con ID " + sIdCfd
                    );

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "text/xml";
                Response.AddHeader("Content-Disposition", "attachment; filename="+Guid.NewGuid()+".xml");
                Response.Write(comprobante.DocumentElement.OuterXml);
                //Response.Write(comprobante.OuterXml);
                Response.End();

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorDescarga);
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorDescarga);
            }
        }
        if (e.CommandName == "pdf")
        {
            int index = Convert.ToInt32(e.CommandArgument);

            GridView gdvComprobante = (GridView)e.CommandSource;            
            GridViewRow row = gdvComprobante.Rows[index];
            Label IdCfd = (Label)row.Cells[9].Controls[1];
            gDAL = new clsOperacionConsulta();
            MemoryStream ms = new MemoryStream(gDAL.fnObtenerComprobantePDFProveedor(Convert.ToInt32(IdCfd.Text)));
            if (ms.Length > 0)
            {
                ms.Position = 0;
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.ContentEncoding = System.Text.Encoding.UTF7;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Charset = "";
                this.EnableViewState = false;

                byte[] fileData = null;
                fileData = new byte[ms.Length + 1];
                long bytesRead = ms.Read(fileData, 0, Convert.ToInt32(ms.Length));
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("content-disposition", "attachment; filename=" + Guid.NewGuid() + ".pdf");
                Response.AddHeader("Content-Length", fileData.Length.ToString());
                Response.BinaryWrite(fileData);
                Response.Flush();
            }
            else
            {
                abreVentana("webConsultasGeneradorPDF.aspx?nic=" + HttpUtility.UrlEncode(IdCfd.Text) + "&doc=" + HttpUtility.UrlEncode("factura") + "&ver=" + HttpUtility.UrlEncode("1"));                
            }
    
        }
    }
    //Metodo para abrir una pagina en otra pestaña en el explorador.
    private void abreVentana(string ventana)
    {
        string Clientscript = "<script>window.open('" +
                              ventana +
                              "')</script>";

        if (!this.IsStartupScriptRegistered("WOpen"))
        {
            this.RegisterStartupScript("WOpen", Clientscript);
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

    protected void gdvComprobantes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gdvComprobantes.PageIndex = e.NewPageIndex;
            fnRealizarConsulta();
        }
        catch
        {
        }
    }

    /// <summary>
    /// Actauliza los creditos disponibles.
    /// </summary>
    private void fnActualizaCreditos()
    {
        DataTable tlbCreditos = new DataTable();
        int idCredito = 0;
        int idEstructura = 0;
        double Creditos = 0;
        int nRetorno = 0;

        tlbCreditos = (DataTable)ViewState["dtCreditos"];

        if (tlbCreditos.Rows.Count > 0)
        {

            idCredito = Convert.ToInt32(tlbCreditos.Rows[0]["id_creditos"]);
            idEstructura = Convert.ToInt32(tlbCreditos.Rows[0]["id_estructura"]);
            Creditos = Convert.ToDouble(tlbCreditos.Rows[0]["creditos"]);

            nRetorno = clsTimbradoFuncionalidad.fnActualizarCreditos(idCredito, idEstructura, Creditos,"RP");


            datosUsuario = clsComun.fnUsuarioEnSesion();
            dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), string.Empty, 'A', 'S');
            if (dtCreditos.Rows.Count > 0)
            {
                fnActualizarLblCreditos();
            }
            else
            {
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
                if (creditos > 0)
                {
                    fnActualizarLblCreditos();
                }
            }

        }


    }

    /// <summary>
    /// Actualizar etiqueta de Creditos.
    /// </summary>
    private void fnActualizarLblCreditos()
    {
        datosUsuario = clsComun.fnUsuarioEnSesion();
        dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), string.Empty, 'A', 'S');
        if (dtCreditos.Rows.Count > 0)
        {
            if (dtCreditos.Rows[0]["creditos"].ToString() == "0.00")
            {
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
                if (creditos == 0)
                {
                    lblCredValor.Text = "0";
                }
                else
                {
                    lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                }
            }
            else
            {
                lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
            }
        }
        else
        {
            clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
            dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
            double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
            if (creditos == 0)
            {
                lblCredValor.Text = "0";
            }
            else
            {
                lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
            }
        }
    }
    protected void btnAceptCred_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }
}
