﻿using System;
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
using Cancelacion = ServicioCancelacionCFDI;
using AutenticacionCancelacion = ServicioCancelacionAutenticacionCFDI;

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
    private Cancelacion.CancelaCFDBindingClient clienteCancelacion;
    private AutenticacionCancelacion.AutenticacionClient clienteAutenticacionCancelacion;
    private clsInicioSesionUsuario DatosUsuario;
    protected DataTable dtCreditos;
    string Respuesta = string.Empty;
    private DataTable DTCompMail;
    private clsOperacionUsuarios gOu;
    private clsOperacionClientes gOc;
    private clsEnvioCorreoDocs cEd;
    private clsInicioSesionUsuario datosUsuario;
    private Thread hilo;
    private static int valor = 0;
    private clsConfiguracionCreditos cCc;
    public delegate DataTable Operacion();
    private static DataSet creditosT;
    private clsOperacionCuenta gOpeCuenta;
    private int gnNumeroRegistros = 50;

    public Cancelacion.CancelaCFDBindingClient ClienteCancelacion
        
    {
        get
        {
            if (clienteCancelacion == null)
            {
                GenerarClienteCancelacion();
            }

            return clienteCancelacion;
        }
    }

    public AutenticacionCancelacion.AutenticacionClient ClienteAutenticacionCancelacion
    {
        get
        {
            if (clienteAutenticacionCancelacion == null)
            {
                GenerarClienteAutenticacionCancelacion();
            }

            return clienteAutenticacionCancelacion;
        }
    }

    private void GenerarClienteCancelacion()
    {
        clienteCancelacion = new Cancelacion.CancelaCFDBindingClient();
    }

    private void GenerarClienteAutenticacionCancelacion()
    {
        clienteAutenticacionCancelacion = new AutenticacionCancelacion.AutenticacionClient();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        clsComun.fnPonerTitulo(this);


        if (!IsPostBack)
        {
            try
            {
                datosUsuario = clsComun.fnUsuarioEnSesion();
                if (datosUsuario == null)
                    return;

                btnAnterior.Visible = false;
                btnSiguiente.Visible = false;
                fnCargarEstatus();
                fnCargarSucursales();
                fnCargarDocumentos();
                //fnCargcarSeries();
                fnCargarDatosPlantillas();
                
                clsConfiguracionPlantilla Plantilla = new clsConfiguracionPlantilla();
                DataTable Plantillas = new DataTable();
                int idEstructura = Convert.ToInt32(ddlSucursales.Items[1].Value);
                Plantillas = Plantilla.fnObtieneConfiguracionPlantilla(idEstructura);

                fnCargarUsuarios(datosUsuario.id_usuario);

                int nPlantilla = Plantilla.fnRecuperaPlantillaRecursiva(idEstructura);
                if (idEstructura != 0)
                {
                    if (Plantillas.Rows.Count > 0)
                    {
                        datosUsuario.plantilla = Convert.ToInt32(Plantillas.Rows[0]["id_plantilla"]);
                        datosUsuario.color = Convert.ToString(Plantillas.Rows[0]["color"]);
                    }
                    else if (nPlantilla != 0)
                    {
                        datosUsuario.plantilla = nPlantilla;

                    }
                }


                string Fecha3 = (string)ViewState["fecha1"];
                string Fecha4 = (string)ViewState["fecha2"];
                if (Fecha3 != null)
                {
                    txtFechaIni.Text = Fecha3;
                }
                else
                {
                    txtFechaIni.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                }

                if (Fecha4 != null)
                {
                    txtFechaFin.Text = Fecha4;
                }
                else
                {
                    txtFechaFin.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                }

                gOpeCuenta = new clsOperacionCuenta();
                DataTable sdrInfo = gOpeCuenta.fnObtenerDatosFiscales();

                if (sdrInfo != null && sdrInfo.Rows.Count > 0)
                {
                    ViewState["rfc_Emisor"] = sdrInfo.Rows[0]["rfc"].ToString();
                    ViewState["razonSocial_Emisor"] = sdrInfo.Rows[0]["razon_social"].ToString();
                }

                fnRegistrarScript();
                ViewState["GuidPathXMLs"] = Guid.NewGuid().ToString();
                ViewState["GuidPathZIPs"] = Guid.NewGuid().ToString();
                rdlArchivo.SelectedIndex = 0;

   
                fnActualizarLblCreditos();
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
            datosUsuario = clsComun.fnUsuarioEnSesion();
            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, objErr.Message);
            Server.ClearError();
            Response.Redirect("~/webGlobalError.aspx");
        }

    }
    protected void ddlSucursales_SelectedIndexChanged(object sender, EventArgs e)
    {
        datosUsuario = clsComun.fnUsuarioEnSesion();
        clsConfiguracionPlantilla Plantilla = new clsConfiguracionPlantilla();

        //fnCargcarSeries();
        //fnCargarReceptores(Convert.ToInt32(ddlSucursales.SelectedValue));
        DataTable Plantillas = new DataTable();
        int nPlantilla;
        int idEstructura = Convert.ToInt32(ddlSucursales.SelectedValue);

        if (idEstructura != 0)
        {
            Plantillas = Plantilla.fnObtieneConfiguracionPlantilla(idEstructura);
            nPlantilla = Plantilla.fnRecuperaPlantillaRecursiva(idEstructura);

            if (Plantillas.Rows.Count > 0)
            {
                datosUsuario.plantilla = Convert.ToInt32(Plantillas.Rows[0]["id_plantilla"]);
                datosUsuario.color = Convert.ToString(Plantillas.Rows[0]["color"]);
            }
            else if (nPlantilla != 0)
            {
                datosUsuario.plantilla = nPlantilla;

            }
        }
        else
        {
            idEstructura = Convert.ToInt32(ddlSucursales.Items[1].Value);
            Plantillas = Plantilla.fnObtieneConfiguracionPlantilla(idEstructura);
            nPlantilla = Plantilla.fnRecuperaPlantillaRecursiva(idEstructura);

            if (Plantillas.Rows.Count > 0)
            {
                datosUsuario.plantilla = Convert.ToInt32(Plantillas.Rows[0]["id_plantilla"]);
                datosUsuario.color = Convert.ToString(Plantillas.Rows[0]["color"]);
            }
            else if (nPlantilla != 0)
            {
                datosUsuario.plantilla = nPlantilla;
            }
        }

    }
    protected void ddlPlantillas_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            gdvComprobantes.DataSource = null;
            gdvComprobantes.DataBind();
            if (gdvComprobantes.Rows.Count > 0)
            {
                btnCancelar.Visible = true;
                btnDescargar.Visible = true;
                cbSeleccionar.Visible = true;
                //cbPaginado.Visible = true;
                btnExportar.Visible = true;
            }
            else
            {
                btnCancelar.Visible = false;
                btnDescargar.Visible = false;
                cbSeleccionar.Visible = false;
                // cbPaginado.Visible = false;
                btnExportar.Visible = false;
            }
        }
        catch (Exception)
        {
            //NA
        }
    }

    protected void ddlDocumentos_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSerie.Enabled = (ddlDocumentos.SelectedItem.Text != "Retenciones") ? true : false; 
    }

    protected void gdvComprobantes_SelectedIndexChanged(object sender, EventArgs e)
    {
        gOu = new clsOperacionUsuarios();
        gOc = new clsOperacionClientes();
        int nid_cfd, nid_estructura;
        string sDoc, snombreDoc, sRfc, sRazon_Social;
        DTCompMail = (DataTable)ViewState["ExportarExcel"];
        nid_cfd = Convert.ToInt32(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["id_cfd"].ToString());
        nid_estructura = Convert.ToInt32(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["id_estructura"].ToString());
        ViewState["id_estructura"] = nid_estructura;

        sDoc = Convert.ToString(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["documento"].ToString());
        snombreDoc = Convert.ToString(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["UUID"].ToString());
        sRfc = Convert.ToString(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["rfc"].ToString());
        sRazon_Social = Convert.ToString(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["razon_social"].ToString());

        txtCorreoEmisor.Text = gOu.fnObtenerCorreoReceptor(nid_cfd);

        gOc = new clsOperacionClientes();
        DataTable DTCorreo = gOc.fnObtenerCorreoCliente(null, nid_estructura, sRfc, sRazon_Social);

        if (DTCorreo.Rows.Count > 0)
        {
            txtCorreoCliente.Text = DTCorreo.Rows[0]["correo"].ToString();
        }
        else
            txtCorreoCliente.Text = string.Empty;

        ViewState["nid_cfd"] = nid_cfd;
        ViewState["sDoc"] = sDoc;
        ViewState["snombreDoc"] = snombreDoc;
        //Color vacio en txt
        txtCorreoCliente.BorderColor = System.Drawing.Color.Empty;
        txtCorreoCC.BorderColor = System.Drawing.Color.Empty;


        if (ViewState["rfc_Emisor"] != null && ViewState["razonSocial_Emisor"] != null)
            txtCorreoMsj.Text = Resources.resCorpusCFDIEs.varMensajeCorreo + "\n" + "\n" + ViewState["razonSocial_Emisor"] + "\n" + "RFC:" + "\n" + ViewState["rfc_Emisor"];

        mpeEnvioCorreo.Show();

    }
    protected void btnAceptarCor_Click(object sender, EventArgs e)
    {
        clsConfiguracionPlantilla PlantillaC = new clsConfiguracionPlantilla();
        gOu = new clsOperacionUsuarios();
        int nid_cfd = Convert.ToInt32(ViewState["nid_cfd"]);
        string snombreDoc, sDoc;
        snombreDoc = Convert.ToString(ViewState["snombreDoc"]);
        sDoc = Convert.ToString(ViewState["sDoc"]);
        string sEmailUsu = gOu.fnObtenerCorreoReceptor(nid_cfd);

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

            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.txtCorreo + " " + sCadena, Resources.resCorpusCFDIEs.varContribuyente);
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


        clsConfiguracionPlantilla plantillas = new clsConfiguracionPlantilla();
        string plantilla = plantillas.fnRecuperaPlantillaNombre(DatosUsuario.plantilla);


        //Verifica si se envia el comprobante en ZIP o no.

        if (rdlArchivo.SelectedIndex == 1)
        {

            bEnvio = cEd.fnPdfEnvioCorreo(plantilla, Convert.ToString(nid_cfd), sDoc,
                              clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + snombreDoc + ".pdf",
                              DatosUsuario.id_contribuyente, DatosUsuario.id_rfc, DatosUsuario.color, sMailTo,
                              "PAXFacturacion", Mensaje, Convert.ToString(ViewState["GuidPathZIPs"]),
                              Convert.ToString(ViewState["GuidPathXMLs"]), snombreDoc, sCC);
        }
        else
        {
            bEnvio = cEd.fnPdfEnvioCorreoSinZIP(plantilla, Convert.ToString(nid_cfd), sDoc,
                              clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + snombreDoc + ".pdf",
                              DatosUsuario.id_contribuyente, DatosUsuario.id_rfc, DatosUsuario.color, sMailTo,
                              "PAXFacturacion", Mensaje, Convert.ToString(ViewState["GuidPathZIPs"]),
                              Convert.ToString(ViewState["GuidPathXMLs"]), snombreDoc, sCC);
        }


        if (bEnvio == true)
        {
            string[] split = sMailTo.Split(',');
            sMailTo = string.Empty;
            foreach (string s in split)
            {
                sMailTo = sMailTo + "\\n" + s;
            }
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgEnvioMail + "<br />" + sMailTo);
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
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgErrorEnvioMail);
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
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        //Valida si el usuario cuenta con el servicio de cancelación
        bool bValSer = true;
        bValSer = fnVerificarServicio();
        if (bValSer == false)
            return;

        bool bValCre = true;
        //Valida si contiene créditos suficientes
        bValCre = fnActualizarLblCreditosCancelación();
        if (bValCre == false)
            return;

        int bander = 0;
        foreach (GridViewRow renglon in gdvComprobantes.Rows)
        {
            CheckBox CbCan;

            CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

            //Seleccionado y activo
            if (CbCan.Checked == true && renglon.Cells[11].Text == "A")
            {
                bander = 1;
            }

        }
        if (bander == 1)
        {
            modalCreditos.Show();
        }
        else
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCancelaVacio);
        }

    }
    /// <summary>
    /// comprime en un zip todos los xml y pdf contenidos en las filas seleccionadas del grid
    /// </summary>
    protected void btnDescargar_Click(object sender, EventArgs e)
    {



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





        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }

    }
    protected void hpCancelar_Click(object sender, EventArgs e)
    {
        try
        {
            //Valida si el usuario cuenta con el servicio de cancelación
            bool bValSer = true;
            bValSer = fnVerificarServicio();
            if (bValSer == false)
                return;

            bool bValCre = true;
            //Valida si contiene créditos suficientes
            bValCre = fnActualizarLblCreditosCancelación();
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
                    CbCan.Checked = true;
                }
            }

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
        try
        {
            bool bValCre = false;
            //Valida si contiene créditos suficientes
            bValCre = fnActualizarLblCreditosCancelación();
            if (bValCre == false)
                return;

            clsConfiguracionCreditos CreditosConf = new clsConfiguracionCreditos();
            double ValorCredito = CreditosConf.fnRecuperaPrecioServicio(2);
            datosUsuario = clsComun.fnUsuarioEnSesion();
            clsConfiguracionCreditos creditos = new clsConfiguracionCreditos();
            string sClaveUsuario = creditos.fnObtenerClaveUsuario(datosUsuario.id_usuario);

            DataSet dCreditos = new DataSet();
            dCreditos = creditos.fnRecuperaCreditos(sClaveUsuario);
            DataTable dtServicios = dCreditos.Tables[2];
            bool TieneCancelacion = false;

            foreach (DataRow renglonServicio in dtServicios.Rows)
            {
                string sDescripcion = Convert.ToString(renglonServicio["descripcion"]);
                if (sDescripcion == "Cancelacion")
                {
                    TieneCancelacion = true;
                }
            }

            if (TieneCancelacion == true)
            {

                ArrayList listUUid = new ArrayList();
                ArrayList idCfd = new ArrayList();
                int bandera = 0;
                Label sIdCfdS = new Label();
                int nIdContribuyente = 0;
                XmlDocument comprobante = new XmlDocument();
                string sRfcEmisor = string.Empty;
                string sUUID = string.Empty;
                string sfechaTimbrado = string.Empty;
                string firma = string.Empty;
                double retCreditos = 0;

                retCreditos = fnRevisaCreditos();

                if (retCreditos > 0)
                {
                    if (retCreditos < ValorCredito)
                    {
                        txtcomentario.Text = string.Empty;
                        modalCreditos.Hide();
                        modalRevisaCreditos.Show();
                        return;
                    }
                    foreach (GridViewRow renglon in gdvComprobantes.Rows)
                    {
                        CheckBox CbCan;

                        CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                        //Seleccionado y activo
                        if (CbCan.Checked == true && renglon.Cells[11].Text == "A")
                        {
                            //Codigo del web service de cancelación
                            //Cancelar SAT
                            gDAL = new clsOperacionConsulta();
                            sIdCfdS = ((Label)renglon.FindControl("lblid_cfd"));
                            nIdContribuyente = clsComun.fnUsuarioEnSesion().id_contribuyente;
                            idCfd.Add(sIdCfdS.Text);
                            //Recupera el comprobante

                            comprobante = gDAL.fnObtenerComprobanteXML(nIdContribuyente, sIdCfdS.Text);
                            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(comprobante.NameTable);
                            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                            XPathNavigator navEncabezado = comprobante.CreateNavigator();

                            sRfcEmisor = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value;
                            sUUID = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;
                            sfechaTimbrado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value;

                            listUUid.Add(sUUID.ToUpper());
                            //clsComun.fnNuevaPistaAuditoria("webConsultasCFDI", "fnCancelarComprobante", "Recupera informacion para prefirma cancelacion.");

                            //verifica si los créditos son suficientes para agregar otro comprobante a la lista
                            retCreditos -= ValorCredito;
                            if (retCreditos < ValorCredito)
                            {
                                break;
                            }
                        }
                    }
                    fnActualizarLblCreditos();

                    bool CancelacionPrueba = Convert.ToBoolean(clsComun.ObtenerParamentro("CancelacionPrueba"));

                    if (CancelacionPrueba == false)
                    {

                        try
                        {

                            clsComun.fnNuevaPistaAuditoria("webConsultasCFDI", "CancelarBloqueCfdi", "Llama el web service del SAT." + sRfcEmisor + listUUid.Count + DatosUsuario.userName);

                            clsCancelacionSAT cancelacion = new clsCancelacionSAT();
                            DatosUsuario = clsComun.fnUsuarioEnSesion();
                            string Respuesta = cancelacion.CancelarBloqueCfdi(sRfcEmisor, listUUid, idCfd, DatosUsuario);
                            XmlDocument xmldoc = new XmlDocument();
                            xmldoc.PreserveWhitespace = false;
                            Respuesta=Respuesta.Replace("&", "&amp;");
                            xmldoc.LoadXml(Respuesta);

                            //clsComun.fnNuevaPistaAuditoria("webConsultasCFDI", "CancelarBloqueCfdi", "Contesta el web service.");

                            int nodos = xmldoc.DocumentElement.ChildNodes.Count;
                            for (int i = 0; i < nodos; i++)
                            {
                                System.Xml.XmlNode nodo = xmldoc.DocumentElement.ChildNodes[i];
                                Respuesta = clsComun.fnRecuperaErrorSAT(nodo.ChildNodes[1].InnerText, "Cancelación");


                                //clsComun.fnNuevaPistaAuditoria("webConsultasCFDI", "CancelarBloqueCfdi", Respuesta);

                                string[] est = Respuesta.Split('-');
                                if (est.Length > 2)
                                {
                                    Respuesta = est[0].Trim() + " - " + est[1].Trim();
                                }
                                else
                                {
                                    Respuesta = est[0].Trim();
                                }

                                if (Respuesta == "201" || Respuesta == "202")
                                {
                                    gDAL = new clsOperacionConsulta();
                                    Int32 idcfdi = cancelacion.fnRecuperaIdCFD(Convert.ToInt32(nodo.ChildNodes[4].InnerText));

                                    int retVal = gDAL.fnCancelarComprobante(idcfdi, txtcomentario.Text);

                                    if (retVal != 0)
                                    {
                                        bandera = 1;

                                        if (Respuesta == "201")
                                            fnActualizaCreditos();

                                        txtcomentario.Text = string.Empty;
                                        clsComun.fnNuevaPistaAuditoria(
                                            "webConsultasCFDI",
                                            "fnCancelarComprobante",
                                            "Se canceló el comprobante con ID " + idcfdi
                                            );
                                    }

                                }
                                else
                                {

                                    clsComun.fnMostrarMensaje(this, nodo.ChildNodes[2].InnerText);
                                }
                            }

                            if (nodos == 0)
                                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varNoCancel);

                            //clsComun.fnNuevaPistaAuditoria("webConsultasCFDI", "CancelarBloqueCfdi", "Envia cancelacion al SAT.");
                        }
                        catch (Exception ex)
                        {
                            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varNoCancel);
                        }
                    }
                    else
                    {
                        fnActualizaCreditos();
                        clsComun.fnMostrarMensaje(this, "Cancelación Exitosa, Modo de Pruebas");

                    }
                    if (bandera == 1)
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCancelacionCfdMultiple);
                        btnConsultar_Click(sender, e);
                        // bool condicion = fnRealizarConsulta();
                        //if (condicion == true)
                        //{
                        //    cbPaginado.Checked = true;
                        //}
                        //else
                        //{
                        //    cbPaginado.Checked = false;
                        //}
                    }
                }
                else
                {
                    txtcomentario.Text = string.Empty;
                    modalCreditos.Hide();
                    modalRevisaCreditos.Show();
                }
            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarNoServicioCancela);
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
    protected void btnExportar_Click(object sender, EventArgs e)
    {

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
                                                        String.Format("<script>window.open('{0}?p={1}&o={2}&d={3}','123','height=160, width= 355, status=yes, resizable= no, scrollbars= no, toolbar= no,menubar= no');</script>",
                                                                       "webDescargaConsultaV2.aspx", sIDS, ddlOrigen.SelectedValue, ddlDocumentos.SelectedItem.Text), false);

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
    protected void btnCancelarCreditos_Click(object sender, EventArgs e)
    {
        modalCreditos.Hide();
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
    protected void btnCancelarModal_Click(object sender, EventArgs e)
    {
        linkModal_ModalPopupExtender.Hide();
        mpeEnvioCorreo.Show();
        fnLimpiarControlesCorreo();
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

            nRetorno = clsTimbradoFuncionalidad.fnActualizarCreditos(idCredito, idEstructura, Creditos, "C");
            clsTimbradoFuncionalidad.fnActualizarCreditosHistorico(idCredito, idEstructura, Creditos);

            DatosUsuario = clsComun.fnUsuarioEnSesion();
            dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(DatosUsuario.id_usuario.ToString(), string.Empty, 'A', 'S');
            if (dtCreditos.Rows.Count > 0)
            {
                fnActualizarLblCreditos();
            }
            else
            {
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
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
        DatosUsuario = clsComun.fnUsuarioEnSesion();
        dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(DatosUsuario.id_usuario.ToString(), string.Empty, 'A', 'S');
        ViewState["dtCreditos"] = dtCreditos;
        cCc = new clsConfiguracionCreditos();
        bool bRespuesta = true;
        double dCostCred = cCc.fnRecuperaPrecioServicio(2); //Precio servicio cancelación
        if (dtCreditos.Rows.Count > 0)
        {
            double TCreditos = 0;
            TCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

            if (TCreditos == 0)//if (dtCreditos.Rows[0]["creditos"].ToString() == "0.00")
            {
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
                double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
                if (creditos == 0)
                {
                    lblCredValor.Text = "0";

                    Label121.Visible = false;
                    lblCosCre.Visible = false;
                    Label7.Visible = true;
                    //modalRevisaCreditos.Show();

                    bRespuesta = false;
                }
                else
                {
                    //lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                    double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                    lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                    //Se valida que tenga créditos suficientes
                    if (dCreditos < dCostCred)
                    {
                        Label121.Visible = false;
                        lblCosCre.Visible = true;
                        Label7.Visible = false;
                        //modalRevisaCreditos.Show();

                        bRespuesta = false;
                    }
                }
            }
            else
            {
                //lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                //Se valida que tenga créditos suficientes
                if (dCreditos < dCostCred)
                {
                    Label121.Visible = false;
                    lblCosCre.Visible = true;
                    Label7.Visible = false;
                    //modalRevisaCreditos.Show();

                    bRespuesta = false;
                }
            }
        }
        else
        {
            clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
            dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
            double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
            if (creditos == 0)
            {
                lblCredValor.Text = "0";

                Label121.Visible = false;
                lblCosCre.Visible = false;
                Label7.Visible = true;
                //modalRevisaCreditos.Show();

                bRespuesta = false;
            }
            else
            {
                //lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();

            }
        }

    }

    /// <summary>
    /// Revisa créditos para la cancelación
    /// </summary>
    /// <returns></returns>
    private bool fnActualizarLblCreditosCancelación()
    {
        DatosUsuario = clsComun.fnUsuarioEnSesion();
        dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(DatosUsuario.id_usuario.ToString(), string.Empty, 'A', 'S');
        cCc = new clsConfiguracionCreditos();
        bool bRespuesta = true;
        double dCostCred = cCc.fnRecuperaPrecioServicio(2); //Precio servicio cancelación
        if (dtCreditos.Rows.Count > 0)
        {
            double TCreditos = 0;
            TCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

            if (TCreditos == 0)//if (dtCreditos.Rows[0]["creditos"].ToString() == "0.00")
            {
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
                double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
                if (creditos == 0)
                {
                    lblCredValor.Text = "0";

                    Label121.Visible = false;
                    lblCosCre.Visible = false;
                    Label7.Visible = true;
                    modalRevisaCreditos.Show();

                    bRespuesta = false;
                }
                else
                {
                    //lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                    double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                    lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                    //Se valida que tenga créditos suficientes
                    if (dCreditos < dCostCred)
                    {
                        Label121.Visible = false;
                        lblCosCre.Visible = true;
                        Label7.Visible = false;
                        modalRevisaCreditos.Show();

                        bRespuesta = false;
                    }
                }
            }
            else
            {
                //lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                //Se valida que tenga créditos suficientes
                if (dCreditos < dCostCred)
                {
                    Label121.Visible = false;
                    lblCosCre.Visible = true;
                    Label7.Visible = false;
                    modalRevisaCreditos.Show();

                    bRespuesta = false;
                }
            }
        }
        else
        {
            clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
            dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
            double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
            if (creditos == 0)
            {
                lblCredValor.Text = "0";

                Label121.Visible = false;
                lblCosCre.Visible = false;
                Label7.Visible = true;
                modalRevisaCreditos.Show();

                bRespuesta = false;
            }
            else
            {
                //lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                //Se valida que tenga créditos suficientes
                if (dCreditos < dCostCred)
                {
                    Label121.Visible = false;
                    lblCosCre.Visible = true;
                    Label7.Visible = false;
                    modalRevisaCreditos.Show();

                    bRespuesta = false;
                }
            }
        }
        return bRespuesta;
    }

    /// <summary>
    /// Método que se encarga de cargar los datos de la plantilla
    /// </summary>
    private void fnCargarDatosPlantillas()
    {
        DatosUsuario = clsComun.fnUsuarioEnSesion();
        clsOperacionRFC rfc = new clsOperacionRFC();
        int id_rfc = 0;
        if (DatosUsuario.id_rfc == 0)
        {
            id_rfc = rfc.fnObtieneidRFCCon(DatosUsuario.rfc, DatosUsuario.id_contribuyente);
            DatosUsuario.id_rfc = id_rfc;
        }
    }

    /// <summary>
    /// Carga los documentos disponibles
    /// </summary>
    private void fnCargarDocumentos()
    {
        try
        {
            gDAL = new clsOperacionConsulta();

            ddlDocumentos.DataSource = gDAL.fnObtenerDocumentosPago(clsComun.fnUsuarioEnSesion().id_usuario);
            ddlDocumentos.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            ddlDocumentos.DataSource = null;
            ddlDocumentos.DataBind();
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

    /// <summary>
    /// Método que se encarga de obtener cargar las plantillas configuradas del usuario
    /// </summary>
    private void fnCargarPlantillas()
    {
        try
        {
            gDAL = new clsOperacionConsulta();
            //Revisar los creditos disponibles.
            DatosUsuario = clsComun.fnUsuarioEnSesion();

            ddlPlantillas.DataSource = gDAL.fnObtenerPlantillasUsuarios(DatosUsuario.id_usuario);
            ddlPlantillas.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            ddlPlantillas.DataSource = null;
            ddlPlantillas.DataBind();
        }
    }

    /// <summary>
    /// Carga los receptores activos para usarlos en los filtros
    /// </summary>
    //private void fnCargarReceptores(int pIdEstructura)
    //{
    //    try
    //    {
    //        gDAL = new clsOperacionConsulta();

    //        ddlReceptor.DataSource = gDAL.fnObtenerReceptores(pIdEstructura);
    //        ddlReceptor.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
    //        ddlReceptor.DataSource = null;
    //        ddlReceptor.DataBind();
    //    }
    //}

    /// <summary>
    /// Carga las series disponibles para la convinación estructura-documento
    /// </summary>
    //private void fnCargcarSeries()
    //{
    //    try
    //    {
    //        gDAL = new clsOperacionConsulta();

    //        ddlSeries.DataSource = gDAL.fnObtenerSeries(ddlSucursales.SelectedValue, ddlDocumentos.SelectedValue);
    //        ddlSeries.DataBind();

    //    }
    //    catch (Exception ex)
    //    {
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
    //        ddlSeries.DataSource = null;
    //        ddlSeries.DataBind();
    //    }
    //}

    /// <summary>
    /// Carga las sucursales disponibles para el usuario
    /// </summary>
    private void fnCargarSucursales()
    {
        try
        {

            gDAL = new clsOperacionConsulta();
            datosUsuario = clsComun.fnUsuarioEnSesion();

            DataTable dtAuxiliar = clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario);
            ViewState["dtAuxiliar"] = dtAuxiliar;
            DataRow drFila = dtAuxiliar.NewRow();
            drFila["id_estructura"] = 0;// dtAuxiliar.Rows[0]["id_estructura"];
            drFila["nombre"] = Resources.resCorpusCFDIEs.VarDropTodos;

            dtAuxiliar.Rows.InsertAt(drFila, 0);

            ddlSucursales.DataSource = dtAuxiliar;//gDAL.fnObtenerSucursales();
            ddlSucursales.DataBind();

            ddlSucursales.SelectedValue = Resources.resCorpusCFDIEs.VarDropTodos;

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
        try
        {
            gDAL = new clsOperacionConsulta();
            ddlUsuarios.DataSource = gDAL.fnObtenerUsuarios(pnIdUsuario);
            ddlUsuarios.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            ddlUsuarios.DataSource = null;
            ddlUsuarios.DataBind();
        }
    }

    /// <summary>
    /// Elimina las carpetas temporales que fueron creadas
    /// </summary>
    public void fnLimpiaCarpetas()
    {

        foreach (string file in Directory.GetFiles(clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"]))
        {
            File.Delete(file);
        }
        Directory.Delete(clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"], true);

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
        string sIdReceptor = parametro;
        gOc = new clsOperacionClientes();
        DataTable sdrLector = gOc.fnEditarReceptor(sIdReceptor);
        txtCorreoCliente.Text = sdrLector.Rows[0]["correo"].ToString();
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
        gDAL = new clsOperacionConsulta();
        try
        {
            if (ddlDocumentos.SelectedItem.Text == "Retenciones" )
            {

                dtComprobantes = gDAL.fnObtenerComprobantesRetenciones(
                        "",
                        txtRFCReceptor.Text,
                        txtRFCEmisor.Text,
                        ddlEstatus.SelectedValue,
                        ddlSucursales.SelectedValue,
                        ddlDocumentos.SelectedValue,
                        txtSerie.Text,
                        txtFolioIni.Text,
                        txtFolioFin.Text,
                        Convert.ToDateTime(txtFechaIni.Text),
                        Convert.ToDateTime(txtFechaFin.Text), txtUUID.Text,
                        ddlUsuarios.SelectedValue,
                        pnPagina,
                        gnNumeroRegistros
                        );
            } 
            else
            {
                switch (ddlOrigen.SelectedValue)             
                {
                    case "R":
                        dtComprobantes = gDAL.fnObtenerComprobantesPortalWebService(
                                ddlOrigen.SelectedValue,
                                txtRFCReceptor.Text,
                                txtRFCEmisor.Text,
                                ddlEstatus.SelectedValue,
                                ddlSucursales.SelectedValue,
                                ddlDocumentos.SelectedValue,
                                txtSerie.Text,
                                txtFolioIni.Text,
                                txtFolioFin.Text,
                                Convert.ToDateTime(txtFechaIni.Text),
                                Convert.ToDateTime(txtFechaFin.Text), txtUUID.Text,
                                ddlUsuarios.SelectedValue,
                                pnPagina,
                                gnNumeroRegistros
                                );
                        break;
                    case "C":
                        dtComprobantes = gDAL.fnObtenerComprobantesPortalWebService(
                                ddlOrigen.SelectedValue,
                                txtRFCReceptor.Text,
                                txtRFCEmisor.Text,
                                ddlEstatus.SelectedValue,
                                ddlSucursales.SelectedValue,
                                ddlDocumentos.SelectedValue,
                                txtSerie.Text,
                                txtFolioIni.Text,
                                txtFolioFin.Text,
                                Convert.ToDateTime(txtFechaIni.Text),
                                Convert.ToDateTime(txtFechaFin.Text), txtUUID.Text,
                                ddlUsuarios.SelectedValue,
                                pnPagina,
                                gnNumeroRegistros
                                );
                            break;
                    case "I":
                            break;
                    case "IG":
                            break;
                    default:
                            break;
                }
            }
            
            

            //dtComprobantes = gDAL.fnObtenerComprobantes(
            //    txtRFCReceptor.Text,
            //    ddlEstatus.SelectedValue,
            //    ddlSucursales.SelectedValue,
            //    ddlDocumentos.SelectedValue,
            //    txtSerie.Text,
            //    txtFolioIni.Text,
            //    txtFolioFin.Text,
            //    Convert.ToDateTime(txtFechaIni.Text),
            //    Convert.ToDateTime(txtFechaFin.Text), txtUUID.Text,
            //    ddlUsuarios.SelectedValue,
            //    pnPagina,
            //    gnNumeroRegistros
            //    );

            ViewState["ExportarExcel"] = dtComprobantes;

            clsComun.fnNuevaPistaAuditoria(
                "webConsultasCFDI",
                "fnObtenerComprobantes",
                "Se consultó con los filtros",
                txtRFCReceptor.Text,
                ddlEstatus.SelectedItem.Text,
                ddlSucursales.SelectedItem.Text,
                ddlDocumentos.SelectedItem.Text,
                txtSerie.Text,
                txtFolioIni.Text,
                txtFolioFin.Text,
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
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorConsulta);
        }
        return dtComprobantes;
    }

    /// <summary>
    /// Función que se encarga de obtener los créditos del usuario
    /// </summary>
    /// <param name="clave_usuario">Usuario</param>
    /// <returns></returns>
    private DataSet fnRecuperaCreditosusuario(string clave_usuario)
    {

        DataSet creditos = new DataSet();
        clsConfiguracionCreditos Creditos = new clsConfiguracionCreditos();
        try
        {
            creditos = Creditos.fnRecuperaCreditos(clave_usuario);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }

        return creditos;
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
    /// Revisa que existan creditos.
    /// </summary>
    private double fnRevisaCreditos()
    {
        double retorno = 0;
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
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
                ViewState["dtCreditos"] = dtCreditos;
                double creditos2 = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
                credit = creditos2;
                if (creditos2 == 0)
                {
                    modalCreditos.Show();
                }
            }
            else
            {
                credit = creditos;
            }

        }
        else
        {
            clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
            dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
            ViewState["dtCreditos"] = dtCreditos;
            double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
            credit = creditos;
            if (creditos == 0)
            {
                modalCreditos.Show();
            }
        }

        retorno = credit;//dtCreditos.Rows.Count;

        return retorno;
    }

    /// <summary>
    /// realiza la búsqueda de los correos disponibles
    /// </summary>
    private void fnTraerCorreos()
    {
        clsOperacionClientes gOc = new clsOperacionClientes();

        try
        {
            string sIdEstructura = Convert.ToString(ViewState["id_estructura"]);

            gdvCorreos.DataSource = gOc.fnObtenerCorreoCliente(null, Convert.ToInt32(sIdEstructura), txtRfcConsulta.Text, txtRazonSocialConsulta.Text);
            gdvCorreos.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    /// <summary>
    /// Función que se encarga de revisar si el usuario cuenta con el servicio de cancelación
    /// </summary>
    /// <returns></returns>
    private bool fnVerificarServicio()
    {
        //Verificamos que ese usuario cuente con el servicio de generación y timbrado
        clsConfiguracionPlantilla configura = new clsConfiguracionPlantilla();
        datosUsuario = clsComun.fnUsuarioEnSesion();

        int idEstrserv = configura.fnRecuperaEstructura(datosUsuario.id_usuario);

        creditosT = fnRecuperaCreditosusuario(datosUsuario.userName);

        DataTable tblServicios = new DataTable();
        tblServicios = creditosT.Tables[2];
        bool TieneCancelacion = false;
        foreach (DataRow renglon in tblServicios.Rows)
        {

            string sDescripcion = Convert.ToString(renglon["descripcion"]);
            if (sDescripcion == "Cancelacion")
            {
                TieneCancelacion = true;
            }
        }

        if (TieneCancelacion == false)
        {
            Label121.Visible = true;
            lblCosCre.Visible = false;
            Label7.Visible = false;
            modalRevisaCreditos.Show();
        }

        return TieneCancelacion;
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

    private HttpRequestMessageProperty AutenticaServicioCancelacion()
    {
        //Aceptar certificados caducados
        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(Consultas_webConsultasCFDI.AcceptAllCertificatePolicy);

        var token = ClienteAutenticacionCancelacion.Autentica();
        var headerValue = string.Format("WRAP access_token=\"{0}\"", HttpUtility.UrlDecode(token));
        var property = new HttpRequestMessageProperty();
        property.Method = "POST";
        property.Headers.Add(HttpRequestHeader.Authorization, headerValue);

        return property;
    }

    public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    /// <summary>
    ///// Herramienta para exportar a excel
    ///// </summary>
    //private void ExportDataTableToCSV(DataTable dt)
    //{
    //    DataTable toExcel = dt.Copy();
    //    HttpContext context = HttpContext.Current;

    //    /* Iteramos cada una de las columnas del datatable para
    //    * escribir el encabezado del CSV */
    //    foreach (DataColumn column in toExcel.Columns)
    //    {
    //        context.Response.Write(column.ColumnName + ",");

    //    }
    //    // Creamos una nueva linea en el archivo despues de escribir el
    //    // encabezado
    //    context.Response.Write(Environment.NewLine);

    //    /* Empezamos a escribir cada una de las columnas
    //    * de cada fila separadas por comas */
    //    foreach (DataRow row in toExcel.Rows)
    //    {
    //        for (int i = 0; i < toExcel.Columns.Count; i++)
    //        {
    //            context.Response.Write(row[i].ToString().Replace(",", string.Empty) + ",");
    //        }
    //        // Se finalizo de escribir la fila, asi que creamos una nueva linea
    //        context.Response.Write(Environment.NewLine);
    //    }
    //}
    //static void SumaCompletada(IAsyncResult itfAR)
    //{
    //    AsyncResult ar = (AsyncResult)itfAR;
    //    Operacion b = (Operacion)ar.AsyncDelegate;
    //    //b.EndInvoke(itfAR);
    //}



    //private void MetodoCallBack(IAsyncResult result)
    //{
    //        DataTable dtAuxiliar = new DataTable();

    //        SqlCommand cmd = (SqlCommand)result.AsyncState;
    //        SqlDataReader dr = cmd.EndExecuteReader(result);
    //        dtAuxiliar.Load(dr);

    //        cmd.Dispose();

    //        //ScriptManager Scritp = new ScriptManager();
    //        //webGlobalMaster master = (webGlobalMaster)this.Master;
    //        //Scritp = (ScriptManager)master.FindControl("ScriptManager");
    //        //System.Web.UI.Control btn = new System.Web.UI.Control();
    //        //btn = (LinkButton)Scritp.FindControl("lnkDescarga");

    //        //Scritp.RegisterDataItem(btn, "Descargar Consulta");
    
    //        DataTable dtNew = new DataTable();
    //        DataColumn columna1 = new DataColumn();
    //        columna1.DataType = System.Type.GetType("System.String");
    //        columna1.AllowDBNull = true;
    //        columna1.Caption = "UUID";
    //        columna1.ColumnName = "UUID";
    //        columna1.DefaultValue = null;
    //        dtNew.Columns.Add(columna1);

    //        DataColumn columna2 = new DataColumn();
    //        columna2.DataType = System.Type.GetType("System.String");
    //        columna2.AllowDBNull = true;
    //        columna2.Caption = "serie";
    //        columna2.ColumnName = "serie";
    //        columna2.DefaultValue = null;
    //        dtNew.Columns.Add(columna2);

    //        DataColumn columna3 = new DataColumn();
    //        columna3.DataType = System.Type.GetType("System.String");
    //        columna3.AllowDBNull = true;
    //        columna3.Caption = "folio";
    //        columna3.ColumnName = "folio";
    //        columna3.DefaultValue = null;
    //        dtNew.Columns.Add(columna3);

    //        DataColumn columna4 = new DataColumn();
    //        columna4.DataType = System.Type.GetType("System.String");
    //        columna4.AllowDBNull = true;
    //        columna4.Caption = "razon_social";
    //        columna4.ColumnName = "razon_social";
    //        columna4.DefaultValue = null;
    //        dtNew.Columns.Add(columna4);

    //        DataColumn columna5 = new DataColumn();
    //        columna5.DataType = System.Type.GetType("System.String");
    //        columna5.AllowDBNull = true;
    //        columna5.Caption = "sucursal";
    //        columna5.ColumnName = "sucursal";
    //        columna5.DefaultValue = null;
    //        dtNew.Columns.Add(columna5);

    //        DataColumn columna6 = new DataColumn();
    //        columna6.DataType = System.Type.GetType("System.String");
    //        columna6.AllowDBNull = true;
    //        columna6.Caption = "documento";
    //        columna6.ColumnName = "documento";
    //        columna6.DefaultValue = null;
    //        dtNew.Columns.Add(columna6);

    //        DataColumn columna7 = new DataColumn();
    //        columna7.DataType = System.Type.GetType("System.String");
    //        columna7.AllowDBNull = true;
    //        columna7.Caption = "total";
    //        columna7.ColumnName = "total";
    //        columna7.DefaultValue = null;
    //        dtNew.Columns.Add(columna7);

    //        DataColumn columna13 = new DataColumn();
    //        columna13.DataType = System.Type.GetType("System.String");
    //        columna13.AllowDBNull = true;
    //        columna13.Caption = "moneda";
    //        columna13.ColumnName = "moneda";
    //        columna13.DefaultValue = null;
    //        dtNew.Columns.Add(columna13);


    //        DataColumn columna8 = new DataColumn();
    //        columna8.DataType = System.Type.GetType("System.String");
    //        columna8.AllowDBNull = true;
    //        columna8.Caption = "fecha";
    //        columna8.ColumnName = "fecha";
    //        columna8.DefaultValue = null;
    //        dtNew.Columns.Add(columna8);

    //        DataColumn columna9 = new DataColumn();
    //        columna9.DataType = System.Type.GetType("System.String");
    //        columna9.AllowDBNull = true;
    //        columna9.Caption = "estatus";
    //        columna9.ColumnName = "estatus";
    //        columna9.DefaultValue = null;
    //        dtNew.Columns.Add(columna9);

    //        DataColumn columna10 = new DataColumn();
    //        columna10.DataType = System.Type.GetType("System.String");
    //        columna10.AllowDBNull = true;
    //        columna10.Caption = "rfc";
    //        columna10.ColumnName = "rfc";
    //        columna10.DefaultValue = null;
    //        dtNew.Columns.Add(columna10);

    //        DataColumn columna11 = new DataColumn();
    //        columna11.DataType = System.Type.GetType("System.String");
    //        columna11.AllowDBNull = true;
    //        columna11.Caption = "fecha_cancelacion";
    //        columna11.ColumnName = "fecha_cancelacion";
    //        columna11.DefaultValue = null;
    //        dtNew.Columns.Add(columna11);

    //        DataColumn columna12 = new DataColumn();
    //        columna12.DataType = System.Type.GetType("System.String");
    //        columna12.AllowDBNull = true;
    //        columna12.Caption = "comentarios_cancelacion";
    //        columna12.ColumnName = "comentarios_cancelacion";
    //        columna12.DefaultValue = null;
    //        dtNew.Columns.Add(columna12);

    //        foreach (DataRow renglon in dtAuxiliar.Rows)
    //        {
    //            DataRow nuevo = dtNew.NewRow();
    //            nuevo["UUID"] = renglon["UUID1"];
    //            nuevo["serie"] = renglon["serie"];
    //            nuevo["folio"] = renglon["folio"];
    //            nuevo["razon_social"] = renglon["razon_social"];
    //            nuevo["sucursal"] = renglon["sucursal"];
    //            nuevo["documento"] = renglon["documento"];
    //            nuevo["total"] = renglon["total"];
    //            nuevo["moneda"] = renglon["moneda"];
    //            nuevo["fecha"] = renglon["fecha"];

    //            switch (Convert.ToString(renglon["estatus"]))
    //            {
    //                case "P":
    //                    nuevo["estatus"] = "Pendiente";
    //                    break;
    //                case "C":
    //                    nuevo["estatus"] = "Cancelado";
    //                    break;
    //                case "A":
    //                    nuevo["estatus"] = "Activo";
    //                    break;
    //                default:
    //                    nuevo["estatus"] = renglon["estatus"];
    //                    break;
    //            }

    //            nuevo["rfc"] = renglon["rfc"];
    //            nuevo["fecha_cancelacion"] = renglon["fecha_cancelacion"];
    //            nuevo["comentarios_cancelacion"] = renglon["comentarios_cancelacion"];
    //            dtNew.Rows.Add(nuevo);
    //        }
    //        ArrayList ejemplo = new ArrayList();
    //        ejemplo.Add("UUID");
    //        ejemplo.Add("Serie");
    //        ejemplo.Add("folio");
    //        ejemplo.Add("razon_social");
    //        ejemplo.Add("sucursal");
    //        ejemplo.Add("documento");
    //        ejemplo.Add("total");
    //        ejemplo.Add("moneda");
    //        ejemplo.Add("fecha");
    //        ejemplo.Add("estatus");
    //        ejemplo.Add("rfc");
    //        ejemplo.Add("fecha_cancelacion");
    //        ejemplo.Add("comentarios_cancelacion");

    //        ExportarExc(ejemplo, dtNew);
    //    //dataTableAExcel(dtNew);

           

    //}

    void TimeoutAsyncOperation(IAsyncResult ar)
    {

    }

//    public void ExportarExc(ArrayList titulos, DataTable datos)
//    {
//        try
//        {
//            Guid Giud = Guid.NewGuid();
//            String dlDir = @"Exceles/";            
//            string rutas = Server.MapPath(dlDir + Giud + ".xls");
//            FileStream fs = new FileStream(rutas, FileMode.Create,
//                                           FileAccess.ReadWrite);
//            StreamWriter w = new StreamWriter(fs);
//            string comillas = char.ConvertFromUtf32(34);
//            StringBuilder html = new StringBuilder();
//            html.Append(@"<!DOCTYPE html PUBLIC" + comillas +
//            "-//W3C//DTD XHTML 1.0 Transitional//EN" + comillas +
//            " " + comillas
//            + "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" + comillas + ">");
//            html.Append(@"<html xmlns=" + comillas
//                         + "http://www.w3.org/1999/xhtml"
//                         + comillas + ">");
//            html.Append(@"<head>");
//            html.Append(@"<meta http-equiv=" + comillas + "Content-Type"
//                         + comillas + "content=" + comillas
//                         + "text/html; charset=utf-8" + comillas + "/>");
//            html.Append(@"<title>Untitled Document</title>");
//            html.Append(@"</head>");
//            html.Append(@"<body>");


//            //Generando encabezados del archivo 
//            //(aquí podemos dar el formato como a una tabla de HTML)
//            html.Append(@"<table WIDTH=730 CELLSPACING=0 CELLPADDING=10 
//                    border=8 BORDERCOLOR=" + comillas + "#333366"
//                        + comillas + " bgcolor=" + comillas + "#FFFFFF"
//                        + comillas + ">");
//            html.Append(@"<tr> <b>");

//            foreach (object item in titulos)
//            {
//                html.Append(@"<th>" + item.ToString() + "</th>");
//            }
//            html.Append(@"</b> </tr>");

//            //Generando datos del archivo
//            for (int i = 0; i < datos.Rows.Count; i++)
//            {
//                html.Append(@"<tr>");
//                for (int j = 0; j < datos.Columns.Count; j++)
//                {
//                    html.Append(@"<td>" +
//                                datos.Rows[i][j].ToString() + "</td>");
//                }
//                html.Append(@"</tr>");
//            }
//            html.Append(@"</body>");
//            html.Append(@"</html>");
//            w.Write(html.ToString());
//            w.Close();

//            btnExportar.Enabled = true;
//            webGlobalMaster master = (webGlobalMaster)this.Master;
//            ((LinkButton)master.FindControl("lnkDescarga")).Text = Resources.resCorpusCFDIEs.lblDescargarConsulta;
//            master.DescargarArchivo(rutas);
          

//            }
//        catch (Exception ex)
//        {

//        }
        
//    }


 





//    private void dataTableAExcel(DataTable tabla)
//    {
//        ScriptManager SM = ScriptManager.GetCurrent(this);
//        SM.RegisterPostBackControl(btnExportar);
//        if (tabla.Rows.Count > 0)
//        {
//            StringBuilder sb = new StringBuilder();
//            StringWriter sw = new StringWriter(sb);
//            HtmlTextWriter htw = new HtmlTextWriter(sw);
//            System.Web.UI.Page pagina = new System.Web.UI.Page();
//            HtmlForm form = new HtmlForm();
//            GridView dg = new GridView();
//            dg.EnableViewState = false;
//            dg.DataSource = tabla;
//            dg.DataBind();
//            pagina.EnableEventValidation = false;
//            pagina.DesignerInitialize();
//            pagina.Controls.Add(form);
//            form.Controls.Add(dg);
//            pagina.RenderControl(htw);
//            Response.Clear();
//            Response.Buffer = true;
//            Response.ContentType = "application/vnd.ms-excel";
//            Response.AddHeader("Content-Disposition", "attachment;filename=" + Guid.NewGuid().ToString() + ".xls");
//            Response.Charset = "UTF-8";
//            Response.ContentEncoding = Encoding.Default;
//            Response.Write(sb.ToString());
//            Response.End();
//        }
//    }

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
        
        DataColumn columna12 = new DataColumn();
        columna12.DataType = System.Type.GetType("System.String");
        columna12.AllowDBNull = true;
        columna12.Caption = "nId_Usuario_Filtro";
        columna12.ColumnName = "nId_Usuario_Filtro";
        columna12.DefaultValue = "0";
        dtNew.Columns.Add(columna12);

        DataRow RenNuevo = dtNew.NewRow();

         RenNuevo["nId_Usuario"] = clsComun.fnUsuarioEnSesion().id_usuario;

          RenNuevo["dFecha_Inicio"] = Convert.ToDateTime(txtFechaIni.Text);
          RenNuevo["dFecha_Fin"] = Convert.ToDateTime(txtFechaFin.Text);
          RenNuevo["nId_Estructura"] = ddlSucursales.SelectedValue;
          RenNuevo["nId_Tipo_Documento"] = ddlDocumentos.SelectedValue;
          RenNuevo["sEstatus"] = ddlEstatus.SelectedValue;
          RenNuevo["sRfc_Receptor"] = txtRFCReceptor.Text;
          RenNuevo["sSerie"] = txtSerie.Text;
          RenNuevo["nFolio_Inicio"] = txtFolioIni.Text;
          RenNuevo["nFolio_Fin"] = txtFolioFin.Text;
          RenNuevo["nUUID"] = txtUUID.Text;
          RenNuevo["nId_Usuario_Filtro"] = ddlUsuarios.SelectedValue;

         dtNew.Rows.Add(RenNuevo);

         return dtNew;

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            return null;
        }
    }

    private void fnPegarAddendaXML(ref XmlDocument pxComprobante, string psIdCfd)
    {
        clsConfiguracionAddenda gADD = new clsConfiguracionAddenda();
        DataTable addenda = new DataTable();
        int idEstructura = 0;
        string AddendaNamespace = string.Empty;

        idEstructura = gADD.fnObtieneEstructuraCFD(Convert.ToInt32(psIdCfd));
        addenda = gADD.fnObtieneAddendaporIdCfd(Convert.ToInt32(psIdCfd), idEstructura);
        if (addenda.Rows.Count > 0)
        {
            XmlDocument xAddenda = new XmlDocument();
            int idModulo = Convert.ToInt32(addenda.Rows[0]["id_modulo"]);
            xAddenda.LoadXml(Convert.ToString(addenda.Rows[0]["addenda"]));
            AddendaNamespace = gADD.fnObtieneNameSpace(idModulo);

            if (AddendaNamespace != "")
            {
                string[] nombre = AddendaNamespace.Split('=');
                XmlAttribute xAttribute = pxComprobante.CreateAttribute(nombre[0]);
                xAttribute.InnerText = AddendaNamespace;
                pxComprobante.ChildNodes[1].Attributes.Append(xAttribute);
            }


            XmlNode childElement = pxComprobante.CreateNode(XmlNodeType.Element, "cfdi:Addenda", pxComprobante.DocumentElement.NamespaceURI);
            pxComprobante.ChildNodes[1].AppendChild(childElement);

            childElement.InnerXml = xAddenda.OuterXml;  
        }
    }  
}
