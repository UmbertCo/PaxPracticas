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
using System.Threading;
using System.Globalization;
using System.Drawing;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.Security;
using System.Xml.Xsl;
using System.IO;

public partial class Default2 : System.Web.UI.Page
{
    private clsOperacionTimbradoSellado gTimbrado;
    private clsValCertificado gCertificado;
    protected DataTable dtCreditos;
    private clsEnvioCorreoDocs cEd;
    string sid_cfd = string.Empty;

    //int nidSucursalFis = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        string sSelcFlash = string.Empty;

        sSelcFlash = this.Request.QueryString["sVal"];

        try
        {
            if (!string.IsNullOrEmpty(sSelcFlash))
                ViewState["SelecFlash"] = sSelcFlash;

            //imgLogo.Visible = false;
            //imgTicket.Visible = false;
            //ltlLeyendaCancelacion.Visible = false;
            //ltlLeyendaCancelacion.Text = string.Empty;
            ltlLeyendaCancelacion.Text = Resources.resCorpusCFDIEs.lblleyendaCancelacion + " " + Convert.ToString(Session["sTelfoCan"]) + " " +
                             Resources.resCorpusCFDIEs.lblleyendaCancelacionCorreo + " " + Convert.ToString(Session["sCorreoCan"]);
            imgpublic.ImageUrl = "~/Imagen.aspx?" + "&Ubic=3";

            if (!IsPostBack)
            {
                ver_menu();
                //string sUrl = Request.Url.ToString();
                //if (sUrl.EndsWith("Default.aspx") || sUrl.EndsWith("/") || sUrl.EndsWith("default.aspx") || sUrl.Contains("default.aspx"))
                //{
                //    //Session.Remove("id_estructura");
                //}
                //else
                //{
                    fnMenuClik(sSelcFlash, sender, null);
                //}

                //if (Session["id_estructura"] != null)
                //{
                //    imgTicket.ImageUrl = "~/ImagenTicket.aspx?estructura=" + Convert.ToString(Session["id_estructura"]);

                //}
                //else
                //{
                //    imgTicket.ImageUrl = "~/ImagenTicket.aspx?estructura=1";

                //}

                //imgTicket.ImageUrl = "~/ImagenTicket.aspx?estructura=1";

                //imgTicket.Visible = true;

                //***********************************************************
                // Se agrego la Imagen de Fondo desde un Tema Dinamico(Ivan Hernandez)
                //byfondo.Style.Add("background-image", "Imagenes/back.jpg");
                //***********************************************************
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX");
            }
        }
        else
        {
            string language = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
        }
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        clsInicioSesionUsuario datosUsuario = clsComun.fnUsuarioEnSesion();

        int nid_usuario = 0;
        if (datosUsuario != null)
            nid_usuario = datosUsuario.id_usuario;

        string sUrlImg = (String)Session["url"];
        //pnlConsulta.Visible = false;
        DataTable tablaTicket = new DataTable();
        //clsOperacionCuenta gDAL = new clsOperacionCuenta();
        //DataTable certificado = new DataTable();
        //string sRFCEmisor = string.Empty;

        clsPistasAuditoria.fnGenerarPistasAuditoria(nid_usuario, DateTime.Now, this.Title + "|" + "22.-OBTIENE TICKET" + "|" + "Realizamos la busqueda del Ticket");

        ////Evaluamos que se haya seleccionado la tienda
        //if (string.IsNullOrEmpty(Convert.ToString(Session["id_estructura"])))
        //{
        //    lblErrorLog.Text = Resources.resCorpusCFDIEs.lblMenu; //SELECCIONE SU TIENDA
        //    mpeErrorLog.Show();
        //    return;
        //}


   

        int idTienda = Convert.ToInt32(clsComun.fnObteneridSucursal(txtTienda.Text));
        Session["id_estructura"] = clsComun.fnObteneridPadreSucursal(idTienda.ToString());
        

        //Buscamos el Ticket 
        //string Formato24 = "yyyddmm HH:mm:ss";
        DateTime fecha = Convert.ToDateTime(txtFecha.Text);
        //string sFecha = fecha.ToString("yyyMMdd HH:mm:ss");
        //string sFecha = fecha.ToString("yyyMMdd HH:mm");
        string sFecha = fecha.ToString("yyyMMdd");

        //String.Format("{0:u}",fecha); //.ToString("yyy dd mm HH:mm:ss"));
        tablaTicket = clsComun.fnRecuperaTicket(idTienda, Convert.ToInt32(txtTicket.Text), txtTienda.Text, txtTotal.Text, sFecha);
        //fnObtenerTicket(Convert.ToInt32(txtTicket.Text), txtTienda.Text, txtTotal.Text, Convert.ToDateTime(txtFecha.Text));

        if (tablaTicket.Rows.Count > 0 && idTienda > 0 )//Si existe Ticket
        {
            clsPistasAuditoria.fnGenerarPistasAuditoria(nid_usuario, DateTime.Now, this.Title + "|" + "22.-OBTIENE TICKET" + "|" + "Obtenemos el ticket para el timbrado");

            /////Aplicamos la regla de negocio
            //bool bEvaluacion = clsComun.fnRegladenegocio(tablaTicket, idTienda);
            //if (bEvaluacion == false)
            //{
            //    lblErrorLog.Text = Resources.resCorpusCFDIEs.vrTicketmsj; //"El Ticket no se encuetra disponible";
            //    mpeErrorLog.Show();
            //    return;
            //}

            /////Validamos que el RFC se ecuentre en estatus A
            //if (clsComun.fnValidarRFC(idTienda) == false)
            //{
            //    lblErrorLog.Text = Resources.resCorpusCFDIEs.lblSucmsj; //"La Sucursal tiene problemas, pongase en contacto con el proveedor";
            //    mpeErrorLog.Show();
            //    //clsComun.fnMostrarMensaje(this, "El Ticket no puede ser Facturado, pongase en contacto con el proveedor", Resources.resCorpusCFDIEs.varContribuyente);
            //    return;
            //}

            /////Checamos que el Emisor Cuente con Timbres
            //if (clsComun.fnValidarTimbresDisponibles(idTienda) == 0)
            //{
            //    lblErrorLog.Text = Resources.resCorpusCFDIEs.vrTimbremsj;// "No es posible realizar el comprobante fiscal, Contacte a su proveedor";
            //    mpeErrorLog.Show();
            //    return;
            //}


            clsPistasAuditoria.fnGenerarPistasAuditoria(nid_usuario, DateTime.Now, this.Title + "|" + "22.-OBTIENE TICKET" + "|" + "Validaciones exitosas");


            //Carga ticket
            //docTicket.LoadXml(tablaTicket.Rows[0]["xml_ticket"].ToString());
            ViewState["id_ticket"] = Convert.ToInt32(tablaTicket.Rows[0]["id_ticket"]);
            Session["id_cfd"] = tablaTicket.Rows[0]["id_cfd"].ToString();
            ViewState["estatus"] = tablaTicket.Rows[0]["estatus"].ToString();

            sid_cfd = Convert.ToString(Session["id_cfd"]);

            ViewState["TablaTicket"] = tablaTicket; //Almacenamos en variable el Ticket

            if (tablaTicket.Rows[0]["estatus"].ToString() == "F")
            {
                //clsComun.fnMostrarMensaje(this, "Ticket timbrado", Resources.resCorpusCFDIEs.varContribuyente);
                string sTipoCom = clsComun.ObtenerParamentro("TipoComprobante");
                hpPDF.NavigateUrl = "~/Consultas/webConsultaPDF.aspx?idcfd=" + sid_cfd;
                hpXML.NavigateUrl = "~/Consultas/webConsultasGeneradorXML.aspx?idcfd=" + sid_cfd;
                mpeConsultaCFDI.Show();
                limpiar(this);
                //mpeConsultadeCFDI.Show();
                //pnlConsulta.Visible = true;
                return;
            }

            btnAcepDatos.Enabled = true;
            mpeDatosRecetor.Show();//Capturamos los datos del Emisor
            return;
        }
        else
        {
            lblErrorLog.Text = Resources.resCorpusCFDIEs.lblDatosTicket; //"Verifique los datos del ticket esten correctos y correspondan ala tienda seleccionada";
            mpeErrorLog.Show();//clsComun.fnMostrarMensaje(this, "Datos de Ticket invalidos ", Resources.resCorpusCFDIEs.varContribuyente);
            return;
        }

    }//Fin Del Click Factura..

    protected void imgbtnLupa_Click(object sender, ImageClickEventArgs e)
    {
        ViewState["dtDatosReceptor"] = fnBuscaReceptor(Convert.ToInt32(Session["id_estructura"]));
    }

    protected void btnAcepDatos_Click(object sender, EventArgs e)
    {
        btnAcepDatos.Enabled = false;
        lblLeyCorreo.Text = string.Empty;

        clsInicioSesionUsuario datosUsuario = clsComun.fnUsuarioEnSesion();
       

        int nid_usuario = 0;
        if (datosUsuario != null)
            nid_usuario = datosUsuario.id_usuario;

        DataTable dAuxiliar = (DataTable)ViewState["dtDatosReceptor"];
        //if (dAuxiliar.Rows.Count > 0)
        //{
        try
        {
            string sUrlImg = (String)Session["url"];
            ///Recuperamos los datos del Emisor
            clsValCertificado vValidadorCertificado = null;
            clsTimbradoFuncionalidad timbrar = new clsTimbradoFuncionalidad();
            XmlDocument xDocTimbrado = new XmlDocument();
            SqlDataReader sdrInfo = null;
            clsOperacionCuenta gDAL = new clsOperacionCuenta();
            DataTable certificado = new DataTable();
            string resValidacion = string.Empty;
            string sRFCEmisor = string.Empty;
            string sRegimenFiscal = string.Empty;
            string sSerie = string.Empty;
            string sFolio = string.Empty;
            int retornoInsert = 0;

            //obtener parametros 
            string sVersion = clsComun.ObtenerParamentro("Version");
            string sTipoComprobante = clsComun.ObtenerParamentro("TipoComprobante");
            string sMoneda = clsComun.ObtenerParamentro("Moneda");
            string sMetodoDePago = clsComun.ObtenerParamentro("MetodoDePago");
            string sdescFormaPago = clsComun.ObtenerParamentro("FormaDePago");

            //Obtenemos la estructura de la sucursal
            int nId_estructura = Convert.ToInt32(Session["id_estructura"]);

            try //control de exepciones de certificado
            {
                //Auditoria
                clsPistasAuditoria.fnGenerarPistasAuditoria(nid_usuario, DateTime.Now, this.Title + "|" + "2.-ObtenerDatosFiscalesMatriz" + "|" + "Se recuperan los Datos fiscales de la matriz");

                ///Asignamos los datos del Emisor al SqlDataReader
                sdrInfo = gDAL.fnObtenerDatosFiscalesMatriz(nId_estructura, txtTienda.Text);//(SqlDataReader)Session["DatosDeEmisor"];

                if (sdrInfo != null && sdrInfo.HasRows && sdrInfo.Read())
                {
                    //Obtener datos del emisor, llave, certificado, y password
                    certificado = clsTimbradoFuncionalidad.ObtenerCertificado(Convert.ToInt32(sdrInfo["id_rfc"].ToString()));

                    if (clsTimbradoFuncionalidad.fnReplaceCaracters(sdrInfo["rfc"].ToString()) != string.Empty)
                        sRFCEmisor = clsTimbradoFuncionalidad.fnReplaceCaracters(sdrInfo["rfc"].ToString());

                    //Auditoria
                    clsPistasAuditoria.fnGenerarPistasAuditoria(nid_usuario, DateTime.Now, this.Title + "|" + "2.-ObtenerCertificado" + "|" + "Se recuperan los certificados del emisor");

                    //Se verifica que el certificado se recupere de la base de datos
                    if (certificado == null || certificado.Rows.Count < 1)
                    {
                        lblErrorLog.Text = Resources.resCorpusCFDIEs.msgNoCertificado;// "No se recupero el certificado para la generacion, intente de nuevo.";
                        mpeErrorLog.Show();
                        //clsComun.fnMostrarMensaje(this, "No se pudo recuperar el certificado.");
                        return;
                    }
                    sRegimenFiscal = sdrInfo["regimen_fiscal"].ToString();
                    //Auditoria
                    clsPistasAuditoria.fnGenerarPistasAuditoria(nid_usuario, DateTime.Now, this.Title + "|" + "2.-ObtenerDatosFiscalesMatriz" + "|" + "Se recuperan los Datos fiscales de la matriz-exitosa");
                }
            }
            catch (Exception ex)//Fin Catch de control de excepciones de certificado
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                lblErrorLog.Text = Resources.resCorpusCFDIEs.msgNoCertificado;
                mpeErrorLog.Show();
                return;
            }

            byte[] bLlave = (byte[])certificado.Rows[0]["key"];
            byte[] bCertificado = (byte[])certificado.Rows[0]["certificado"];
            string sPassword = certificado.Rows[0]["password"].ToString();

            ////realizamos las validaciones de SAT sobre el archivo
            //vValidadorCertificado = new clsValCertificado(bCertificado);
            //if (sRFCEmisor != vValidadorCertificado.VerificarExistenciaCertificado())
            //    resValidacion = Resources.resCorpusCFDIEs.valEmisionCer;
            //if (!vValidadorCertificado.ComprobarFechas())
            //    resValidacion = Resources.resCorpusCFDIEs.valFechaCer;
            //if (!vValidadorCertificado.fnCSD308())
            //    resValidacion = Resources.resCorpusCFDIEs.varCSD308;

            /////Auditoria
            //clsPistasAuditoria.fnGenerarPistasAuditoria(nid_usuario, DateTime.Now, this.Title + "|" + "3.-vValidadorCertificado" + "|" + "Se validan los certificados del emisor");

            //if (!string.IsNullOrEmpty(resValidacion))
            //{
            //    lblAviso.Text = resValidacion;
            //    mpeAvisos.Show();
            //    //clsComun.fnMostrarMensaje(this, resValidacion);
            //    return;
            //}
            //else
            //{
            //nidSucursalFis = Convert.ToInt32(Session["id_estructura"]);
            ////Preparamos los objetos de manejo tanto de la llave como del certificado
            gTimbrado = new clsOperacionTimbradoSellado(bLlave, sPassword);
            gCertificado = new clsValCertificado(bCertificado);

            ///Auditoria
            //clsPistasAuditoria.fnGenerarPistasAuditoria(nid_usuario, DateTime.Now, this.Title + "|" + "4.-gCertificado" + "|" + "Preparamos los objetos de manejo tanto de la llave como del certificado");

            Comprobante cfd = new Comprobante();

            //se verifica si la sucursal cuenta con serie y folio
            DataTable dtSeriesFolios = clsComun.fnObtenerSerieFolio(nId_estructura, txtTienda.Text);



            if (dtSeriesFolios.Rows.Count > 0)
            {
                sSerie = dtSeriesFolios.Rows[0]["serie"].ToString();
                sFolio = dtSeriesFolios.Rows[0]["folio"].ToString();
            }

            switch (sVersion)
            {
                case "3.2":
                    cfd = timbrar.fnObtenerXML3_2(
                                            sVersion,
                                            sTipoComprobante,
                                            sMoneda,
                                            txtRfcReceptor.Text,
                                            txtRazonReceptor.Text,
                                            clsTimbradoFuncionalidad.fnReplaceCaracters(txtPais.Text),
                                            clsTimbradoFuncionalidad.fnReplaceCaracters(txtEstado.Text),
                                            clsTimbradoFuncionalidad.fnReplaceCaracters(txtMunicipio.Text),
                                            clsTimbradoFuncionalidad.fnReplaceCaracters(txtLocalidad.Text),
                                            clsTimbradoFuncionalidad.fnReplaceCaracters(txtCalle.Text),
                                            clsTimbradoFuncionalidad.fnReplaceCaracters(txtNoExterior.Text),
                                            clsTimbradoFuncionalidad.fnReplaceCaracters(txtNoInterior.Text),
                                            clsTimbradoFuncionalidad.fnReplaceCaracters(txtColonia.Text),
                                            clsTimbradoFuncionalidad.fnReplaceCaracters(txtCodigoPostal.Text),
                                            sRegimenFiscal,
                                            sMetodoDePago,
                                            bCertificado,
                                            sdescFormaPago,
                        //nidSucursalFis,
                                            (DataTable)ViewState["TablaTicket"],
                                            this.Title,
                                            nid_usuario,
                                            nId_estructura,
                                            txtTienda.Text,
                                            sSerie,
                                            sFolio);
                    break;
            }

            ////Generamos el primer XML necesario para generar la cadena original
            switch (sVersion)
            {
                case "3.2":
                    xDocTimbrado = gTimbrado.fnGenerarXML32(cfd, null);
                    break;
            }

            ///Auditoria
            clsPistasAuditoria.fnGenerarPistasAuditoria(nid_usuario, DateTime.Now, this.Title + "|" + "8.-fnGenerarXML" + "|" + "Se contruye XML con los datos generados.");

            string sCadenaOriginal = string.Empty;
            string sCOriginal = String.Empty;
            string noCertificadoPAC = string.Empty;
            string selloPAC = string.Empty;
            //int retornoInsert = 0;

            string scadena = "";
            switch (sVersion)
            {
                case "3.2":
                    scadena = "cadenaoriginal_3_2";
                    break;
            }

            //Generamos la cadena original
            XPathNavigator navNodoTimbre = xDocTimbrado.CreateNavigator();
            //sCadenaOriginal = gTimbrado.fnConstruirCadenaTimbrado(navNodoTimbre, scadena); //"cadenaoriginal_3_2"); 

            XslCompiledTransform xslt;
            XsltArgumentList args;
            MemoryStream ms;
            StreamReader srDll;

            switch (sVersion)
            {
                case "3.2":

                    // Load the type of the class.
                    xslt = new XslCompiledTransform();
                    xslt.Load(typeof(CaOri.V32));

                    ms = new MemoryStream();
                    args = new XsltArgumentList();

                    xslt.Transform(xDocTimbrado.CreateNavigator(), args, ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    srDll = new StreamReader(ms);

                    sCadenaOriginal = srDll.ReadToEnd();

                    break;
            }

            ///Auditoria
            clsPistasAuditoria.fnGenerarPistasAuditoria(nid_usuario, DateTime.Now, this.Title + "|" + "9.-fnConstruirCadenaTimbrado" + "|" + "Generamos la cadena original." + sCadenaOriginal);

            ///Generamos el Sello
            switch (sVersion)
            {
                case "3.2":
                    cfd.sello = gTimbrado.fnGenerarSello(sCadenaOriginal, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1, true);
                    break;
            }

            ///Auditoria
            clsPistasAuditoria.fnGenerarPistasAuditoria(nid_usuario, DateTime.Now, this.Title + "|" + "9.1.-fnConstruirCadenaTimbrado" + "|" + "Generamos SELLO." + cfd.sello);



            xDocTimbrado = gTimbrado.fnGenerarXML32(cfd, null); //Agrega el sello 

            //Generar el Hash para revisar si no hay uno existente en la BD
            string HASHEmisor = clsEnvioSAT.GetHASH(sCadenaOriginal).ToUpper();

            ///Busca el HASH Emisor
            if (clsTimbradoFuncionalidad.fnBuscarHashComprobantes(nid_usuario, HASHEmisor, "Emisor"))
            {
                lblErrorLog.Text = Resources.resCorpusCFDIEs.msgNoCompGenerado;
                mpeErrorLog.Show();
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado, Resources.resCorpusCFDIEs.varContribuyente);
                return;
            }
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);
            //string sVersionCompro = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Version").ToString();
            ///Instancia la Referencia Web para enviar a Timbrar el XML Final 
            wsRecepcionTASMX.wcfRecepcionASMXSoapClient wsRecepcionTASMX = new wsRecepcionTASMX.wcfRecepcionASMXSoapClient();

            //Session["id_estructura"] = clsComun.fnObteneridSucursal(txtTienda.Text);
            //int idTienda = Convert.ToInt32(Session["id_estructura"]);

            ///Recuperamos el idEstructura de la pagina de cobro
            DataTable dtDatosdeUsuarioCobro = clsComun.fnRecuperaridestructura(nId_estructura, 0);

            int nidEstructuraCobro = Convert.ToInt32(dtDatosdeUsuarioCobro.Rows[0]["idestructuraCobro"].ToString());
            string sUsuarioCobro = dtDatosdeUsuarioCobro.Rows[0]["clave_usuario"].ToString();
            string sPassWordCobroWSDL = Utilerias.Encriptacion.Base64.EncriptarBase64(dtDatosdeUsuarioCobro.Rows[0]["password"].ToString());

            clsPistasAuditoria.fnGenerarPistasAuditoria(nid_usuario, DateTime.Now, this.Title + "|" + "10.- Envia al webServices el xml nidEstructuraCobro=" + nidEstructuraCobro) ;
            clsPistasAuditoria.fnGenerarPistasAuditoria(nid_usuario, DateTime.Now, this.Title + "|" + "10.- Envia al webServices el xml sUsuarioCobro=" + sUsuarioCobro);
            clsPistasAuditoria.fnGenerarPistasAuditoria(nid_usuario, DateTime.Now, this.Title + "|" + "10.- Envia al webServices el xml sPassWordCobroWSDL=" + sPassWordCobroWSDL);

            ///Auditoria
            clsPistasAuditoria.fnGenerarPistasAuditoria(nid_usuario, DateTime.Now, this.Title + "|" + "10.- Envia al webServices el xml");

            string sXML = wsRecepcionTASMX.fnEnviarXML(xDocTimbrado.DocumentElement.OuterXml, sTipoComprobante, nidEstructuraCobro, sUsuarioCobro, sPassWordCobroWSDL, sVersion);
            //string sXML = wsRecepcionTASMX.fnEnviarXML(xDocTimbrado.DocumentElement.OuterXml, sTipoComprobante, nidEstructuraCobro, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==", sVersion);
            ///Validacion de respuesta
            char[] cCad = { '-' };
            string[] sCad = sXML.Split(cCad);

            if (sCad.Length <= 3)
            {
                string smensajefinal = string.Empty;
                //Recorre el string para armar el msj
                for (int n = 0; n < sCad.Length; n++)
                {
                    string sMensaje = sCad[n];
                    smensajefinal += sMensaje + " ";
                }

                //En caso de marcar error al timbrar se muestra msj
                lblErrorLog.Text = smensajefinal;
                mpeErrorLog.Show();
                //clsComun.fnMostrarMensaje(this, smensajefinal);
                return;

            }
            else
            {
                //Si el xml fue timbrado con exito
                XmlDocument XMLTimbrado = new XmlDocument();

                XMLTimbrado.LoadXml(sXML);

                XmlNamespaceManager nsmComprobanteT = new XmlNamespaceManager(XMLTimbrado.NameTable);
                nsmComprobanteT.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComprobanteT.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                XPathNavigator navEncabezado = XMLTimbrado.CreateNavigator();

                string snombreDoc = string.Empty;
                try { snombreDoc = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobanteT).Value; }
                catch { snombreDoc = Guid.NewGuid().ToString(); }

                ///Auditoria
                clsPistasAuditoria.fnGenerarPistasAuditoria(nid_usuario, DateTime.Now, this.Title + "|" + "11.-GenerarTimbre del Web Services" + "|" + "Generamos el sello del SAT, se lo agregamos al objeto.");
                //Obtenemos el id de la sucursal  
                int idEstructura = clsComun.fnObteneridSucursal(txtTienda.Text);

                //Obtenemos el folio actual
                if (!string.IsNullOrEmpty(sSerie))
                {
                    XmlNamespaceManager nsmSerie = new XmlNamespaceManager(XMLTimbrado.NameTable);
                    nsmSerie.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

                    sFolio = fnCargarFolioGen(idEstructura);

                    XMLTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@folio", nsmSerie).SetValue(sFolio);
                }

                //Insertamos el Comprobante ya timbrado a la BD y regresamos el id
                retornoInsert = clsTimbradoFuncionalidad.fnInsertarComprobante(XMLTimbrado.DocumentElement.OuterXml, 1, 'A',
                                                            DateTime.Now, idEstructura, nid_usuario, "R", HASHEmisor);

                if (retornoInsert > 0)//Comprobante Generado
                {
                    ///Habilitamos el panel para la descaga el XML o PDF
                    hpPDF.NavigateUrl = "~/Consultas/webConsultaPDF.aspx?idcfd=" + retornoInsert;
                    hpXML.NavigateUrl = "~/Consultas/webConsultasGeneradorXML.aspx?idcfd=" + retornoInsert;
                    //pnlConsulta.Visible = true;
                    ////Se guarda XML en ruta especificada 
                    //XMLTimbrado.Save(ConectorPAXGenerico.Properties.Settings.Default["RutaDocZips"].ToString() + snombreDoc + ".xml");
                    //Session["xDocTimbrado"] = xDocTimbrado.InnerXml;

                    ///Actializamos tabla de ticket
                    clsComun.fnActualziaTicket(Convert.ToInt32(ViewState["id_ticket"]), retornoInsert);
                    cEd = new clsEnvioCorreoDocs();

                    #region Envio de correo
                    string sCorrieo = txtCorreo.Text;

                    if (!string.IsNullOrEmpty(sCorrieo))
                    {
                        string sCorCli = string.Empty; //Valida si el campo esta lleno
                        sCorCli = cEd.fValidaEmail(sCorrieo/*txtCorreo.Text*/); //Valida formato de correo
                        string sDoc = clsComun.ObtenerParamentro("TipoComprobante");
                        string sMailTo = txtCorreo.Text;
                        string sCC = string.Empty;

                        int nid_cfd = retornoInsert;

                        if (sCorCli != string.Empty) //Si esta vacio entonces los correos estan escritos correctamente
                        {
                            txtCorreo.BorderColor = System.Drawing.Color.Red;

                            lblErrorLog.Text = Resources.resCorpusCFDIEs.txtCorreo;
                            mpeErrorLog.Show();
                        }

                        clsOperacionCuenta gOpeCuenta = new clsOperacionCuenta();
                        sdrInfo = gOpeCuenta.fnObtenerDatosFiscales(nid_cfd, null);

                        string sRFC = string.Empty;
                        string sRazonSocial = string.Empty;
                        string sCorreoMsj = string.Empty;

                        if (sdrInfo != null && sdrInfo.HasRows && sdrInfo.Read())
                        {
                            sRFC = sdrInfo["rfc"].ToString();//ViewState["rfc_Emisor"]
                            sRazonSocial = sdrInfo["razon_social"].ToString();//ViewState["razonSocial_Emisor"]
                        }
                        if (!string.IsNullOrEmpty(sRFC) && !string.IsNullOrEmpty(sRazonSocial))
                            sCorreoMsj = Resources.resCorpusCFDIEs.varMensajeCorreo + "\n" + "\n" + sRazonSocial + "\n" + "RFC:" + "\n" + sRFC;

                        bool bEnvio;
                        string Mensaje = string.Empty;
                        Mensaje = "<table>";
                        Mensaje = Mensaje + "<tr><td>" + sCorreoMsj.Replace("\n", @"<br />") + "</td><td></td></tr>";
                        Mensaje = Mensaje + "</table>";

                        if (Mensaje == string.Empty)
                            Mensaje = "Comprobantes PAX Facturación";


                        //clsConfiguracionPlantilla plantillas = new clsConfiguracionPlantilla();
                        string plantilla = "Logo";// plantillas.fnRecuperaPlantillaNombre(DatosUsuario.plantilla);


                        //Verifica si se envia el comprobante en ZIP o no.

                        //if (rdlArchivo.SelectedIndex == 1)
                        //{

                        bEnvio = cEd.fnPdfEnvioCorreo(plantilla, Convert.ToString(nid_cfd), sDoc,
                                          clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLsGen"] + "\\" + snombreDoc + ".pdf",
                            /*DatosUsuario.id_contribuyente*/0, 0/*DatosUsuario.id_rfc*/, "Black", sMailTo,
                                          "PAXFacturacion", Mensaje, Convert.ToString(ViewState["GuidPathZIPsGen"]),
                                          Convert.ToString(ViewState["GuidPathXMLsGen"]), snombreDoc, sCC);
                        //}
                        //else
                        //{
                        //bEnvio = cEd.fnPdfEnvioCorreoSinZIP(plantilla, Convert.ToString(nid_cfd), sDoc,
                        //                  clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLsGen"] + "\\" + snombreDoc + ".pdf",
                        //                  DatosUsuario.id_contribuyente, DatosUsuario.id_rfc, "Black", sMailTo,
                        //                  "PAXFacturacion", Mensaje, Convert.ToString(ViewState["GuidPathZIPsGen"]),
                        //                  Convert.ToString(ViewState["GuidPathXMLsGen"]), snombreDoc, sCC);
                        //}

                        if (bEnvio)
                        {
                            lblLeyCorreo.Visible = true;
                            lblLeyCorreo.Text = Resources.resCorpusCFDIEs.msgEnvioMail + "<br />" + sMailTo;
                        }
                        else
                        {
                            lblLeyCorreo.Visible = true;
                            lblLeyCorreo.Text = Resources.resCorpusCFDIEs.msgErrorEnvioMail;
                        }
                    }

                    #endregion

                    ///Agregado de Datos Fiscales Del Receptor ligados a la tienda en caso de no existir
                    if (dAuxiliar.Rows.Count <= 0)
                    {
                        try
                        {
                            ///Validacion para insertar el Nuevo receptor
                            if (txtRfcReceptor.Text != string.Empty || txtRazonReceptor.Text != string.Empty)
                            {
                                if (clsComun.fnValidaExpresion(txtRfcReceptor.Text, @"[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
                                {
                                    clsComun.fnAgregarReceptor(idEstructura, txtRfcReceptor.Text,
                                        txtRazonReceptor.Text, txtPais.Text, txtEstado.Text, txtMunicipio.Text, txtLocalidad.Text, txtCalle.Text,
                                        txtNoExterior.Text, txtNoInterior.Text, txtColonia.Text, Convert.ToInt32(txtCodigoPostal.Text), txtTelefono.Text, txtCorreo.Text, 1);
                                }
                            }
                            else
                            {
                                lblErrorLog.Text = Resources.resCorpusCFDIEs.varValidacionError;
                                mpeErrorLog.Show();
                                //clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
                            }
                        }
                        catch (SqlException sEx)
                        {
                            clsErrorLog.fnNuevaEntrada(sEx, clsErrorLog.TipoErroresLog.BaseDatos);
                            if (string.IsNullOrEmpty(hdIdRfc.Value))
                            {
                                lblErrorLog.Text = Resources.resCorpusCFDIEs.varErrorAlta;
                                mpeErrorLog.Show();
                                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
                            }
                            else
                            {
                                lblErrorLog.Text = Resources.resCorpusCFDIEs.varErrorCambio;
                                mpeErrorLog.Show();
                                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCambio);
                            }
                            return;
                        }
                    }///Fin Agregado de Datos de Receptor
                    else
                    {
                        ///Validacion para Modificar el Receptor
                        if (txtRfcReceptor.Text != string.Empty || txtRazonReceptor.Text != string.Empty)
                        {
                            if (clsComun.fnValidaExpresion(txtRfcReceptor.Text, @"[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
                            {
                                clsComun.fnAgregarReceptor(idEstructura, txtRfcReceptor.Text,
                                    txtRazonReceptor.Text, txtPais.Text, txtEstado.Text, txtMunicipio.Text, txtLocalidad.Text, txtCalle.Text,
                                    txtNoExterior.Text, txtNoInterior.Text, txtColonia.Text, Convert.ToInt32(txtCodigoPostal.Text), txtTelefono.Text, txtCorreo.Text, 0);
                            }
                        }
                        //else
                        //{
                        //    clsComun.fnMostrarError(this, "No fue posible modificar los datos.");//Resources.resCorpusCFDIEs.varValidacionError);
                        //}
                    }
                }
                else
                {
                    lblErrorLog.Text = Resources.resCorpusCFDIEs.msgNoCompGenerado;
                    mpeErrorLog.Show();
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado, Resources.resCorpusCFDIEs.varContribuyente);
                    return;
                }

                ///Auditoria
                clsPistasAuditoria.fnGenerarPistasAuditoria(nid_usuario, DateTime.Now, this.Title + "|" + "12.-fnInsertarComprobante" + "|" + "Inserta comprobante generado exitosamente en BD.");
            }

            //}//fin de Validacion del SAT sobre el archivo
        }
        catch (Exception ex)
        {
            lblErrorLog.Text = Resources.resCorpusCFDIEs.lblErroProblema;
            mpeErrorLog.Show();
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            return;
        }

        btnAcepDatos.Enabled = true;
        //imgLogo.Visible = false;
        //imgTicket.Visible = false;
        mpeDatosRecetor.Hide();
        mpeConsultaCFDI.Show();
        limpiar(this);
        ViewState.Clear();
        Session.Clear();

    }//Fin btnAcepDatos_Click....

    protected void txtRfcReceptor_TextChanged(object sender, EventArgs e)
    {
        //ViewState["dtDatosReceptor"] = fnBuscaReceptor(Convert.ToInt32(Session["id_estructura"]));
    }

    protected void btnCancelarDatos_Click(object sender, EventArgs e)
    {
        limpiar(this);
        txtRazonReceptor.Enabled = false;
        txtPais.Enabled = false;
        txtEstado.Enabled = false;
        txtMunicipio.Enabled = false;
        txtLocalidad.Enabled = false;
        txtCalle.Enabled = false;
        txtNoExterior.Enabled = false;
        txtNoInterior.Enabled = false;
        txtColonia.Enabled = false;
        txtCodigoPostal.Enabled = false;
        txtTelefono.Enabled = false;
        txtCorreo.Enabled = false;
    }

    protected void mpeDatosRecetor_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtRazonReceptor.Enabled = false;
            txtPais.Enabled = false;
            txtEstado.Enabled = false;
            txtMunicipio.Enabled = false;
            txtLocalidad.Enabled = false;
            txtCalle.Enabled = false;
            txtNoExterior.Enabled = false;
            txtNoInterior.Enabled = false;
            txtColonia.Enabled = false;
            txtCodigoPostal.Enabled = false;
            txtTelefono.Enabled = false;
            txtCorreo.Enabled = false;
        }
    }

    protected void btnAviso_Click(object sender, EventArgs e)
    {
        mpeDatosRecetor.Show();
    }

    public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    protected void btnConsultaCDFI_Click(object sender, EventArgs e)
    {
        Session.Clear();
        ViewState.Clear();
        Response.Redirect("~/Default.aspx", true);
    }

    public void Page_Error(object sender, EventArgs e)
    {
        Exception objErr = Server.GetLastError().GetBaseException();

        if (!string.IsNullOrEmpty(objErr.Message))
        {
            Server.ClearError();
            clsComun.fnMostrarMensaje(this, objErr.Message, "Error");

            //Response.Redirect("~/webGlobalError.aspx");
        }
    }

    protected void mnuTiendas_MenuItemClick(object sender, MenuEventArgs e)
    {

        fnMenuClik(null, sender, e);
    }

    public void fnMenuClik(string sOrigen, object sender, MenuEventArgs e)
    {
        DataTable dtAuxiliar = new DataTable();

        if (Session["dtFranquicias"] != null)
        {
            dtAuxiliar = ((DataTable)Session["dtFranquicias"]);
            string sid_estructura = string.Empty;
            string sValor = string.Empty;
            MenuItem item = new MenuItem();

            if (string.IsNullOrEmpty(sOrigen))
            {
                item = mnuTiendas.SelectedItem;
                //sValor = item.Value.ToString();
                sValor = "1";
            }
            else
            {
                if (sOrigen == "1")
                {
                    sValor = "1";// 
                    //sValor = "2";// local..
                }
                else
                {
                    sValor = "2";//
                    //sValor = "1";//   local...
                }

                //System.Drawing.ColorConverter colConvert = new ColorConverter();
                //System.Drawing.Color ColorT = new System.Drawing.Color();

                //ColorT = (System.Drawing.Color)colConvert.ConvertFromString("#A5D10F");

            }

            DataRow[] row = dtAuxiliar.Select("IdMenu=" + sValor);
            if (row.Length > 0)
            {
                sid_estructura = row[0]["id_estructura"].ToString();
            }


            //imgTicket.Visible = true;
            ltlLeyendaCancelacion.Visible = true;
            string sTelfoCan = string.Empty;
            string sCorreoCan = string.Empty;

            //Muestra la leyenda para la cancelacion donde la variable sValor es 1 .- Subway 2.- Dominos


            //imgLogo.ImageUrl = "~/Imagen.aspx?estructura=" + sid_estructura + "&Ubic=2";
            //imgTicket.ImageUrl = "~/ImagenTicket.aspx?estructura=" + sid_estructura + "&Ubic=2";

            if (sValor == "1")
            {
                sTelfoCan = "TelAlitas";
                sCorreoCan = "CorreoAlitas";
                Session["theme"] = "Alitas";
                Session["sTelfoCan"] = clsComun.ObtenerParamentro(sTelfoCan);
                Session["sCorreoCan"] = clsComun.ObtenerParamentro(sCorreoCan);
                Session["id_estructura"] = sid_estructura;
                //Response.Redirect(Request.RawUrl);
                //sTelfoCan = "TelSubWay";  //Local
            }
            else if (sValor == "2")
            {
                sTelfoCan = "TelFogon"; //Producción
                sCorreoCan = "CorreoFogon";
                Session["theme"] = "Fogon";
                Session["sTelfoCan"] = clsComun.ObtenerParamentro(sTelfoCan);
                Session["sCorreoCan"] = clsComun.ObtenerParamentro(sCorreoCan);
                Session["id_estructura"] = sid_estructura;
                ltlLeyendaCancelacion.Text = Resources.resCorpusCFDIEs.lblleyendaCancelacion + " " + clsComun.ObtenerParamentro(sTelfoCan) + " " +
                             Resources.resCorpusCFDIEs.lblleyendaCancelacionCorreo + " " + clsComun.ObtenerParamentro(sCorreoCan);
                Response.Redirect(Request.RawUrl);
                //sTelfoCan = "TelDominos";  //Local
            }
            else
            {
                sTelfoCan = "TelAlitas";
                sCorreoCan = "CorreoAlitas";
                Session["theme"] = "Alitas";
                Session["sTelfoCan"] = clsComun.ObtenerParamentro(sTelfoCan);
                Session["sCorreoCan"] = clsComun.ObtenerParamentro(sCorreoCan);
                Session["id_estructura"] = sid_estructura;
                ltlLeyendaCancelacion.Text = Resources.resCorpusCFDIEs.lblleyendaCancelacion + " " + clsComun.ObtenerParamentro(sTelfoCan) + " " +
                             Resources.resCorpusCFDIEs.lblleyendaCancelacionCorreo + " " + clsComun.ObtenerParamentro(sCorreoCan);
                Response.Redirect(Request.RawUrl);
            }

            



        }
    }

    /// <summary>
    /// Recupera los folios por tipo de sucursal y documento al momento de generar documento.
    /// </summary>
    private string fnCargarFolioGen(int nSucursal)
    {
        string sFolio = "0";
        //int nSucursal = 0;

        try
        {
            clsOperacionSeriesFolios giSq = new clsOperacionSeriesFolios();
            DataTable table = new DataTable();
            table = clsComun.fnObtenerSerieFolio(nSucursal, txtTienda.Text);//clsTimbradoFuncionalidad.LlenarDropSeries(Convert.ToString(nSucursal), 1);// Convert.ToInt32(ddlTipoDoc.SelectedValue.Split('|')[0]));
            if (table.Rows.Count > 0)
            {
                //sFolio = table.Rows[ddlSerie.SelectedIndex]["folio"].ToString();
                //DataRow[] row = table.Select("id_serie=" + ddlSerie.SelectedValue);
                sFolio = table.Rows[0]["folio"].ToString();//row[0]["folio"].ToString();
                string sSerie = table.Rows[0]["serie"].ToString();
                giSq.fnActualizarSerie(Convert.ToInt32(nSucursal), 1, sSerie);// Convert.ToInt32(ddlTipoDoc.SelectedValue.Split('|')[0]), row[0]["serie"].ToString());
            }
            else
            {
                sFolio = "0";
            }
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            sFolio = "0";
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            sFolio = "0";
        }

        return sFolio;
    }

    #region Funciones

    /// <summary>
    /// Se encarga de reinciar los controles del modalpoupextender
    /// </summary>
    private void fnLimpiarControles()
    {
        txtRazonReceptor.Text = string.Empty;
        txtPais.Text = string.Empty;
        txtEstado.Text = string.Empty;
        txtMunicipio.Text = string.Empty;
        txtLocalidad.Text = string.Empty;
        txtCalle.Text = string.Empty;
        txtNoExterior.Text = string.Empty;
        txtNoInterior.Text = string.Empty;
        txtColonia.Text = string.Empty;
        txtCodigoPostal.Text = string.Empty;
        txtTelefono.Text = string.Empty;
        txtCorreo.Text = string.Empty;
    }

    //private DataTable fnRealizarBusquedaDatosReceptor(int nIdReceptor)
    //{
    //    DataTable tblDatos = new DataTable();
    //    tblDatos = clsTimbradoFuncionalidad.RecuperaSucReceptor(nIdReceptor);
    //    return tblDatos;s
    //}

    public static string GetHASH(string text)
    {
        byte[] hashValue;
        byte[] message = Encoding.UTF8.GetBytes(text);

        SHA1Managed hashString = new SHA1Managed();
        string hex = "";

        hashValue = hashString.ComputeHash(message);
        foreach (byte x in hashValue)
        {
            hex += String.Format("{0:x2}", x);
        }
        return hex;
    }

    /// <summary>
    /// Checamos el RFC si existe en la base de datos 
    /// </summary>
    /// <returns>bBusqueda
    /// </returns>
    public DataTable fnBuscaReceptor(int id_estructura)
    {
        DataTable dtDatosReceptor = new DataTable("DatosReceptor");

        if (!(txtRfcReceptor.Text == string.Empty || txtRfcReceptor.Text == null))//Si le campo de Texto esta lleno
        {
            //Busca el RFC en la base de datos
            dtDatosReceptor = clsComun.fnRecuperaReceptor(txtRfcReceptor.Text, id_estructura);
            if (dtDatosReceptor.Rows.Count > 0) //Si la Busqueda optiene resultados
            {
                int id_rfc_receptor = Convert.ToInt32(dtDatosReceptor.Rows[0]["id_rfc_receptor"].ToString());
                DataTable dtDatos = clsTimbradoFuncionalidad.RecuperaSucReceptor(id_rfc_receptor); // fnRealizarBusquedaDatosReceptor(id_rfc_receptor);

                txtRazonReceptor.Text = dtDatosReceptor.Rows[0]["nombre_receptor"].ToString();
                txtPais.Text = dtDatos.Rows[0]["pais"].ToString();
                txtEstado.Text = dtDatos.Rows[0]["estado"].ToString();
                txtMunicipio.Text = dtDatos.Rows[0]["municipio"].ToString();
                txtLocalidad.Text = dtDatos.Rows[0]["localidad"].ToString();
                txtCalle.Text = dtDatos.Rows[0]["calle"].ToString();
                txtNoExterior.Text = dtDatos.Rows[0]["numero_exterior"].ToString();
                txtNoInterior.Text = dtDatos.Rows[0]["numero_interior"].ToString();
                txtColonia.Text = dtDatos.Rows[0]["colonia"].ToString();
                txtCodigoPostal.Text = dtDatos.Rows[0]["codigo_postal"].ToString();
                txtTelefono.Text = dtDatos.Rows[0]["telefono"].ToString();
                txtCorreo.Text = dtDatos.Rows[0]["email"].ToString();
            }
            else
            {
                ViewState["vsApagado"] = "1";
                fnLimpiarControles();
                //lblAviso.Text = "Agregar Datos Fiscales ";
                //mpeAvisos.Show();
                Page.Validate("RegisterUserValidationGroup");

                //clsComun.fnMostrarMensaje(this, "Agregar Datos Fiscales ", Resources.resCorpusCFDIEs.varContribuyente);
            }
        }
        //else
        //{
        //    mpeDatosRecetor.Hide();
        //    lblAviso.Text = "Debe ingresar por lo menos un RFC: ";
        //    mpeAvisos.Show();
        //    //clsComun.fnMostrarMensaje(this, "Ingresar RFC: ", Resources.resCorpusCFDIEs.varContribuyente);
        //}
        txtRazonReceptor.Enabled = true;
        txtPais.Enabled = true;
        txtEstado.Enabled = true;
        txtMunicipio.Enabled = true;
        txtLocalidad.Enabled = true;
        txtCalle.Enabled = true;
        txtNoExterior.Enabled = true;
        txtNoInterior.Enabled = true;
        txtColonia.Enabled = true;
        txtCodigoPostal.Enabled = true;
        txtTelefono.Enabled = true;
        txtCorreo.Enabled = true;

        mpeDatosRecetor.Show();
        return dtDatosReceptor;

    }

    /// <summary>
    /// Se encarga de limpiar los campos de textos
    /// </summary>
    /// <param name="parent"></param>
    private void limpiar(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
            {
                limpiar(c);
            }
            else
            {
                switch (c.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.TextBox":
                        ((TextBox)c).Text = "";
                        break;
                }
            }
        }
    }

    #endregion

    /***Agregamos el Menu dinamico***/
    #region Menu dinamico
    //metodo muestra el menu dependiendo del personal
    public void ver_menu()
    {
        //la variable contiene el resultado de la funcion
        DataTable dt = clsComun.fnMostrarMenu(); // negocio.fnMotrarMenu(lbltypo.Text);
        //Almacenamos la tabla en una variable de ssesion
        Session["dtFranquicias"] = dt;
        //recorremos el datatable para agregar los itemn en la cabezera (itemns padres ) 
        foreach (DataRow drmenuItem in dt.Rows)
        {
            //crea una nueva instancia de tipo MenuItem
            MenuItem NuevoMenuItem = new MenuItem();
            //indicamos qué elementos son padres 
            if (drmenuItem["IdMenu"].Equals(drmenuItem["IdPadre"]))
            {

                NuevoMenuItem.Value = drmenuItem["IdMenu"].ToString();
                NuevoMenuItem.Text = drmenuItem["Descripcion"].ToString();

                mnuTiendas.Items.Add(NuevoMenuItem);
                //llamamos al metodo recursivo encargado de general el arbol 
                agregarmenu(ref NuevoMenuItem, dt);
            }
        }
    }

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
            Response.Redirect("~/Default.aspx");
        }
        else
        {
            Response.Redirect("~/Account/Login.aspx");
        }
    }

    ///// <summary>
    ///// Recupera las series por sucursal y tipo de documento.
    ///// </summary>
    //private void fnCargarSeries()
    //{
    //    try
    //    {
    //        string nSucursal = string.Empty;

    //        if (!string.IsNullOrEmpty(hdnSelVal.Value))
    //            nSucursal = hdnSelVal.Value;

    //        DataTable table = new DataTable();

    //        table = clsTimbradoFuncionalidad.LlenarDropSeries(nSucursal, 1);// Convert.ToInt32(ddlTipoDoc.SelectedValue.Split('|')[0]));
    //        if (table.Rows.Count > 0)
    //        {
    //            ddlSerie.DataSource = table;
    //            tblSeriesFolios.Visible = true;

    //        }
    //        else
    //        {
    //            ddlSerie.Items.Clear();
    //            ddlSerie.Items.Add("N/A");
    //            tblSeriesFolios.Visible = false;
    //        }

    //        ddlSerie.DataBind();
    //    }
    //    catch (SqlException ex)
    //    {
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
    //        ddlSerie.DataSource = null;
    //        ddlSerie.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //        //referencia nula
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
    //        ddlSerie.DataSource = null;
    //        ddlSerie.DataBind();
    //    }
    //}

    ///// <summary>
    ///// Recupera los folios por tipo de sucursal y documento.
    ///// </summary>
    //private void fnCargarFolio()
    //{
    //    try
    //    {
    //        string nSucursal = string.Empty;

    //        if (!string.IsNullOrEmpty(hdnSelVal.Value))
    //            nSucursal = hdnSelVal.Value;

    //        DataTable table = new DataTable();
    //        table = clsTimbradoFuncionalidad.LlenarDropSeries(nSucursal, 1);// Convert.ToInt32(ddlTipoDoc.SelectedValue.Split('|')[0]));
    //        if (ddlSerie.SelectedValue != "N/A")
    //        {
    //            //txtFolio.Text = table.Rows[ddlSerie.SelectedIndex]["folio"].ToString(); /* 16 - 02 - 2013 */
    //            DataRow[] row = table.Select("id_serie=" + ddlSerie.SelectedValue);
    //            txtFolio.Text = row[0]["folio"].ToString();
    //        }
    //        else
    //        {
    //            txtFolio.Text = "0";
    //        }
    //    }
    //    catch (SqlException ex)
    //    {
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
    //        txtFolio.Text = "0";
    //    }
    //    catch (Exception ex)
    //    {
    //        //referencia nula
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
    //        txtFolio.Text = "0";
    //    }
    //}

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



}//Fin de la clase