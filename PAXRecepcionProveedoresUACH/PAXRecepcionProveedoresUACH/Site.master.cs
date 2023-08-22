using System;
using System.Web.UI.WebControls;
using System.Xml.XPath;
using System.Xml;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;

public partial class Site : System.Web.UI.MasterPage
{
    DataTable tablaModulos;
    DataTable dtMenuItems;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                clsInicioSesionSolicitudReg busquedaUsuario = new clsInicioSesionSolicitudReg();
                clsInicioSesionUsuario usuario = clsComun.fnUsuarioEnSesion();
                lblErrorGenerico.Text = string.Empty;
                string userName = string.Empty;
                if (usuario != null)
                {
                    lnkSalir.Text = Resources.resCorpusCFDIEs.mnuSalir;
                    lblNombre.Text = usuario.userName;
                    lnkInicio.Visible = false;
                    userName = usuario.userName;

                    if (usuario.estatus == 'A')
                    {
                        generarMenuGlobal();
                    }
                    else
                    {
                        lblNombre.Visible = false;
                        lblBienvenido.Visible = false;
                        lblDosPuntos.Visible = false;
                        lnkSalir.Text = string.Empty;
                        lblCorchDer.Visible = false;
                        lblCorchIzq.Visible = false;
                    }
                }
                else
                {
                    //lnkInicio.Visible = true;
                    lblNombre.Visible = false;
                    lblBienvenido.Visible = false;
                    lblDosPuntos.Visible = false;
                    lnkSalir.Text = string.Empty;
                    lblCorchDer.Visible = false;
                    lblCorchIzq.Visible = false;
                    //return;



                    tablaModulos = busquedaUsuario.fnRecuperaModulosUsuario(string.IsNullOrWhiteSpace(userName) ? "PAX" : userName);

                    //Recupera los modulos asignados al usuario.
                    if (Session["objModulos"] != null)
                    {
                        dtMenuItems = new DataTable();

                        dtMenuItems.Columns.Add("MenuId");
                        dtMenuItems.Columns.Add("descripcion");
                        dtMenuItems.Columns.Add("Icono");
                        dtMenuItems.Columns.Add("Url");
                        dtMenuItems.Columns.Add("PadreId");
                        dtMenuItems.Columns.Add("Target");

                        DataRow row;
                        string sTarget;

                        foreach (DataRow rows in tablaModulos.Rows)
                        {
                            sTarget = String.Empty;
                            row = dtMenuItems.NewRow();

                            if (Convert.ToBoolean(rows["es_link"]))
                            {
                                sTarget = "_blank";
                            }

                            dtMenuItems.Rows.Add(
                                rows["id_modulo"],
                                fnRevisaResource(rows["nombre_modulo"].ToString()),
                                "",
                                rows["modulo"],
                                rows["id_modulo_padre"],
                                sTarget);
                        }

                        //Manda la lista de modulos para agregarlos en el menu.
                        fnAddMenuFirstItem(dtMenuItems);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
    }

    private void generarMenuGlobal()
    {
        //ibPdf.Attributes.Add("target", "_blank");  
        //imgLogo.ImageUrl = "~/Imagenes/LogoBlanco.jpg"; 
        //Recupera los modulos asignados al usuario.
        if (Session["objModulos"] != null)
        {
            tablaModulos = (DataTable)Session["objModulos"];
            dtMenuItems = new DataTable();

            dtMenuItems.Columns.Add("MenuId");
            dtMenuItems.Columns.Add("descripcion");
            dtMenuItems.Columns.Add("Icono");
            dtMenuItems.Columns.Add("Url");
            dtMenuItems.Columns.Add("PadreId");
            dtMenuItems.Columns.Add("Target");

            DataRow row;
            string sTarget;

            foreach (DataRow rows in tablaModulos.Rows)
            {
                sTarget = String.Empty;
                row = dtMenuItems.NewRow();

                if (Convert.ToBoolean(rows["es_link"]))
                {
                    sTarget = "_blank";
                }

                dtMenuItems.Rows.Add(
                    rows["id_modulo"],
                    fnRevisaResource(rows["nombre_modulo"].ToString()),
                    "",
                    rows["modulo"],
                    rows["id_modulo_padre"],
                    sTarget);
            }

            //Manda la lista de modulos para agregarlos en el menu.
            fnAddMenuFirstItem(dtMenuItems);
        }
    }

    /// <summary>
    /// Funcion encargada de descargar los archivos del servidor.
    /// </summary>
    /// <param name="filename"></param>
    private void fnDescargaArchivo(string filename)
    {
        try
        {
            if (!String.IsNullOrEmpty(filename))
            {
                String dlDir = @"Manuales/";
                String path = Server.MapPath(dlDir + filename).Replace(@"\InicioSesion", "");

                System.IO.FileInfo toDownload = new System.IO.FileInfo(path);

                if (toDownload.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
                    Response.AddHeader("Content-Length", toDownload.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.WriteFile(path);
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }


    /// <summary>
    /// Funcion que revisa los nodos para agregarlos al menu
    /// </summary>
    /// <param name="dtMenuItems"></param>
    private void fnAddMenuFirstItem(DataTable dtMenuItems)
    {
        HtmlGenericControl ul = new HtmlGenericControl("ul");


        foreach (DataRow drMenuItem in dtMenuItems.Rows)
        {

            MenuItem mnuMenuItem = new MenuItem();

            if (drMenuItem["MenuId"].Equals(drMenuItem["PadreId"]))
            {
                mnuMenuItem.Value = drMenuItem["MenuId"].ToString();
                mnuMenuItem.Text = drMenuItem["descripcion"].ToString();
                mnuMenuItem.ImageUrl = drMenuItem["Icono"].ToString();
                mnuMenuItem.NavigateUrl = drMenuItem["Url"].ToString();


                mnuPrincipal.Items.Add(mnuMenuItem);

                AddMenuChildItem(ref mnuMenuItem, dtMenuItems);
            }


        }

    }

    protected void lnkSalir_Click(object sender, EventArgs e)
    {
        clsInicioSesionUsuario usuario = clsComun.fnUsuarioEnSesion();
        if (usuario != null)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();

            Response.Redirect("~/InicioSesion/webInicioSesionLogin.aspx");
        }
        else
        {
            Response.Redirect("~/InicioSesion/webInicioSesionLogin.aspx");

        }
    }

    /// <summary>
    /// Revisa los nodos hijos y los agrega al menu
    /// </summary>
    /// <param name="mnuMenuItem"></param>
    /// <param name="dtMenuItems"></param>
    private void AddMenuChildItem(ref MenuItem mnuMenuItem, DataTable dtMenuItems)
    {

        foreach (DataRow drMenuItem in dtMenuItems.Rows)
        {


            if (drMenuItem["PadreId"].ToString().Equals(mnuMenuItem.Value) &&
               !(drMenuItem["MenuId"].Equals(drMenuItem["PadreId"])))
            {
                MenuItem mnuNewMenuItem = new MenuItem();
                mnuNewMenuItem.Value = drMenuItem["MenuId"].ToString();
                mnuNewMenuItem.Text = drMenuItem["descripcion"].ToString();
                mnuNewMenuItem.ImageUrl = drMenuItem["Icono"].ToString();
                mnuNewMenuItem.NavigateUrl = drMenuItem["Url"].ToString();

                mnuMenuItem.ChildItems.Add(mnuNewMenuItem);

                AddMenuChildItem(ref mnuNewMenuItem, dtMenuItems);


            }

        }

    }

    /// <summary>
    /// Revisa el nombre de cadamenu para asiganrle su resource correspondiente.
    /// </summary>
    /// <param name="sMensaje"></param>
    /// <returns></returns>
    private string fnRevisaResource(string sMensaje)
    {
        string sRetorno = string.Empty;

        switch (sMensaje)
        {
            case "Catalogos":
                sRetorno = Resources.resCorpusCFDIEs.mnuCatalogos;
                break;

            case "Sucursales":
                //sRetorno = Resources.resCorpusCFDIEs.mnuSucursales;
                sRetorno = Resources.resCorpusCFDIEs.mnufacultades;
                break;

            case "Series y Folios":
                sRetorno = Resources.resCorpusCFDIEs.mnuSeriesFolios;
                break;

            case "Documentos e Impuestos":
                sRetorno = Resources.resCorpusCFDIEs.mnuDocImpuestos;
                break;

            case "Clientes":
                sRetorno = Resources.resCorpusCFDIEs.mnuClientes;
                break;

            case "Generacion CFDI":
                sRetorno = Resources.resCorpusCFDIEs.lblCFDI;
                break;

            case "Consultas":
                sRetorno = Resources.resCorpusCFDIEs.mnuConsultas;
                break;

            case "Cuenta":
                sRetorno = Resources.resCorpusCFDIEs.mnuCuenta;
                break;

            case "Ayuda":
                sRetorno = Resources.resCorpusCFDIEs.mnuAyuda;
                break;

            case "Soporte":
                sRetorno = Resources.resCorpusCFDIEs.mnuSoporte;
                break;

            case "Consultas CFDI":
                sRetorno = Resources.resCorpusCFDIEs.mnuConsultasCFDI;
                break;

            case "Datos Fiscales Matriz":
                sRetorno = Resources.resCorpusCFDIEs.lblSubCuenta;
                break;

            case "Certificados":
                sRetorno = Resources.resCorpusCFDIEs.lblSubCuentaCertificados;
                break;

            case "Cambio Contraseña":
                //sRetorno = Resources.resCorpusCFDIEs.lblSubUsrPass;
                sRetorno = Resources.resCorpusCFDIEs.mnuCambioPass;
                break;

            case "Baja Cuenta":
                sRetorno = Resources.resCorpusCFDIEs.mnuBajaCuenta;
                break;

            case "Generacion":
                sRetorno = Resources.resCorpusCFDIEs.mnuGeneracion;
                break;

            case "Pistas":
                sRetorno = Resources.resCorpusCFDIEs.mnuPistas;
                break;

            case "Parametros":
                sRetorno = Resources.resCorpusCFDIEs.mnuParametros;
                break;

            case "Auditorias":
                sRetorno = Resources.resCorpusCFDIEs.mnuPAud;
                break;

            case "Configuracion":
                sRetorno = Resources.resCorpusCFDIEs.mnuPParam;
                break;

            case "Inicio":
                sRetorno = Resources.resCorpusCFDIEs.mnuInicio;
                break;

            case "Alta Usuarios":
                sRetorno = Resources.resCorpusCFDIEs.mnuUsuarios;
                break;

            case "Validación":
                sRetorno = Resources.resCorpusCFDIEs.lblValidacion;
                break;

            case "Consultas de Facturas":
                sRetorno = Resources.resCorpusCFDIEs.mnuConsultasCFDI;
                break;

            case "Validacion":
                sRetorno = Resources.resCorpusCFDIEs.lblValidacion;
                break;

            case "Alta Empresas":
                sRetorno = Resources.resCorpusCFDIEs.lblEmpresas;
                break;

            case "Proveedores":
                sRetorno = Resources.resCorpusCFDIEs.lblProveedores;
                break;

            case "Plantillas":
                sRetorno = Resources.resCorpusCFDIEs.varPlantillas;
                break;

            default:
                sRetorno = sMensaje;
                break;
        }

        return sRetorno;
    }
    protected void HeadLoginView_ViewChanged(object sender, EventArgs e)
    {

    }


    #region Idioma

    protected void lnkEnglish_Click(object sender, EventArgs e)
    {
        Session["Culture"] = "en-Us";
        Response.Redirect(Request.RawUrl);
    }
    protected void lnkEspañol_Click(object sender, EventArgs e)
    {
        Session["Culture"] = "es-Mx";
        Response.Redirect(Request.RawUrl);
    }
    protected void lnkEnglish_PreRender(object sender, EventArgs e)
    {
        if ((!IsPostBack) && (Session["Culture"] != null))
        {
            string lang = Session["Culture"].ToString();
        }
    }
    protected void lnkEspañol_PreRender(object sender, EventArgs e)
    {
        if ((!IsPostBack) && (Session["Culture"] != null))
        {
            string lang = Session["Culture"].ToString();
        }
    }

    #endregion

}
