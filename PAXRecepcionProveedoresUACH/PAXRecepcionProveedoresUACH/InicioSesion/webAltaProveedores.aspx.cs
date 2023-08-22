using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Xml;
using System.Security.Cryptography.X509Certificates;


public partial class InicioSesion_webAltaProveedores : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtRfc.Attributes.Add("onchange", "return copiarContenido();");
            InitializeCulture();
            fnCargarPaises();
            fnCargarEstados();
            fnCargarMunicipios();
            fnCargarEmpresas();
        }
    }

    protected void TextValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = (args.Value.Length >= 8);
    }

    public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    private int RevisaRfc(string rfc)
    {
        try
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
            string uri = "https://adquisiciones.uach.mx:446/Proveedores/requisiciones/ConsultaProveedor/"+rfc;
            HttpWebRequest request = (HttpWebRequest)
            WebRequest.Create(uri); request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            var responseReader = new StreamReader(responseStream);
            var result = responseReader.ReadToEnd();
            result = "<resultado>" + result + "</input></resultado>";
            XmlDocument xmlresultado = new XmlDocument();
            xmlresultado.LoadXml(result);
            return Convert.ToInt16(xmlresultado.DocumentElement["input"].GetAttribute("value"));
        }
        catch (UriFormatException)
        {
            return 0;
        }
    }

    protected void btnGuardarEmpresa_Click(object sender, EventArgs e)
    {


        txtUsuario.Text = txtRfc.Text;
        if (string.IsNullOrEmpty(txtNombre.Text)
            || string.IsNullOrEmpty(txtContacto.Text)
            || string.IsNullOrEmpty(txtCalle.Text)
            || string.IsNullOrEmpty(txtUsuario.Text))
        {
            //clsComun.fnMostrarMensaje(this, "Faltan datos");
            return;
        }

        //if (RevisaRfc(txtRfc.Text) == 0)
        //{
        //    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varProveedorValido);
        //    return;
        //}

        //if (!clsComun.fnValidaExiste("rfc", "tbl_rfp_Proveedores_cat", txtRfc.Text))
        //{
            
        //}
        //else
        //{
        //    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblRFCExistente);
        //    return;
        //}


        //Verifica el código postal
        if (!string.IsNullOrEmpty(txtCodigoPostal.Text))
        {
            try
            {
                Convert.ToInt32(txtCodigoPostal.Text);
            }
            catch (Exception ex)
            {
                //clsComun.fnMostrarMensaje(this, "El Código postal debe ser numérico");
                txtCodigoPostal.Focus();
                return;
            }
            if (txtCodigoPostal.Text.Length != 5)
            {
                //clsComun.fnMostrarMensaje(this, "El Código postal debe ser de 5 dígitos");
                txtCodigoPostal.Focus();
                return;
            }
        }
        string stxtUsuario = txtUsuario.Text.Trim();
        string sPassword = GeneradorPassword.GetPassword();
        bool bSucursal = false;
        foreach (GridViewRow renglon in gvSucursales.Rows)
        {
            CheckBox CbCan;

            CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

            if (CbCan.Checked)
            {
                bSucursal = true;
                break;
            }
        }

        if (!bSucursal)
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblSelecctionarFacultad);
            return;
        }
        //string sConfirmarPassword = txtConfirmarPassword.Text;
        //Validación de password
        //if (!string.IsNullOrEmpty(txtPassword.Text))
        //{
        //    if (sPassword == sConfirmarPassword)
        //    {
        //        //Validacion del lado del servidor
        //        if (!clsComun.fnValidaExpresion(stxtUsuario, @"(?=^.{8,}$).*$"))
        //        {
        //            clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.txtUsuario);
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valConNewConf);
        //        return;
        //    }
        //}
        //else
        //{
        //    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valConfirmaNueva);
        //    return;
        //}
        int nIdMunicipio = Convert.ToInt32(ddlMunicipio.SelectedValue);
        //Verifica que el nombre de usuario no exista
        if (new clsInicioSesionSolicitudReg().buscarClaveExistente(stxtUsuario) > 0)
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgClaveExistente, Resources.resCorpusCFDIEs.varContribuyente);
            return;
        }
        // Verifica que el nombre del proveedor no exista
        if (new clsOperacionProveedores().fnNombreExiste(txtNombre.Text) > 0)
        {
            clsComun.fnMostrarMensaje(this,  Resources.resCorpusCFDIEs.lblProveedorRegistrado);
            return;
        }
        try
        {
            int nIdPerfil = clsComun.fnObtenerIdPerfil("proveedor");
            int nIdUsuario = 0;
            if (nIdPerfil > 0)
            {
                //Agrega al usuario
                nIdUsuario = new clsOperacionUsuarios().fnRegistroContribuyente(stxtUsuario, txtCorreo.Text, Utilerias.Encriptacion.Base64.EncriptarBase64(sPassword), "1", "C", nIdPerfil, 0, "C");
                if (nIdUsuario > 0)
                {
                    //Si el usuario se agrega, guarda el proveedor.
                    int nIdProveedor = new clsOperacionProveedores().fnGuardarProveedor(txtNombre.Text, txtContacto.Text, nIdMunicipio, txtLocalidad.Text,
                    txtColonia.Text, txtCalle.Text, txtNoExterior.Text, txtNoInterior.Text, txtCodigoPostal.Text, txtCorreo.Text, txtTelefono.Text, nIdUsuario);
                   //,txtRfc.Text,txtCodigo.Text);

                    if (nIdProveedor > 0)
                    {
                        //Guarda la relación de proveedor con sucursales
                        foreach (GridViewRow renglon in gvSucursales.Rows)
                        {
                            CheckBox CbCan;

                            CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                            if (CbCan.Checked)
                            {
                                Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
                                int nIdSucursal = Convert.ToInt32(sIdSucursal.Text);
                                new clsOperacionProveedores().fnProveedorSucursalRel(nIdProveedor, nIdSucursal);
                            }
                        }
                        clsPlantillaCorreo plantilla = new clsPlantillaCorreo();
                        plantilla = new clsPlantillaCorreo(clsPlantillaCorreo.Tipo.AltaProveedor);
                        string strMensaje = plantilla.fnObtenerMensajeAltaProveedor(txtNombre.Text, txtUsuario.Text, sPassword,
                            (plantilla.logoImagen != null && plantilla.logoImagen.Length > 0),
                            (plantilla.firmaImagen != null && plantilla.firmaImagen.Length > 0));
                        //StringBuilder strMensaje = new StringBuilder();
                        //strMensaje.Append("<table>");
                        //strMensaje.Append("<tr><td colspan='2'><b>A nuestro proveedor: " + txtNombre.Text + "</b></td></tr>");
                        //strMensaje.Append("<tr><td colspan='2'>Se ha registrado en nuestro portal.</td></tr>");
                        //strMensaje.Append("<tr><td colspan='2'>Para acompletar el registro, inicie sesión con los siguientes datos.</td></tr>");
                        //strMensaje.Append("<tr><td><b>Usuario:</b></td><td>" + txtUsuario.Text + "</td></tr>");
                        //strMensaje.Append("<tr><td><b>Contraseña temporal:</b></td><td>" + sPassword + "</td></tr>");
                        //strMensaje.Append("</table>");
                        //clsComun.fnMostrarMensaje(this, "Ahora podrá ingresar con sus datos.");
                        //new clsGeneraEMAIL().EnviarCorreo(txtCorreo.Text, "Registro de proveedor.", strMensaje.ToString());
                        new clsGeneraEMAIL().EnviarCorreoPlantilla(txtCorreo.Text, plantilla.asunto, strMensaje);

                        //InicioSesion_webInicioSesionLogin.bAltaProveedor = true;
                        Session["altaProveedor"] = true;
                        Response.Redirect("~/InicioSesion/webInicioSesionLogin.aspx");
                    }
                    else
                    {
                        clsComun.fnMostrarMensaje(this, "Hubo una falla al registrar los datos.");
                        return;
                    }

                }
                else
                {
                    clsComun.fnMostrarMensaje(this, "Hubo una falla al registrar los datos.");
                    return;
                }

            }
            else
            {
                clsComun.fnMostrarMensaje(this, "Hubo una falla al registrar los datos.");
                return;
            }
        }
        catch (Exception ex)
        {
            clsComun.fnMostrarMensaje(this, "Hubo una falla al registrar los datos.");
            return;
        }
        finally
        {

        }
        Response.Redirect("~/InicioSesion/webInicioSesionLogin.aspx");

    }

    protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarMunicipios();
    }

    protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarEstados();
    }

    /// <summary>
    /// Método encargado de llenar el drop de los paises
    /// </summary>
    private void fnCargarPaises()
    {
        ddlPais.DataSource = clsComun.fnLlenarDropPaises();
        ddlPais.DataValueField = "id_pais";
        ddlPais.DataTextField = "nombre";
        ddlPais.DataBind();
    }

    /// <summary>
    /// Método encargado de llenar el drop de los estados
    /// </summary>
    /// <param name="psIdEstado"></param>
    private void fnCargarEstados()
    {
        ddlEstado.DataSource = clsComun.fnLlenarDropEstados(ddlPais.SelectedValue);
        ddlEstado.DataValueField = "id_estado";
        ddlEstado.DataTextField = "nombre";
        ddlEstado.DataBind();
    }

    /// <summary>
    /// Método encargado de llenar el drop de los municipios
    /// </summary>
    /// <param name="psIdPais"></param>
    private void fnCargarMunicipios()
    {
        ddlMunicipio.DataSource = clsComun.fnLlenarDropMunicipios(ddlEstado.SelectedValue);
        ddlMunicipio.DataValueField = "id_municipio";
        ddlMunicipio.DataTextField = "nombre";
        ddlMunicipio.DataBind();
    }

    protected void GrvModulos_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void GrvModulos_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    private void fnCargarEmpresas()
    {
        try
        {
            DataTable dtSucursales = new DataTable();
            clsInicioSesionUsuario datosUsuario = clsComun.fnUsuarioEnSesion();
            dtSucursales = new clsOperacionSucursales().fnObtenerSucursalesEmpresas();
            gvSucursales.DataSource = dtSucursales;
            gvSucursales.DataBind();
            gvSucursales.Visible = true;
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

    protected void gvSucursales_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSucursales.PageIndex = e.NewPageIndex;
        fnCargarEmpresas();

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
    protected void txtRfc_TextChanged(object sender, EventArgs e)
    {
        txtUsuario.Enabled = true;
        txtUsuario.Text = txtRfc.Text;
        txtUsuario.Enabled = false;
    }
}