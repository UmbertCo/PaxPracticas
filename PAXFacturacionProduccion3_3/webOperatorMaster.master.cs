using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class webOperatorMaster : System.Web.UI.MasterPage
{
    clsInicioSesionUsuario cInicioSesionUsuario;
    DataTable dtModulos;
    DataTable dtMenuItems;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Header.DataBind();

        lblErrorGenerico.Text = string.Empty;

        if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["SesionTimeout"]))
        {
            lblSesion.Text = string.Format(Resources.resCorpusCFDIEs.varRecuerdaSesion, System.Configuration.ConfigurationManager.AppSettings["SesionTimeout"]);
        }

        if (Session["objUsuario"] == null)
        {
            Response.Redirect("~/InicioSesion/webInicioSesionLogin.aspx");
        }
        else
        {
            clsInicioSesionUsuario isSesion = (clsInicioSesionUsuario)Session["objUsuario"];

            if (isSesion.sEstatus != 'A')
            {
                Response.Redirect("~/InicioSesion/webInicioSesionLogin.aspx");
                return;
            }
        }

        if (!IsPostBack)
        {

            double sTimeOut = 10;
            //Tiempo de sesion por variable.
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["SesionTimeout"]))
            {
                sTimeOut = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["SesionTimeout"]);
                sTimeOut = sTimeOut * 60000;
            }



            //Recupera los modulos asignados al usuario.
            if (Session["objModulos"] != null)
            {
                dtModulos = (DataTable)Session["objModulos"];
                dtMenuItems = new DataTable();

                dtMenuItems.Columns.Add("MenuId");
                dtMenuItems.Columns.Add("descripcion");
                dtMenuItems.Columns.Add("Icono");
                dtMenuItems.Columns.Add("Url");
                dtMenuItems.Columns.Add("PadreId");
                dtMenuItems.Columns.Add("Target");

                DataRow row;
                string sTarget;

                foreach (DataRow rows in dtModulos.Rows)
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
            cInicioSesionUsuario = clsComun.fnUsuarioEnSesion();
            lblNombre.Text = cInicioSesionUsuario.sUserName;
            //lblVersionVal.Text = cInicioSesionUsuario.sVersion;

        }
    }
    public void Page_Error(object sender, EventArgs e)
    {
        Exception objErr = Server.GetLastError().GetBaseException();

        if (!string.IsNullOrEmpty(objErr.Message))
        {
            clsErrorLog.fnNuevaEntrada(objErr, clsErrorLog.TipoErroresLog.Datos, "Page_Error", "webOperatorMaster");
            Server.ClearError();
            Response.Redirect("~/webGlobalError.aspx", false);
        }

    }
    protected void HeadLoginStatus_LoggedOut(object sender, EventArgs e)
    {
        try
        {
            cInicioSesionUsuario = clsComun.fnUsuarioEnSesion();
            clsPistasAuditoria.fnGenerarPistasAuditoria(cInicioSesionUsuario.nIdUsuario, DateTime.Now, "Salida Sistema" + "|" + "FormsAuthentication" + "|" + "Cierre de sesion exitosa.");
        }
        catch (Exception)
        {
            clsPistasAuditoria.fnGenerarPistasAuditoria(0, DateTime.Now, "Salida Sistema" + "|" + "FormsAuthentication" + "|" + "Cierre de sesion exitosa.");
        }

        Session.Abandon();
        FormsAuthentication.SignOut();
    }
    protected void lnkEnglish_Click(object sender, EventArgs e)
    {
        Session["Culture"] = "en-Us";
        Response.Redirect(Request.RawUrl);
    }
    protected void lnkEnglish_PreRender(object sender, EventArgs e)
    {
        if ((!IsPostBack) && (Session["Culture"] != null))
        {
            string lang = Session["Culture"].ToString();
        }
    }
    protected void lnkEspañol_Click(object sender, EventArgs e)
    {
        Session["Culture"] = "es-Mx";
        Response.Redirect(Request.RawUrl);
    }
    protected void lnkEspañol_PreRender(object sender, EventArgs e)
    {
        if ((!IsPostBack) && (Session["Culture"] != null))
        {
            string lang = Session["Culture"].ToString();
        }
    }
    protected void lnkSalir_Click(object sender, EventArgs e)
    {
        try
        {
            cInicioSesionUsuario = clsComun.fnUsuarioEnSesion();
            clsPistasAuditoria.fnGenerarPistasAuditoria(cInicioSesionUsuario.nIdUsuario, DateTime.Now, "Salida Sistema" + "|" + "FormsAuthentication" + "|" + "Cierre de sesion exitosa.");
        }
        catch (Exception)
        {
            clsPistasAuditoria.fnGenerarPistasAuditoria(0, DateTime.Now, "Salida Sistema" + "|" + "FormsAuthentication" + "|" + "Cierre de sesion exitosa.");
        }

        Session.Abandon();
        FormsAuthentication.SignOut();
        Response.Redirect("~/InicioSesion/webInicioSesionLogin.aspx");
    }
    
    /// <summary>
    ///	Author		: César Negrete Villa
    /// Date  		: 12/07/2016  12:06
    /// Description : Agrega el Elemento Padre del Menu
    /// </summary>
    /// <param name="dtMenuItems">DataTable con todos los elementos del Menu</param>
    private void fnAddMenuFirstItem(DataTable dtMenuItems)
    {
        divMenu.Controls.Clear();

        foreach (DataRow drMenuItem in dtMenuItems.Rows)
        {
            if (drMenuItem["MenuId"].Equals(drMenuItem["PadreId"]))
            {
                System.Web.UI.HtmlControls.HtmlGenericControl createDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("li");
                createDiv.Controls.Clear();
                createDiv.ID = drMenuItem["descripcion"].ToString();

                if (drMenuItem["descripcion"].ToString() != Resources.resCorpusCFDIEs.mnuInicio)
                {
                    string sURL = string.Empty;
                    createDiv.InnerHtml = string.Format("<a style=\"text-decoration:none\" href=\"{0}\" class=\"dropdown-toggle\" data-toggle=\"dropdown\" role=\"button\" aria-haspopup=\"true\" aria-expanded=\"false\" >" + drMenuItem["descripcion"].ToString() + "<span class=\"caret\"></span></a>", drMenuItem["Url"].ToString());

                    if (dtMenuItems.Select("PadreId = " + "'" + drMenuItem["MenuId"].ToString() + "'").Length > 1)
                    {
                        fnAddMenuChildItem(drMenuItem["MenuId"].ToString(), dtMenuItems, ref createDiv);
                    }

                    createDiv.Attributes.Add("Class", "dropdown");
                    divMenu.Controls.Add(createDiv);
                }
                else
                {
                    //createDiv.InnerHtml = string.Format("<a style=\"text-decoration:none\" href=\"" + VirtualPathUtility.ToAbsolute(drMenuItem["Url"].ToString()) + "\" class=\"dropdown-toggle disabled\" data-toggle=\"dropdown\" role=\"button\" aria-haspopup=\"true\" aria-expanded=\"true\" >" + drMenuItem["descripcion"].ToString() + "</a>", drMenuItem["Url"].ToString());
                    //createDiv.Attributes.Add("Class", "dropdown");
                    //divMenu.Controls.Add(createDiv);
                }
            }
        }
    }

    /// <summary>
    ///	Author		: César Negrete Villa
    /// Date  		: 12/07/2016  12:05
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

                ListaMenu += "<li><a style=\"text-decoration:none\" href=\"" + VirtualPathUtility.ToAbsolute(sUrlPag) + "\" runat=\"server\" >" + drMenuItem["descripcion"].ToString() + "</a></li>";
                fnAddMenuChildItem(drMenuItem["MenuId"].ToString(), dtMenuItems, ref createDivUl);
            }
        }
        createDivUl.InnerHtml = ListaMenu;
        HtmlDiv.Controls.Add(createDivUl);
    }

    /// <summary>
    ///	Author		: César Negrete Villa
    /// Date  		: 12/07/2016  12:26
    /// Description : Revisa el nombre de cada menú para asignarle su resource correspondiente.
    /// </summary>
    /// <param name="sMensaje">Nombre del Menu</param>
    /// <returns>Resource Text</returns>
    private string fnRevisaResource(string sMensaje)
    {
        string sRetorno = string.Empty;

        switch (sMensaje)
        {

            case "Cambio de Contraseña":
                sRetorno = Resources.resCorpusCFDIEs.lblSubUsrPass;
                break;
            case "Catálogos":
                sRetorno = Resources.resCorpusCFDIEs.mnuCatalogos;
                break;
            case "Inicio":
                sRetorno = Resources.resCorpusCFDIEs.mnuInicio;
                break;
            case "Configuración de Diseño":
                sRetorno = "";//Resources.resCorpusCFDIEs.mnuDesign;
                break;
            case "Configuración Plantillas":
                sRetorno = Resources.resCorpusCFDIEs.mnuConfPlantilla;
                break;
            case "Consultas":
                sRetorno = Resources.resCorpusCFDIEs.mnuConsultas;
                break;
            case "Consultas CFDI":
                sRetorno = Resources.resCorpusCFDIEs.mnuConsultasCFDI;
                break;
            case "Cuenta":
                sRetorno = Resources.resCorpusCFDIEs.mnuCuenta;
                break;
            case "Organismos":
                sRetorno = Resources.resCorpusCFDIEs.hedDatosFisc;
                break;
            case "Usuarios":
                sRetorno = Resources.resCorpusCFDIEs.mnuUsuarios;
                break;
            case "Generación":
                sRetorno = Resources.resCorpusCFDIEs.mnuGeneracion;
                break;
            case "Ayuda":
                sRetorno = Resources.resCorpusCFDIEs.mnuAyuda;
                break;
            case "Soporte":
                sRetorno = Resources.resCorpusCFDIEs.mnuSoporte;
                break;
            case "Distribuidor":
                sRetorno = Resources.resCorpusCFDIEs.mnuDistribuidor;
                break;
            case "Sucursales":
                sRetorno = Resources.resCorpusCFDIEs.mnuSucursales;
                break;
            case "Clientes":
                sRetorno = Resources.resCorpusCFDIEs.mnuClientes;
                break;
            case "Artículos":
                sRetorno = Resources.resCorpusCFDIEs.mnuArticulos;
                break;
            case "Empleados":
                sRetorno = Resources.resCorpusCFDIEs.mnuEmpleados;
                break;
            case "Generación CFDI":
                sRetorno = Resources.resCorpusCFDIEs.mnuTimbrado;
                break;
            case "Registro Nomina":
                sRetorno = Resources.resCorpusCFDIEs.mnuNominaRegistro;
                break;
            case "Generacion Nómina":
                sRetorno = Resources.resCorpusCFDIEs.mnuNominaGeneracion;
                break;
            case "Consulta Nomina":
                sRetorno = Resources.resCorpusCFDIEs.mnuNominaConsulta;
                break;
            case "Consulta Proveedores":
                sRetorno = Resources.resCorpusCFDIEs.mnuConsProv;
                break;
            case "Consulta Estado de Cuenta":
                sRetorno = Resources.resCorpusCFDIEs.mnuEstadoCuenta;
                break;
            case "Reporte Créditos Acumulado":
                sRetorno = Resources.resCorpusCFDIEs.mnuReporteAcumulado;
                break;
            case "Reporte Créditos Histórico":
                sRetorno = Resources.resCorpusCFDIEs.mnuReporteHistorico;
                break;
            case "Concentrado Clientes Distribuidor":
                sRetorno = Resources.resCorpusCFDIEs.mnuReporteConcentradoDistribuidor;
                break;
            case "Asignación de Créditos":
                sRetorno = Resources.resCorpusCFDIEs.mnuAsignaCreditos;
                break;
            case "Usuarios Conectados":
                sRetorno = Resources.resCorpusCFDIEs.mnuUsuariosConectados;
                break; 
            case "Datos Fiscales Matriz":
                sRetorno = Resources.resCorpusCFDIEs.mnuDatosFiscales;
                break;
            case "Baja Cuenta":
                sRetorno = Resources.resCorpusCFDIEs.mnuBajaCuenta;
                break;
            case "Preguntas Frecuentes":
                sRetorno = Resources.resCorpusCFDIEs.mnuPreguntasFrecuentes;
                break;
            case "Asignación de Créditos Distribuidor":
                sRetorno = Resources.resCorpusCFDIEs.mnuAsignaDistribuidor;
                break;
            case "Asignación Base64":
                sRetorno = Resources.resCorpusCFDIEs.mnuAsignaBase64;
                break;
            default:
                sRetorno = sMensaje;
                break;
        }

        return sRetorno;
    }
}
