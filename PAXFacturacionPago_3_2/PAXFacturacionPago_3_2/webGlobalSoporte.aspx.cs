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

/// <summary>
/// Pantalla desde la cual se podrán enviar tickets de soporte
/// </summary>
public partial class webGlobalSoporte : System.Web.UI.Page
{
    private clsGlobalSoporte gDAL;
    private clsInicioSesionUsuario datosUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fnCargarAsuntos();
            datosUsuario = clsComun.fnUsuarioEnSesion();
        }
    }

    /// <summary>
    /// Obtiene la lista de tipos de incidentes disponibles
    /// </summary>
    private void fnCargarAsuntos()
    {
        gDAL = new clsGlobalSoporte();

        try
        {
            ddlAsunto.DataSource = gDAL.fnCargarAsuntos();
            ddlAsunto.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    protected void btnEnviar_Click1(object sender, EventArgs e)
    {
        gDAL = new clsGlobalSoporte();
        string ruta = null;
        int archivoEnBytes = 0;
        bool psarchivo = true;
        bool Archivo = true;
        if (fuAsunto.FileName.ToString() != "")
        {
            Archivo = gDAL.fnverificaarchivo(fuAsunto.FileName);
            if (Archivo == true)
            {
                string psArchivo = fuAsunto.FileName.ToString();  
                System.Guid miGUID = System.Guid.NewGuid();
                String sGUID = miGUID.ToString();
                string valor = clsComun.ObtenerParamentro("RutaDocInc") + sGUID +psArchivo;
                //System.Security.AccessControl.FileSecurity seguridad = new System.Security.AccessControl.FileSecurity();
                //FileStream theFile = File.Create(valor, Tamano, FileOptions.WriteThrough);
                //theFile.Flush();
                //theFile.Close();               
                fuAsunto.SaveAs(valor);
                ruta = clsComun.ObtenerParamentro("RutaDocInc") + sGUID + fuAsunto.FileName;
                FileInfo info = new FileInfo(ruta);
                archivoEnBytes = (Convert.ToInt32(info.Length) / 1024);
                psarchivo = gDAL.fnVerificaTamanioMax(archivoEnBytes);
            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorArchivo + " " + clsComun.ObtenerParamentro("Extensiones"));
            }
        }
                
            if (fuAsunto.FileName.ToString() == "")
            {
                Archivo = true;
            }
            if (Archivo == true)
            {
                if (psarchivo == false)
                {
                    
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorTamanio);
                }
                else
                {
                    DataTable Tabla = new DataTable("usuarios");
                    Tabla.Columns.Add("idusuario", typeof(Int32));
                    Tabla.Columns.Add("carga", typeof(Int32));

                    DataSet dsUsuario = gDAL.fnObtieneUsuarioSoporte(Convert.ToInt32(ddlAsunto.SelectedValue));
                    foreach (DataRow renglon in dsUsuario.Tables[0].Rows)
                    {

                        DataTable dsCarga = new DataTable();
                        dsCarga = gDAL.fnObtieneIncidenciasporUsuario(Convert.ToInt32(renglon["id_usuario_soporte"]));
                        DataRow nuevo;
                        nuevo = Tabla.NewRow();
                        nuevo["idusuario"] = Convert.ToInt32(renglon["id_usuario_soporte"]);
                        nuevo["carga"] = Convert.ToInt32(dsCarga.Rows.Count);
                        Tabla.Rows.Add(nuevo);
                    }
                    if (dsUsuario.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtOrdenado = FiltrarDataTable(Tabla, "", "carga ASC");
                        datosUsuario = clsComun.fnUsuarioEnSesion();
                        string sNoTicket = gDAL.fnEnviarTicket(ddlAsunto.SelectedValue, txtDescripcion.Text, Convert.ToInt32(dtOrdenado.Rows[0]["idusuario"]), ruta,datosUsuario.id_usuario,1);

                        if (!string.IsNullOrEmpty(sNoTicket))
                        {
                            clsComun.fnMostrarMensaje(this, string.Format(Resources.resCorpusCFDIEs.varTicket, sNoTicket));

                            clsComun.fnNuevaPistaAuditoria(
                                "webGlobalSoporte",
                                "fnEnviarTicket",
                                "El usuario ha levantado un caso de incidente con ticket " + sNoTicket
                                );

                            ddlAsunto.SelectedIndex = 0;
                            txtDescripcion.Text = string.Empty;
                        }
                        else
                        {
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
                        }
                    }
                    else
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
                    }

                }
            }
            else
            {            
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorArchivo);
            }
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

    public DataTable FiltrarDataTable(DataTable dt, string filtro, string orden)
    {
        DataRow[] rows;
        DataTable dtNew;

        try
        {
            dtNew = dt.Clone();
            rows = dt.Select(filtro, orden);

            Array.ForEach(rows, dtNew.ImportRow);

            return dtNew;
        }
        catch (Exception ex)
        {
            throw new Exception(String.Format("FiltrarDataTable - {0} - {1}", ex.Source, ex.Message));
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
}