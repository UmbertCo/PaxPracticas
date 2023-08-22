using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Threading;
/// <summary>
/// Pantalla desde la cual se podrán enviar tickets de soporte
/// </summary>
public partial class Pantallas_Incidentes_RegistroIncidencias : System.Web.UI.Page
{
    private clsUsuarios gDAL;
    private clsIncidencias gINS;
    protected void Page_Load(object sender, EventArgs e)
    {
      
        if (Session["objUsuario"] == null)
        {
            Response.Redirect("~/Pantallas/Login/webInicioSesionLogin.aspx");
        }
        if (!IsPostBack)
        {
            fnCargaTipoIncidencias();
        }
      
    }

    private void fnCargaTipoIncidencias()
    {
        try
        {            
            gDAL = new clsUsuarios();
            ddlIncidencia.DataSource = gDAL.fnCargarCatalogoTipoIncidencias();
            ddlIncidencia.DataTextField = "tipo_incidente";
            ddlIncidencia.DataValueField = "id_tipo_incidente";
            ddlIncidencia.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnCargaTipoIncidencias", "webCatalogoIncidencias.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnCargaTipoIncidencias", "webCatalogoIncidencias.aspx.cs");
        }
    }


    protected void button1_Click(object sender, EventArgs e)
    {
        button1.Enabled = true;
        try
        {
            gINS = new clsIncidencias();
            string ruta = null;
            int archivoEnBytes = 0;
            bool psarchivo = true;
            bool Archivo = true;
            if (txtDescripcionI.Text != "")
            {

                if (fuPruebas.FileName.ToString() != "" && fuPruebas.FileName.ToString() != null)
                {
                    Archivo = gINS.fnverificaarchivo(fuPruebas.FileName);
                    if (Archivo == true)
                    {
                        string psArchivo = fuPruebas.FileName.ToString();
                        System.Guid miGUID = System.Guid.NewGuid();
                        String sGUID = miGUID.ToString();
                        fuPruebas.SaveAs(clsComun.fnObtenerParamentro("RutaDocInc") + miGUID + fuPruebas.FileName);
                        ruta = clsComun.fnObtenerParamentro("RutaDocInc") + miGUID + fuPruebas.FileName;
                        FileInfo info = new FileInfo(ruta);
                        archivoEnBytes = (Convert.ToInt32(info.Length) / 1024);
                        psarchivo = gINS.fnVerificaTamanioMax(archivoEnBytes);
                    }
                    else
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorArchivo + " " + clsComun.fnObtenerParamentro("Extensiones"));
                    }
                }


                if (fuPruebas.FileName.ToString() == "")
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

                        DataSet dsUsuario = gINS.fnObtieneUsuarioSoporte(Convert.ToInt32(ddlIncidencia.SelectedValue));
                        foreach (DataRow renglon in dsUsuario.Tables[0].Rows)
                        {

                            DataTable dsCarga = new DataTable();
                            dsCarga = gINS.fnObtieneIncidenciasporUsuario(Convert.ToInt32(renglon["id_usuario_soporte"]));
                            DataRow nuevo;
                            nuevo = Tabla.NewRow();
                            nuevo["idusuario"] = Convert.ToInt32(renglon["id_usuario_soporte"]);
                            nuevo["carga"] = Convert.ToInt32(dsCarga.Rows.Count);
                            Tabla.Rows.Add(nuevo);
                        }
                        DataTable dtOrdenado = FiltrarDataTable(Tabla, "", "carga ASC");
                        string sNoTicket = gINS.fnEnviarTicket(ddlIncidencia.SelectedValue, txtDescripcionI.Text, Convert.ToInt32(dtOrdenado.Rows[0]["idusuario"]), ruta, 2);

                        //FileInfo info = new FileInfo(ruta);
                        //info.Delete();

                        if (!string.IsNullOrEmpty(sNoTicket))
                        {
                            clsComun.fnMostrarMensaje(this, string.Format(Resources.resCorpusCFDIEs.varTicket, sNoTicket));

                            clsComun.fnNuevaPistaAuditoria(
                                "webCatalogoIncidencias",
                                "fnEnviarTicket",
                                 "El usuario ha levantado un caso de incidente con ticket " + sNoTicket);

                            ddlIncidencia.SelectedIndex = 0;
                            txtDescripcionI.Text = string.Empty;
                        }
                        else
                        {
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
                        }
                    }
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorArchivo + " " + clsComun.fnObtenerParamentro("Extensiones"));
                }
            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblSoporteDescipcion);
            }
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "button1_Click", "webCatalogoIncidencias.aspx.cs");
           
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "button1_Click", "webCatalogoIncidencias.aspx.cs");
            
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