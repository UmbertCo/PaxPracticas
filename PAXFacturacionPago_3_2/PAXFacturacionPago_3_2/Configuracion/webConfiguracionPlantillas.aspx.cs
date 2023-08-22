using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Globalization;

public partial class Configuracion_webConfiguracionPlantillas : System.Web.UI.Page
{
    clsInicioSesionUsuario datosUsuario;

    protected void Page_Load(object sender, EventArgs e)
    {
         try
        {
            if (!IsPostBack) 
            {
                fnCargarSucursalesEmisor();
                //fnCargaPlantillasBase();
                clsConfiguracionPlantilla conf = new clsConfiguracionPlantilla();
                datosUsuario = clsComun.fnUsuarioEnSesion();
                int idEstructura = conf.fnRecuperaEstructura(datosUsuario.id_usuario);
                ddlSucursales.SelectedValue = Convert.ToString(idEstructura);
                ddlSucursales_SelectedIndexChanged(sender, null);
            }
        }
         catch (Exception ex)
         {
             clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
         }
    }

    private void fnCargarSucursalesEmisor()
    {
        try
        {

            datosUsuario = clsComun.fnUsuarioEnSesion();
            
            ddlSucursales.DataSource = clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario);
            ddlSucursales.DataBind();
            
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            ddlSucursales.DataSource = null;
            ddlSucursales.DataBind();
        }
        catch
        {
            //referencia nula
        }
    }

    protected void ddlSucursales_SelectedIndexChanged(object sender, EventArgs e)
    {
        clsConfiguracionPlantilla conf = new clsConfiguracionPlantilla();
        try
        {
            DataTable tblConf = new DataTable();
            tblConf = conf.fnObtieneConfiguracionPlantilla(Convert.ToInt32(ddlSucursales.SelectedValue));
            if (tblConf.Rows.Count > 0)
            {
                foreach (DataRow renglon in tblConf.Rows)
                {
                    int idconf = Convert.ToInt32(renglon["id_configuracion"]);
                    ViewState["id_configuracion"] = idconf;
                    int pidPlantilla = Convert.ToInt32(renglon["id_plantilla"]);
                    ViewState["idplantilla"] = pidPlantilla;
                    if(pidPlantilla != 1 && pidPlantilla != 2)
                    ViewState["idplantillaper"] = pidPlantilla;
                    switch (pidPlantilla)
                    {
                        case 1:
                            rbSinLogo.Checked = true;
                            rbConLogo.Checked = false;
                            rbPersonal.Checked = false;
                            break;
                        case 2:
                            rbPersonal.Checked = false;
                            rbConLogo.Checked = true;
                            rbPersonal.Checked = false;
                            break;
                        default:
                            rbSinLogo.Checked = false;
                            rbConLogo.Checked = false;
                            rbPersonal.Checked = true;
                            rbPersonal.Visible = true;
                            imgPersonal.Visible = true;
                            break;
                    }
                    string sColor = Convert.ToString(renglon["color"]);
                    ViewState["Color"] = sColor;
                    switch (sColor)
                    {
                        case "Black":
                            rbNegro.Checked = true;
                            rbRojo.Checked =  false;
                            rbVerde.Checked =  false;
                            rbAzul.Checked = false;
                            imgSinlogo.ImageUrl = "~/Imagenes/Generica_Negra.jpg";
                            imgLogo.ImageUrl = "~/Imagenes/Logo_Negro.jpg";
                            break;
                        case "DarkRed":
                            rbRojo.Checked = true;
                            rbNegro.Checked = false;
                            rbVerde.Checked =  false;
                            rbAzul.Checked = false;
                            imgSinlogo.ImageUrl = "~/Imagenes/Generica_Rojo.jpg";
                            imgLogo.ImageUrl = "~/Imagenes/Logo_Rojo.jpg";
                            break;
                        case "Green":
                            rbVerde.Checked = true;
                            rbNegro.Checked = false;
                            rbRojo.Checked =  false;
                            rbAzul.Checked = false;
                            imgSinlogo.ImageUrl = "~/Imagenes/Generica_Verde.jpg";
                            imgLogo.ImageUrl = "~/Imagenes/Logo_Verde.jpg";
                            break;
                        case "Navy":
                            rbAzul.Checked = true;
                            rbNegro.Checked = false;
                            rbRojo.Checked =  false;
                            rbVerde.Checked =  false;
                            imgSinlogo.ImageUrl = "~/Imagenes/Generica_Azul.jpg";
                            imgLogo.ImageUrl = "~/Imagenes/Logo_Azul.jpg";
                            break;
                        default:
                            rbNegro.Checked = false;
                            rbRojo.Checked =  false;
                            rbVerde.Checked =  false;
                            rbAzul.Checked = false;
                            imgSinlogo.ImageUrl = "~/Imagenes/Generica_Negra.jpg";
                            imgLogo.ImageUrl = "~/Imagenes/Logo_Negro.jpg";
                            break;
                    }
                }
            }
            else
            {
                rbNegro.Checked = true;
                rbRojo.Checked = false;
                rbVerde.Checked = false;
                rbAzul.Checked = false;
                rbSinLogo.Checked = true;
                rbConLogo.Checked = false;
                imgSinlogo.ImageUrl = "~/Imagenes/Generica_Negra.jpg";
                imgLogo.ImageUrl = "~/Imagenes/Logo_Negro.jpg";
                ViewState["id_configuracion"] = 0;
                ViewState["idplantilla"] = 0;
            }

        }
        catch (Exception  ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
        
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        clsConfiguracionPlantilla conf = new clsConfiguracionPlantilla();
        try
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();
            int idConfiguracion = Convert.ToInt32(ViewState["id_configuracion"]);

            string sColor = string.Empty;
            if (rbNegro.Checked == true)
            {
                sColor = "Black";
            }
            else if(rbRojo.Checked == true)
            {
                sColor = "DarkRed";
            }
            else if (rbVerde.Checked == true)
            {
                sColor = "Green";
            }
            else if (rbAzul.Checked == true)
            {
                sColor = "Navy";
            }

            int idPlantilla = 0;
            if (rbSinLogo.Checked == true)
            {
                idPlantilla = 1;
            }
            else if (rbConLogo.Checked == true)
            {
                idPlantilla = 2;
            }
            else
            {
                idPlantilla = Convert.ToInt32(ViewState["idplantillaper"]);
                sColor = String.Empty;
            }


            idConfiguracion = conf.fnActualizaPlantilla(idConfiguracion, idPlantilla, Convert.ToInt32(ddlSucursales.SelectedValue), sColor, datosUsuario.id_usuario);
            ViewState["id_configuracion"] = idConfiguracion;
        
          

            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }
    protected void rbNegro_CheckedChanged(object sender, EventArgs e)
    {
        imgSinlogo.ImageUrl = "~/Imagenes/Generica_Negra.jpg";
        imgLogo.ImageUrl = "~/Imagenes/Logo_Negro.jpg";
    }
    protected void rbAzul_CheckedChanged(object sender, EventArgs e)
    {
        imgSinlogo.ImageUrl = "~/Imagenes/Generica_Azul.jpg";
        imgLogo.ImageUrl = "~/Imagenes/Logo_Azul.jpg";
    }
    protected void rbVerde_CheckedChanged(object sender, EventArgs e)
    {
        imgSinlogo.ImageUrl = "~/Imagenes/Generica_Verde.jpg";
        imgLogo.ImageUrl = "~/Imagenes/Logo_Verde.jpg";
    }
    protected void rbRojo_CheckedChanged(object sender, EventArgs e)
    {
        imgSinlogo.ImageUrl = "~/Imagenes/Generica_Rojo.jpg";
        imgLogo.ImageUrl = "~/Imagenes/Logo_Rojo.jpg";
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
