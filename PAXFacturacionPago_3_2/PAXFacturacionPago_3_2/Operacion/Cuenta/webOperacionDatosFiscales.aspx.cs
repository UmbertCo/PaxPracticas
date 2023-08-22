using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Security;
using System.Threading;
using System.Globalization;

/// <summary>
/// Pantalla para la administración de los aspectos básicos de la cuenta como serían
/// los datos de la sucursal matriz, sus certificados y la contraseña de la cuenta
/// </summary>
public partial class webOperacionDatosFiscales : System.Web.UI.Page
{
    private clsOperacionCuenta gDAL;
    private clsInicioSesionUsuario datosUsuario;
    private clsOperacionRFC gRFC;

    protected void Page_Load(object sender, EventArgs e)
    {
        clsComun.fnPonerTitulo(this);
        
        //Cargamos los datos del usuario a pantalla
        if (!IsPostBack)
        {
            gDAL = new clsOperacionCuenta();
            datosUsuario = clsComun.fnUsuarioEnSesion();
            fnCargarPaises();
            fnCargarEstados(ddlPais.SelectedValue);

            try
            {
                DataTable sdrInfo =  gDAL.fnObtenerDatosFiscales();
                //SqlDataReader sdrInfoEx = gDAL.fnObtenerDatosFiscalesEx();

                if(sdrInfo != null && sdrInfo.Rows.Count > 0)
                {
                    hdIdRfc.Value = sdrInfo.Rows[0]["id_rfc"].ToString();
                    txtRFC.Text = sdrInfo.Rows[0]["rfc"].ToString();
                    txtRazonSocial.Text = sdrInfo.Rows[0]["razon_social"].ToString();

                    hdIdEstructura.Value = sdrInfo.Rows[0]["id_estructura"].ToString();
                    txtSucursal.Text = sdrInfo.Rows[0]["nombre"].ToString();
                    txtCalle.Text = sdrInfo.Rows[0]["calle"].ToString();
                    txtNoExterior.Text = sdrInfo.Rows[0]["numero_exterior"].ToString();
                    txtNoInterior.Text = sdrInfo.Rows[0]["numero_interior"].ToString();
                    txtColonia.Text = sdrInfo.Rows[0]["colonia"].ToString();
                    txtReferencia.Text = sdrInfo.Rows[0]["referencia"].ToString();
                    txtCodigoPostal.Text = sdrInfo.Rows[0]["codigo_postal"].ToString();
                    txtLocalidad.Text = sdrInfo.Rows[0]["localidad"].ToString();
                    txtMunicipio.Text = sdrInfo.Rows[0]["municipio"].ToString();
                    ddlPais.SelectedValue = sdrInfo.Rows[0]["id_pais"].ToString();
                    ddlEstado.SelectedValue = sdrInfo.Rows[0]["id_estado"].ToString();
                    txtRegimenFiscal.Text = sdrInfo.Rows[0]["regimen_fiscal"].ToString();
                }

                if (datosUsuario.version == "3.2")
                {
                    lblRegimenFiscal.Visible = true;
                    txtRegimenFiscal.Visible = true;
                    rfvRegimenFiscal.EnableClientScript = true;
                    rfvRegimenFiscal.Visible = true;
                }
                else
                {
                    lblRegimenFiscal.Visible = false;
                    txtRegimenFiscal.Visible = false;
                    rfvRegimenFiscal.EnableClientScript = false;
                    rfvRegimenFiscal.Visible = false;
                }

                //    tbl3_2.Visible = true;
 
                //    if (sdrInfoEx != null && sdrInfoEx.HasRows && sdrInfoEx.Read())
                //    {
                //        ddlPais0.SelectedValue = sdrInfoEx["id_pais"].ToString();
                //        ddlEstado0.SelectedValue = sdrInfoEx["id_estado"].ToString();
                //        txtMunicipioEmisor.Text = sdrInfoEx["municipio"].ToString();
                //        txtCalleEmisor.Text = sdrInfoEx["calle"].ToString();
                //        txtCodigoPostalEmisor.Text = sdrInfoEx["codigo_postal"].ToString();
                //    }
                //}
            }
            catch(SqlException ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
                Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                Response.Redirect("~/Default.aspx");
            }

            //Establecemos el modal
            fnEstablecerDialogoModal();
        }
    }

    private void fnEstablecerDialogoModal()
    {
        try
        {
            string sScript = "function bajaUsuario(){ return confirm('" + Resources.resCorpusCFDIEs.varConfirmacionBaja + "' );  }";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "fnBajaUsu", sScript, true);
        }
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
        }
    }

    /// <summary>
    /// Método encargado de llenar el drop de los países
    /// </summary>
    private void fnCargarPaises()
    {
        ddlPais.DataSource = clsComun.fnLlenarDropPaises();
        ddlPais.DataBind();

        //ddlPais0.DataSource = clsComun.fnLlenarDropPaises();
        //ddlPais0.DataBind();
    }
    protected void btnActualizarDomicilio_Click(object sender, EventArgs e)
    {
        ScriptManager SM = ScriptManager.GetCurrent(this);
        SM.RegisterPostBackControl(btnActualizarDomicilio);
        //Validaciones
        if (string.IsNullOrEmpty(txtSucursal.Text)
            || string.IsNullOrEmpty(txtMunicipio.Text)
            || string.IsNullOrEmpty(txtCalle.Text)
            || string.IsNullOrEmpty(txtCodigoPostal.Text)
            || !clsComun.fnIsInt(txtCodigoPostal.Text)
            || txtCodigoPostal.Text.Length > 5
            )
        {
            clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }

        gDAL = new clsOperacionCuenta();
        datosUsuario = clsComun.fnUsuarioEnSesion();

        try
        {
            int retVal = gDAL.fnGuardarDatosFiscales(
                hdIdEstructura.Value,
                txtSucursal.Text,
                PAXCrypto.CryptoAES.EncriptaAES(txtCalle.Text),
                PAXCrypto.CryptoAES.EncriptaAES(txtNoExterior.Text),
                PAXCrypto.CryptoAES.EncriptaAES(txtNoInterior.Text),
                PAXCrypto.CryptoAES.EncriptaAES(txtColonia.Text),
                txtReferencia.Text,
                PAXCrypto.CryptoAES.EncriptaAES(txtLocalidad.Text),
                PAXCrypto.CryptoAES.EncriptaAES(txtMunicipio.Text),
                ddlEstado.SelectedValue,
                //PAXCrypto.CryptoAES.EncriptaAES(txtCodigoPostal.Text),
                PAXCrypto.CryptoAES.EncriptaAES(string.Format("{0:00000}", Convert.ToInt32(txtCodigoPostal.Text))),
                txtRegimenFiscal.Text);
            /////////
            gRFC = new clsOperacionRFC();
             byte[] Logo = { };
             if (fupLogo.FileName != "")
             {
                 //verificamos que el archivo de certificado público sea un .cer
                 if (Path.GetExtension(fupLogo.FileName).ToLower() != ".jpg")
                 {
                     clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarLogo);
                     return;
                 }
                 int psMaximo = Convert.ToInt32(clsComun.ObtenerParamentro("MaxFileLogo"));
                 System.Web.HttpPostedFile mifichero = fupLogo.PostedFile;
                 string path = mifichero.FileName;
                 double tamanoArchivo = (mifichero.ContentLength / 1024) / 1024;
                 if (tamanoArchivo > psMaximo)
                 {
                     clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarErrorLogo);
                     return;
                 }
                 else
                 {
                     Logo = fupLogo.FileBytes;
                 }

                 gRFC.fnActualizaLogo(Convert.ToInt32(hdIdRfc.Value), Logo);
             }

            //int retValex = 0;

            //if (!string.IsNullOrEmpty(txtMunicipioEmisor.Text) || !string.IsNullOrEmpty(txtCalleEmisor.Text) || !string.IsNullOrEmpty(txtCodigoPostalEmisor.Text))
            //{
            //    string sMuniciopio = string.Empty;
            //    string sCalle = string.Empty;
            //    string sCodigoPostal = string.Empty;

            //    if (!string.IsNullOrEmpty(txtMunicipioEmisor.Text))
            //        sMuniciopio = txtMunicipioEmisor.Text;

            //    if (!string.IsNullOrEmpty(txtCalleEmisor.Text))
            //        sCalle = txtCalleEmisor.Text;

            //    if (!string.IsNullOrEmpty(txtCodigoPostalEmisor.Text))
            //        sCodigoPostal = txtCodigoPostalEmisor.Text.PadLeft(5, '0');

            //        retValex = gDAL.fnGuardarDatosFiscalesEx(datosUsuario.id_contribuyente, sCalle,
            //        sMuniciopio, ddlEstado0.SelectedValue, ddlPais0.SelectedValue, sCodigoPostal);
            //}

            if (retVal != 0)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);

                clsComun.fnNuevaPistaAuditoria(
                    "webGlobalCuenta",
                    "fnGuardarDatosFiscales",
                    "Se modificó la sucursal matriz con los datos",
                    hdIdEstructura.Value,
                    txtSucursal.Text,
                    txtCalle.Text,
                    txtNoExterior.Text,
                    txtNoInterior.Text,
                    txtColonia.Text,
                    txtReferencia.Text,
                    txtLocalidad.Text,
                    txtMunicipio.Text,
                    ddlEstado.SelectedItem.Text,
                    txtCodigoPostal.Text
                    );
            }
            else
                throw new Exception("No se actualizó registro alguno.");
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

    protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sIdPais = (sender as DropDownList).SelectedValue;
        fnCargarEstados(sIdPais);
    }

    /// <summary>
    /// Método encargado de llenar el drop de los estados
    /// </summary>
    /// <param name="psIdPais"></param>
    private void fnCargarEstados(string psIdPais)
    {
        ddlEstado.DataSource = clsComun.fnLlenarDropEstados(psIdPais);
        ddlEstado.DataBind();

        //ddlEstado0.DataSource = clsComun.fnLlenarDropEstados(psIdPais);
        //ddlEstado0.DataBind();
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
    public void fnObtieneLogo()
    {
        try
        {
            gRFC = new clsOperacionRFC();
       
                MemoryStream ms = new MemoryStream(gRFC.fnObtenerImagenRFC(Convert.ToInt32(hdIdRfc.Value)));
                if (ms.Length > 0)
                {
                    ms.Position = 0;
                    this.EnableViewState = false;
                    byte[] fileData = null;
                    fileData = new byte[ms.Length + 1];
                    long bytesRead = ms.Read(fileData, 0, Convert.ToInt32(ms.Length));
                    Response.ContentType = "image/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + Guid.NewGuid().ToString() + ".jpg");
                    Response.Expires = 0;
                    Response.Buffer = true;
                    Response.Clear();
                    Response.BinaryWrite(fileData);
                    Response.End();
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarSinLogo);
                }
            }
   
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    protected void imgLogo_Click(object sender, ImageClickEventArgs e)
    {
        fnObtieneLogo();
    }
}