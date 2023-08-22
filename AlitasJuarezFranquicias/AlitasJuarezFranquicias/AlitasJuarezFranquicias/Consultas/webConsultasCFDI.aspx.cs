using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Xml;
using System.Threading;
using System.Globalization;
using System.IO.Compression;
using System.IO;
using System.Collections;
using Ionic.Zip;
using ICSharpCode.SharpZipLib.Zip;
using Root.Reports;
using System.Web.UI.HtmlControls;
using System.Xml.XPath;
using System.ServiceModel.Channels;
using System.Net;
//Cancelacion SAT
//using Cancelacion = ServicioCancelacionCFDI;
//using AutenticacionCancelacion = ServicioCancelacionAutenticacionCFDI;

using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using System.ServiceModel;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;


/// <summary>
/// Pantalla para la consulta, descarga y cancelación de CFDIs
/// </summary>
public partial class Consultas_webConsultasCFDI : System.Web.UI.Page
{
    private clsOperacionConsulta gDAL;
    private clsOperacionClientes gOp;
   // private Cancelacion.CancelaCFDBindingClient clienteCancelacion;
    //private AutenticacionCancelacion.AutenticacionClient clienteAutenticacionCancelacion;
    private clsInicioSesionUsuario DatosUsuario;
    protected DataTable dtCreditos;
    string Respuesta = string.Empty;
    private DataTable DTCompMail;
    private clsOperacionUsuarios gOu;
    private clsEnvioCorreoDocs cEd;
    private clsInicioSesionUsuario datosUsuario;
    private Thread hilo;
    private static int valor = 0;
    public delegate DataTable Operacion();
    private static DataSet creditosT;
    private clsOperacionCuenta gOpeCuenta;
    //private clsConfiguracionCreditos cCc;

  

    protected void Page_Load(object sender, EventArgs e)
    {
       //clsComun.fnPonerTitulo(this);
       // int pIdusuario= clsComun.fnUsuarioEnSesion().id_usuario;       
        try
        {
            if (Session["objUsuario"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                string sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
                if (!new clsOperacionUsuarios().fnObtenerPermisoPagina(sPagina))
                {
                    lblErrorLog.Text = Resources.resCorpusCFDIEs.varPermisos;
                    mpeErrorLog.Show();
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varPermisos);
                    Response.Redirect("~/Default.aspx");
                }

            }
            //string Fecha1 = (string)ViewState["fecha1"];
            //string Fecha2 = (string)ViewState["fecha2"];
            //if (Fecha1 != null)
            //{
            //    txtFechaIni_CalendarExtender.SelectedDate = Convert.ToDateTime(Fecha1);
            //}
            //else
            //{
            //    txtFechaIni_CalendarExtender.SelectedDate = DateTime.Now;
            //}

            //if (Fecha2 != null)
            //{
            //    txtFechaFin_CalendarExtender.SelectedDate = Convert.ToDateTime(Fecha2);
            //}
            //else
            //{
            //    txtFechaFin_CalendarExtender.SelectedDate = DateTime.Now;
            //}

            if (!IsPostBack)
            {
                btnAnterior.Visible = false;
                btnSiguiente.Visible = false;
                fnCargarEstatus();
                fnCargarSucursales();

                txtFechaIni.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaFin.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //fnCargarDocumentos();
                //fnCargcarSeries();
                //fnCargarDatosPlantillas();
                datosUsuario = clsComun.fnUsuarioEnSesion();
                //clsConfiguracionPlantilla Plantilla = new clsConfiguracionPlantilla();
                //DataTable Plantillas = new DataTable();
                //int idEstructura = Convert.ToInt32(ddlSucursales.Items[1].Value);
                //Plantillas = Plantilla.fnObtieneConfiguracionPlantilla(idEstructura);

                //int nPlantilla = Plantilla.fnRecuperaPlantillaRecursiva(idEstructura);
                //if (idEstructura != 0)
                //{
                //    if (Plantillas.Rows.Count > 0)
                //    {
                //        datosUsuario.plantilla = Convert.ToInt32(Plantillas.Rows[0]["id_plantilla"]);
                //        datosUsuario.color = Convert.ToString(Plantillas.Rows[0]["color"]);
                //    }
                //    else if (nPlantilla != 0)
                //    {
                //        datosUsuario.plantilla = nPlantilla;

                //    }
                //}


                //gOpeCuenta = new clsOperacionCuenta();
                //SqlDataReader sdrInfo = gOpeCuenta.fnObtenerDatosFiscales(datosUsuario.id_contribuyente);

                //if (sdrInfo != null && sdrInfo.HasRows && sdrInfo.Read())
                //{
                //    ViewState["rfc_Emisor"] = sdrInfo["rfc"].ToString();
                //    ViewState["razonSocial_Emisor"] = sdrInfo["razon_social"].ToString();
                //}

                fnRegistrarScript();
                ViewState["GuidPathXMLs"] = Guid.NewGuid().ToString();
                ViewState["GuidPathZIPs"] = Guid.NewGuid().ToString();
                rdlArchivo.SelectedIndex = 0;

                //btnAcepCreditos.OnClientClick = "return confirm('" + Resources.resCorpusCFDIEs.varConfirmacionCancelacionMasiva + "');";

                //fnActualizarLblCreditos();
            }
        }
        catch (Exception ex)
        { clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion); }

    }

    /// <summary>
    /// Registra el script de confirmación para la cancelación de un comprobante
    /// </summary>
    private void fnRegistrarScript()
    {
        string sScript = "function confirmacion(){ return confirm('" + Resources.resCorpusCFDIEs.varConfirmacionCancelacion + "'); }";
        ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "cancelacion", sScript, true);
    }
    
    /// <summary>
    /// Carga las sucursales disponibles para el usuario
    /// </summary>
    /// 
    private void fnCargarSucursales()
    {
        try
        {
            DataTable tblEstructura = new DataTable();
            datosUsuario = clsComun.fnUsuarioEnSesion();
            tblEstructura = clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario); //clsComun.LlenarDropSucursales(true);
            DataRow drFila = tblEstructura.NewRow();
            drFila["id_estructura"] = 0;
            drFila["nombre"] = Resources.resCorpusCFDIEs.VarDropTodos;

            tblEstructura.Rows.InsertAt(drFila, 0);

            ddlSucursales.DataSource = tblEstructura;
            //grvSucursales.DataSource = tblEstructura;
            ViewState["tblEstructura"] = tblEstructura;
            ddlSucursales.DataBind();

            fnCargarrfcReceptores(Convert.ToInt32(ddlSucursales.SelectedValue));
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);

        }
        catch
        {
            //referencia nula
        }
    }

    /// <summary>
    /// Obtiene los rfcs de las estructuras
    /// </summary>
    /// <param name="pIdEstructura"></param>
    private void fnCargarrfcReceptores(int pIdEstructura)
    {
        try
        {
            gDAL = new clsOperacionConsulta();

            ddlrfcRecptor.DataSource = gDAL.fnObtenerReceptores(pIdEstructura);
            ddlrfcRecptor.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            ddlrfcRecptor.DataSource = null;
            ddlrfcRecptor.DataBind();
        }
    }
  
    /// <summary>
    /// Carga el drop con las opciones TODOS, ACTIVO y CANCELADO
    /// </summary>
    private void fnCargarEstatus()
    {
        gDAL = new clsOperacionConsulta();

        ddlEstatus.DataSource = gDAL.fnObtenerEstatus();
        ddlEstatus.DataBind();
    }

    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        //cbPaginado.Checked = true;
        //if (grvSucursales.Rows.Count > 0)
        //{
            //if (SucursalSeleccionada())
            //{
                gdvComprobantes.AllowPaging = true;
                bool condicion = fnRealizarConsulta();
                if (condicion != true)
                {
                    // cbPaginado.Checked = false;
                    gdvComprobantes.AllowPaging = false;
                }

                ViewState["fecha1"] = txtFechaIni.Text;
                ViewState["fecha2"] = txtFechaFin.Text;
            //}
            //else
            //{
            //    lblErrorLog.Text = Resources.resCorpusCFDIEs.varSucursalSeleccionar;
            //    mpeErrorLog.Show();
            //    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varSucursalSeleccionar);
            //}
        //}
        //else
        //{
        //    lblErrorLog.Text = Resources.resCorpusCFDIEs.varSucursalAsignada;
        //    mpeErrorLog.Show();
        //    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varSucursalAsignada);
        //}
    }
        
    /// <summary>
    /// Realiza la consulta de los comprobantes
    /// </summary>
    /// <returns></returns>
    private bool fnRealizarConsulta()
    {
        datosUsuario = clsComun.fnUsuarioEnSesion();
        ViewState["paginado"] = 1;
        gDAL = new clsOperacionConsulta();
        bool condicion = false;
        string sSucursalesPista = "";
        try
        {
            //DataTable resultado = new DataTable();
            //resultado.Columns.Add("id_cfd");
            //resultado.Columns.Add("cfd");
            //resultado.Columns.Add("razon_social");
            //resultado.Columns.Add("serie");
            //resultado.Columns.Add("folio");
            //resultado.Columns.Add("UUID");
            //resultado.Columns.Add("total");
            //resultado.Columns.Add("moneda");
            //resultado.Columns.Add("documento");
            //resultado.Columns.Add("sucursal");
            //resultado.Columns.Add("fecha");
            //resultado.Columns.Add("estatus");
            //resultado.Columns.Add("fecha_cancelacion");
            //resultado.Columns.Add("comentarios_cancelacion");
            //resultado.Columns.Add("RowNum");
            //resultado.Columns.Add("UUIDM");
            //resultado.Columns.Add("NumTicket");
            //if (grvSucursales.Rows.Count > 0)
            //{
            //    foreach (GridViewRow renglon in grvSucursales.Rows)
            //    {

            //        CheckBox cbCan;
            //        cbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
            //        //cbCan.Checked = false;
            //        if (cbCan.Checked)
            //        {
            //            Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
            //            sSucursalesPista += ((Label)renglon.FindControl("lblsucursal")).Text + ", ";
                        DataTable res = gDAL.fnObtenerComprobantes(
                            ddlEstatus.SelectedValue,
                            ddlSucursales.SelectedValue,
                            Convert.ToDateTime(txtFechaIni.Text),
                        Convert.ToDateTime(txtFechaFin.Text), 1, datosUsuario.id_usuario,
                        txtUUID.Text, ddlrfcRecptor.SelectedValue , txtNumticket.Text);
                        //foreach (DataRow row in res.Rows)
                        //{
                        //    resultado.ImportRow(row);
                        //}
            //        }
            //    }
            //}
            
            gdvComprobantes.DataSource = res;
            gdvComprobantes.DataBind();
            ViewState["ExportarExcel"] = gdvComprobantes.DataSource;
          
            if (gdvComprobantes.Rows.Count > 0)
            {
                btnCancelar.Visible = true;
                btnDescargar.Visible = true;
                cbSeleccionar.Visible = true;
               // cbPaginado.Visible = true;
                btnExportar.Visible = true;
                condicion = true;
                bool bPaginas = fnVerificarPaginas(2);
                if (bPaginas == true)
                {
                    btnSiguiente.Visible = true;
                    btnAnterior.Visible = false;
                }
                else
                {
                    btnSiguiente.Visible = false;
                    btnAnterior.Visible = false;
                }
            }
            else
            {
                btnCancelar.Visible = false;
                btnDescargar.Visible = false;
                cbSeleccionar.Visible = false;
             //   cbPaginado.Visible = false;
                btnExportar.Visible = false;
                condicion = false;
                btnSiguiente.Visible = false;
                btnAnterior.Visible = false;
            }

            cbSeleccionar.Checked = false;
            clsComun.fnNuevaPistaAuditoria(
                "webConsultasCFDI",
                "fnObtenerComprobantes",
                "Se consultó con los filtros",
                ddlEstatus.SelectedItem.Text,
                sSucursalesPista,
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
        return condicion;
    }

    protected void gdvComprobantes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //gdvComprobantes.PageIndex = e.NewPageIndex;
        //bool condicion = fnRealizarConsulta();
        //if (condicion == true)
        //{
        //    cbPaginado.Checked = true;
        //}
        //else
        //{
        //    cbPaginado.Checked = false;
        //}
    }

    protected void hpCancelar_Click(object sender, EventArgs e)
    {
        try
        {
            //Valida si el usuario cuenta con el servicio de cancelación
            bool bValSer = true;
            //bValSer = fnVerificarServicio();
            if (bValSer == false)
                return;

            bool bValCre = true;
            //Valida si contiene créditos suficientes
            //bValCre = fnActualizarLblCreditosCancelación();
            if (bValCre == false)
                return;

            string sIdCfd = ((LinkButton)sender).CommandArgument;
            foreach (GridViewRow renglon in gdvComprobantes.Rows)
            {
                
                CheckBox CbCan;

                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                Label sIdCFDcheck = (Label)(renglon.FindControl("lblid_cfd"));

                if (sIdCfd == sIdCFDcheck.Text)
                {
                    Label srfc = (Label)(renglon.FindControl("lblrfcReceptor"));
                    //ViewState["rfc"] = srfc.Text;
                    Label lblUUID = (Label)(renglon.FindControl("lblUUID"));
                    string[] sAUUID = {lblUUID.Text};
                    ViewState["UUID"] = sAUUID;
                    CbCan.Checked = true;
                }
            }
            ViewState["nIdCfd"] = sIdCfd;
            ViewState["nOrigen"] = 0;
            modalCreditos.Show();
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

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ////Valida si el usuario cuenta con el servicio de cancelación
        //bool bValSer = true;
        //bValSer = fnVerificarServicio();
        //if (bValSer == false)
        //    return;

        //bool bValCre = true;
        ////Valida si contiene créditos suficientes
        //bValCre = fnActualizarLblCreditosCancelación();
        //if (bValCre == false)
        //    return;

        int bander = 0;

        foreach (GridViewRow renglon in gdvComprobantes.Rows)
        {
            CheckBox CbCan;

            CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
            if (CbCan.Checked == true && renglon.Cells[10].Text == "A")
            {
                bander = 1;
            }
        }

        if (bander == 1)
        {
            ViewState["nOrigen"] = 1;
            modalCreditos.Show();
        }
        else
        {
            lblAviso.Text = Resources.resCorpusCFDIEs.varCancelaVacio;
            mpeAvisos.Show();
            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCancelaVacio);
        }
       
    }

    /// <summary>
    /// comprime en un zip todos los xml y pdf contenidos en las filas seleccionadas del grid
    /// </summary>
    protected void btnDescargar_Click(object sender, EventArgs e)
    {
        try
        {
            int bandera = 0;
            byte[] buffer = { };
            byte[] bufferPDF = { };
            ArrayList Final = new ArrayList();

            //double retCreditos = 0;

            //retCreditos = fnRevisaCreditos();

            //if (retCreditos > 0)
            //{
                Directory.CreateDirectory(clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"]);
                Directory.CreateDirectory(clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"]);

                string snombreDoc = string.Empty;
                int i = 0;
                foreach (GridViewRow renglon in gdvComprobantes.Rows)
                {
                    //CheckBox CbCan;

                    //CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                    //if (CbCan.Checked)
                    //{
                        Guid Gid;
                        Gid = Guid.NewGuid();

                        Label sIdCfd = ((Label)renglon.FindControl("lblid_cfd"));

                        gDAL = new clsOperacionConsulta();
                        XmlDocument comprobante = new XmlDocument();
                        string sTipoDocumento = clsComun.ObtenerParamentro("TipoComprobante"); //HttpUtility.HtmlDecode(renglon.Cells[8].Text);
                        snombreDoc = renglon.Cells[3].Text;
                       // string sTipoDocumento = HttpUtility.UrlDecode(Request.QueryString["doc"]);
                        int nIdContribuyente = clsComun.fnUsuarioEnSesion().id_contribuyente;
                        comprobante = gDAL.fnObtenerComprobanteXML(nIdContribuyente, sIdCfd.Text);

                        clsPlantillaLista pdf = new clsPlantillaLista();
                        //clsOperacionConsultaPdf pdf;
                        //pdf = new clsOperacionConsultaPdf(gDAL.fnObtenerComprobanteXML(nIdContribuyente, sIdCfd.Text));

                        //if (!string.IsNullOrEmpty(sTipoDocumento))
                        //    pdf.TipoDocumento = sTipoDocumento.ToUpper();


                        string pathPDF = clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + snombreDoc + ".pdf";//Gid + ".pdf";

                        DatosUsuario = clsComun.fnUsuarioEnSesion();
                        pdf.fnCrearPLantillaEnvio(gDAL.fnObtenerComprobanteXML(nIdContribuyente, sIdCfd.Text), "Logo", sIdCfd.Text, sTipoDocumento, pathPDF, nIdContribuyente, DatosUsuario.id_rfc, "Black");
                        //pdf.fnObtenerPLantilla(gDAL.fnObtenerComprobanteXML(nIdContribuyente, sIdCfd.Text), string.Empty, string.Empty, sTipoDocumento, this, pathPDF, nIdContribuyente,DatosUsuario.id_rfc, DatosUsuario.color);

                        //pdf.fnGenerarPDFSave(pathPDF);


                        clsComun.fnNuevaPistaAuditoria(
                            "webConsultasGeneradorPDF",
                            "fnGenerarPDF",
                            "Se generó el PDF para el comprobante con ID " + sIdCfd.Text
                            );

                        bandera = 1;

                        buffer = Encoding.UTF8.GetBytes(comprobante.InnerXml);

                        string path = clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + snombreDoc + ".xml";//Gid + ".xml";


                        // Create the text file if it doesn't already exist.
                        if (!File.Exists(path))
                        {
                            //fnActualizaCreditos();
                            File.WriteAllBytes(path, buffer);
                        }
                    //}
                    i += 1; //Verifica si se selecciono mas de un registro
                }
                if (bandera == 1)
                {

                    Guid Gid;
                    Gid = Guid.NewGuid();

                    string Ruta = string.Empty; //clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"] + "\\" + Gid + ".zip";
                    if (i > 1) //Si se selecciono mas de un registro se guarda un nombre generico.
                        Ruta = clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"] + "\\" + Gid + ".zip";
                    else //Si selecciono un registro se guarda con el nombre del documento
                        Ruta = clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"] + "\\" + snombreDoc + ".zip";

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
                    Response.AddHeader("content-disposition", "attachment; filename=" + snombreDoc + ".zip");//Gid + ".zip");

                    Response.BinaryWrite(File.ReadAllBytes(Ruta));
                    Response.Flush();
                    Response.Close();


                    LimpiaCarpetas();


                }
                else
                {
                    lblErrorLog.Text = Resources.resCorpusCFDIEs.varDescargaVacio;
                    mpeErrorLog.Show();
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varDescargaVacio);
                }
            //}
            //else
            //{
            //    modalCreditos.Hide();
            //    modalRevisaCreditos.Show();
            //}
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

    /// <summary>
    /// Elimina las carpetas temporales que fueron creadas
    /// </summary>
    public void LimpiaCarpetas()
    {

        foreach (string file in Directory.GetFiles(clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"]))
        {
            File.Delete(file);
        }
        Directory.Delete(clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"], true);

    }

    /// <summary>
    /// herramienta para comprimir la carpeta segun la ruta
    /// </summary>
    public static void CompressFile(string path)
    {
        FileStream sourceFile = File.OpenRead(path);
        FileStream destinationFile = File.Create(path);

        byte[] buffer = new byte[sourceFile.Length];
        sourceFile.Read(buffer, 0, buffer.Length);

        using (GZipStream output = new GZipStream(destinationFile,
            CompressionMode.Compress))
        {
            Console.WriteLine("Compressing {0} to {1}.", sourceFile.Name,
                destinationFile.Name, false);

            output.Write(buffer, 0, buffer.Length);
        }

        // Close the files.
        sourceFile.Close();
        destinationFile.Close();
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

    /// <summary>
    /// selecciona todas las filas del gridview
    /// </summary>
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

    protected void cbPaginado_CheckedChanged(object sender, EventArgs e)
    {
        //if (cbPaginado.Checked)
        //{
        //    gdvComprobantes.AllowPaging = true;
        //    gdvComprobantes.PageSize = 10;
        //    Panel234.ScrollBars = ScrollBars.None;
        //    bool condicion = fnRealizarConsulta();
        //    if (condicion == true)
        //    {
        //        cbPaginado.Checked = true;
        //    }
        //    else
        //    {
        //        cbPaginado.Checked = false;
        //    }
        //}
        //else
        //{
        //    gdvComprobantes.AllowPaging = false;
        //    Panel234.ScrollBars = ScrollBars.Auto;
        //    fnRealizarConsulta();
            
        //}
    }

    /// <summary>
    /// Elimina todos los comprobantes seleccionados en el grid
    /// </summary>
    protected void btnAcepCreditos_Click(object sender, EventArgs e)
    {
        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);

        //Referencia al Web de Cancelacion
        wsCancelacion.wcfCancelaASMX wsCancelacion = new wsCancelacion.wcfCancelaASMX();
        datosUsuario = clsComun.fnUsuarioEnSesion();
        int nIdCfd = 0;

        try
        {
            ArrayList listUUid = new ArrayList();
            int bandera = 0;
            string sRespuesta = string.Empty;
            string[] sUUID = (string[])ViewState["UUID"];
            nIdCfd = Convert.ToInt32(ViewState["nIdCfd"]);
            double retCreditos = 0;
            string sMsjPrueba = string.Empty;

            retCreditos = 1;// fnRevisaCreditos();
            //Cancelacion modo Produccion o Pruebas
            //if (!(clsComun.ObtenerParamentro("CancelacionPrueba") == "true")) //Modo Pruebas
            //{
                string sUsuarioCobro = string.Empty;
                string sPassWordCobroWSDL = string.Empty;
                string sRfcEmisor = string.Empty;

                ///Se verifica desde donde se Cancela
                if (Convert.ToInt32(ViewState["nOrigen"]) > 0)
                {
                    //Se Cancela desde Boton Cancelar
                    string sIdCFD = string.Empty;
                    //Recorremos el GridView                                        
                    foreach (GridViewRow renglon in gdvComprobantes.Rows)
                    {
                        CheckBox CbCan;
                        CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                        //Validamos los comprobantes que esten Checados y esten en estatus A
                        if (CbCan.Checked == true && renglon.Cells[10].Text == "A")
                        {
                            Label sIdCFDcheck = (Label)(renglon.FindControl("lblid_cfd"));
                            sIdCFD = sIdCFDcheck.Text;
                            //Label srfc = (Label)(renglon.FindControl("lblrfcReceptor"));
                            Label lblUUID = (Label)(renglon.FindControl("lblUUID"));
                            listUUid.Add(lblUUID.Text);
                        }
                    }

                    nIdCfd = Convert.ToInt32(sIdCFD);

                    sUUID = new string[listUUid.Count];
                    for (int c = 0; c < listUUid.Count; c++)
                    {
                        string uuid = listUUid[c].ToString();
                        sUUID[c] = uuid;
                    }
                }

                //obtenemos los datos del Emisor para cancelar por medio del nIdcfd
                DataTable dtAuxiliar = clsComun.fnRecuperaridestructura(0, nIdCfd);
                sUsuarioCobro = dtAuxiliar.Rows[0]["clave_usuario"].ToString();
                sPassWordCobroWSDL = Utilerias.Encriptacion.Base64.EncriptarBase64(dtAuxiliar.Rows[0]["password"].ToString());
                sRfcEmisor = dtAuxiliar.Rows[1]["RFC"].ToString();
                
                ////se envia petición de cancelación por webservice
                sRespuesta = wsCancelacion.fnCancelarXML(sUUID, sRfcEmisor, 0, sUsuarioCobro, sPassWordCobroWSDL);
                sRespuesta = sRespuesta.Replace("&", "&amp;");
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.PreserveWhitespace = false;
                xmldoc.LoadXml(sRespuesta);

                //txtcomentario.Text = String.Empty;
                //ViewState.Remove("nOrigen");
                //lblErrorLog.Text = sRespuesta;
                //mpeErrorLog.Show();

                //Si no existe ningun conflicto en la respuesta del WebServices Obtiene el total de nodos
                int nodos = xmldoc.DocumentElement.ChildNodes.Count;
                for (int i = 0; i < 1; i++)
                {
                    System.Xml.XmlNode nodo = xmldoc.DocumentElement.ChildNodes[i];
                    //Consulta el id error y obtiene la descripción
                    sRespuesta = nodo.ChildNodes[1].InnerText.Trim();

                    if (clsComun.ObtenerParamentro("CancelacionPrueba") == "true")//El Web Services es de pruebas
                    {
                        if (sRespuesta == "201" || sRespuesta == "202" /*Este es de Pruebas*/ || sRespuesta == "999")
                        {
                            sMsjPrueba = " - Modo Pruebas";
                            ViewState.Remove("nOrigen");
                            lblAviso.Text = Resources.resCorpusCFDIEs.varCancelacionCfd + sMsjPrueba; ;
                            mpeAvisos.Show();
                            for (int a = 0; a < sUUID.Length; a++)
                            {
                                string sUUIDCan = sUUID[a].ToString();

                                foreach (GridViewRow renglon in gdvComprobantes.Rows)
                                {
                                    //Validamos los comprobantes que esten Checados y esten en estatus A
                                    if (sUUIDCan == renglon.Cells[3].Text)
                                    {
                                        Label sIdCFDcheck = (Label)(renglon.FindControl("lblid_cfd"));
                                        int idcfdi = Convert.ToInt32(sIdCFDcheck.Text);
                                        int retVal = clsComun.fnCancelarComprobante(idcfdi, txtcomentario.Text, datosUsuario.id_usuario);

                                        //Si no ocurre un error en la cancelación
                                        if (retVal != 0)
                                        {
                                            bandera = 1;
                                            txtcomentario.Text = string.Empty;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //En caso de error se avisa al usuario
                            lblErrorLog.Text = Resources.resCorpusCFDIEs.vrUUIDCancelarError;// "UUID No puede ser Cancelado, intente mas tarde (Todabia no se encuentra en el SAT).";
                            mpeErrorLog.Show();
                            //MessageBox.Show("UUID No puede ser Cancelado, intente mas tarde (Todabia no se encuentra en el SAT)."/*sResp*/, PAX_Resouces.Resource.lblContribuyente, MessageBoxButtons.OK, MessageBoxIcon.Information);
        
                        }
                    }
                    else
                    //Produccion
                    {
                        //Si no ocurre error se cancela el comprobante
                        if (sRespuesta == "201" || sRespuesta == "202")
                        {
                            //Se obtiene el id_cfdi del comprobante a cancelar
                            int idcfdi = clsComun.fnRecuperaIdCFD(nodo.ChildNodes[0].InnerText);
                            //Se cancela el comprobante actualizando el motivo
                            int retVal = clsComun.fnCancelarComprobante(idcfdi, txtcomentario.Text, datosUsuario.id_usuario);
                            //Si no ocurre un error en la cancelación
                            if (retVal != 0)
                            {
                                bandera = 1;
                                txtcomentario.Text = string.Empty;
                            }
                        }
                        else
                        {
                            //En caso de error se avisa al usuario
                            lblErrorLog.Text = Resources.resCorpusCFDIEs.vrUUIDCancelarError;// "UUID No puede ser Cancelado, intente mas tarde (Todabia no se encuentra en el SAT).";
                            mpeErrorLog.Show();
                            //MessageBox.Show("UUID No puede ser Cancelado, intente mas tarde (Todabia no se encuentra en el SAT)."/*sResp*/, PAX_Resouces.Resource.lblContribuyente, MessageBoxButtons.OK, MessageBoxIcon.Information);
                     
                        }
                    }
                }
                //Si no regreso ningua respuesta el sat, avisa al usuario mediante un msj
                if (nodos == 0)
                {
                    lblErrorLog.Text = Resources.resCorpusCFDIEs.varNoCancel;// "No hay respuesta para cancelar en el SAT";
                    mpeErrorLog.Show();
                }

            //Si se cancela el comprobante avisa al usuario mediante un msj
            if (bandera == 1)
            {
                lblErrorLog.Text = Resources.resCorpusCFDIEs.varCancelacionCfd + sMsjPrueba;// "Los comprobantes han sido cancelados ante hacienda" + sMsjPrueba;
                ViewState["Cancelacion"] = 1;
                mpeErrorLog.Show();
                //Los comprobantes han sido cancelados ante hacienda
                //MessageBox.Show(PAX_Resouces.Resource.msjCancelacionCfdMultiple + sMsjPrueba, PAX_Resouces.Resource.lblContribuyente, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.Close();
            }

            gdvComprobantes.AllowPaging = true;
            bool condicion = fnRealizarConsulta();
            if (condicion != true)
            {
                // cbPaginado.Checked = false;
                gdvComprobantes.AllowPaging = false;
            }

            ViewState.Remove("rfc");
            ViewState.Remove("UUID");

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
    
    public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    void TimeoutAsyncOperation(IAsyncResult ar)
    {

    }

    protected void btnExportar_Click(object sender, EventArgs e)
    {
        if (gdvComprobantes.Rows.Count > 0)
        {
            Session["dtConsultaExc"] = null;
            DataTable dtConsulta = fnRealizarConsultaAsincrona();
            

            Session["dtConsultaExc"] = dtConsulta;


            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('webDescargaConsulta.aspx','123','height=120, width= 355, status=yes, resizable= no, scrollbars= no, toolbar= no,menubar= no');</script>", "webDescargaConsulta.aspx"));
        }
        else
        {
            lblErrorLog.Text = Resources.resCorpusCFDIEs.varDatosExportar;
            mpeErrorLog.Show();
            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varDatosExportar);
        }
    }

    /// <summary>
    /// Revisa que existan creditos.
    /// </summary>
    private double fnRevisaCreditos()
    {
        double retorno=0;
        double credit = 0;

        //Revisar los creditos disponibles.
        DatosUsuario = clsComun.fnUsuarioEnSesion();
        dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(DatosUsuario.id_usuario.ToString(), "Timbrado", 'A', 'N');
        ViewState["dtCreditos"] = dtCreditos;

        if (dtCreditos.Rows.Count > 0)
        {
            double creditos = 0;
            creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
            if (creditos == 0)
            {
                /*clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
                ViewState["dtCreditos"] = dtCreditos;
                double creditos2 = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
                credit = creditos2;
                if (creditos2 == 0)
                {
                    modalCreditos.Show();
                }*/
            }
            else
            {
                credit = creditos;
            }

        }
        else
        {
            /*clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
            dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
            ViewState["dtCreditos"] = dtCreditos;
            double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
            credit = creditos;
            if (creditos == 0)
            {
                modalCreditos.Show();
            }*/
        }

        retorno = credit;//dtCreditos.Rows.Count;

        return retorno;        
    }

    protected void gdvComprobantes_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtCorreoCliente.Text = string.Empty;
        gOu = new clsOperacionUsuarios();
        int nid_cfd;
        string sDoc, snombreDoc;
        DTCompMail = (DataTable)ViewState["ExportarExcel"];
        nid_cfd = Convert.ToInt32(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["id_cfd"].ToString());
        string sRFCCli = DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["rfc"].ToString();
        sDoc = clsComun.ObtenerParamentro("TipoComprobante");
        snombreDoc = Convert.ToString(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["UUID"].ToString());
        
        txtCorreoEmisor.Text = gOu.fnObtenerCorreoReceptor(nid_cfd);

        ViewState["nid_cfd"]=nid_cfd;
        ViewState["sDoc"]=sDoc;
        ViewState["snombreDoc"] = snombreDoc;
        //Color vacio en txt
        txtCorreoCliente.BorderColor = System.Drawing.Color.Empty; 
        txtCorreoCC.BorderColor = System.Drawing.Color.Empty;

        datosUsuario = clsComun.fnUsuarioEnSesion();
        gOpeCuenta = new clsOperacionCuenta();
        SqlDataReader sdrInfo = gOpeCuenta.fnObtenerDatosFiscales(nid_cfd, null);

        txtCorreoCliente.Text = clsComun.fnObtenerEmailCliente(nid_cfd, sRFCCli);

        string sRFC = string.Empty;
        string sRazonSocial = string.Empty;

        if (sdrInfo != null && sdrInfo.HasRows && sdrInfo.Read())
        {
            sRFC = sdrInfo["rfc"].ToString();//ViewState["rfc_Emisor"]
             sRazonSocial = sdrInfo["razon_social"].ToString();//ViewState["razonSocial_Emisor"]
        }
        if (!string.IsNullOrEmpty(sRFC) && !string.IsNullOrEmpty(sRazonSocial))
            txtCorreoMsj.Text = Resources.resCorpusCFDIEs.varMensajeCorreo + "\n" + "\n" + sRazonSocial + "\n" + "RFC:" + "\n" + sRFC;

        //if (ViewState["rfc_Emisor"] != null && ViewState["razonSocial_Emisor"] != null)
        //    txtCorreoMsj.Text = Resources.resCorpusCFDIEs.varMensajeCorreo + "\n" + "\n" + ViewState["razonSocial_Emisor"] + "\n" + "RFC:" + "\n" + ViewState["rfc_Emisor"];

        mpeEnvioCorreo.Show();
      
    }

    protected void btnAceptarCor_Click(object sender, EventArgs e)
    {
        //clsConfiguracionPlantilla PlantillaC = new clsConfiguracionPlantilla();
        gOu = new clsOperacionUsuarios();
        int nid_cfd = Convert.ToInt32(ViewState["nid_cfd"]);

        string snombreDoc, sDoc;
        snombreDoc = Convert.ToString(ViewState["snombreDoc"]);
        sDoc = Convert.ToString(ViewState["sDoc"]);
        string sEmailUsu = gOu.fnObtenerCorreoReceptor(nid_cfd);

        
        string sUrlImg = "";
        //Enviar correo con archivos XML y PDF adjuntos
        cEd = new clsEnvioCorreoDocs();
        DatosUsuario = clsComun.fnUsuarioEnSesion();

        string sCC = txtCorreoCC.Text;
        string sMailTo = txtCorreoEmisor.Text;
        datosUsuario = clsComun.fnUsuarioEnSesion();
        if (txtCorreoCliente.Text != string.Empty)
            sMailTo += "," + txtCorreoCliente.Text;

        string sCorCli = string.Empty; //Valida si el campo esta lleno
        if (txtCorreoCliente.Text != string.Empty)
            sCorCli = cEd.fValidaEmail(txtCorreoCliente.Text); //Valida formato de correo

        string sCorCC = string.Empty;
        if (txtCorreoCC.Text != string.Empty)
            sCorCC = cEd.fValidaEmail(txtCorreoCC.Text); //Valida formato de correo

        if (sCorCli != string.Empty || sCorCC != string.Empty) //Si esta vacio entonces los correos estan escritos correctamente
        {
            string sCadena = string.Empty;
            if (sCorCli != string.Empty) //Pinta el borde del textbox cliente
                txtCorreoCliente.BorderColor = System.Drawing.Color.Red;
            else
                txtCorreoCliente.BorderColor = System.Drawing.Color.Empty;

            if (sCorCC != string.Empty) //Pinta el borde del textbox CC
                txtCorreoCC.BorderColor = System.Drawing.Color.Red;
            else
                txtCorreoCC.BorderColor = System.Drawing.Color.Empty;

            if (sCorCli == string.Empty && sCorCC != string.Empty)
                sCadena = sCorCC;
            else
                sCadena = sCorCli;

            if (sCorCC != string.Empty && sCorCC != string.Empty)
                sCadena = sCorCli + ", " + sCorCC;

            lblErrorLog.Text = Resources.resCorpusCFDIEs.txtCorreo + " " + sCadena;
            mpeErrorLog.Show();
            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.txtCorreo + " " + sCadena, Resources.resCorpusCFDIEs.varContribuyente);
            mpeEnvioCorreo.Show();
            return;
        }


            bool bEnvio;
            string Mensaje = string.Empty;
            Mensaje = "<table>";
            Mensaje = Mensaje + "<tr><td>" + txtCorreoMsj.Text.Replace("\n", @"<br />") + "</td><td></td></tr>";
            Mensaje = Mensaje + "</table>";

            if (Mensaje == string.Empty)
                Mensaje = "Comprobantes PAX Facturación";


            //clsConfiguracionPlantilla plantillas = new clsConfiguracionPlantilla();
            string plantilla = "Logo";// plantillas.fnRecuperaPlantillaNombre(DatosUsuario.plantilla);


            //Verifica si se envia el comprobante en ZIP o no.

            if (rdlArchivo.SelectedIndex == 1)
            {

                bEnvio = cEd.fnPdfEnvioCorreo(plantilla, Convert.ToString(nid_cfd), sDoc,
                                  clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLsGen"] + "\\" + snombreDoc + ".pdf",
                                  DatosUsuario.id_contribuyente, DatosUsuario.id_rfc, "Black", sMailTo,
                                  "PAXFacturacion", Mensaje, Convert.ToString(ViewState["GuidPathZIPsGen"]),
                                  Convert.ToString(ViewState["GuidPathXMLsGen"]), snombreDoc, sCC);
            }
            else
            {
                bEnvio = cEd.fnPdfEnvioCorreoSinZIP(plantilla, Convert.ToString(nid_cfd), sDoc,
                                  clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLsGen"] + "\\" + snombreDoc + ".pdf",
                                  DatosUsuario.id_contribuyente, DatosUsuario.id_rfc, "Black", sMailTo,
                                  "PAXFacturacion", Mensaje, Convert.ToString(ViewState["GuidPathZIPsGen"]),
                                  Convert.ToString(ViewState["GuidPathXMLsGen"]), snombreDoc, sCC);
            }


            if (bEnvio == true)
            {

                lblErrorLog.Text = Resources.resCorpusCFDIEs.msgEnvioMail + "<br />" + sMailTo;
                mpeErrorLog.Show();
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgEnvioMail + "<br />" + sMailTo);
                ViewState["nid_cfd"] = string.Empty;
                ViewState["sDoc"] = string.Empty;
                ViewState["snombreDoc"] = string.Empty;
                txtCorreoCC.Text = string.Empty;
                txtCorreoCliente.Text = string.Empty;
                txtCorreoMsj.Text = string.Empty;
                txtCorreoEmisor.Text = string.Empty;
                //mpeEnvioCorreo.Show();
                mpeEnvioCorreo.Hide();

            }
            else
            {
                lblErrorLog.Text = Resources.resCorpusCFDIEs.msgErrorEnvioMail;
                mpeErrorLog.Show();
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgErrorEnvioMail);
                mpeEnvioCorreo.Show();
            }

            txtCorreoCliente.BorderColor = System.Drawing.Color.Empty;
            txtCorreoCliente.BorderColor = System.Drawing.Color.Empty;

    }

    protected void btnCancelarCor_Click(object sender, EventArgs e)
    {
        ViewState["nid_cfd"] = string.Empty;
        ViewState["sDoc"] = string.Empty;
        ViewState["snombreDoc"] = string.Empty;
        txtCorreoCC.Text = string.Empty;
        txtCorreoCliente.Text = string.Empty;
        txtCorreoMsj.Text = string.Empty;
        txtCorreoEmisor.Text = string.Empty;
    }

    protected void btnAnterior_Click(object sender, EventArgs e)
    {
        int pagina = Convert.ToInt32(ViewState["paginado"]);
        pagina -= 1;

        if (pagina > 0)
        {
            ViewState["paginado"] = pagina;
            fnRealizarConsultaPaginado(pagina, 2);
            pagina -= 1;
            if (pagina == 0)
                btnAnterior.Visible = false;
            else
                btnAnterior.Visible = true;
        }
        else
        {
            btnAnterior.Visible = false;
        }

    }

    protected void btnSiguiente_Click(object sender, EventArgs e)
    {
        int pagina = Convert.ToInt32(ViewState["paginado"]);
        pagina += 1;
        ViewState["paginado"] = pagina;
        fnRealizarConsultaPaginado(pagina, 1);
        pagina += 1;
        bool bPaginas = fnVerificarPaginas(pagina);
        if (bPaginas == true)
            btnSiguiente.Visible = true;
        else
            btnSiguiente.Visible = false;
        // btnAnterior.Visible = true;
    }

    /// <summary>
    /// Verifica si existe más de una página
    /// </summary>
    /// <param name="PAgina"></param>
    /// <param name="Avance"></param>
    /// <returns></returns>
    private bool fnVerificarPaginas(int Pagina)
    {
        datosUsuario = clsComun.fnUsuarioEnSesion();
        gDAL = new clsOperacionConsulta();
        bool condicion = false;
        try
        {
            //string sSucursalesPista= "";
            DataTable resultado = new DataTable();
            //if (grvSucursales.Rows.Count > 0)
            //{
            //    foreach (GridView renglon in grvSucursales.Rows)
            //    {

            //        CheckBox cbCan;
            //        cbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
            //        cbCan.Checked = false;

            //        Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
            //        sSucursalesPista += ((Label)renglon.FindControl("lblsucursal")).Text + ", ";
            //        DataTable res = gDAL.fnObtenerComprobantes(
            //            ddlEstatus.SelectedValue,
            //            sIdSucursal.Text,
            //            Convert.ToDateTime(txtFechaIni.Text),
            //        Convert.ToDateTime(txtFechaFin.Text), 1, datosUsuario.id_usuario,
            //        txtUUID.Text, txtRfc.Text, txtNumticket.Text);

            //        foreach (DataRow row in res.Rows)
            //        {
            //            resultado.ImportRow(row);
            //        }
            //    }
            //}
            //else
            //{
            resultado = gDAL.fnObtenerComprobantes(ddlEstatus.SelectedValue,ddlSucursales.SelectedValue, Convert.ToDateTime(txtFechaIni.Text),
                Convert.ToDateTime(txtFechaFin.Text), Pagina, datosUsuario.id_usuario,txtUUID.Text,ddlrfcRecptor.SelectedValue , txtNumticket.Text);
            //}
            
            DataTable dtGrid = new DataTable();
            dtGrid = resultado;


            if (dtGrid.Rows.Count > 0)
            {
                condicion = true;
            }
            else
            {
                condicion = false;
            }
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
        return condicion;
    }

    protected void ddlSucursales_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarrfcReceptores(Convert.ToInt32(ddlSucursales.SelectedValue));
    }

    private bool fnRealizarConsultaPaginado(int Pagina, int Avance)
    {
        // 1 - Avance 
        // 2 - Retroceso
        gDAL = new clsOperacionConsulta();
        datosUsuario = clsComun.fnUsuarioEnSesion();
        bool condicion = false;
        try
        {
            string sSucursalesPista = "";
            DataTable dtGrid = new DataTable();
            DataTable resultado = new DataTable();
            //if (grvSucursales.Rows.Count > 0)
            //{
            //    foreach (GridView renglon in grvSucursales.Rows)
            //    {

            //        CheckBox cbCan;
            //        cbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
            //        cbCan.Checked = false;

            //        Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
            //        sSucursalesPista += ((Label)renglon.FindControl("lblsucursal")).Text + ", ";
            //        DataTable res = gDAL.fnObtenerComprobantes(
            //            ddlEstatus.SelectedValue,
            //            sIdSucursal.Text,
            //            Convert.ToDateTime(txtFechaIni.Text),
            //        Convert.ToDateTime(txtFechaFin.Text), Pagina, datosUsuario.id_usuario,
            //        txtUUID.Text, txtRfc.Text, txtNumticket.Text);

            //        foreach (DataRow row in res.Rows)
            //        {
            //            resultado.ImportRow(row);
            //        }
            //    }
            //}
            //else
            //{
                resultado = gDAL.fnObtenerComprobantes(ddlEstatus.SelectedValue,ddlSucursales.SelectedValue,
                    Convert.ToDateTime(txtFechaIni.Text),
                    Convert.ToDateTime(txtFechaFin.Text), Pagina, datosUsuario.id_usuario,
                    txtUUID.Text, ddlrfcRecptor.SelectedValue, txtNumticket.Text);
            //}
            dtGrid = resultado;


            if (dtGrid.Rows.Count > 0)
            {

                gdvComprobantes.DataSource = dtGrid;
                gdvComprobantes.DataBind();
                ViewState["ExportarExcel"] = gdvComprobantes.DataSource;

                btnCancelar.Visible = true;
                btnDescargar.Visible = true;
                cbSeleccionar.Visible = true;
                //cbPaginado.Visible = true;
                btnExportar.Visible = true;
                condicion = true;
                btnSiguiente.Visible = true;
                btnAnterior.Visible = true;

            }
            else
            {
                btnCancelar.Visible = false;
                btnDescargar.Visible = false;
                cbSeleccionar.Visible = false;
                // cbPaginado.Visible = false;
                btnExportar.Visible = false;
                condicion = false;
                if (Avance == 1)
                {
                    btnSiguiente.Visible = false;
                }
                else
                {
                    btnAnterior.Visible = false;
                }

            }
            cbSeleccionar.Checked = false;
            clsComun.fnNuevaPistaAuditoria(
                "webConsultasCFDI",
                "fnObtenerComprobantes",
                "Se consultó con los filtros",
                ddlEstatus.SelectedItem.Text,
                sSucursalesPista,
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
        return condicion;
    }

    public DataTable fnRealizarConsultaAsincrona()
    {
         try
         {
        DataTable dtNew = new DataTable();
        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.Int32");
        columna1.AllowDBNull = true;
        columna1.Caption = "nId_Usuario";
        columna1.ColumnName = "nId_Usuario";
        columna1.DefaultValue = null;
        dtNew.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.DateTime");
        columna2.AllowDBNull = true;
        columna2.Caption = "dFecha_Inicio";
        columna2.ColumnName = "dFecha_Inicio";
        columna2.DefaultValue = null;
        dtNew.Columns.Add(columna2);


        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.DateTime");
        columna3.AllowDBNull = true;
        columna3.Caption = "dFecha_Fin";
        columna3.ColumnName = "dFecha_Fin";
        columna3.DefaultValue = null;
        dtNew.Columns.Add(columna3);


        DataColumn columna4 = new DataColumn();
        columna4.DataType = System.Type.GetType("System.String");
        columna4.AllowDBNull = true;
        columna4.Caption = "nId_Estructura";
        columna4.ColumnName = "nId_Estructura";
        columna4.DefaultValue = 0;
        dtNew.Columns.Add(columna4);


        DataColumn columna5 = new DataColumn();
        columna5.DataType = System.Type.GetType("System.String");
        columna5.AllowDBNull = true;
        columna5.Caption = "nId_Tipo_Documento";
        columna5.ColumnName = "nId_Tipo_Documento";
        columna5.DefaultValue = "0";
        dtNew.Columns.Add(columna5);

        DataColumn columna6 = new DataColumn();
        columna6.DataType = System.Type.GetType("System.String");
        columna6.AllowDBNull = true;
        columna6.Caption = "sEstatus";
        columna6.ColumnName = "sEstatus";
        columna6.DefaultValue = "0";
        dtNew.Columns.Add(columna6);

        DataColumn columna7 = new DataColumn();
        columna7.DataType = System.Type.GetType("System.String");
        columna7.AllowDBNull = true;
        columna7.Caption = "sRfc_Receptor";
        columna7.ColumnName = "sRfc_Receptor";
        columna7.DefaultValue = "0";
        dtNew.Columns.Add(columna7);

        DataColumn columna8 = new DataColumn();
        columna8.DataType = System.Type.GetType("System.String");
        columna8.AllowDBNull = true;
        columna8.Caption = "sSerie";
        columna8.ColumnName = "sSerie";
        columna8.DefaultValue = "0";
        dtNew.Columns.Add(columna8);

        DataColumn columna9 = new DataColumn();
        columna9.DataType = System.Type.GetType("System.String");
        columna9.AllowDBNull = true;
        columna9.Caption = "nFolio_Inicio";
        columna9.ColumnName = "nFolio_Inicio";
        columna9.DefaultValue = "0";
        dtNew.Columns.Add(columna9);

        DataColumn columna10 = new DataColumn();
        columna10.DataType = System.Type.GetType("System.String");
        columna10.AllowDBNull = true;
        columna10.Caption = "nFolio_Fin";
        columna10.ColumnName = "nFolio_Fin";
        columna10.DefaultValue = "0";
        dtNew.Columns.Add(columna10);


        DataColumn columna11 = new DataColumn();
        columna11.DataType = System.Type.GetType("System.String");
        columna11.AllowDBNull = true;
        columna11.Caption = "nUUID";
        columna11.ColumnName = "nUUID";
        columna11.DefaultValue = "0";
        dtNew.Columns.Add(columna11);

        //if (gd.Rows.Count > 0)
        //{
        //    foreach (GridViewRow renglon in grvSucursales.Rows)
        //    {

                //CheckBox cbCan;
                //cbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                //cbCan.Checked = false;
                //if (cbCan.Checked)
                //{
                    //Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
                    DataRow RenNuevo = dtNew.NewRow();

                    RenNuevo["nId_Usuario"] = clsComun.fnUsuarioEnSesion().id_usuario;

                    RenNuevo["dFecha_Inicio"] = Convert.ToDateTime(txtFechaIni.Text);
                    RenNuevo["dFecha_Fin"] = Convert.ToDateTime(txtFechaFin.Text);
                    RenNuevo["nId_Estructura"] = ddlSucursales.SelectedValue;
                    RenNuevo["nId_Tipo_Documento"] = "";
                    RenNuevo["sEstatus"] = ddlEstatus.SelectedValue;
                    RenNuevo["sRfc_Receptor"] = "";
                    RenNuevo["sSerie"] = "";
                    RenNuevo["nFolio_Inicio"] = "0";
                    RenNuevo["nFolio_Fin"] = "";
                    RenNuevo["nUUID"] = "";

                    dtNew.Rows.Add(RenNuevo);
                //}
            //} }
       
        

         return dtNew;

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            return null;
        }
    }

    protected void btnErrorLog_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Convert.ToString(ViewState["Cancelacion"])))
        {
            string nAccion = ViewState["Cancelacion"].ToString();

            switch (nAccion)
            {
                case "1":
                    gdvComprobantes.AllowPaging = true;
                    bool condicion = fnRealizarConsulta();
                    if (condicion != true)
                    {
                        // cbPaginado.Checked = false;
                        gdvComprobantes.AllowPaging = false;
                    }
                    break;

                default:

                    break;
            }
        }
    }
    
    public void Page_Error(object sender, EventArgs e)
     {
         Exception objErr = Server.GetLastError().GetBaseException();

         if (!string.IsNullOrEmpty(objErr.Message))
         {
             Server.ClearError();
             Response.Redirect("~/webGlobalError.aspx");
         }

     }



}

/// <summary>
/// Actauliza los creditos disponibles.
/// </summary>
//private void fnActualizaCreditos()
//{
//    DataTable tlbCreditos = new DataTable();
//    int idCredito = 0;
//    int idEstructura = 0;
//    double Creditos = 0;
//    int nRetorno = 0;

//    tlbCreditos = (DataTable)ViewState["dtCreditos"];

//    if (tlbCreditos.Rows.Count > 0)
//    {

//        idCredito = Convert.ToInt32(tlbCreditos.Rows[0]["id_creditos"]);
//        idEstructura = Convert.ToInt32(tlbCreditos.Rows[0]["id_estructura"]);
//        Creditos = Convert.ToDouble(tlbCreditos.Rows[0]["creditos"]);

//        nRetorno = clsTimbradoFuncionalidad.fnActualizarCreditos(idCredito, idEstructura, Creditos,"C");
//        clsTimbradoFuncionalidad.fnActualizarCreditosHistorico(idCredito, idEstructura, Creditos);

//        DatosUsuario = clsComun.fnUsuarioEnSesion();
//        dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(DatosUsuario.id_usuario.ToString(), string.Empty, 'A', 'S');
//        if (dtCreditos.Rows.Count > 0)
//        {
//            fnActualizarLblCreditos();
//        }
//        else
//        {
//           /* clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
//            dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
//            double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
//            if (creditos > 0)
//            {
//                fnActualizarLblCreditos();
//            }*/
//        }

//    }


//}

/// <summary>
/// Actualizar etiqueta de Creditos.
/// </summary>
//private void fnActualizarLblCreditos()
//{
//    DatosUsuario = clsComun.fnUsuarioEnSesion();
//    dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(DatosUsuario.id_usuario.ToString(), string.Empty, 'A', 'S');
//    ViewState["dtCreditos"] = dtCreditos;
//    //cCc = new clsConfiguracionCreditos();
//    bool bRespuesta = true;
//    //double dCostCred = cCc.fnRecuperaPrecioServicio(2); //Precio servicio cancelación
//    if (dtCreditos.Rows.Count > 0)
//    {
//        double TCreditos = 0;
//        TCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

//        if (TCreditos == 0)//if (dtCreditos.Rows[0]["creditos"].ToString() == "0.00")
//        {
//            //clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
//            //dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
//            double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
//            if (creditos == 0)
//            {
//                //lblCredValor.Text = "0";

//                Label121.Visible = false;
//                lblCosCre.Visible = false;
//                Label7.Visible = true;
//                //modalRevisaCreditos.Show();

//                bRespuesta = false;
//            }
//            else
//            {
//                //lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
//                double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
//                //lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
//                //Se valida que tenga créditos suficientes
//                if (dCreditos < dCostCred)
//                {
//                    Label121.Visible = false;
//                    lblCosCre.Visible = true;
//                    Label7.Visible = false;
//                    //modalRevisaCreditos.Show();

//                    bRespuesta = false;
//                }
//            }
//        }
//        else
//        {
//            //lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
//            double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
//            //lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
//            //Se valida que tenga créditos suficientes
//            if (dCreditos < dCostCred)
//            {
//                Label121.Visible = false;
//                lblCosCre.Visible = true;
//                Label7.Visible = false;
//                //modalRevisaCreditos.Show();

//                bRespuesta = false;
//            }
//        }
//    }
//    else
//    {
//        /*clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
//        dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
//        double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
//        if (creditos == 0)
//        {
//            lblCredValor.Text = "0";

//            Label121.Visible = false;
//            lblCosCre.Visible = false;
//            Label7.Visible = true;
//            //modalRevisaCreditos.Show();

//            bRespuesta = false;
//        }
//        else
//        {
//            //lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
//            double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
//            lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();

//        }*/
//    }

//}

/// <summary>
/// Revisa créditos para la cancelación
/// </summary>
/// <returns></returns>
//private bool fnActualizarLblCreditosCancelación()
//{
//    DatosUsuario = clsComun.fnUsuarioEnSesion();
//    dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(DatosUsuario.id_usuario.ToString(), string.Empty, 'A', 'S');
//    cCc = new clsConfiguracionCreditos();
//    bool bRespuesta = true;
//    double dCostCred = cCc.fnRecuperaPrecioServicio(2); //Precio servicio cancelación
//    if (dtCreditos.Rows.Count > 0)
//    {
//        double TCreditos = 0;
//        TCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

//        if (TCreditos == 0)//if (dtCreditos.Rows[0]["creditos"].ToString() == "0.00")
//        {
//            /*clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
//            dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
//            double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
//            if (creditos == 0)
//            {
//                lblCredValor.Text = "0";

//                Label121.Visible = false;
//                lblCosCre.Visible = false;
//                Label7.Visible = true;
//                modalRevisaCreditos.Show();

//                bRespuesta = false;
//            }
//            else
//            {
//                //lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
//                double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
//                lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
//                //Se valida que tenga créditos suficientes
//                if (dCreditos < dCostCred)
//                {
//                    Label121.Visible = false;
//                    lblCosCre.Visible = true;
//                    Label7.Visible = false;
//                    modalRevisaCreditos.Show();

//                    bRespuesta = false;
//                }
//            }*/

//        }
//        else
//        {
//            //lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
//            double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
//            //lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
//            //Se valida que tenga créditos suficientes
//            if (dCreditos < dCostCred)
//            {
//                Label121.Visible = false;
//                lblCosCre.Visible = true;
//                Label7.Visible = false;
//                modalRevisaCreditos.Show();

//                bRespuesta = false;
//            }
//        }
//    }
//    else
//    {
//        /*clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
//        dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
//        double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);*/
//        /*if (creditos == 0)
//        {
//            lblCredValor.Text = "0";

//            Label121.Visible = false;
//            lblCosCre.Visible = false;
//            Label7.Visible = true;
//            modalRevisaCreditos.Show();

//            bRespuesta = false;
//        }
//        else
//        {
//            //lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
//            double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
//            lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
//            //Se valida que tenga créditos suficientes
//            if (dCreditos < dCostCred)
//            {
//                Label121.Visible = false;
//                lblCosCre.Visible = true;
//                Label7.Visible = false;
//                modalRevisaCreditos.Show();

//                bRespuesta = false;
//            }
//        }*/
//    }
//    return bRespuesta;
//}