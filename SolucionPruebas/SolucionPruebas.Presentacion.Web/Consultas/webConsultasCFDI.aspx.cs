using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolucionPruebas.Presentacion.Web.Consultas
{
    public partial class webConsultasCFDI : System.Web.UI.Page
    {

        private int gnNumeroRegistros = 50;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    //datosUsuario = clsComun.fnUsuarioEnSesion();
                    //if (datosUsuario == null)
                    //    return;

                    //btnAnterior.Visible = false;
                    //btnSiguiente.Visible = false;
                    //fnCargarEstatus();
                    //fnCargarSucursales();
                    //fnCargarDocumentos();
                    //fnCargcarSeries();
                    //fnCargarDatosPlantillas();

                    //clsConfiguracionPlantilla Plantilla = new clsConfiguracionPlantilla();
                    //DataTable Plantillas = new DataTable();
                    //int idEstructura = Convert.ToInt32(ddlSucursales.Items[1].Value);
                    //Plantillas = Plantilla.fnObtieneConfiguracionPlantilla(idEstructura);

                    //fnCargarUsuarios(datosUsuario.id_usuario);

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


                    //string Fecha3 = (string)ViewState["fecha1"];
                    //string Fecha4 = (string)ViewState["fecha2"];
                    //if (Fecha3 != null)
                    //{
                    //    txtFechaIni.Text = Fecha3;
                    //}
                    //else
                    //{
                    //    txtFechaIni.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                    //}

                    //if (Fecha4 != null)
                    //{
                    //    txtFechaFin.Text = Fecha4;
                    //}
                    //else
                    //{
                    //    txtFechaFin.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                    //}

                    //gOpeCuenta = new clsOperacionCuenta();
                    //DataTable sdrInfo = gOpeCuenta.fnObtenerDatosFiscales();

                    //if (sdrInfo != null && sdrInfo.Rows.Count > 0)
                    //{
                    //    ViewState["rfc_Emisor"] = sdrInfo.Rows[0]["rfc"].ToString();
                    //    ViewState["razonSocial_Emisor"] = sdrInfo.Rows[0]["razon_social"].ToString();
                    //}

                    //fnRegistrarScript();
                    //ViewState["GuidPathXMLs"] = Guid.NewGuid().ToString();
                    //ViewState["GuidPathZIPs"] = Guid.NewGuid().ToString();
                    //rdlArchivo.SelectedIndex = 0;

                }
                catch (Exception)
                {
                }
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            string Fecha1 = (string)ViewState["fecha1"];
            string Fecha2 = (string)ViewState["fecha2"];
            if (Fecha1 != null)
            {
                txtFechaIni.Text = Fecha1;
            }
            else
            {
                txtFechaIni.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            }

            if (Fecha2 != null)
            {
                txtFechaFin.Text = Fecha2;
            }
            else
            {
                txtFechaFin.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            }
        }
        public void Page_Error(object sender, EventArgs e)
        {
            Exception objErr = Server.GetLastError().GetBaseException();

            if (!string.IsNullOrEmpty(objErr.Message))
            {
                //datosUsuario = clsComun.fnUsuarioEnSesion();
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, objErr.Message);
                //Server.ClearError();
                //Response.Redirect("~/webGlobalError.aspx");
            }

        }
        protected void ddlSucursales_SelectedIndexChanged(object sender, EventArgs e)
        {
            //datosUsuario = clsComun.fnUsuarioEnSesion();
            //clsConfiguracionPlantilla Plantilla = new clsConfiguracionPlantilla();

            //fnCargcarSeries();
            //fnCargarReceptores(Convert.ToInt32(ddlSucursales.SelectedValue));
            //DataTable Plantillas = new DataTable();
            //int nPlantilla;
            //int idEstructura = Convert.ToInt32(ddlSucursales.SelectedValue);

            //if (idEstructura != 0)
            //{
            //    Plantillas = Plantilla.fnObtieneConfiguracionPlantilla(idEstructura);
            //    nPlantilla = Plantilla.fnRecuperaPlantillaRecursiva(idEstructura);

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
            //else
            //{
            //    idEstructura = Convert.ToInt32(ddlSucursales.Items[1].Value);
            //    Plantillas = Plantilla.fnObtieneConfiguracionPlantilla(idEstructura);
            //    nPlantilla = Plantilla.fnRecuperaPlantillaRecursiva(idEstructura);

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

        }
        protected void ddlDocumentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //fnCargcarSeries();
        }
        protected void gdvComprobantes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //gOu = new clsOperacionUsuarios();
            //gOc = new clsOperacionClientes();
            //int nid_cfd, nid_estructura;
            //string sDoc, snombreDoc, sRfc, sRazon_Social;
            //DTCompMail = (DataTable)ViewState["ExportarExcel"];
            //nid_cfd = Convert.ToInt32(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["id_cfd"].ToString());
            //nid_estructura = Convert.ToInt32(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["id_estructura"].ToString());
            //ViewState["id_estructura"] = nid_estructura;

            //sDoc = Convert.ToString(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["documento"].ToString());
            //snombreDoc = Convert.ToString(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["UUID"].ToString());
            //sRfc = Convert.ToString(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["rfc"].ToString());
            //sRazon_Social = Convert.ToString(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["razon_social"].ToString());

            //txtCorreoEmisor.Text = gOu.fnObtenerCorreoReceptor(nid_cfd);

            //gOc = new clsOperacionClientes();
            //DataTable DTCorreo = gOc.fnObtenerCorreoCliente(null, nid_estructura, sRfc, sRazon_Social);

            //if (DTCorreo.Rows.Count > 0)
            //{
            //    txtCorreoCliente.Text = DTCorreo.Rows[0]["correo"].ToString();
            //}
            //else
            //    txtCorreoCliente.Text = string.Empty;

            //ViewState["nid_cfd"] = nid_cfd;
            //ViewState["sDoc"] = sDoc;
            //ViewState["snombreDoc"] = snombreDoc;
            ////Color vacio en txt
            //txtCorreoCliente.BorderColor = System.Drawing.Color.Empty;
            //txtCorreoCC.BorderColor = System.Drawing.Color.Empty;


            //if (ViewState["rfc_Emisor"] != null && ViewState["razonSocial_Emisor"] != null)
            //    txtCorreoMsj.Text = Resources.resCorpusCFDIEs.varMensajeCorreo + "\n" + "\n" + ViewState["razonSocial_Emisor"] + "\n" + "RFC:" + "\n" + ViewState["rfc_Emisor"];

            //mpeEnvioCorreo.Show();

        }
        protected void btnAceptarCor_Click(object sender, EventArgs e)
        {
            //clsConfiguracionPlantilla PlantillaC = new clsConfiguracionPlantilla();
            //gOu = new clsOperacionUsuarios();
            //int nid_cfd = Convert.ToInt32(ViewState["nid_cfd"]);
            //string snombreDoc, sDoc;
            //snombreDoc = Convert.ToString(ViewState["snombreDoc"]);
            //sDoc = Convert.ToString(ViewState["sDoc"]);
            //string sEmailUsu = gOu.fnObtenerCorreoReceptor(nid_cfd);

            ////Enviar correo con archivos XML y PDF adjuntos
            //cEd = new clsEnvioCorreoDocs();
            //DatosUsuario = clsComun.fnUsuarioEnSesion();

            //string sCC = txtCorreoCC.Text;
            //string sMailTo = txtCorreoEmisor.Text;
            //datosUsuario = clsComun.fnUsuarioEnSesion();
            //if (txtCorreoCliente.Text != string.Empty)
            //    sMailTo += "," + txtCorreoCliente.Text;

            //string sCorCli = string.Empty; //Valida si el campo esta lleno
            //if (txtCorreoCliente.Text != string.Empty)
            //    sCorCli = cEd.fValidaEmail(txtCorreoCliente.Text); //Valida formato de correo

            //string sCorCC = string.Empty;
            //if (txtCorreoCC.Text != string.Empty)
            //    sCorCC = cEd.fValidaEmail(txtCorreoCC.Text); //Valida formato de correo

            //if (sCorCli != string.Empty || sCorCC != string.Empty) //Si esta vacio entonces los correos estan escritos correctamente
            //{
            //    string sCadena = string.Empty;
            //    if (sCorCli != string.Empty) //Pinta el borde del textbox cliente
            //        txtCorreoCliente.BorderColor = System.Drawing.Color.Red;
            //    else
            //        txtCorreoCliente.BorderColor = System.Drawing.Color.Empty;

            //    if (sCorCC != string.Empty) //Pinta el borde del textbox CC
            //        txtCorreoCC.BorderColor = System.Drawing.Color.Red;
            //    else
            //        txtCorreoCC.BorderColor = System.Drawing.Color.Empty;

            //    if (sCorCli == string.Empty && sCorCC != string.Empty)
            //        sCadena = sCorCC;
            //    else
            //        sCadena = sCorCli;

            //    if (sCorCC != string.Empty && sCorCC != string.Empty)
            //        sCadena = sCorCli + ", " + sCorCC;

            //    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.txtCorreo + " " + sCadena, Resources.resCorpusCFDIEs.varContribuyente);
            //    mpeEnvioCorreo.Show();
            //    return;
            //}


            //bool bEnvio;
            //string Mensaje = string.Empty;
            //Mensaje = "<table>";
            //Mensaje = Mensaje + "<tr><td>" + txtCorreoMsj.Text.Replace("\n", @"<br />") + "</td><td></td></tr>";
            //Mensaje = Mensaje + "</table>";

            //if (Mensaje == string.Empty)
            //    Mensaje = "Comprobantes PAX Facturación";


            //clsConfiguracionPlantilla plantillas = new clsConfiguracionPlantilla();
            //string plantilla = plantillas.fnRecuperaPlantillaNombre(DatosUsuario.plantilla);


            ////Verifica si se envia el comprobante en ZIP o no.

            //if (rdlArchivo.SelectedIndex == 1)
            //{

            //    bEnvio = cEd.fnPdfEnvioCorreo(plantilla, Convert.ToString(nid_cfd), sDoc,
            //                      clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + snombreDoc + ".pdf",
            //                      DatosUsuario.id_contribuyente, DatosUsuario.id_rfc, DatosUsuario.color, sMailTo,
            //                      "PAXFacturacion", Mensaje, Convert.ToString(ViewState["GuidPathZIPs"]),
            //                      Convert.ToString(ViewState["GuidPathXMLs"]), snombreDoc, sCC);
            //}
            //else
            //{
            //    bEnvio = cEd.fnPdfEnvioCorreoSinZIP(plantilla, Convert.ToString(nid_cfd), sDoc,
            //                      clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + snombreDoc + ".pdf",
            //                      DatosUsuario.id_contribuyente, DatosUsuario.id_rfc, DatosUsuario.color, sMailTo,
            //                      "PAXFacturacion", Mensaje, Convert.ToString(ViewState["GuidPathZIPs"]),
            //                      Convert.ToString(ViewState["GuidPathXMLs"]), snombreDoc, sCC);
            //}


            //if (bEnvio == true)
            //{
            //    string[] split = sMailTo.Split(',');
            //    sMailTo = string.Empty;
            //    foreach (string s in split)
            //    {
            //        sMailTo = sMailTo + "\\n" + s;
            //    }
            //    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgEnvioMail + "<br />" + sMailTo);
            //    ViewState["nid_cfd"] = string.Empty;
            //    ViewState["sDoc"] = string.Empty;
            //    ViewState["snombreDoc"] = string.Empty;
            //    txtCorreoCC.Text = string.Empty;
            //    txtCorreoCliente.Text = string.Empty;
            //    txtCorreoMsj.Text = string.Empty;
            //    txtCorreoEmisor.Text = string.Empty;
            //    //mpeEnvioCorreo.Show();
            //    mpeEnvioCorreo.Hide();

            //}
            //else
            //{
            //    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgErrorEnvioMail);
            //    mpeEnvioCorreo.Show();
            //}

            //txtCorreoCliente.BorderColor = System.Drawing.Color.Empty;
            //txtCorreoCliente.BorderColor = System.Drawing.Color.Empty;

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
            btnAnterior.Visible = false;
            btnSiguiente.Visible = true;

            ddlCantidadPaginas.SelectedValue = (Convert.ToInt32(ddlCantidadPaginas.SelectedValue) - 1).ToString();

            gdvComprobantes.DataSource = fnRealizarConsulta(Convert.ToInt32(ddlCantidadPaginas.SelectedValue), gnNumeroRegistros);
            gdvComprobantes.DataBind();

            if (Convert.ToInt32(ddlCantidadPaginas.SelectedValue) > 1)
                btnAnterior.Visible = true;
        }
        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            btnSiguiente.Visible = false;
            btnAnterior.Visible = true;

            ddlCantidadPaginas.SelectedValue = (Convert.ToInt32(ddlCantidadPaginas.SelectedValue) + 1).ToString();

            gdvComprobantes.DataSource = fnRealizarConsulta(Convert.ToInt32(ddlCantidadPaginas.SelectedValue), gnNumeroRegistros);
            gdvComprobantes.DataBind();

            if (ddlCantidadPaginas.SelectedIndex < (ddlCantidadPaginas.Items.Count - 1))
                btnSiguiente.Visible = true;
        }
        protected void ddlCantidadPaginas_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAnterior.Visible = false;
            btnSiguiente.Visible = false;

            gdvComprobantes.DataSource = fnRealizarConsulta(Convert.ToInt32(ddlCantidadPaginas.SelectedValue), gnNumeroRegistros);
            gdvComprobantes.DataBind();

            if (Convert.ToInt32(ddlCantidadPaginas.SelectedValue) > 1)
                btnAnterior.Visible = true;

            if (ddlCantidadPaginas.SelectedIndex < (ddlCantidadPaginas.Items.Count - 1))
                btnSiguiente.Visible = true;
        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            DataTable dtComprobantes = new DataTable();

            btnSiguiente.Visible = false;
            btnAnterior.Visible = false;
            btnCancelar.Visible = false;
            btnDescargar.Visible = false;
            btnExportar.Visible = false;
            cbSeleccionar.Visible = false;

            ddlCantidadPaginas.DataSource = null;
            ddlCantidadPaginas.DataBind();
            ddlCantidadPaginas.Items.Clear();
            ddlCantidadPaginas.Visible = false;

            cbSeleccionar.Checked = false;

            dtComprobantes = fnRealizarConsulta(1, gnNumeroRegistros);

            gdvComprobantes.DataSource = dtComprobantes;
            gdvComprobantes.DataBind();

            if (dtComprobantes.Rows.Count > 0)
            {
                btnDescargar.Visible = true;
                btnExportar.Visible = true;

                double nRegistros = Convert.ToDouble(dtComprobantes.Rows[0]["registros"]);
                int nNumeroPaginas = Convert.ToInt32(Math.Round((nRegistros / Convert.ToDouble(gnNumeroRegistros)) + 0.5));

                for (int i = 1; i <= nNumeroPaginas; i++)
                {
                    ddlCantidadPaginas.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }

                if (nNumeroPaginas > 1)
                {
                    ddlCantidadPaginas.Visible = true;
                    btnSiguiente.Visible = true;
                }
            }

            ViewState["fecha1"] = txtFechaIni.Text;
            ViewState["fecha2"] = txtFechaFin.Text;
        }
        protected void btnDescargar_Click(object sender, EventArgs e)
        {


            #region Comentado por Hector Portillo 2014-01-20

            /*
             Razon = Esto hace referencia a webDescargaComprobantes.aspx que hace una descarga completa de todos los comprobantes en vez de solo los visibles
             
             */

            //try
            //{
            //    //Pasar parámetros de consulta para la descarga masiva de comprobantes
            //    //Receptor|Estatus|Sucursal|Documentos|Series|Folio inicio|Folio fin|Fecha inicio|Fecha fin|Usuario


            //    string sParametros = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}",
            //                                       ddlReceptor.SelectedValue,
            //                                       ddlEstatus.SelectedValue,
            //                                       ddlSucursales.SelectedValue,
            //                                       ddlDocumentos.SelectedValue,
            //                                       ddlSeries.SelectedValue,
            //                                       txtFolioIni.Text,
            //                                       txtFolioFin.Text,
            //                                       Convert.ToDateTime(txtFechaIni.Text),
            //                                       Convert.ToDateTime(txtFechaFin.Text), txtUUID.Text,
            //                                       ddlUsuarios.SelectedValue);

            //    //Se encriptan los parámetros
            //    string sParamEncriptados = Utilerias.Encriptacion.Base64.EncriptarBase64(sParametros);


            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "descargarComprobantes",
            //                                                String.Format("<script>window.open('{0}?p={1}','123','height=160, width= 355, status=yes, resizable= no, scrollbars= no, toolbar= no,menubar= no');</script>",
            //                                                               "webDescargaComprobantes.aspx", sParamEncriptados), false); 



            #region Comentado 14 - 02 - 2013
            //string errores = string.Empty;

            //int bandera = 0;
            //byte[] buffer = { };
            //byte[] bufferPDF = { };
            //ArrayList Final = new ArrayList();

            ////double retCreditos = 0;

            ////retCreditos = fnRevisaCreditos();

            ////if (retCreditos > 0)
            ////{
            //    Directory.CreateDirectory(clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"]);
            //    Directory.CreateDirectory(clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"]);

            //    string snombreDoc = string.Empty;
            //    int i = 0;

            //    #region comentado 09_01_2013
            //    //foreach (GridViewRow renglon in gdvComprobantes.Rows)
            //    //{
            //    //    CheckBox CbCan;

            //    //    CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

            //    //    if (CbCan.Checked)
            //    //    {
            //    //        Guid Gid;
            //    //        Gid = Guid.NewGuid();

            //    //        Label sIdCfd = ((Label)renglon.FindControl("lblid_cfd"));

            //    //        gDAL = new clsOperacionConsulta();
            //    //        XmlDocument comprobante = new XmlDocument();
            //    //        string sTipoDocumento = HttpUtility.HtmlDecode(renglon.Cells[8].Text);
            //    //        snombreDoc = renglon.Cells[3].Text;
            //    //       // string sTipoDocumento = HttpUtility.UrlDecode(Request.QueryString["doc"]);
            //    //        int nIdContribuyente = clsComun.fnUsuarioEnSesion().id_contribuyente;
            //    //        comprobante = gDAL.fnObtenerComprobanteXML(nIdContribuyente, sIdCfd.Text);

            //    //        clsPlantillaLista pdf = new clsPlantillaLista();
            //    //        //clsOperacionConsultaPdf pdf;
            //    //        //pdf = new clsOperacionConsultaPdf(gDAL.fnObtenerComprobanteXML(nIdContribuyente, sIdCfd.Text));

            //    //        //if (!string.IsNullOrEmpty(sTipoDocumento))
            //    //        //    pdf.TipoDocumento = sTipoDocumento.ToUpper();


            //    //        string pathPDF = clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + snombreDoc + ".pdf";//Gid + ".pdf";

            //    //        DatosUsuario = clsComun.fnUsuarioEnSesion();
            //    //        pdf.fnObtenerPLantilla(gDAL.fnObtenerComprobanteXML(nIdContribuyente, sIdCfd.Text), string.Empty, string.Empty, sTipoDocumento, this, pathPDF, nIdContribuyente,DatosUsuario.id_rfc, DatosUsuario.color);

            //    //        //pdf.fnGenerarPDFSave(pathPDF);


            //    //        clsComun.fnNuevaPistaAuditoria(
            //    //            "webConsultasGeneradorPDF",
            //    //            "fnGenerarPDF",
            //    //            "Se generó el PDF para el comprobante con ID " + sIdCfd.Text
            //    //            );

            //    //        bandera = 1;

            //    //        buffer = Encoding.UTF8.GetBytes(comprobante.InnerXml);

            //    //        string path = clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + snombreDoc + ".xml";//Gid + ".xml";


            //    //        // Create the text file if it doesn't already exist.
            //    //        if (!File.Exists(path))
            //    //        {
            //    //            //fnActualizaCreditos();
            //    //            File.WriteAllBytes(path, buffer);
            //    //        }
            //    //    }
            //    //    i += 1; //Verifica si se selecciono mas de un registro
            //    //}
            //    #endregion

            //    gDAL = new clsOperacionConsulta();

            //    int nIdContribuyente = clsComun.fnUsuarioEnSesion().id_contribuyente;

            //    DataTable dtComprobantes = gDAL.fnObtenerComprobantesDescarga(
            //                                       ddlReceptor.SelectedValue,
            //                                       ddlEstatus.SelectedValue,
            //                                       ddlSucursales.SelectedValue,
            //                                       ddlDocumentos.SelectedValue,
            //                                       ddlSeries.SelectedValue,
            //                                       txtFolioIni.Text,
            //                                       txtFolioFin.Text,
            //                                       Convert.ToDateTime(txtFechaIni.Text),
            //                                       Convert.ToDateTime(txtFechaFin.Text), txtUUID.Text,
            //                                       ddlUsuarios.SelectedValue
            //                                       );

            //    foreach (DataRow renglon in dtComprobantes.Rows)
            //    {

            //            Guid Gid;
            //            Gid = Guid.NewGuid();

            //            //Label sIdCfd = ((Label)renglon.FindControl("lblid_cfd"));
            //            string sIdCfd = renglon["id_cfd"].ToString();
            //        try
            //        {
            //            gDAL = new clsOperacionConsulta();
            //            XmlDocument comprobante = new XmlDocument();
            //            string sTipoDocumento = renglon["documento"].ToString(); // HttpUtility.HtmlDecode(renglon.Cells[8].Text);
            //            snombreDoc = renglon["UUID"].ToString(); //renglon.Cells[3].Text;
            //            // string sTipoDocumento = HttpUtility.UrlDecode(Request.QueryString["doc"]);
            //            //int nIdContribuyente = clsComun.fnUsuarioEnSesion().id_contribuyente;

            //            /* Obtiene XML de comprobante */
            //            comprobante = gDAL.fnObtenerComprobanteXML(nIdContribuyente, sIdCfd);

            //            // Create an XML declaration. 
            //            XmlDeclaration xmldecl;
            //            xmldecl = comprobante.CreateXmlDeclaration("1.0", null, null);
            //            xmldecl.Encoding = "UTF-8";
            //            xmldecl.Standalone = null;

            //            // Add the new node to the document.
            //            XmlElement root = comprobante.DocumentElement;
            //            comprobante.InsertBefore(xmldecl, root);

            //            //Pega addenda en caso de que exista
            //            fnPegarAddendaXML(ref comprobante, sIdCfd);

            //            /* Fin obtiene comprobante */

            //            clsPlantillaLista pdf = new clsPlantillaLista();
            //            //clsOperacionConsultaPdf pdf;
            //            //pdf = new clsOperacionConsultaPdf(gDAL.fnObtenerComprobanteXML(nIdContribuyente, sIdCfd.Text));

            //            //if (!string.IsNullOrEmpty(sTipoDocumento))
            //            //    pdf.TipoDocumento = sTipoDocumento.ToUpper();
            //            DatosUsuario = clsComun.fnUsuarioEnSesion();
            //            clsConfiguracionPlantilla PlantillaC = new clsConfiguracionPlantilla();
            //            string plantilla = PlantillaC.fnRecuperaPlantillaNombre(DatosUsuario.plantilla);

            //            string pathPDF = clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + snombreDoc + ".pdf";//Gid + ".pdf";

            //            //pdf.fnObtenerPLantilla(gDAL.fnObtenerComprobanteXML(nIdContribuyente, sIdCfd), string.Empty, string.Empty, sTipoDocumento, this, pathPDF, nIdContribuyente, DatosUsuario.id_rfc, DatosUsuario.color);
            //            pdf.fnCrearPLantillaEnvio(gDAL.fnObtenerComprobanteXML(nIdContribuyente, sIdCfd), plantilla, sIdCfd, sTipoDocumento, pathPDF, nIdContribuyente, DatosUsuario.id_rfc, DatosUsuario.color);

            //            //pdf.fnGenerarPDFSave(pathPDF);

            //            clsComun.fnNuevaPistaAuditoria(
            //                "webConsultasGeneradorPDF",
            //                "fnGenerarPDF",
            //                "Se generó el PDF para el comprobante con ID " + sIdCfd
            //                );

            //            bandera = 1;

            //            buffer = Encoding.UTF8.GetBytes(comprobante.InnerXml);

            //            string path = clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + snombreDoc + ".xml";//Gid + ".xml";


            //            // Create the text file if it doesn't already exist.
            //            if (!File.Exists(path))
            //            {
            //                //fnActualizaCreditos();
            //                File.WriteAllBytes(path, buffer);
            //            }

            //            i += 1;
            //        }
            //        catch (Exception ex)
            //        {
            //            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            //        }
            //    }

            //    if (bandera == 1)
            //    {

            //        Guid Gid;
            //        Gid = Guid.NewGuid();

            //        string Ruta = string.Empty; //clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"] + "\\" + Gid + ".zip";
            //        if (i > 1) //Si se selecciono mas de un registro se guarda un nombre generico.
            //            Ruta = clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"] + "\\" + Gid + ".zip";
            //        else //Si selecciono un registro se guarda con el nombre del documento
            //            Ruta = clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"] + "\\" + snombreDoc + ".zip";

            //        ICSharpCode.SharpZipLib.Zip.ZipOutputStream zip = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(File.Create(Ruta));
            //        zip.SetLevel(6);

            //        string folder = clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"];

            //        ComprimirCarpeta(folder, folder, zip);

            //        zip.Finish();
            //        zip.Close();

            //        foreach (string file in Directory.GetFiles(clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"]))
            //        {
            //            File.Delete(file);
            //        }
            //        Directory.Delete(clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"], true);

            //        //Response.Clear();
            //        //Response.ClearHeaders();
            //        //Response.ClearContent();
            //        //Response.CacheControl = "public";
            //        //Response.ContentType = "application/zip";
            //        //Response.AddHeader("content-disposition", "attachment; filename=" + snombreDoc + ".zip");//Gid + ".zip");

            //        //Response.BinaryWrite(File.ReadAllBytes(Ruta));
            //        ////Response.WriteFile(Ruta);
            //        ////Response.TransmitFile(Ruta);
            //        //Response.Flush();
            //        //Response.Close();

            //        //ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('webDescargaConsulta.aspx','123','height=120, width= 355, status=yes, resizable= no, scrollbars= no, toolbar= no,menubar= no');</script>", "webDescargaConsulta.aspx"));

            //        //Parámetros
            //        //p=Carpeta donde se guardó el archivo zip
            //        //f=Nombre del archivo
            //        FileInfo f = new FileInfo(Ruta);
            //        string sFileName = f.Name.Replace(f.Extension, "");
            //        string sFolderZIP = f.Directory.Name;
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "descargarComprobantes", 
            //                                            String.Format("<script>window.open('{0}?p={1}&f={2}','123','height=140, width= 355, status=yes, resizable= no, scrollbars= no, toolbar= no,menubar= no');</script>", 
            //                                                           "webDescargaComprobantes.aspx", sFolderZIP, sFileName), false);

            //        //LimpiaCarpetas();


            //    }
            //    else
            //    {
            //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varDescargaVacio);
            //    }
            ////}
            ////else
            ////{
            ////    modalCreditos.Hide();
            ////    modalRevisaCreditos.Show();
            ////}
            #endregion
            //}
            //catch (Exception ex)
            //{
            //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            //}
            #endregion

            try
            {

                string sIDS = "";

                for (int i = 0; i < gdvComprobantes.Rows.Count; i++)
                {

                    sIDS += "" + gdvComprobantes.DataKeys[i].Value + ",";
                    //sIDS += "" + gdvComprobantes.Rows[i].Cells[3].Text + ",";

                }

                sIDS = sIDS.Substring(0, sIDS.Length - 1);





                sIDS = Utilerias.Encriptacion.Base64.EncriptarBase64(sIDS);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "descargarComprobantesV2",
                                                            String.Format("<script>window.open('{0}?p={1}','150','height=200, width= 355, status=yes, resizable= no, scrollbars= no, toolbar= no,menubar= no');</script>",
                                                                           "webDescargaComprobantesV2.aspx", sIDS), false);



                #region Comentado por Hector Portillo 2014-02-01
                //Session["sIDS"] = sIDS;

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "descargarComprobantesV2",
                //                                            String.Format("<script>window.open('{0}','150','height=200, width= 355, status=yes, resizable= no, scrollbars= no, toolbar= no,menubar= no');</script>",
                //                                                           "webDescargaComprobantesV2.aspx"), false);
                #endregion


            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            }

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
        protected void btnExportar_Click(object sender, EventArgs e)
        {
            #region Comentado Hector Portillo 2014-01-30
            // Razon = Se intenta limitar la consulta a solamente lo que esta visible

            //Session["dtConsultaExc"] = null;
            //DataTable dtConsulta = fnRealizarConsultaAsincrona();
            //Session["dtConsultaExc"] = dtConsulta;

            //ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('webDescargaConsulta.aspx','123','height=160, width= 355, status=yes, resizable= no, scrollbars= no, toolbar= no,menubar= no');</script>", "webDescargaConsulta.aspx"));
            #endregion


            try
            {

                string sIDS = "";

                for (int i = 0; i < gdvComprobantes.Rows.Count; i++)
                {

                    sIDS += "" + gdvComprobantes.DataKeys[i].Value + ",";


                }

                sIDS = sIDS.Substring(0, sIDS.Length - 1);

                sIDS = Utilerias.Encriptacion.Base64.EncriptarBase64(sIDS);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "webDescargaConsultaV2.aspx",
                                                            String.Format("<script>window.open('{0}?p={1}','123','height=160, width= 355, status=yes, resizable= no, scrollbars= no, toolbar= no,menubar= no');</script>",
                                                                           "webDescargaConsultaV2.aspx", sIDS), false);

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
        protected void gdvCorreos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string parametro = gdvCorreos.SelectedDataKey.Values["id_rfc_receptor"].ToString();

            gdvCorreos.DataSource = null;
            gdvCorreos.DataBind();
            linkModal_ModalPopupExtender.Hide();
            mpeEnvioCorreo.Show();
            fnRealizarBusquedaDatosCorreo(parametro);
            fnLimpiarControlesCorreo();

        }
        protected void gdvCorreos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvCorreos.PageIndex = e.NewPageIndex;
            fnTraerCorreos();
            linkModal_ModalPopupExtender.Show();
            mpeEnvioCorreo.Hide();
        }
        protected void btnConsulta_Click(object sender, EventArgs e)
        {
            fnTraerCorreos();
            linkModal_ModalPopupExtender.Show();
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

        /// <summary>
        /// Carga los documentos disponibles
        /// </summary>
        private void fnCargarDocumentos()
        {
            //try
            //{
            //    gDAL = new clsOperacionConsulta();

            //    ddlDocumentos.DataSource = gDAL.fnObtenerDocumentosPago(clsComun.fnUsuarioEnSesion().id_usuario);
            //    ddlDocumentos.DataBind();
            //}
            //catch (Exception ex)
            //{
            //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            //    ddlDocumentos.DataSource = null;
            //    ddlDocumentos.DataBind();
            //}
        }

        /// <summary>
        /// Carga el drop con las opciones TODOS, ACTIVO y CANCELADO
        /// </summary>
        private void fnCargarEstatus()
        {
            //gDAL = new clsOperacionConsulta();

            //ddlEstatus.DataSource = gDAL.fnObtenerEstatus();
            //ddlEstatus.DataBind();
        }      

        /// <summary>
        /// Carga los receptores activos para usarlos en los filtros
        /// </summary>
        private void fnCargarReceptores(int pIdEstructura)
        {
            //try
            //{
            //    gDAL = new clsOperacionConsulta();

            //    ddlReceptor.DataSource = gDAL.fnObtenerReceptores(pIdEstructura);
            //    ddlReceptor.DataBind();
            //}
            //catch (Exception ex)
            //{
            //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            //    ddlReceptor.DataSource = null;
            //    ddlReceptor.DataBind();
            //}
        }       

        /// <summary>
        /// Carga las sucursales disponibles para el usuario
        /// </summary>
        private void fnCargarSucursales()
        {
            try
            {

                //gDAL = new clsOperacionConsulta();
                //datosUsuario = clsComun.fnUsuarioEnSesion();

                //DataTable dtAuxiliar = clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario);
                //ViewState["dtAuxiliar"] = dtAuxiliar;
                //DataRow drFila = dtAuxiliar.NewRow();
                //drFila["id_estructura"] = 0;// dtAuxiliar.Rows[0]["id_estructura"];
                //drFila["nombre"] = Resources.resCorpusCFDIEs.VarDropTodos;

                //dtAuxiliar.Rows.InsertAt(drFila, 0);

                //ddlSucursales.DataSource = dtAuxiliar;//gDAL.fnObtenerSucursales();
                //ddlSucursales.DataBind();

                //ddlSucursales.SelectedValue = Resources.resCorpusCFDIEs.VarDropTodos;

                //fnCargarReceptores(Convert.ToInt32(ddlSucursales.SelectedValue));
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                ddlSucursales.DataSource = null;
                ddlSucursales.DataBind();
            }
        }

        /// <summary>
        /// Función que se encarga de cargar los usuarios
        /// </summary>
        /// <param name="pnIdUsuario">ID del Usuario</param>
        private void fnCargarUsuarios(int pnIdUsuario)
        {
            //try
            //{
            //    gDAL = new clsOperacionConsulta();
            //    ddlUsuarios.DataSource = gDAL.fnObtenerUsuarios(pnIdUsuario);
            //    ddlUsuarios.DataBind();
            //}
            //catch (Exception ex)
            //{
            //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            //    ddlUsuarios.DataSource = null;
            //    ddlUsuarios.DataBind();
            //}
        }

        /// <summary>
        /// Elimina las carpetas temporales que fueron creadas
        /// </summary>
        public void fnLimpiaCarpetas()
        {

            //foreach (string file in Directory.GetFiles(clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"]))
            //{
            //    File.Delete(file);
            //}
            //Directory.Delete(clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"], true);

        }

        /// <summary>
        /// Método que se encarga de limpiar los controles del panel de envio de correo
        /// </summary>
        private void fnLimpiarControlesCorreo()
        {
            txtRfcConsulta.Text = string.Empty;
            txtRazonSocialConsulta.Text = string.Empty;
            gdvCorreos.DataSource = null;
            gdvCorreos.DataBind();
        }

        /// <summary>
        /// Método que se encarga de realizar la busqueda de los correos
        /// </summary>
        /// <param name="parametro"></param>
        private void fnRealizarBusquedaDatosCorreo(string parametro)
        {
            //string sIdReceptor = parametro;
            //gOc = new clsOperacionClientes();
            //DataTable sdrLector = gOc.fnEditarReceptor(sIdReceptor);
            //txtCorreoCliente.Text = sdrLector.Rows[0]["correo"].ToString();
        }

        /// <summary>
        /// Función que se encarga de realizar la busqueda de comprobantes
        /// </summary>
        /// <param name="pnPagina">Página a visualizar</param>
        /// <param name="pnCantidadRegistros">Número de registros a visualizar</param>
        /// <returns></returns>
        private DataTable fnRealizarConsulta(int pnPagina, int pnCantidadRegistros)
        {
            DataTable dtComprobantes = new DataTable();
            //gDAL = new clsOperacionConsulta();
            //try
            //{
            //    dtComprobantes = gDAL.fnObtenerComprobantes(
            //        ddlReceptor.SelectedValue,
            //        ddlEstatus.SelectedValue,
            //        ddlSucursales.SelectedValue,
            //        ddlDocumentos.SelectedValue,
            //        ddlSeries.SelectedValue,
            //        txtFolioIni.Text,
            //        txtFolioFin.Text,
            //        Convert.ToDateTime(txtFechaIni.Text),
            //        Convert.ToDateTime(txtFechaFin.Text), txtUUID.Text,
            //        ddlUsuarios.SelectedValue,
            //        pnPagina,
            //        gnNumeroRegistros
            //        );

            //    ViewState["ExportarExcel"] = dtComprobantes;

            //    clsComun.fnNuevaPistaAuditoria(
            //        "webConsultasCFDI",
            //        "fnObtenerComprobantes",
            //        "Se consultó con los filtros",
            //        ddlReceptor.SelectedItem.Text,
            //        ddlEstatus.SelectedItem.Text,
            //        ddlSucursales.SelectedItem.Text,
            //        ddlDocumentos.SelectedItem.Text,
            //        ddlSeries.SelectedItem.Text,
            //        txtFolioIni.Text,
            //        txtFolioFin.Text,
            //        txtFechaIni.Text,
            //        txtFechaFin.Text
            //        );

            //}
            //catch (SqlException ex)
            //{
            //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            //}
            //catch (Exception ex)
            //{
            //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            //    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorConsulta);
            //}
            return dtComprobantes;
        }

        /// <summary>
        /// realiza la búsqueda de los correos disponibles
        /// </summary>
        private void fnTraerCorreos()
        {
            //clsOperacionClientes gOc = new clsOperacionClientes();

            //try
            //{
            //    string sIdEstructura = Convert.ToString(ViewState["id_estructura"]);

            //    gdvCorreos.DataSource = gOc.fnObtenerCorreoCliente(null, Convert.ToInt32(sIdEstructura), txtRfcConsulta.Text, txtRazonSocialConsulta.Text);
            //    gdvCorreos.DataBind();
            //}
            //catch (Exception ex)
            //{
            //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            //}
        }    
    }
}