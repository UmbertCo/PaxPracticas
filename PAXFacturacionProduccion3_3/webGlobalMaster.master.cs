using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class webGlobalMaster : System.Web.UI.MasterPage
{
    DataTable tablaModulos;
    DataTable dtMenuItems;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Header.DataBind();
        clsInicioSesionUsuario isSesion = (clsInicioSesionUsuario)Session["objUsuario"];
        if (isSesion != null)
        {
            if (isSesion.sEstatus.Equals('A'))
            {
                if ((Request.Path.ToLower().Contains("webiniciosesionlogin.aspx")
                || Request.Path.ToLower().Contains("webiniciosesioncorrecto.aspx")
                || Request.Path.ToLower().Contains("webiniciosesionreactivar.aspx")
                || Request.Path.ToLower().Contains("webiniciosesionrecupera.aspx")
                || Request.Path.ToLower().Contains("webiniciosesionregdatos.aspx")
                || Request.Path.ToLower().Contains("webiniciosesionregistro.aspx")
                || Request.Path.ToLower().Contains("webiniciosesioncambiarpwd.aspx")))
                {
                    Session.Add("UsuarioActivo", isSesion.sUserName);
                    Response.Redirect("~/Default.aspx");
                    return;
                }
            }
        }

        clsInicioSesionSolicitudReg busquedaUsuario = new clsInicioSesionSolicitudReg();
        lblErrorGenerico.Text = string.Empty;

        try
        {
            if (!IsPostBack)
            {
                tablaModulos = new DataTable();
                tablaModulos.Columns.Add("id_modulo");
                tablaModulos.Columns.Add("nombre_modulo");
                tablaModulos.Columns.Add("modulo");
                tablaModulos.Columns.Add("id_modulo_padre");
                tablaModulos.Columns.Add("es_link");
                tablaModulos.Columns.Add("sTarget");

                tablaModulos.Rows.Add("25", "Inicio", "~/InicioSesion/webInicioSesionLogin.aspx", "25", "false", "_blank");


                //Recupera los modulos asignados al usuario.
                if (tablaModulos.Rows.Count > 0)
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
        catch (Exception)
        {
            //Nothing
        }
    }
    protected void lnkMSoporte_Click(object sender, EventArgs e)
    {
        string filename = "DMS.V1 Manual de Soporte.pdf";
        fnDescargaArchivo(filename);
    }
    protected void lnkMUsuario_Click(object sender, EventArgs e)
    {
        string filename = "DMS.A5.PT1.V1 Manual de Usuario.pdf";
        fnDescargaArchivo(filename);
    }
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

    /// <summary>
    ///	Author		: César Negrete Villa
    /// Date  		: 12/07/2016  10:23
    /// Description : Revisa los nodos hijos y los agrega al menu
    /// </summary>
    /// <param name="mnuMenuItem">Menu</param>
    /// <param name="dtMenuItems">DataTable con todos los elementos del Menu</param>
    private void fnAddMenuChildItem(string psIdMenu, DataTable dtMenuItems, ref HtmlGenericControl HtmlDiv)
    {
        System.Web.UI.HtmlControls.HtmlGenericControl createDivUl = new System.Web.UI.HtmlControls.HtmlGenericControl("ul");
        createDivUl.ID = psIdMenu;
        createDivUl.Attributes.Add("Class", "dropdown-menu");
        string ListaMenu = string.Empty;
        string sUrlPag = string.Empty;
        foreach (DataRow drMenuItem in dtMenuItems.Rows)
        {

            if (drMenuItem["PadreId"].ToString().Equals(psIdMenu) &&
               !(drMenuItem["MenuId"].Equals(drMenuItem["PadreId"])))
            {
                sUrlPag = drMenuItem["Url"].ToString();

                ListaMenu += "<li><a style=\"text-decoration:none\" href=\"" + VirtualPathUtility.ToAbsolute(sUrlPag) + "\">" + drMenuItem["descripcion"].ToString() + "</a></li>";

                fnAddMenuChildItem(drMenuItem["MenuId"].ToString(), dtMenuItems, ref createDivUl);
            }
        }
        createDivUl.InnerHtml = ListaMenu;
        HtmlDiv.Controls.Add(createDivUl);
    }

    /// <summary>
    ///	Author		: César Negrete Villa
    /// Date  		: 12/07/2016  10:22
    /// Description : Agrega el Elemento Padre del Menu
    /// </summary>
    /// <param name="dtMenuItems">DataTable con todos los elementos del Menu</param>
    private void fnAddMenuFirstItem(DataTable dtMenuItems)
    {

        foreach (DataRow drMenuItem in dtMenuItems.Rows)
        {
            if (drMenuItem["MenuId"].Equals(drMenuItem["PadreId"]))
            {
                System.Web.UI.HtmlControls.HtmlGenericControl createDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("li");
                createDiv.Controls.Clear();
                divMenu.Controls.Clear();
                createDiv.ID = drMenuItem["descripcion"].ToString();

                if (drMenuItem["descripcion"].ToString() != "Inicio" && drMenuItem["descripcion"].ToString() != "Start")
                {
                    string sURL = string.Empty;
                    createDiv.InnerHtml = string.Format("<a style=\"text-decoration:none\" href=\"{0}\" class=\"dropdown-toggle\" data-toggle=\"dropdown\" role=\"button\" aria-haspopup=\"true\" aria-expanded=\"false\" >" + drMenuItem["descripcion"].ToString() + "<span class=\"caret\"></span></a>", drMenuItem["Url"].ToString());

                    if (dtMenuItems.Select("PadreId = " + drMenuItem["MenuId"].ToString()).Length > 1)
                    {
                        fnAddMenuChildItem(drMenuItem["MenuId"].ToString(), dtMenuItems, ref createDiv);
                    }

                    createDiv.Attributes.Add("Class", "dropdown");
                    divMenu.Controls.Add(createDiv);
                }
                else
                {
                    createDiv.InnerHtml = string.Format("<a style=\"text-decoration:none\" href=\"" + VirtualPathUtility.ToAbsolute(drMenuItem["Url"].ToString()) + "\" class=\"dropdown-toggle disabled\" data-toggle=\"dropdown\" role=\"button\" aria-haspopup=\"true\" aria-expanded=\"true\" >" + drMenuItem["descripcion"].ToString() + "</a>", drMenuItem["Url"].ToString());
                    createDiv.Attributes.Add("Class", "dropdown");
                    divMenu.Controls.Add(createDiv);
                }
            }
        }
    }

    /// <summary>
    ///	Author		: César Negrete Villa
    /// Date  		: 12/07/2016  10:46
    /// Description : Agrega elemento del Menu
    /// </summary>
    /// <param name="mmenuItem">Menu</param>
    /// <param name="dtmenuItems">DataTable con todos los elementos del Menu</param>
    public void fnAgregarMenu(ref MenuItem mmenuItem, DataTable dtmenuItems)
    {

        foreach (DataRow drmenuItem in dtmenuItems.Rows)
        {

            if (drmenuItem["IdPadre"].ToString().Equals(mmenuItem.Value) && !(drmenuItem["IdMenu"].Equals(drmenuItem["IdPadre"])))
            {
                MenuItem NuevoMenuItem = new MenuItem();
                NuevoMenuItem.Value = drmenuItem["IdMenu"].ToString();
                NuevoMenuItem.Text = drmenuItem["Descripcion"].ToString();

                mmenuItem.ChildItems.Add(NuevoMenuItem);
                fnAgregarMenu(ref NuevoMenuItem, dtmenuItems);
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
                sRetorno = Resources.resCorpusCFDIEs.mnuSucursales;
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

            case "Cambio de Contraseña":
                sRetorno = Resources.resCorpusCFDIEs.lblSubUsrPass;
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

            default:
                sRetorno = sMensaje;
                break;
        }

        return sRetorno;
    }
    protected void imgDescarga_Click(object sender, ImageClickEventArgs e)
    {
        string filename = "DMS.A5.PT1.V1 Manual de Usuario.pdf";
        fnDescargaArchivo(filename);
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
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnDescargaArchivo", "webGlobalMaster");
        }
    }
}
