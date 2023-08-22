using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Threading;
using System.Globalization;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class webGlobalMaster : System.Web.UI.MasterPage
{
    DataTable tablaModulos;
    DataTable dtMenuItems; 

    protected void Page_Load(object sender, EventArgs e)
    {
        clsInicioSesionUsuario isSesion = (clsInicioSesionUsuario)Session["objUsuario"];
        if (isSesion != null)
        {
            if (isSesion.estatus.Equals('A'))
            {
                if ((Request.Path.ToLower().Contains("webiniciosesionlogin.aspx")
                || Request.Path.ToLower().Contains("webiniciosesioncorrecto.aspx")
                || Request.Path.ToLower().Contains("webiniciosesionreactivar.aspx")
                || Request.Path.ToLower().Contains("webiniciosesionrecupera.aspx")
                || Request.Path.ToLower().Contains("webiniciosesionregdatos.aspx")
                || Request.Path.ToLower().Contains("webiniciosesionregistro.aspx")
                || Request.Path.ToLower().Contains("webiniciosesioncambiarpwd.aspx")))
                {
                    Session.Add("UsuarioActivo", isSesion.userName);
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

                //tablaModulos = busquedaUsuario.fnRecuperaModulosUsuario("PAX");
                
                tablaModulos.Rows.Add("25", "Inicio", "InicioSesion/webInicioSesionLogin.aspx", "25", "false", "_blank");
                tablaModulos.Rows.Add(new Object[] { "26", "", "#", "26", "false", "_blank" });
                tablaModulos.Rows.Add(new Object[] { "26", "", "#", "26", "false", "_blank" });
                tablaModulos.Rows.Add(new Object[] { "26", "", "#", "26", "false", "_blank" });
                tablaModulos.Rows.Add(new Object[] { "26", "", "#", "26", "false", "_blank" });
                tablaModulos.Rows.Add(new Object[] { "26", "", "#", "26", "false", "_blank" });
                tablaModulos.Rows.Add(new Object[] { "26", "", "#", "26", "false", "_blank" });
                tablaModulos.Rows.Add(new Object[] { "26", "", "#", "26", "false", "_blank" });
                tablaModulos.Rows.Add(new Object[] { "26", "", "#", "26", "false", "_blank" });
                tablaModulos.Rows.Add(new Object[] { "26", "", "#", "26", "false", "_blank" });


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


    protected void lnkMUsuario_Click(object sender, EventArgs e)
    {
        string filename = "DMS.A5.PT1.V1 Manual de Usuario.pdf";
        fnDescargaArchivo(filename);

    }
    protected void lnkMSoporte_Click(object sender, EventArgs e)
    {
        string filename = "DMS.V1 Manual de Soporte.pdf";
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
                    Response.AddHeader("Content-Disposition","attachment; filename=" + toDownload.Name);
                    Response.AddHeader("Content-Length",toDownload.Length.ToString());
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

    #region MenuDinamico

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

    #endregion
    protected void imgDescarga_Click(object sender, ImageClickEventArgs e)
    {
        string filename = "DMS.A5.PT1.V1 Manual de Usuario.pdf";
        fnDescargaArchivo(filename);
    }
}
