using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Threading;
using System.Globalization;


public partial class Cuenta_webOperacionPublicidad : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
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
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varPermisos);
                    Response.Redirect("~/Default.aspx");
                }

            }

            if (!IsPostBack)
            {
                fnCargardgvPublicidad();
                fnDesHabilitar();
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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

    public void fnCargardgvPublicidad()
    {
        DataTable dtPublic = new DataTable();

        dtPublic = clsComun.fnObtenerPublicidad();
        if (dtPublic.Rows.Count > 0)
        {
            dgvPublicidad.DataSource = dtPublic;
            dgvPublicidad.DataBind();
            int c = 0;
            foreach(GridViewRow renglon in dgvPublicidad.Rows)
            {
                c = renglon.DataItemIndex;
                CheckBox cbCan;
                cbCan =(CheckBox)renglon.FindControl("cbSeleccion");
                cbCan.Checked = Convert.ToBoolean(dtPublic.Rows[c]["Seleccion"].ToString());
            }
        }
    }

    protected void btnNCancelar_Click(object sender, EventArgs e)
    {

        dgvPublicidad.Enabled = true;
        btnNuevo.Enabled = true;
        dgvPublicidad.SelectedIndex= -1;
        hdnSel.Value = string.Empty;
        hdnSelVal.Value = string.Empty;
        txtTitulo.Text = string.Empty;
        txtDescrib.Text = string.Empty;
        ckbSeleccion.Checked = false;
        fnDesHabilitar();
    }

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        dgvPublicidad.Enabled = false;
        btnBorrar.Enabled = false;
        fnHabilitarDatos();
        btnEditar.Enabled = false;
    }

    protected void btnGuardarActualizar_Click(object sender, EventArgs e)
    {
        if (!(string.IsNullOrEmpty(txtTitulo.Text)) && !(string.IsNullOrEmpty(txtDescrib.Text)))
        {
            string sTitulo = txtTitulo.Text;
            string sDescripcion = txtDescrib.Text;
            byte[] bytImg = { };
            int nidPublicidad = 0;

            if (!(string.IsNullOrEmpty(hdnSelVal.Value)))
            {

                if (!string.IsNullOrEmpty(fupImgPublicidad.FileName))
                {

                    if (Path.GetExtension(fupImgPublicidad.FileName).ToLower() != ".gif")
                    {
                        //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarLogo);
                        lblAviso.Text = Resources.resCorpusCFDIEs.varLogoGif;
                        mpeAvisos.Show();
                        return;
                    }

                    int psMaximo = Convert.ToInt32(clsComun.ObtenerParamentro("MaxFileLogo"));
                    System.Web.HttpPostedFile mifichero = fupImgPublicidad.PostedFile;
                    double dTamañoArchivo = mifichero.ContentLength / 1024;
                    System.Drawing.Image dImg = System.Drawing.Image.FromStream(fupImgPublicidad.FileContent);
                    if (dTamañoArchivo > psMaximo)
                    {
                        //El tamaño máximo del logo es de 1MB
                        //clsComun.fnMostrarMensaje(this, string.Format(Resources.resCorpusCFDIEs.VarErrorLogo));
                        lblAviso.Text = Resources.resCorpusCFDIEs.VarErrorLogo;
                        mpeAvisos.Show();
                        return;
                    }

                    bytImg = fupImgPublicidad.FileBytes;
                }

                nidPublicidad = Convert.ToInt32(hdnSelVal.Value);

            }
            else
            {

                if (string.IsNullOrEmpty(fupImgPublicidad.FileName))
                {
                    lblAviso.Text = Resources.resCorpusCFDIEs.lblSelecImagen;
                    mpeAvisos.Show();
                    return;
                }

                if (Path.GetExtension(fupImgPublicidad.FileName).ToLower() != ".gif")
                {
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarLogo);
                    lblAviso.Text = Resources.resCorpusCFDIEs.varLogoGif;
                    mpeAvisos.Show();
                    return;
                }

                int psMaximo = Convert.ToInt32(clsComun.ObtenerParamentro("MaxFileLogo"));
                System.Web.HttpPostedFile mifichero = fupImgPublicidad.PostedFile;
                double dTamañoArchivo = mifichero.ContentLength / 1024;
                System.Drawing.Image dImg = System.Drawing.Image.FromStream(fupImgPublicidad.FileContent);
                if (dTamañoArchivo > psMaximo)
                {
                    //El tamaño máximo del logo es de 1MB
                    //clsComun.fnMostrarMensaje(this, string.Format(Resources.resCorpusCFDIEs.VarErrorLogo));
                    lblAviso.Text = Resources.resCorpusCFDIEs.VarErrorLogo;
                    mpeAvisos.Show();
                    return;
                }

                bytImg = fupImgPublicidad.FileBytes;
            }

            bool bSeleccion = ckbSeleccion.Checked;
            if (bSeleccion)//si condicion ponemos false en el campo de seleccion en todos los registros
            {               //y actualizamos la seleccion en el id especificado
                clsComun.fnActualziaTabla();
            }

            int nDat = clsComun.fnAgregarPublicidad(nidPublicidad, sTitulo, sDescripcion, bytImg, bSeleccion);

            if (nDat > 0)
            {
                lblAviso.Text = Resources.resCorpusCFDIEs.varAlta;
                mpeAvisos.Show();
            }
            else
            {
                hdnSelVal.Value = string.Empty;
                lblAviso.Text = Resources.resCorpusCFDIEs.varCambio;
                mpeAvisos.Show();
            }

            fnCargardgvPublicidad();
            dgvPublicidad.SelectedIndex = -1;

            txtTitulo.Text = string.Empty;
            txtDescrib.Text = string.Empty;
            ckbSeleccion.Checked = false;
            fnDesHabilitar();
            dgvPublicidad.Enabled = true;

        }
    }

    protected void btnBorrar_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hdnSelVal.Value))
        {
            int nEstructura = Convert.ToInt32(hdnSelVal.Value);
            DataTable dtPublic = new DataTable();

            dtPublic = clsComun.fnObtenerPublicidad();

            foreach (GridViewRow gdrow in dgvPublicidad.Rows)
            {
                CheckBox CbCan;
                int nfila = Convert.ToInt32(dgvPublicidad.SelectedRow.RowIndex);

                if (nfila == gdrow.RowIndex)
                {
                    CbCan = (CheckBox)(gdrow.FindControl("cbSeleccion"));

                    if (CbCan.Checked)
                    {
                        lblAviso.Text = "La publicidad esta seleccionada para ser mostrada no se puede eliminar ";
                        mpeAvisos.Show();
                        return;
                    }
                    else 
                    {
                        clsComun.fnBajaPublicidad(Convert.ToInt32(hdnSelVal.Value));
                        fnCargardgvPublicidad();
                        lblAviso.Text = Resources.resCorpusCFDIEs.varBaja;
                        mpeAvisos.Show();
                    }
                }
            }
             dgvPublicidad.SelectedIndex = -1;
        }
        else 
        {
            lblAviso.Text = Resources.resCorpusCFDIEs.varPublicidadSeleccionar;
            mpeAvisos.Show();
        }
    }

    protected void btnEditar_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hdnSelVal.Value))
        {
            DataTable dtDatos = new DataTable();
            int idpublicidad = Convert.ToInt32(hdnSelVal.Value);
            
            dtDatos = clsComun.fnObtenerPublicidadSelec(idpublicidad);

            txtTitulo.Text = dtDatos.Rows[0]["Titulo"].ToString();
            txtDescrib.Text = dtDatos.Rows[0]["Descripcion"].ToString();
            ckbSeleccion.Checked =Convert.ToBoolean(dtDatos.Rows[0]["Seleccion"].ToString());

            btnBorrar.Enabled = false;
            btnNuevo.Enabled = false;
            btnGuardarActualizar.Enabled = true;
            txtTitulo.Enabled = true;
            txtDescrib.Enabled = true;
            fupImgPublicidad.Enabled = true;
            dgvPublicidad.Enabled = false;
            ckbSeleccion.Enabled = true;
        }
        else
        {
            lblAviso.Text = Resources.resCorpusCFDIEs.varPublicidadSeleccionar;
            mpeAvisos.Show();
        }
    }

    public void fnDesHabilitar()
    {
        ckbSeleccion.Enabled = false;
        btnEditar.Enabled = false;
        btnGuardarActualizar.Enabled = false;
        txtTitulo.Enabled = false;
        txtDescrib.Enabled = false;
        fupImgPublicidad.Enabled = false;
    }

    public void fnHabilitarDatos()
    {
        ckbSeleccion.Enabled = true;
        btnEditar.Enabled = true;
        btnGuardarActualizar.Enabled = true;
        txtTitulo.Enabled = true;
        txtDescrib.Enabled = true;
        fupImgPublicidad.Enabled = true;
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

    protected void dgvPublicidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdnSelVal.Value = Convert.ToString(dgvPublicidad.SelectedValue);
        btnEditar.Enabled = true;
        btnBorrar.Enabled = true;
    }

    protected void dgvPublicidad_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvPublicidad.PageIndex = e.NewPageIndex;
        fnCargardgvPublicidad();
    }
}