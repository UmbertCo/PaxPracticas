using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.XPath;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.Globalization;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    DataTable tablaModulos;
    DataTable dtMenuItems;
    protected void Page_Load(object sender, EventArgs e)
    {
        //DataTable dtCreditos;
        if (!IsPostBack)
        {
            try
            {
                clsInicioSesionSolicitudReg busquedaUsuario = new clsInicioSesionSolicitudReg();
                clsInicioSesionUsuario usuario = clsComun.fnUsuarioEnSesion();
                if (usuario != null)
                {
                    lblNombre.Text = usuario.userName;
                    lnkSalir.Text = Resources.resCorpusCFDIEs.mnuSalir;
                    if (usuario.estatus == 'A')
                    {
                        generarMenuGlobal();
                    }
                }
                else
                {
                    lblNombre.Visible = false;
                    lblBienvenido.Visible = false;
                    lblDosPuntos.Visible = false;
                    lnkSalir.Text = string.Empty;
                    lblCorchDer.Visible = false;
                    lblCorchIzq.Visible = false;

                    tablaModulos = busquedaUsuario.fnRecuperaModulosUsuario("PAX");

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
            catch (Exception ex)
            { clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion); }
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

    /***Agregamos el Menu dinamico***/
    #region

    public void agregarmenu(ref MenuItem mmenuItem, DataTable dtmenuItems)
    {
        //recorremos el datatable para definir los items hijos dado el parametro 
        //pasado por referencia
        foreach (DataRow drmenuItem in dtmenuItems.Rows)
        {
            //indicamos qué items son hijos
            if (drmenuItem["IdPadre"].ToString().Equals(mmenuItem.Value) &&
                !(drmenuItem["IdMenu"].Equals(drmenuItem["IdPadre"])))
            {
                MenuItem NuevoMenuItem = new MenuItem();
                NuevoMenuItem.Value = drmenuItem["IdMenu"].ToString();
                NuevoMenuItem.Text = drmenuItem["Descripcion"].ToString();

                mmenuItem.ChildItems.Add(NuevoMenuItem);
                //llamamos recursivamente el metodo para ver si aun tiene items hijos
                agregarmenu(ref NuevoMenuItem, dtmenuItems);
            }
        }
    }

    #endregion

    protected void lnkSalir_Click(object sender, EventArgs e)
    {
        clsInicioSesionUsuario usuario = clsComun.fnUsuarioEnSesion();
        if (usuario != null)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            //Response.Redirect("~/Default.aspx");
            Response.Redirect("~/Account/Login.aspx");
        }
        else
        {
            Response.Redirect("~/Account/Login.aspx");
        }
    }

    /// <summary>
    /// Funcion que revisa los nodos para agregarlos al menu
    /// </summary>
    /// <param name="dtMenuItems"></param>
    private void fnAddMenuFirstItem(DataTable dtMenuItems)
    {
        //HtmlGenericControl ul = new HtmlGenericControl("ul");

        foreach (DataRow drMenuItem in dtMenuItems.Rows)
        {
            MenuItem mnuMenuItem = new MenuItem();

            if (drMenuItem["MenuId"].Equals(drMenuItem["PadreId"]))
            {
                mnuMenuItem.Value = drMenuItem["MenuId"].ToString();
                mnuMenuItem.Text = drMenuItem["descripcion"].ToString();
                mnuMenuItem.ImageUrl = drMenuItem["Icono"].ToString();
                mnuMenuItem.NavigateUrl = drMenuItem["Url"].ToString();
                mnuMenuItem.Target = drMenuItem["Target"].ToString();

                if (mnuMenuItem.NavigateUrl == "#")
                    mnuMenuItem.Selectable = false;

                mnuPrincipal.Items.Add(mnuMenuItem);

                AddMenuChildItem(ref mnuMenuItem, dtMenuItems);
            }
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
                mnuNewMenuItem.Target = drMenuItem["Target"].ToString();

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
            case "Inicio":
                sRetorno = Resources.resCorpusCFDIEs.mnuInicio;
                break;

            case "Catálogos":
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

            case "Datos Fiscales Matríz":
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

            case "Articulos":
                sRetorno = Resources.resCorpusCFDIEs.mnuArticulos;
                break;

            case "Usuarios":
                sRetorno = Resources.resCorpusCFDIEs.mnuUsuarios;
                break;

            case "Consultas Proveedores":
                sRetorno = Resources.resCorpusCFDIEs.mnuConsProv;
                break;

            case "Consulta Estado de Cuenta":
                sRetorno = Resources.resCorpusCFDIEs.mnuEstadoCuenta;
                break;

            case "Asignación de Créditos":
                sRetorno = Resources.resCorpusCFDIEs.mnuDistAsigCreditos;
                break;

            case "Artículos":
                sRetorno = Resources.resCorpusCFDIEs.mnuArticulos;
                break;

            case "Distribuidor":
                sRetorno = Resources.resCorpusCFDIEs.mnuDistribuidor;
                break;

            case "Generación":
                sRetorno = Resources.resCorpusCFDIEs.mnuGeneracion;
                break;

            case "Generación CFDI":
                sRetorno = Resources.resCorpusCFDIEs.mnuTimbrado;
                break;

            case "Asignación de Créditos Distribuidor":
                sRetorno = Resources.resCorpusCFDIEs.mnuAsignaDistribuidor;
                break;

            case "Distribuidores":
                sRetorno = Resources.resCorpusCFDIEs.mnuDistribuidores;
                break;

            case "Reporte Créditos Acumulado":
                sRetorno = Resources.resCorpusCFDIEs.mnuReporteAcumulado;
                break;

            case "Reporte Créditos Histórico":
                sRetorno = Resources.resCorpusCFDIEs.mnuReporteHistorico;
                break;

            case "Configuración Plantillas":
                sRetorno = Resources.resCorpusCFDIEs.mnuConfPlantilla;
                break;
            case "Publicidad":
                sRetorno = Resources.resCorpusCFDIEs.mnuPublicidad;
                break;


            default:
                sRetorno = sMensaje;
                break;
        }

        return sRetorno;
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
