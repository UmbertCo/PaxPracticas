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

public partial class Operacion_Empresas_webOperacionEmpresas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                clsInicioSesionUsuario sesUsuario = clsComun.fnUsuarioEnSesion();
                clsOperacionUsuarios oOpUsuarios = new clsOperacionUsuarios();
                //SqlDataReader sdrInfo = gOp.fnObtenerDatosUsuario();
                btnBorrar.OnClientClick = "return confirm('" + Resources.resCorpusCFDIEs.varEliminarEmpresa + "');";
                int nId_Usuario = sesUsuario.id_usuario;
                if (nId_Usuario > 0)
                {
                    DataTable tblModulosPerfil = oOpUsuarios.fnSeleccionaModulosHijo(sesUsuario.Id_perfil, true);
                    string[] urlActual = Request.Url.AbsolutePath.Split('/');
                    int encontrado = tblModulosPerfil.AsEnumerable().Where(t => t.Field<string>("modulo").Contains(urlActual[urlActual.Length - 1])).Count();
                    if (encontrado < 1)
                        Response.Redirect("~/Default.aspx", false);


                }
                fnObtenerEmpresas();
                btnNCancelar.Enabled = false;
                btnGuardar.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/webGlobalError.aspx");
        }
    }

    private void fnObtenerEmpresas()
    {
        ddlEmpresas.Items.Clear();
        clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
        DataTable dtEmpresas = new clsOperacionSucursales().fnObtenerEmpresasUsuario(usuarioActivo.id_usuario);
        if (dtEmpresas != null && dtEmpresas.Rows.Count > 0)
        {
            ddlEmpresas.DataSource = dtEmpresas;
            ddlEmpresas.DataTextField = "razon_social";
            ddlEmpresas.DataValueField = "id_empresa";
            ddlEmpresas.DataBind();
            ddlEmpresas.Enabled = true;
            btnEditar.Enabled = true;
            btnBorrar.Enabled = true;
            ViewState["empresas"] = dtEmpresas;
        }
        else
        {
            ddlEmpresas.Items.Add(new ListItem("<" + Resources.resCorpusCFDIEs.varRegistreEmpresa + ">", "0"));
            ddlEmpresas.DataBind();
            ddlEmpresas.Enabled = false;
            btnEditar.Enabled = false;
            btnBorrar.Enabled = false;
        }

    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtNombreEmpresa.Text)
            || string.IsNullOrEmpty(txtRfc.Text)
            )
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }

        if (!clsComun.fnValidaExpresion(txtRfc.Text, "[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.regxRFC);
            return;
        }

        byte[] logo = new byte[0];
        if (fupLogo.HasFile)
        {
            if (Path.GetExtension(fupLogo.FileName).ToLower() != ".jpg")
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarLogo);
                return;
            }

            int psMaximo = Convert.ToInt32(clsComun.ObtenerParamentro("MaxFileLogo"));
            System.Web.HttpPostedFile mifichero = fupLogo.PostedFile;
            double dTamanoArchivo = (mifichero.ContentLength / 1024);
            System.Drawing.Image dImg = System.Drawing.Image.FromStream(fupLogo.FileContent);
            if (dTamanoArchivo > psMaximo)
            {
                //El tamaño máximo del logo es de 1MB
                clsComun.fnMostrarMensaje(this, string.Format(Resources.resCorpusCFDIEs.VarErrorLogo));
                return;
            }
            logo = fupLogo.FileBytes;
        }
        int nIdEmpresa = 0;
        if (ViewState["id_empresa"] != null)
        {
            nIdEmpresa = Convert.ToInt32(ViewState["id_empresa"]);
        }
        try
        {
            clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
            int res = new clsOperacionSucursales().fnGuardarEmpresa(usuarioActivo.id_usuario, nIdEmpresa, txtNombreEmpresa.Text, txtRfc.Text, logo);

            if (res > 0)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varEmpresaAgregada);
            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varEmpresaError);
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            btnEditar.Enabled = true;
            btnBorrar.Enabled = true;
            btnNCancelar.Enabled = false;
            txtNombreEmpresa.Text = "";
            txtRfc.Text = "";
            ddlEmpresas.Enabled = true;
            fnObtenerEmpresas();
            ViewState.Remove("id_empresa");
            btnGuardar.Enabled = false;
            txtNombreEmpresa.Enabled = false;
            txtRfc.Enabled = false;
            fupLogo.Enabled = false;
            btnNuevo.Enabled = true;
        }


    }

    protected void btnBorrar_Click(object sender, EventArgs e)
    {
        int nIdEmpresa = Convert.ToInt32(ddlEmpresas.SelectedValue);
        if (nIdEmpresa > 0)
        {
            if (new clsOperacionSucursales().fnVerificaEmpresaSucursal(nIdEmpresa))
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varEmpresaRelacionada);
                return;
            }

            try
            {
                if (!new clsOperacionSucursales().fnEliminarEmpresa(nIdEmpresa))
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varEmpresaNoBorrada);
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varEmpresaBorrada);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                btnEditar.Enabled = true;
                btnBorrar.Enabled = true;
                btnNCancelar.Enabled = false;
                txtNombreEmpresa.Text = "";
                txtRfc.Text = "";
                fnObtenerEmpresas();

            }
        }

    }

    protected void btnEditar_Click(object sender, EventArgs e)
    {
        int nIdEmpresa = Convert.ToInt32(ddlEmpresas.SelectedValue);
        if (nIdEmpresa > 0)
        {
            DataTable dtEmpresas = (DataTable)ViewState["empresas"];
            DataRow drEmpresa = dtEmpresas.Select("id_empresa=" + nIdEmpresa)[0];
            txtNombreEmpresa.Text = drEmpresa["razon_social"].ToString();
            txtRfc.Text = drEmpresa["rfc"].ToString();
            ViewState["id_empresa"] = nIdEmpresa;
            ddlEmpresas.Enabled = false;
            btnBorrar.Enabled = false;
            btnEditar.Enabled = false;
            btnGuardar.Enabled = true;
            btnNCancelar.Enabled = true;
            btnNuevo.Enabled = false;
            txtNombreEmpresa.Enabled = true;
            txtRfc.Enabled = true;
            fupLogo.Enabled = true;
        }
    }

    protected void btnNCancelar_Click(object sender, EventArgs e)
    {
        btnEditar.Enabled = true;
        btnBorrar.Enabled = true;
        btnGuardar.Enabled = false;
        txtNombreEmpresa.Text = "";
        txtRfc.Text = "";
        ddlEmpresas.Enabled = true;
        txtNombreEmpresa.Enabled = false;
        txtRfc.Enabled = false;
        fupLogo.Enabled = false;
        btnNCancelar.Enabled = false;
        btnNuevo.Enabled = true;
    }

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

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        btnEditar.Enabled = false;
        btnBorrar.Enabled = false;
        btnGuardar.Enabled = true;
        txtNombreEmpresa.Text = "";
        txtRfc.Text = "";
        ddlEmpresas.Enabled = false;
        btnNCancelar.Enabled = true;
        txtNombreEmpresa.Enabled = true;
        txtRfc.Enabled = true;
        fupLogo.Enabled = true;
        btnNuevo.Enabled = false;
    }
}