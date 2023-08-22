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
using Cancelacion = ServicioCancelacionCFDI;
using AutenticacionCancelacion = ServicioCancelacionAutenticacionCFDI;

using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using System.ServiceModel;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;

public partial class Consultas_WebConsultasNomina : System.Web.UI.Page
{
    private string SeleccioneUnValor = Resources.resCorpusCFDIEs.varSeleccione;

    #region Variables

    private clsOperacionConsulta gDAL;
    private clsOperacionUsuarios gOu;
    private clsOperacionClientes gOc;
    private clsInicioSesionUsuario datosUsuario;
    private DataTable dtCreditos;
    private clsConfiguracionCreditos cCc;
    private clsOperacionCuenta gOpeCuenta;
    private DataTable DTCompMail;
    private static DataSet creditosT;
    private clsEnvioCorreoDocs cEd;
    private int gnMaxNumRegistros = 50;

    #endregion

    #region Funciones

    #region Consultas

    /// <summary>
    /// Busca empleado por parametros de la ventana modal pnlBusquedaEmpleados
    /// </summary>
    private void fnBuscarEmpleados()
    {
        clsOperacionEmpleados cEmpleados = new clsOperacionEmpleados();
        try
        {
            gdvEmpleados.DataSource = cEmpleados.fnBuscarEmpleados(txtNumeroEmpleadoBusqueda.Text, txtRfcEmpleadoBusqueda.Text,
                                        txtNombreEmpleadoBusqueda.Text, Convert.ToInt32(ddlSucursales.SelectedValue));
            gdvEmpleados.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

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
    /// Realiza la consulta de los Comprobantes de Nomina 
    /// </summary>
    /// <returns>
    /// True si trae mas de 1 pagina 
    /// False si no trae mas de una pagina o la fallo la consulta 
    /// </returns>
    private DataTable fnRealizarConsulta(int pnPagina, int pnCantidadMaximaRegistros)
    {
        DataTable dtComprobantes = new DataTable();
        gDAL = new clsOperacionConsulta();
        try
        {
            dtComprobantes = gDAL.fnObtenerComprobantesNomina(
               txtRFC.Text,
                ddlEstatus.SelectedValue,
                ddlSucursales.SelectedValue,
                ViewState["Id_Empleado"].ToString(),
                ddlPeriodos.SelectedValue,
                Convert.ToDateTime(txtFechaIni.Text),
                Convert.ToDateTime(txtFechaFin.Text),
                pnPagina,
                pnCantidadMaximaRegistros
                );

            ViewState["ExportarExcel"] = dtComprobantes;

            cbSeleccionar.Checked = false;
            clsComun.fnNuevaPistaAuditoria(
                "webConsultaNomina",
                "btnConsultar",
                "Se consultó con los filtros",
                ddlSucursales.SelectedItem.Text,
                ddlEstatus.SelectedItem.Text,
                txtEmpleado.Text,
                txtRFC.Text,
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

    #endregion

    #region Cargar Controles

    /// <summary>
    /// Carga las sucursales disponibles para el usuario
    /// </summary>
    private void fnCargarSucursales()
    {
        try
        {

            datosUsuario = clsComun.fnUsuarioEnSesion();

            DataTable dtAuxiliar = clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario);
            ViewState["dtAuxiliar"] = dtAuxiliar;
            //DataRow drFila = dtAuxiliar.NewRow();
            //drFila["id_estructura"] = 0;// dtAuxiliar.Rows[0]["id_estructura"];
            //drFila["nombre"] = Resources.resCorpusCFDIEs.VarDropTodos;
            //dtAuxiliar.Rows.InsertAt(drFila, 0);

            ddlSucursales.DataSource = dtAuxiliar;//gDAL.fnObtenerSucursales();
            ddlSucursales.DataBind();

            // ddlSucursales.SelectedValue = 


        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            ddlSucursales.DataSource = null;
            ddlSucursales.DataBind();
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
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
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
            dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
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
        datosUsuario = clsComun.fnUsuarioEnSesion();
        dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), string.Empty, 'A', 'S');
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
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
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
            dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
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
    /// Revisa que existan creditos.
    /// </summary>
    private double fnRevisaCreditos()
    {
        double retorno = 0;
        double credit = 0;

        //Revisar los creditos disponibles.
        datosUsuario = clsComun.fnUsuarioEnSesion();
        dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), "Timbrado", 'A', 'N');
        ViewState["dtCreditos"] = dtCreditos;

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
            dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
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
    /// Método que se encarga de cargar los períodos disponibles
    /// </summary>
    private void fnCargarPeriodos()
    {
        clsTimbradoNomina cTimbradoNomina = new clsTimbradoNomina();
        try
        {
            ddlPeriodos.DataSource = cTimbradoNomina.LlenarDropPeriodos();
            ddlPeriodos.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            ddlPeriodos.DataSource = null;
            ddlPeriodos.DataBind();
        }
    }

    #endregion

    #region Eventos

    #region Eventos Interfaz

    #region Eventos Area Consulta
    /// <summary>
    /// Abre la ventana modal de Buscar Empleados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBuscaEmpleados_Click(object sender, EventArgs e)
    {
        fnBuscarEmpleados();
        linkModal_ModalPopupExtender.Show();
    }

    /// <summary>
    /// Boton que reliza la Consulta
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>6
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

        dtComprobantes = fnRealizarConsulta(1, gnMaxNumRegistros);
        gdvComprobantes.DataSource = dtComprobantes;
        gdvComprobantes.DataBind();

        if (dtComprobantes.Rows.Count > 0)
        {
            btnDescargar.Visible = true;
            btnExportar.Visible = true;

            double nRegistros = Convert.ToDouble(dtComprobantes.Rows[0]["registros"]);
            int nNumeroPaginas = Convert.ToInt32(Math.Round((nRegistros / Convert.ToDouble(gnMaxNumRegistros)) + 0.5));

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
            if (CbCan.Checked == true && renglon.Cells[10].Text == "A")
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

    #endregion

    #region Eventos Area Descarga

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

            //sIDS = Utilerias.Encriptacion.Base64.EncriptarBase64(sIDS);
            sIDS = PAXCrypto.CryptoAES.EncriptarAES64(sIDS);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "webDescargaComprobantesNomina",
                                                        String.Format("<script>window.open('{0}?p={1}','150','height=200, width= 355, status=yes, resizable= no, scrollbars= no, toolbar= no,menubar= no');</script>",
                                                                       "webDescargaComprobantesNomina.aspx", sIDS), false);
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

            //sIDS = Utilerias.Encriptacion.Base64.EncriptarBase64(sIDS);            
            sIDS = PAXCrypto.CryptoAES.EncriptarAES64(sIDS);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "webDescargaConsultaNomina.aspx",
                                                        String.Format("<script>window.open('{0}?p={1}','123','height=160, width= 355, status=yes, resizable= no, scrollbars= no, toolbar= no,menubar= no');</script>",
                                                                       "webDescargaConsultaNomina.aspx", sIDS), false);

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    #endregion

    #region Eventos Tabla Consulta

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

    protected void gdvComprobantes_SelectedIndexChanged(object sender, EventArgs e)
    {
        gOu = new clsOperacionUsuarios();
        gOc = new clsOperacionClientes();
        int nid_cfd, nid_Sucursal;
        string sDoc, snombreDoc, sRfc, sRazon_Social;
        DTCompMail = (DataTable)ViewState["ExportarExcel"];
        nid_cfd = Convert.ToInt32(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["id_cfd"].ToString());
        nid_Sucursal = Convert.ToInt32(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["id_Sucursal"].ToString());
        ViewState["id_estructura"] = nid_Sucursal;

        sDoc = Convert.ToString(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["documento"].ToString());
        snombreDoc = Convert.ToString(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["UUID"].ToString());
       
        txtCorreoEmisor.Text = gOu.fnObtenerCorreoReceptor(nid_cfd);



        if (!String.IsNullOrEmpty(DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["Correo"].ToString()))
        {
            txtCorreoCliente.Text = DTCompMail.Rows[gdvComprobantes.SelectedRow.DataItemIndex]["Correo"].ToString();
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

    protected void btnAnterior_Click(object sender, EventArgs e)
    {
        btnAnterior.Visible = false;
        btnSiguiente.Visible = true;

        ddlCantidadPaginas.SelectedValue = (Convert.ToInt32(ddlCantidadPaginas.SelectedValue) - 1).ToString();

        gdvComprobantes.DataSource = fnRealizarConsulta(Convert.ToInt32(ddlCantidadPaginas.SelectedValue), gnMaxNumRegistros);
        gdvComprobantes.DataBind();

        if (Convert.ToInt32(ddlCantidadPaginas.SelectedValue) > 1)
            btnAnterior.Visible = true;
    }

    protected void btnSiguiente_Click(object sender, EventArgs e)
    {
        btnSiguiente.Visible = false;
        btnAnterior.Visible = true;

        ddlCantidadPaginas.SelectedValue = (Convert.ToInt32(ddlCantidadPaginas.SelectedValue) + 1).ToString();

        gdvComprobantes.DataSource = fnRealizarConsulta(Convert.ToInt32(ddlCantidadPaginas.SelectedValue), gnMaxNumRegistros);
        gdvComprobantes.DataBind();

        if (ddlCantidadPaginas.SelectedIndex < (ddlCantidadPaginas.Items.Count - 1))
            btnSiguiente.Visible = true;
    }

    protected void ddlCantidadPaginas_SelectedIndexChanged(object sender, EventArgs e)
     {
         btnAnterior.Visible = false;
         btnSiguiente.Visible = false;

         gdvComprobantes.DataSource = fnRealizarConsulta(Convert.ToInt32(ddlCantidadPaginas.SelectedValue), gnMaxNumRegistros);
         gdvComprobantes.DataBind();

         if (Convert.ToInt32(ddlCantidadPaginas.SelectedValue) > 1)
             btnAnterior.Visible = true;

         if (ddlCantidadPaginas.SelectedIndex < (ddlCantidadPaginas.Items.Count - 1))
             btnSiguiente.Visible = true;
     }

    protected void ddlPeriodos_DataBound(object sender, EventArgs e)
    {
        AgregaOpcionSeleccione(sender, e);
    }

    protected void AgregaOpcionSeleccione(System.Object sender, System.EventArgs e)
    {
        ((DropDownList)sender).Items.Insert(0, new ListItem(SeleccioneUnValor, "0"));
    }

    #endregion

    #region Eventos Panel Empleados

    /// <summary>
    /// Boton que cierra la pantalla modal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancelarModal_Click(object sender, EventArgs e)
    {
        gdvEmpleados.DataSource = null;
        gdvEmpleados.DataBind();
        linkModal_ModalPopupExtender.Hide();
    }

    /// <summary>
    /// Evento de la tabla que selecciona un empleado y cierra la ventana modal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gdvEmpleados_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            if (gdvEmpleados.SelectedIndex < 0)
                return;

            string sId_Empleado = gdvEmpleados.SelectedDataKey.Values["Id_Empleado"].ToString();
          

            ViewState["Id_Empleado"] = sId_Empleado;

            txtEmpleado.Text = gdvEmpleados.SelectedDataKey.Values["nombre"].ToString();

            linkModal_ModalPopupExtender.Hide();

        



        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    protected void gdvEmpleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvEmpleados.PageIndex = e.NewPageIndex;
        fnBuscarEmpleados();
        linkModal_ModalPopupExtender.Show();
    }
 
    #endregion

    #region Eventos Area Envio Correo

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
        datosUsuario = clsComun.fnUsuarioEnSesion();

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
        string plantilla = "Nomina";


        //Verifica si se envia el comprobante en ZIP o no.

        if (rdlArchivo.SelectedIndex == 1)
        {

            bEnvio = cEd.fnPdfEnvioCorreo(plantilla, Convert.ToString(nid_cfd), sDoc,
                              clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + snombreDoc + ".pdf",
                              datosUsuario.id_contribuyente, datosUsuario.id_rfc, datosUsuario.color, sMailTo,
                              "PAXFacturacion", Mensaje, Convert.ToString(ViewState["GuidPathZIPs"]),
                              Convert.ToString(ViewState["GuidPathXMLs"]), snombreDoc, sCC);
        }
        else
        {
            bEnvio = cEd.fnPdfEnvioCorreoSinZIP(plantilla, Convert.ToString(nid_cfd), sDoc,
                              clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + snombreDoc + ".pdf",
                              datosUsuario.id_contribuyente, datosUsuario.id_rfc, datosUsuario.color, sMailTo,
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

    #endregion

    #region Eventos Motivo Cancelacion

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
                        if (CbCan.Checked == true && renglon.Cells[10].Text == "A")
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

                            clsComun.fnNuevaPistaAuditoria("webConsultasCFDI", "CancelarBloqueCfdi", "Llama el web service del SAT." + sRfcEmisor +"|"+ listUUid.Count +"|"+ idCfd.Count +"|"+ datosUsuario.userName);

                            clsCancelacionSAT cancelacion = new clsCancelacionSAT();
                            datosUsuario = clsComun.fnUsuarioEnSesion();
                            string Respuesta = cancelacion.CancelarBloqueCfdi(sRfcEmisor, listUUid, idCfd, datosUsuario);
                            XmlDocument xmldoc = new XmlDocument();
                            xmldoc.PreserveWhitespace = false;
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

    protected void btnCancelarCreditos_Click(object sender, EventArgs e)
    {
        modalCreditos.Hide();
    }

    #endregion

    #endregion

    #region Eventos Pagina

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Page_Error(object sender, EventArgs e)
    {
        Exception objErr = Server.GetLastError().GetBaseException();

        if (!string.IsNullOrEmpty(objErr.Message))
        {
            clsErrorLog.fnNuevaEntrada(objErr, clsErrorLog.TipoErroresLog.Datos);
            Server.ClearError();
            Response.Redirect("~/webGlobalError.aspx");
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                //clsTimbradoFuncionalidad.fnInsertarComprobante(xDocTimbrado.DocumentElement.OuterXml, Convert.ToInt32(ddlTipoDoc.SelectedValue.Split('|')[0]), 'G',
                //                                                       DateTime.Now, Convert.ToInt32(ddlSucursalesFis.SelectedValue), datosUsuario.id_usuario, sSerie, "C", HASHTimbre, HASHEmisor);


                btnAnterior.Visible = false;
                btnSiguiente.Visible = false;
                fnCargarSucursales();
                fnCargarEstatus();
                fnCargarPeriodos();
                datosUsuario = clsComun.fnUsuarioEnSesion();

              
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

                ViewState["Id_Empleado"] = "0";

            }
            catch { }
        }
    }

    #endregion

    #endregion

    #region Utiles
    /// <summary>
    /// Registra el script de confirmación para la cancelación de un comprobante
    /// </summary>
    private void fnRegistrarScript()
    {
        string sScript = "function confirmacion(){ return confirm('" + Resources.resCorpusCFDIEs.varConfirmacionCancelacion + "'); }";
        ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "cancelacion", sScript, true);
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



    #endregion

    #endregion    
}

       
