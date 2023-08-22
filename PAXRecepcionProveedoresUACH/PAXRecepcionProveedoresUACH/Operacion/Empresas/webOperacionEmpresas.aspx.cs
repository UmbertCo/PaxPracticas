using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Threading;
using System.Globalization;

public partial class Operacion_Empresas_webOperacionEmpresas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeCulture();
            clsInicioSesionUsuario sesUsuario = clsComun.fnUsuarioEnSesion();
            clsOperacionUsuarios oOpUsuarios = new clsOperacionUsuarios();
            //SqlDataReader sdrInfo = gOp.fnObtenerDatosUsuario();
            int nId_Usuario = sesUsuario.id_usuario;
            if (nId_Usuario > 0)
            {
                DataTable tblModulosPerfil = oOpUsuarios.fnSeleccionaModulosHijo(sesUsuario.Id_perfil, true);
                string[] urlActual = Request.Url.AbsolutePath.Split('/');
                int encontrado = tblModulosPerfil.AsEnumerable().Where(t => t.Field<string>("modulo").Contains(urlActual[urlActual.Length - 1])).Count();
                if (encontrado < 1)
                    Response.Redirect("~/Default.aspx");


            }
            fnObtenerEmpresas();
            btnNCancelar.Enabled = false;
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
            ddlEmpresas.Items.Add(new ListItem("<Registre una empresa>", "0"));
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
            //clsComun.fnMostrarMensaje(this, "Faltan datos");
            return;
        }

        if (!clsComun.fnValidaExpresion(txtRfc.Text, "[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
        {
            //clsComun.fnMostrarMensaje(this, "RFC no válido");
            return;
        }

        byte[] logo = new byte[0];
        if (fupLogo.HasFile)
        {
            if (Path.GetExtension(fupLogo.FileName).ToLower() != ".jpg"
                && Path.GetExtension(fupLogo.FileName).ToLower() != ".png")
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarLogo);
                return;
            }

            int psMaximo = Convert.ToInt32(clsComun.ObtenerParamentro("MaxFileLogo"));
            System.Web.HttpPostedFile mifichero = fupLogo.PostedFile;
            double dTamañoArchivo = mifichero.ContentLength / 1024;
            System.Drawing.Image dImg = System.Drawing.Image.FromStream(fupLogo.FileContent);
            if (dTamañoArchivo > psMaximo)
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
                clsComun.fnMostrarMensaje(this, "Empresa agregada");
            }
            else
            {
                clsComun.fnMostrarMensaje(this, "La empresa no pudo agregarse");
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
            txtNombreEmpresa.Enabled = false;
            txtRfc.Enabled = false;
            fupLogo.Enabled = false;
            ddlEmpresas.Enabled = true;
            fnObtenerEmpresas();
            ViewState.Remove("id_empresa");
            btnGuardar.Enabled = false;
            ddlEmpresas.Enabled = true;
            //btnAgregar.Text = Resources.resCorpusCFDIEs.lblAgregar;
        }


    }

    protected void btnBorrar_Click(object sender, EventArgs e)
    {
        int nIdEmpresa = Convert.ToInt32(ddlEmpresas.SelectedValue);
        if (nIdEmpresa > 0)
        {
            if (new clsOperacionSucursales().fnVerificaEmpresaSucursal(nIdEmpresa))
            {
                clsComun.fnMostrarMensaje(this, "La empresa está relacionada con una o varias sucursales");
                return;
            }

            try
            {
                if (!new clsOperacionSucursales().fnEliminarEmpresa(nIdEmpresa))
                {
                    clsComun.fnMostrarMensaje(this, "La empresa no se pudo eliminar");
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, "La empresa se borró correctamente");
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
                //btnGuardar.Enabled = false;
                //btnAgregar.Text = Resources.resCorpusCFDIEs.lblAgregar;
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
            btnNCancelar.Enabled = true;
            txtNombreEmpresa.Enabled = true;
            txtRfc.Enabled = true;
            fupLogo.Enabled = true;
            btnGuardar.Enabled = true;
           // btnAgregar.Text = Resources.resCorpusCFDIEs.btnGuardar;
        }
    }

    protected void btnNCancelar_Click(object sender, EventArgs e)
    {
        btnEditar.Enabled = true;
        btnBorrar.Enabled = true;
        txtNombreEmpresa.Text = "";
        txtRfc.Text = "";
        ddlEmpresas.Enabled = true;
        txtNombreEmpresa.Enabled = false;
        txtRfc.Enabled = false;
        fupLogo.Enabled = false;
        btnGuardar.Enabled = false;
       // btnAgregar.Text=Resources.resCorpusCFDIEs.lblAgregar;
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
        txtNombreEmpresa.Text = "";
        txtRfc.Text = "";
        txtNombreEmpresa.Enabled = true;
        txtRfc.Enabled = true;
        fupLogo.Enabled = true;
        ddlEmpresas.Enabled = false;
        btnGuardar.Enabled = true;
        btnNCancelar.Enabled = true;
    }
}