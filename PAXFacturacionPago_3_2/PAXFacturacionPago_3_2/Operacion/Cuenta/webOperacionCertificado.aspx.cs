using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Operacion_Cuenta_webOperacionDatosFiscales : System.Web.UI.Page
{
    private clsOperacionCuenta gDAL;

    protected void Page_Load(object sender, EventArgs e)
    {
        clsComun.fnPonerTitulo(this);

        //Cargamos los datos del usuario a pantalla
        if (!IsPostBack)
        {
            gDAL = new clsOperacionCuenta();

            try
            {
                DataTable sdrInfo = gDAL.fnObtenerDatosFiscales();

                if (sdrInfo != null && sdrInfo.Rows.Count > 0)
                {
                    hdIdRfc.Value = sdrInfo.Rows[0]["id_rfc"].ToString();
                    hdRfc.Value = sdrInfo.Rows[0]["rfc"].ToString();
                }
            }
            catch (SqlException ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
                Response.Redirect("~/Default.aspx", false);
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                Response.Redirect("~/Default.aspx", false);
            }
        }
    }


    protected void btnActualizarCertificados_Click(object sender, EventArgs e)
    {
        //Validaciones
        if (string.IsNullOrEmpty(txtPass.Text)
            || string.IsNullOrEmpty(fupKey.FileName)
            || string.IsNullOrEmpty(fupCer.FileName)
            )
        {
            clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }

        gDAL = new clsOperacionCuenta();
        clsValCertificado vValidadorCertificado = null;
        string resValidacion = string.Empty;

        //Verificamos que el archivo de llave privada sea un .key
        if (Path.GetExtension(fupKey.FileName).ToLower() != ".key")
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valExtensionKey);
            return;
        }

        //verificamos que el archivo de certificado público sea un .cer
        if (Path.GetExtension(fupCer.FileName).ToLower() != ".cer")
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valExtensionCer);
            return;
        }
        else
        {
            try
            {
                //realizamos las validaciones de SAT sobre el archivo
                vValidadorCertificado = new clsValCertificado(fupCer.FileBytes);
                vValidadorCertificado.LlavePrivada = new clsOperacionTimbradoSellado(fupKey.FileBytes, System.Text.Encoding.Unicode.GetBytes(txtPass.Text));
                resValidacion = vValidadorCertificado.ValidarCertificado(hdRfc.Value);

                if (!string.IsNullOrEmpty(resValidacion))
                {
                    clsComun.fnMostrarMensaje(this, resValidacion);
                    return;
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            }
        }


        //Una vez validados ambos archivos y su password los guardamos en la BD
        try
        {
            clsInicioSesionUsuario datosUsuario = clsComun.fnUsuarioEnSesion();
            clsCancelacionSAT cancelacion = new clsCancelacionSAT();
            byte[] certPFx = cancelacion.crearPfx(fupCer.FileBytes, fupKey.FileBytes, txtPass.Text);

            //certPFx = Utilerias.Encriptacion.DES3.Encriptar(certPFx);
            certPFx = PAXCrypto.CryptoAES.EncriptaAESB(certPFx);

            int retVal = gDAL.fnGuardarCertificados(hdIdRfc.Value, vValidadorCertificado, certPFx);

            if (retVal != 0)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);

                clsComun.fnNuevaPistaAuditoria(
                    "webGlobalCuenta",
                    "fnGuardarCertificados",
                    "Se modificaron los certificados para el RFC con ID " + hdIdRfc
                    );
            }
            else
                throw new Exception("No se actualizó ningún registro");
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCambio);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCambio);
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