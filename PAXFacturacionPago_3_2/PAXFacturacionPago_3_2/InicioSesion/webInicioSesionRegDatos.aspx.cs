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
using System.Transactions;

public partial class InicioSesion_webInicioSesionRegDatos : System.Web.UI.Page
{
    private clsOperacionDistribuidores gOp;

    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable tblVersiones = new DataTable();

        if (!IsPostBack)
        {
            try
            {
                if (Session["objUsuario"] == null)
                {
                    Response.Redirect("~/InicioSesion/webInicioSesionLogin.aspx", false);
                }

                ddlPais.DataSource = clsComun.fnLlenarDropPaises();
                ddlPais.DataBind();

                ddlEstado.DataSource = clsComun.fnLlenarDropEstados(ddlPais.SelectedValue);
                ddlEstado.DataBind();

                //ddlPais0.DataSource = clsComun.fnLlenarDropPaises();
                //ddlPais0.DataBind();

                //ddlEstado0.DataSource = clsComun.fnLlenarDropEstados(ddlPais.SelectedValue);
                //ddlEstado0.DataBind();


                txtRFC.Focus();

                tblVersiones = clsInicioSesionRegistroDatos.fnRecuperaVersionesVigentes("");
                int id_version = Convert.ToInt32(tblVersiones.Rows[0]["id_version"]);
                drpVersion.DataSource = tblVersiones;
                drpVersion.DataBind();

                //ScriptManager SM = ScriptManager.GetCurrent(this);
                //SM.RegisterPostBackControl(btnActualizarDomicilio);

                //verificamos que el usuario sea un distribuidor
                clsInicioSesionUsuario datosUsuario = clsComun.fnUsuarioEnSesion();
                gOp = new clsOperacionDistribuidores();
                DataTable tblDistribuidor = new DataTable();
                tblDistribuidor = gOp.fnObtieneDistribuidoresporidUsuario(datosUsuario.id_usuario);
                ViewState["tblDistribuidor"] = tblDistribuidor;
                if (tblDistribuidor.Rows.Count > 0)
                {                    
                    CheckBox cbCertificado = new CheckBox();
                    cbCertificado.Checked = Convert.ToBoolean(tblDistribuidor.Rows[0]["certificado"]);
                    if (cbCertificado.Checked == false)
                    {
                        ViewState["Distribuidor"] = 0;
                        regxKey.ValidationGroup = "";
                        rfvKey.ValidationGroup = "";
                        valCer.ValidationGroup = "";
                        rfvCer.ValidationGroup = "";
                        rfvPass.ValidationGroup = "";
                        fupKey.Enabled = false;
                        fupCer.Enabled = false;
                        txtPass.Enabled = false;
                        txtNumDistribuidor.ReadOnly = true;
                    }
                    else
                    {
                        ViewState["Distribuidor"] = 1;
                        txtNumDistribuidor.ReadOnly = true;
                    }
                }
                DataTable dtInfoUsuario = new clsOperacionUsuarios().fnObtenerInfoUsuario(datosUsuario.id_usuario);
                txtRazonSocial.Text = dtInfoUsuario.Rows.Count > 0 ? dtInfoUsuario.Rows[0]["nombre"].ToString() : string.Empty;;
            }
            catch (Exception)
            {
                Response.Redirect("~/InicioSesion/webInicioSesionLogin.aspx",false);
            }


        }
    }


    protected void btnActualizarDomicilio_Click(object sender, EventArgs e)
    {

        System.Threading.Thread.Sleep(1000);

        clsValCertificado vValidadorCertificado = null;
        string resValidacion = string.Empty;

        clsConfiguracionPlantilla conf = new clsConfiguracionPlantilla();


        if (string.IsNullOrEmpty(txtRFC.Text.Trim()) ||
            string.IsNullOrEmpty(txtRazonSocial.Text.Trim()) ||
            string.IsNullOrEmpty(txtSucursal.Text.Trim()) ||
            string.IsNullOrEmpty(txtMunicipio.Text.Trim()) ||            
            string.IsNullOrEmpty(txtCalle.Text.Trim()) ||
            string.IsNullOrEmpty(txtCodigoPostal.Text.Trim()))
        {
            clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }


        if (!clsComun.fnValidaExpresion(txtRFC.Text, @"[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
        {
            clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.regxRFC);
            return;
        }
        byte[] Logo = { };
        // si es 0 quiere decir que no es un distribuidor de caso contrario se registra en el else
        int Distribuidor = Convert.ToInt32(ViewState["Distribuidor"]);
        DataTable tblDistribuidor = (DataTable)ViewState["tblDistribuidor"];
        if (tblDistribuidor.Rows.Count > 0)
        {
            if (Distribuidor != 0)
            {
                if (string.IsNullOrEmpty(txtPass.Text.Trim()))
                {
                    clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
                    return;
                }


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
                    //using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required, System.TimeSpan.MaxValue))
                    //{
                        try
                        {
                            //Generacion de Objetos.
                            clsInicioSesionUsuario datosUsuario = clsComun.fnUsuarioEnSesion();
                            datosUsuario.Actualizar();

                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "Revisa Certificados" + "|" + "Revisa que los certificados sean .cer y .key");

                            try
                            {
                                //realizamos las validaciones de SAT sobre el archivo
                                vValidadorCertificado = new clsValCertificado(fupCer.FileBytes);
                                vValidadorCertificado.LlavePrivada = new clsOperacionTimbradoSellado(fupKey.FileBytes, System.Text.Encoding.Unicode.GetBytes(txtPass.Text));
                                resValidacion = vValidadorCertificado.ValidarCertificado(txtRFC.Text.TrimStart(' '));
                                //Verificamos que el certificado del comprobante se de tipo CSD
                            }
                            catch (Exception)
                            {
                                //tran.Dispose();
                                clsComun.fnMostrarMensaje(this, "Revisa que los certificados sean .cer y .key");
                                return;
                            }



                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "ValidarCertificado" + "|" + "Revisa que el rfc corresponda al certificado" + "|" + txtRFC.Text);


                            if (!string.IsNullOrEmpty(resValidacion))
                            {
                                //tran.Dispose();
                                clsComun.fnMostrarMensaje(this, resValidacion);
                                return;
                            }
                            else
                            {
                                clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "solicitudRegistroContribuyente" + "|" + "Guarda los datos del contribuyente." + "|" + txtRFC.Text);

                            }
                        }
                        catch (Exception ex)
                        {
                            //tran.Dispose();
                            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                            //Response.Redirect("webInicioSesionLogin.aspx");
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
                            return;
                        }

                        try
                        {
                            clsInicioSesionUsuario datosUsuario = clsComun.fnUsuarioEnSesion();
                            clsCancelacionSAT cancelacion = new clsCancelacionSAT();
                            byte[] certPFx = cancelacion.crearPfx(fupCer.FileBytes, fupKey.FileBytes, txtPass.Text);

                            //certPFx = Utilerias.Encriptacion.DES3.Encriptar(certPFx);
                            certPFx = PAXCrypto.CryptoAES.EncriptaAESB(certPFx);

                            int id_version = Convert.ToInt32(drpVersion.SelectedValue);


                            if (fupLogo.FileName != "")
                            {
                                //verificamos que el archivo de logo .jpg
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
                            }

                            if (!clsInicioSesionRegistroDatos.solicitudRegistroContribuyenteDistribuidor(txtRFC.Text.TrimStart(' '),
                            txtRazonSocial.Text, txtSucursal.Text, datosUsuario.id_contribuyente,
                            Convert.ToInt32(ddlEstado.SelectedValue), PAXCrypto.CryptoAES.EncriptaAES(txtMunicipio.Text), 
                            PAXCrypto.CryptoAES.EncriptaAES(txtLocalidad.Text), PAXCrypto.CryptoAES.EncriptaAES(txtCalle.Text),
                            PAXCrypto.CryptoAES.EncriptaAES(txtNoExterior.Text), PAXCrypto.CryptoAES.EncriptaAES(txtNoInterior.Text), 
                            PAXCrypto.CryptoAES.EncriptaAES(txtColonia.Text), txtReferencia.Text, PAXCrypto.CryptoAES.EncriptaAES(txtCodigoPostal.Text.PadLeft(5, '0')),
                            datosUsuario.id_usuario, id_version, Logo, txtRegimenFiscal.Text, vValidadorCertificado.fnEncriptarCertificado(), 
                            vValidadorCertificado.LlavePrivada.fnEncriptarLlave(), null, vValidadorCertificado.fnEncriptarPassword(),
                            vValidadorCertificado.Certificado.NotBefore, vValidadorCertificado.Certificado.NotAfter, certPFx))
                            {
   
                            }
                            else
                            {
                                if (fupLogo.FileName != "")
                                {
                                    int idEstructura = conf.fnRecuperaEstructura(datosUsuario.id_usuario);
                                    conf.fnActualizaPlantilla(0, 2, idEstructura, "Black", datosUsuario.id_usuario);
                                }
                                else
                                {
                                    int idEstructura = conf.fnRecuperaEstructura(datosUsuario.id_usuario);
                                    conf.fnActualizaPlantilla(0, 1, idEstructura, "Black", datosUsuario.id_usuario);
                                }
                            }
                            //tran.Complete();
                        }
                        catch (Exception)
                        {
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varPFX);
                            return;
                        }
                        finally
                        {
                            //tran.Dispose();
                        }

                        Response.Redirect("webInicioSesionCambiarPWD.aspx", false);
                    //}
                }
            }
            else
            {
                //using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required, System.TimeSpan.MaxValue))
                //{
                    try
                    {
                        clsInicioSesionUsuario datosUsuario = clsComun.fnUsuarioEnSesion();
                        int id_version = Convert.ToInt32(drpVersion.SelectedValue);


                        if (fupLogo.FileName != "")
                        {
                            //verificamos que el archivo de logo .jpg
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
                        }

                        
                        if (!clsInicioSesionRegistroDatos.solicitudRegistroContribuyenteDistribuidor(txtRFC.Text.TrimStart(' '),
                         txtRazonSocial.Text, txtSucursal.Text, datosUsuario.id_contribuyente,
                         Convert.ToInt32(ddlEstado.SelectedValue), PAXCrypto.CryptoAES.EncriptaAES(txtMunicipio.Text), 
                         PAXCrypto.CryptoAES.EncriptaAES(txtLocalidad.Text), PAXCrypto.CryptoAES.EncriptaAES(txtCalle.Text),
                         PAXCrypto.CryptoAES.EncriptaAES(txtNoExterior.Text), PAXCrypto.CryptoAES.EncriptaAES(txtNoInterior.Text), 
                         PAXCrypto.CryptoAES.EncriptaAES(txtColonia.Text), txtReferencia.Text, PAXCrypto.CryptoAES.EncriptaAES(txtCodigoPostal.Text.PadLeft(5, '0')),
                         datosUsuario.id_usuario, id_version, Logo, txtRegimenFiscal.Text, null, null, null, null , DateTime.Today, DateTime.Today, null))
                        {
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
                            return;
                        }
                        else
                        {

                        }
                        //tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
                        return;
                    }
                    finally
                    {
                        //tran.Dispose();
                    }
                    Response.Redirect("webInicioSesionCambiarPWD.aspx", false);
                //}
            }
        }
        else
        {
            if (string.IsNullOrEmpty(txtPass.Text.Trim()))
            {
                clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
                return;
            }


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
                //using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required, System.TimeSpan.MaxValue))
                //{
                    try
                    {
                        //Generacion de Objetos.
                        clsInicioSesionUsuario datosUsuario = clsComun.fnUsuarioEnSesion();
                        datosUsuario.Actualizar();

                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "Revisa Certificados" + "|" + "Revisa que los certificados sean .cer y .key");

                        try
                        {
                            //realizamos las validaciones de SAT sobre el archivo
                            vValidadorCertificado = new clsValCertificado(fupCer.FileBytes);
                            vValidadorCertificado.LlavePrivada = new clsOperacionTimbradoSellado(fupKey.FileBytes, System.Text.Encoding.Unicode.GetBytes(txtPass.Text));
                            resValidacion = vValidadorCertificado.ValidarCertificado(txtRFC.Text.TrimStart(' '));
                            //Verificamos que el certificado del comprobante se de tipo CSD
                        }
                        catch (Exception)
                        {
                            //tran.Dispose();
                            clsComun.fnMostrarMensaje(this, "Revisa que los certificados sean .cer y .key");
                            return;
                        }



                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "ValidarCertificado" + "|" + "Revisa que el rfc corresponda al certificado" + "|" + txtRFC.Text);


                        if (!string.IsNullOrEmpty(resValidacion))
                        {
                            //tran.Dispose();
                            clsComun.fnMostrarMensaje(this, resValidacion);
                            return;
                        }
                        else
                        {
                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "solicitudRegistroContribuyente" + "|" + "Guarda los datos del contribuyente." + "|" + txtRFC.Text);

                        }
                    }
                    catch (Exception ex)
                    {
                        //tran.Dispose();
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                        //Response.Redirect("webInicioSesionLogin.aspx");
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
                        return;
                    }

                    try
                    {
                        clsInicioSesionUsuario datosUsuario = clsComun.fnUsuarioEnSesion();
                        clsCancelacionSAT cancelacion = new clsCancelacionSAT();
                        byte[] certPFx = cancelacion.crearPfx(fupCer.FileBytes, fupKey.FileBytes, txtPass.Text);

                        //certPFx = Utilerias.Encriptacion.DES3.Encriptar(certPFx);                        
                        certPFx = PAXCrypto.CryptoAES.EncriptaAESB(certPFx);

                        int id_version = Convert.ToInt32(drpVersion.SelectedValue);


                        if (fupLogo.FileName != "")
                        {
                            //verificamos que el archivo de logo .jpg
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
                        }


                        if (!clsInicioSesionRegistroDatos.solicitudRegistroContribuyenteCobro(txtRFC.Text.TrimStart(' '), txtRazonSocial.Text, txtSucursal.Text, datosUsuario.id_contribuyente,
                            Convert.ToInt32(ddlEstado.SelectedValue), PAXCrypto.CryptoAES.EncriptaAES(txtMunicipio.Text), 
                            PAXCrypto.CryptoAES.EncriptaAES(txtLocalidad.Text), vValidadorCertificado.LlavePrivada.fnEncriptarLlave(),
                            vValidadorCertificado.fnEncriptarCertificado(), null, vValidadorCertificado.Certificado.NotBefore, vValidadorCertificado.Certificado.NotAfter, vValidadorCertificado.fnEncriptarPassword(),
                            PAXCrypto.CryptoAES.EncriptaAES(txtCalle.Text), PAXCrypto.CryptoAES.EncriptaAES(txtNoExterior.Text), 
                            PAXCrypto.CryptoAES.EncriptaAES(txtNoInterior.Text), PAXCrypto.CryptoAES.EncriptaAES(txtColonia.Text), txtReferencia.Text, 
                            PAXCrypto.CryptoAES.EncriptaAES(txtCodigoPostal.Text.PadLeft(5, '0')), datosUsuario.id_usuario, certPFx, id_version, Logo, txtRegimenFiscal.Text))
                        {
                            gOp = new clsOperacionDistribuidores();
                            int idDistID = Convert.ToInt32(ViewState["idDistID"]);
                            if (idDistID != 0)
                            {
                                gOp.fnEliminaDistribuidorRelacion(idDistID, datosUsuario.id_usuario);
                            }
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
                            return;
                        }
                        else
                        {
                            ////Registrar los datos de la
                            //if (!string.IsNullOrEmpty(txtMunicipioEmisor.Text) || !string.IsNullOrEmpty(txtCalleEmisor.Text) || !string.IsNullOrEmpty(txtCodigoPostalEmisor.Text))
                            //{
                            //    string sMuniciopio = string.Empty;
                            //    string sCalle = string.Empty;
                            //    string sCodigoPostal = string.Empty;

                            //    if (!string.IsNullOrEmpty(txtMunicipioEmisor.Text))
                            //        sMuniciopio = txtMunicipioEmisor.Text;

                            //    if (!string.IsNullOrEmpty(txtCalleEmisor.Text))
                            //        sCalle = txtMunicipioEmisor.Text;

                            //    if (!string.IsNullOrEmpty(txtCodigoPostalEmisor.Text))
                            //        sCodigoPostal = txtCodigoPostalEmisor.Text.PadLeft(5, '0');

                            //    clsInicioSesionRegistroDatos.fnRegistraDomicilioEx(datosUsuario.id_contribuyente,
                            //        Convert.ToInt32(ddlEstado0.SelectedValue),
                            //        Convert.ToInt32(ddlPais0.SelectedValue),
                            //        sMuniciopio,
                            //        sCalle,
                            //        sCodigoPostal);                            
                            //}

                            //insertamos la relacion del usuario con el distribuidor si es que dependera de un distribuidor
                            if (txtNumDistribuidor.Text != "" || txtNumDistribuidor.Text != string.Empty)
                            {
                                gOp = new clsOperacionDistribuidores();
                                DataTable dtDistVal = new DataTable();

                                dtDistVal = gOp.fnObtieneDistribuidoresporNumero(txtNumDistribuidor.Text);
                                int idDistID = 0;
                                ViewState["idDistID"] = idDistID;
                                if (dtDistVal.Rows.Count > 0)
                                {
                                    idDistID = Convert.ToInt32(dtDistVal.Rows[0]["id_distribuidor"]);
                                    ViewState["idDistID"] = idDistID;
                                    //clsInicioSesionUsuario datosUsuario = clsComun.fnUsuarioEnSesion();
                                    gOp.fnInsertaDistribuidorRelacion(idDistID, datosUsuario.id_usuario, "R", "A", DateTime.Now);
                                }
                                else
                                {
                                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarErrorDistVal);
                                    return;
                                }
                            }
                            if (fupLogo.FileName != "")
                            {
                                int idEstructura = conf.fnRecuperaEstructura(datosUsuario.id_usuario);
                                conf.fnActualizaPlantilla(0, 2, idEstructura, "Black", datosUsuario.id_usuario);
                            }
                            else
                            {
                                int idEstructura = conf.fnRecuperaEstructura(datosUsuario.id_usuario);
                                conf.fnActualizaPlantilla(0, 1, idEstructura, "Black", datosUsuario.id_usuario);
                            }
                        }
                        //tran.Complete();
                    }
                    catch (Exception)
                    {
                        clsComun.fnMostrarMensaje(this, "Verificar archivo PFX.");
                        return;
                    }
                    finally
                    {
                        //tran.Dispose();
                    }

                    Response.Redirect("webInicioSesionCambiarPWD.aspx", false);
                //}
            }
        }


    }

    protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlEstado.DataSource = clsComun.fnLlenarDropEstados(ddlPais.SelectedValue );
        ddlEstado.DataBind(); 
    }
    public void Page_Error(object sender, EventArgs e)
    {
        Exception objErr = Server.GetLastError().GetBaseException();

        if (!string.IsNullOrEmpty(objErr.Message))
        {
            Server.ClearError();
            Response.Redirect("~/webGlobalError.aspx", false);
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

    protected void drpVersion_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (drpVersion.SelectedItem.Text)
        {
            case "3.0":
             lblRegimenFiscal.Visible = false;
             txtRegimenFiscal.Visible = false;
             rfvRegimenFiscal.EnableClientScript = false;
             rfvRegimenFiscal.Visible = false;
                break;
            case "3.2":
             lblRegimenFiscal.Visible = true;
             txtRegimenFiscal.Visible = true;
             rfvRegimenFiscal.EnableClientScript = true;
             rfvRegimenFiscal.Visible = true;
            break;
        }
        //if (drpVersion.SelectedItem.Text == "3.2")
        //{
        //    tbl3_2.Visible = true; 

        //}
    }
}
