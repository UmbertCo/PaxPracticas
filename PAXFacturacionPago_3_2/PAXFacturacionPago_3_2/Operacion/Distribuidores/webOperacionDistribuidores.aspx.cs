using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using System.Globalization;

public partial class Operacion_Distribuidores_webOperacionDistribuidores : System.Web.UI.Page
{
    private clsOperacionDistribuidores gOp;

    protected void Page_Load(object sender, EventArgs e)
    {
           clsComun.fnPonerTitulo(this);

        //Cargamos los datos del usuario a pantalla
           if (!IsPostBack)
           {
               try
               {
                   fnObtieneDistribuidoresTodos();
               }
               catch (Exception ex)
               {
                   clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
               }
           }
    }
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            int Modifica = Convert.ToInt32(ViewState["modificar"]);
            if (Modifica == 0)
            {
                if (!string.IsNullOrEmpty(txtFechaIniRec.Text) &&
                !string.IsNullOrEmpty(txtFechaFinRec.Text))
                {
                    gOp = new clsOperacionDistribuidores();
                    int pidUsuario = Convert.ToInt32(ViewState["idUsuario"]);
                    DateTime FechaIni = Convert.ToDateTime(txtFechaIniRec.Text);
                    DateTime FechaFin = Convert.ToDateTime(txtFechaFinRec.Text);
                    int RetVal = 0;
                    RetVal = gOp.fnInsertaDistribuidor(pidUsuario, txtConsecutivo.Text, cbCertificado.Checked, FechaIni, FechaFin, "A");
                    if (RetVal != 0)
                    {
                       // gOp.fnInsertarModulosDistribuidor(pidUsuario);
                        fnObtieneDistribuidoresTodos();
                        txtConsecutivo.Text = string.Empty;
                        cbCertificado.Checked = false;
                        txtFechaIniRec.Text = string.Empty;
                        txtFechaFinRec.Text = string.Empty;
                        ViewState["idUsuario"] = null;
                        ViewState["idDistribuidor"] = null;
                        txtCorreo.Text = string.Empty;
                        ViewState["modificar"] = 0;
                        txtUsuario.Enabled = true;
                        txtCorreo.Enabled = true;
                        txtFechaIniRec.Enabled = true;
                        txtConsecutivo.Enabled = true;
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
                    }
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblRecuperaDatos);
                    return;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(txtFechaFinRec.Text))
                {
                    gOp = new clsOperacionDistribuidores();
                    int idContribuyente = Convert.ToInt32(ViewState["idDistribuidor"]);
                    DateTime FechaFin = Convert.ToDateTime(txtFechaFinRec.Text);
                    gOp.fnActualizaDistribuidor(idContribuyente, cbCertificado.Checked, FechaFin);
                    fnObtieneDistribuidoresTodos();
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblRecuperaDatos);
                    return;
                }
            }
            gdvDistribuidores.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    
    }
    protected void btnCancelarRfc_Click(object sender, EventArgs e)
    {
        txtConsecutivo.Text = string.Empty;
        cbCertificado.Checked = false;
        txtFechaIniRec.Text = string.Empty;
        txtFechaFinRec.Text = string.Empty;
        txtUsuario.Text = string.Empty;
        ViewState["idUsuario"] = null;
        ViewState["idDistribuidor"] = null;
        txtCorreo.Text = string.Empty;
        ViewState["modificar"] = 0;
        txtUsuario.Enabled = true;
        txtCorreo.Enabled = true;
        txtFechaIniRec.Enabled = true;
        txtConsecutivo.Enabled = true;
        gdvDistribuidores.SelectedIndex = -1;
        fnObtieneDistribuidoresTodos();
    }
    protected void txtCorreo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtUsuario.Text != "" || txtUsuario.Text != String.Empty)
            {
                fnCargaDistribuidoresporUsuario(txtUsuario.Text, txtCorreo.Text);
            }

        }
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void txtUsuario_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtCorreo.Text != "" || txtCorreo.Text != String.Empty)
            {
                fnCargaDistribuidoresporUsuario(txtUsuario.Text, txtCorreo.Text);
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    public void fnCargaDistribuidoresporUsuario(string sClaveUsuario, string sEmail)
    {
        gOp = new clsOperacionDistribuidores();
        int idUsuario = 0;
        idUsuario = gOp.fnObtieneidUsuarioDistribuidor(sClaveUsuario, sEmail);
        if (idUsuario == 0)
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblErrorDistribuidor);
            return;
        }
        else
        {
            DataTable dTDists = new DataTable();
            dTDists = gOp.fnObtieneDistribuidoresporidUsuario(idUsuario);
            if (dTDists.Rows.Count == 0)
            {
                DataTable dtCliDist = new DataTable();
                dtCliDist = gOp.fnObtieneClientedeDistribuidor(idUsuario);
                if (dtCliDist.Rows.Count == 0)
                {
                    ViewState["idUsuario"] = idUsuario;
                    btnGuardar.Enabled = true;
                    string usuario = txtUsuario.Text.Substring(0, 3).ToUpper();
                    txtConsecutivo.Text = "PAX" + usuario + idUsuario;
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblclientepertdist);
                }
            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblDistribuidorExistente);
            }
        }

    }
    protected void gdvDistribuidores_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            gOp = new clsOperacionDistribuidores();
            int idDistribuidor = Convert.ToInt32(e.Keys["id_distribuidor"].ToString());
            GridViewRow gvrFila = (GridViewRow)gdvDistribuidores.SelectedRow;             
            gOp.fnEliminaDistribuidor(idDistribuidor);
            fnObtieneDistribuidoresTodos();
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varBaja);            
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void gdvDistribuidores_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void gdvDistribuidores_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gvrFila = (GridViewRow)gdvDistribuidores.SelectedRow;
            txtConsecutivo.Text = ((Label)gvrFila.FindControl("lblnumerodist")).Text;
            CheckBox cbSeleccion = new CheckBox();
            cbSeleccion = (CheckBox)gvrFila.FindControl("cbCertifica");
            cbCertificado.Checked = cbSeleccion.Checked;
            txtFechaIniRec.Text = ((Label)gvrFila.FindControl("lblFechainicio")).Text;
            txtFechaFinRec.Text = ((Label)gvrFila.FindControl("lblFechafin")).Text;
            ViewState["idUsuario"] = ((Label)gvrFila.FindControl("lblidusuario")).Text;
            ViewState["idDistribuidor"] = ((Label)gvrFila.FindControl("lbliddistribuidor")).Text;
            txtCorreo.Text = ((Label)gvrFila.FindControl("lblcorreo")).Text;
            txtUsuario.Text = ((Label)gvrFila.FindControl("lblcveusuario")).Text;
            txtUsuario.Enabled = false;
            txtCorreo.Enabled = false;
            txtFechaIniRec.Enabled = false;
            txtConsecutivo.Enabled = false;
            ViewState["modificar"] = 1;
            btnGuardar.Enabled = true;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    public void fnObtieneDistribuidoresTodos()
    {
        try
        {
            gOp = new clsOperacionDistribuidores();
            DataTable dtDistAll = new DataTable();
            dtDistAll = gOp.fnObtieneDistribuidoresAll();
            gdvDistribuidores.DataSource = dtDistAll;
            gdvDistribuidores.DataBind();
        }
        catch(Exception ex)
        {
            gdvDistribuidores.DataSource = null;
            gdvDistribuidores.DataBind();
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }

    }


    protected void btnNuevoDist_Click(object sender, EventArgs e)
    {
        txtConsecutivo.Text = string.Empty;
        txtUsuario.Text = string.Empty;
        cbCertificado.Checked = false;
        txtFechaIniRec.Text = string.Empty;
        txtFechaFinRec.Text = string.Empty;
        ViewState["idUsuario"] = null;
        ViewState["idDistribuidor"] = null;
        txtCorreo.Text = string.Empty;
        ViewState["modificar"] = 0;
        txtUsuario.Enabled = true;
        txtCorreo.Enabled = true;
        txtFechaIniRec.Enabled = true;
        txtConsecutivo.Enabled = true;
        gdvDistribuidores.SelectedIndex = -1;
        fnObtieneDistribuidoresTodos();
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
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
            }
        }
        else
        {
            string language = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
        }
    }
}