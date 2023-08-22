using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Xml;
using System.IO.Compression;
using System.Text;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;


    public partial class Consultas_WebDescargaComprobantesNomina : System.Web.UI.Page
    {
        string sArchivoZIP = string.Empty;
        string sCarpetaZIP = string.Empty;
        Thread t;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //btnCerrar.Attributes.Add("onclick", "window.close();");

                fnLimpiarHilos();
                ViewState["GuidPathXMLs"] = Guid.NewGuid().ToString();
                ViewState["GuidPathZIPs"] = Guid.NewGuid().ToString();
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

        //Timer actualiza el progreso del proceso de descarga
        protected void timerDescarga_Tick(object sender, EventArgs e)
        {
            lblProgresoDescarga.Text = Session["estatus"].ToString();

            if (Session["completado"] as bool? == true)
            {
                timerDescarga.Enabled = false;

                fnDescargarArchivo();
            }
        }

        protected void btnDescarga_Click(object sender, EventArgs e)
        {

            ParametrosThreadDescargaNomina parametrosThreadDescarga = new ParametrosThreadDescargaNomina();

            //Obtener parámetros para la descarga
            #region Comentado por Hector Portillo 2014-02-01
            string sQueryString = (Request.QueryString["p"] != null) ? Request.QueryString["p"].ToString().Replace(" ", "+") : string.Empty;
            #endregion

            #region Comentado por Hector Portillo 2014-02-01
            //string sQueryString = Session["sIDS"].ToString();
            #endregion

            if (string.IsNullOrEmpty(sQueryString)) return;


            try
            {
                if (!String.IsNullOrEmpty(sQueryString))
                {



                    //string sIDS = Utilerias.Encriptacion.Base64.DesencriptarBase64(sQueryString);
                    string sIDS = PAXCrypto.CryptoAES.DesencriptaAES64(sQueryString);





                    parametrosThreadDescarga.sComprobantes = sIDS;

                    parametrosThreadDescarga.SesionUsuario = clsComun.fnUsuarioEnSesion();


                    //Start the Process
                    lock (Session.SyncRoot)
                    {
                        Session["completado"] = false;
                        Session["estatus"] = "";
                    }

                    //Se asginan datos de la Session actual
                    parametrosThreadDescarga.Session = Session;

                    //Se asignan datos del contexto actual
                    parametrosThreadDescarga.Conext = HttpContext.Current;

                    t = new Thread(new ParameterizedThreadStart(fnThreadDescargaComprobantes));
                    t.Start(parametrosThreadDescarga);

                    Session["tDescargaComp"] = t;

                    lblProgresoDescarga.Visible = true;
                    timerDescarga.Enabled = true;
                    btnDescarga.Enabled = false;

                }

            }
            catch (Exception ex)
            {

                lblProgresoDescarga.Text = "No se pudo iniciar la descarga del archivo";
                lblProgresoDescarga.Visible = true;
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);

                fnLimpiarHilos();

            }

            #region Comentado por Hector Portillo 2014-01-20
            // try
            //{

            //if (!String.IsNullOrEmpty(sQueryString))
            //{
            //    //Se desencriptan los parámetros de la descarga
            //    string sDesencriptado = PAXCrypto.CryptoAES.DesencriptaAES64(sQueryString);
            //    string[] asParametros = sDesencriptado.Split('|');

            //    ParametroConsulta parametrosConsulta = new ParametroConsulta();
            //    parametrosConsulta.Receptor = asParametros[0];
            //    parametrosConsulta.Estatus = asParametros[1];
            //    parametrosConsulta.Sucursal = asParametros[2];
            //    parametrosConsulta.Documento = asParametros[3];
            //    parametrosConsulta.Serie = asParametros[4];
            //    parametrosConsulta.FolioIni = asParametros[5];
            //    parametrosConsulta.FolioFin = asParametros[6];
            //    parametrosConsulta.FechaIni = Convert.ToDateTime(asParametros[7]);
            //    parametrosConsulta.FechaFin = Convert.ToDateTime(asParametros[8]);
            //    parametrosConsulta.UUID = asParametros[9];
            //    parametrosConsulta.Usuario = asParametros[10];


            //    //Se asignan parámetros de la consulta
            //    parametrosThreadDescarga.ParametrosConsulta = parametrosConsulta;

            //    parametrosThreadDescarga.SesionUsuario = clsComun.fnUsuarioEnSesion();

            //    //Start the Process
            //    lock (Session.SyncRoot)
            //    {
            //        Session["completado"] = false;
            //        Session["estatus"] = "";
            //    }

            //    //Se asginan datos de la Session actual
            //    parametrosThreadDescarga.Session = Session;

            //    //Se asignan datos del contexto actual
            //    parametrosThreadDescarga.Conext = HttpContext.Current;

            //    //Se crear hilo para la generación de comprobantes para la descarga
            //    t = new Thread(new ParameterizedThreadStart(fnThreadDescargaComprobantes));
            //    t.Start(parametrosThreadDescarga);

            //    Session["tDescargaComp"] = t;

            //    lblProgresoDescarga.Visible = true;
            //    timerDescarga.Enabled = true;
            //    btnDescarga.Enabled = false;

            //     }

            //}
            //catch (Exception ex)
            //{
            //    lblProgresoDescarga.Text = "No se pudo iniciar la descarga del archivo";
            //    lblProgresoDescarga.Visible = true;
            //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);

            //    fnLimpiarHilos();
            //}

            #endregion



        }

        private void fnThreadDescargaComprobantes(object data)
        {
            try
            {

                ParametrosThreadDescargaNomina parametrosThreadDescarga = (ParametrosThreadDescargaNomina)data;
                //Se recupera session
                System.Web.SessionState.HttpSessionState s = (System.Web.SessionState.HttpSessionState)parametrosThreadDescarga.Session;

                if (s["Culture"] != null)
                {
                    string lang = s["Culture"].ToString();
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

                //Se recupera contexto
                HttpContext.Current = parametrosThreadDescarga.Conext;

                //Se indica que se ha iniciado el proceso
                lock (s.SyncRoot)
                {
                    s["estatus"] = "Iniciando proceso...";
                }

                string errores = string.Empty;

                int bandera = 0;
                byte[] buffer = { };
                byte[] bufferPDF = { };
                ArrayList Final = new ArrayList();

                Directory.CreateDirectory(clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"]);
                Directory.CreateDirectory(clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"]);

                string snombreDoc = string.Empty;
                int i = 0;

                #region comentado 09_01_2013
                //foreach (GridViewRow renglon in gdvComprobantes.Rows)
                //{
                //    CheckBox CbCan;

                //    CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                //    if (CbCan.Checked)
                //    {
                //        Guid Gid;
                //        Gid = Guid.NewGuid();

                //        Label sIdCfd = ((Label)renglon.FindControl("lblid_cfd"));

                //        gDAL = new clsOperacionConsulta();
                //        XmlDocument comprobante = new XmlDocument();
                //        string sTipoDocumento = HttpUtility.HtmlDecode(renglon.Cells[8].Text);
                //        snombreDoc = renglon.Cells[3].Text;
                //       // string sTipoDocumento = HttpUtility.UrlDecode(Request.QueryString["doc"]);
                //        int nIdContribuyente = clsComun.fnUsuarioEnSesion().id_contribuyente;
                //        comprobante = gDAL.fnObtenerComprobanteXML(nIdContribuyente, sIdCfd.Text);

                //        clsPlantillaLista pdf = new clsPlantillaLista();
                //        //clsOperacionConsultaPdf pdf;
                //        //pdf = new clsOperacionConsultaPdf(gDAL.fnObtenerComprobanteXML(nIdContribuyente, sIdCfd.Text));

                //        //if (!string.IsNullOrEmpty(sTipoDocumento))
                //        //    pdf.TipoDocumento = sTipoDocumento.ToUpper();


                //        string pathPDF = clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + snombreDoc + ".pdf";//Gid + ".pdf";

                //        DatosUsuario = clsComun.fnUsuarioEnSesion();
                //        pdf.fnObtenerPLantilla(gDAL.fnObtenerComprobanteXML(nIdContribuyente, sIdCfd.Text), string.Empty, string.Empty, sTipoDocumento, this, pathPDF, nIdContribuyente,DatosUsuario.id_rfc, DatosUsuario.color);

                //        //pdf.fnGenerarPDFSave(pathPDF);


                //        clsComun.fnNuevaPistaAuditoria(
                //            "webConsultasGeneradorPDF",
                //            "fnGenerarPDF",
                //            "Se generó el PDF para el comprobante con ID " + sIdCfd.Text
                //            );

                //        bandera = 1;

                //        buffer = Encoding.UTF8.GetBytes(comprobante.InnerXml);

                //        string path = clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + snombreDoc + ".xml";//Gid + ".xml";


                //        // Create the text file if it doesn't already exist.
                //        if (!File.Exists(path))
                //        {
                //            //fnActualizaCreditos();
                //            File.WriteAllBytes(path, buffer);
                //        }
                //    }
                //    i += 1; //Verifica si se selecciono mas de un registro
                //}
                #endregion



                clsOperacionConsulta gDAL = new clsOperacionConsulta();

                clsInicioSesionUsuario DatosUsuario = parametrosThreadDescarga.SesionUsuario;
                int nIdContribuyente = DatosUsuario.id_contribuyente;

                //Se obtiene comprobantes
                DataTable dtComprobantes = fnObtenerComprobantesDescargaThread(parametrosThreadDescarga.sComprobantes);



                //Se recorren comprobantes para generar XML y PDF
                foreach (DataRow renglon in dtComprobantes.Rows)
                {

                    Guid Gid;
                    Gid = Guid.NewGuid();

                    string sIdCfd = renglon["id_cfd"].ToString();
                    try
                    {
                        gDAL = new clsOperacionConsulta();
                        XmlDocument comprobante = new XmlDocument();
                        XmlDocument xdComprobante_aux = new XmlDocument();
                        string sTipoDocumento = renglon["documento"].ToString();
                        snombreDoc = renglon["UUID"].ToString();

                        /* Obtiene XML de comprobante */
                        string sXml = renglon["xml"].ToString();
                        //fnObtenerXml(sIdCfd);

                        comprobante.LoadXml(sXml);
                        xdComprobante_aux.LoadXml(sXml);

                        // Create an XML declaration. 
                        XmlDeclaration xmldecl;
                        xmldecl = comprobante.CreateXmlDeclaration("1.0", null, null);
                        xmldecl.Encoding = "UTF-8";
                        xmldecl.Standalone = null;

                        // Add the new node to the document.
                        XmlElement root = comprobante.DocumentElement;
                        comprobante.InsertBefore(xmldecl, root);

                        XmlDocument xdAddenda = new XmlDocument();


                        try
                        {
                            xdAddenda.LoadXml(renglon["addenda"].ToString());

                            fnPegarAddendaXML(ref comprobante, renglon);




                        }
                        catch { }







                        //Pega addenda en caso de que exista


                        /* Fin obtiene comprobante */

                        /* Se genera PDF del comprobante */
                        clsPlantillaLista pdf = new clsPlantillaLista();

                        clsConfiguracionPlantilla PlantillaC = new clsConfiguracionPlantilla();
                        string plantilla = "Nomina";

                        string pathPDF = clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + snombreDoc + ".pdf";//Gid + ".pdf";

                        //pdf.fnObtenerPLantilla(gDAL.fnObtenerComprobanteXML(nIdContribuyente, sIdCfd), string.Empty, string.Empty, sTipoDocumento, this, pathPDF, nIdContribuyente, DatosUsuario.id_rfc, DatosUsuario.color);
                        //pdf.fnCrearPLantillaEnvio(gDAL.fnObtenerComprobanteXML(nIdContribuyente, sIdCfd), plantilla, sIdCfd, sTipoDocumento, pathPDF, nIdContribuyente, DatosUsuario.id_rfc, DatosUsuario.color);



                        pdf.fnCrearPLantillaEnvio(xdComprobante_aux, plantilla, sIdCfd, sTipoDocumento, pathPDF, nIdContribuyente, DatosUsuario.id_rfc, DatosUsuario.color);

                        //pdf.fnGenerarPDFSave(pathPDF);
                        clsComun.fnNuevaPistaAuditoria(
                                                "webConsultasGeneradorPDF",
                                                "fnGenerarPDF",
                                                "Se generó el PDF para el comprobante con ID " + sIdCfd
                                                );
                        /* Fin genera PDF */

                        bandera = 1;

                        buffer = Encoding.UTF8.GetBytes(comprobante.InnerXml);

                        string path = clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"] + "\\" + snombreDoc + ".xml";//Gid + ".xml";

                        // Create the text file if it doesn't already exist.
                        if (!File.Exists(path))
                        {
                            //fnActualizaCreditos();
                            File.WriteAllBytes(path, buffer);
                        }

                        i += 1;
                    }
                    catch (Exception ex)
                    {
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorConsulta);
                    }

                    //Reporta progreso
                    lock (s.SyncRoot)
                    {
                        //La generación de comprobantes es el 90% del proceso
                        s["estatus"] = "Procesando: " + (i * 90) / dtComprobantes.Rows.Count + "%";
                    }
                }

                if (bandera == 1)
                {

                    Guid Gid;
                    Gid = Guid.NewGuid();

                    string Ruta = string.Empty; //clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"] + "\\" + Gid + ".zip";
                    if (i > 1) //Si se selecciono mas de un registro se guarda un nombre generico.
                        Ruta = clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"] + "\\" + Gid + ".zip";
                    else //Si selecciono un registro se guarda con el nombre del documento
                        Ruta = clsComun.ObtenerParamentro("RutaDocZips") + ViewState["GuidPathZIPs"] + "\\" + snombreDoc + ".zip";

                    ICSharpCode.SharpZipLib.Zip.ZipOutputStream zip = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(File.Create(Ruta));
                    zip.SetLevel(6);

                    string folder = clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"];

                    ComprimirCarpeta(folder, folder, zip);

                    zip.Finish();
                    zip.Close();

                    foreach (string file in Directory.GetFiles(clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"]))
                    {
                        File.Delete(file);
                    }
                    Directory.Delete(clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLs"], true);

                    //Se guarda en sesión la ruta del archivo ZIP generado para su posterior descarga
                    s["rutaZIPDescarga"] = Ruta;


                    //Reporta progreso
                    lock (s.SyncRoot)
                    {
                        //La generación de comprobantes es el 90% del proceso
                        s["estatus"] = "Procesando: 99%";
                    }

                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varDescargaVacio);
                }

                //fnLimpiaCarpetas();

                lock (s.SyncRoot)
                {
                    s["estatus"] = "Completado, ya puede descargar los archivos.";
                    s["completado"] = true;
                }

            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                lblProgresoDescarga.Text = "No se pudo completar el proceso de descarga del archivo";
                lblProgresoDescarga.Visible = true;
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);

                fnLimpiarHilos();
            }
            catch (SqlException ex)
            {
                lblProgresoDescarga.Text = "No se pudo completar el proceso de descarga del archivo";
                lblProgresoDescarga.Visible = true;
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);

                fnLimpiarHilos();
            }
            catch (Exception ex)
            {
                lblProgresoDescarga.Text = "No se pudo completar el proceso de descarga del archivo";
                lblProgresoDescarga.Visible = true;
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);

                fnLimpiarHilos();
            }
        }

        //Manda llamar al Web Handler para la descarga del archivo ZIP
        private bool fnDescargarArchivo()
        {
            try
            {
                if (Session["rutaZIPDescarga"] != null)
                {
                    //Response.Redirect("~/Consultas/DownloadFile.ashx?file=" + Session["rutaZIPDescarga"].ToString());
                    string sNombreArchivo = Session["rutaZIPDescarga"].ToString();
                    string sGuidPathZip = (new FileInfo(sNombreArchivo)).Directory.Name;


                    ScriptManager.RegisterStartupScript(this, this.GetType(), "descargarComprobantes",
                                                            String.Format("<script>window.location.href='{0}?d={1}&f={2}'</script>",
                                                                           "DescargarComprobantes.ashx", sGuidPathZip, Path.GetFileNameWithoutExtension(sNombreArchivo)), false);

                    fnLimpiarHilos();
                    return true;
                }
                else
                {

                    fnLimpiarHilos();
                    return false;
                }


            }
            catch (Exception ex)
            {
                lblProgresoDescarga.Text = "No se pudo completar la descarga del archivo";
                lblProgresoDescarga.Visible = true;
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);

                fnLimpiarHilos();

                return false;
            }
        }


        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            //fnObtenerRutas();
            fnLimpiaCarpetas();
            fnLimpiarHilos();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarDescargaComprobantes", "<script>window.close();</script>", false);
        }

        #region Métodos auxiliares

        /// <summary>
        /// Elimina las carpetas temporales que fueron creadas
        /// </summary>
        public void fnLimpiaCarpetas()
        {
            try
            {
                string sFolder = clsComun.ObtenerParamentro("RutaDocZips");// +ViewState["GuidPathZIPs"]; //+ sCarpetaZIP;
                string sFolderXmls = clsComun.ObtenerParamentro("RutaDocXmlZips");

                if (Directory.Exists(sFolder))
                {
                    foreach (string d in Directory.GetDirectories(sFolder))
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(d);

                        TimeSpan tsDirectorio = new TimeSpan(dirInfo.CreationTime.Ticks);
                        TimeSpan tsActual = new TimeSpan(DateTime.Now.Ticks);

                        //Elimina todas los directorios con:
                        //  - un tiempo de creación mayor a 30 minutos
                        //  - el directorio actual que se está descargando
                        // No se elimina el directorio donde se guardan los PDF y XML (sFolderXmls)
                        string sCarpetaZIP = ViewState["GuidPathZIPs"].ToString();

                        if (d.ToUpper() + "\\" != sFolderXmls.ToUpper() && (d.ToUpper() == (sFolder + sCarpetaZIP).ToUpper() || (tsActual.TotalMinutes - tsDirectorio.TotalMinutes) >= 30))
                        {
                            foreach (string file in Directory.GetFiles(d))
                            {
                                File.Delete(file);
                            }
                            Directory.Delete(d, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            }
        }

        private void fnLimpiarHilos()
        {
            Session["rutaZIPDescarga"] = null;
            Session["completado"] = false;
            Session["estatus"] = "";

            if (timerDescarga != null)
            {
                timerDescarga.Dispose();
            }

            if (Session["tDescargaComp"] != null)
            {
                t = (Thread)Session["tDescargaComp"];

                if (t != null && t.IsAlive)
                {
                    t.Abort();
                }

                Session["tDescargaComp"] = null;
            }
        }

        /// <summary>
        /// herramienta para comprimir la carpeta segun la ruta
        /// </summary>
        public static void CompressFile(string path)
        {
            FileStream sourceFile = File.OpenRead(path);
            FileStream destinationFile = File.Create(path);

            byte[] buffer = new byte[sourceFile.Length];
            sourceFile.Read(buffer, 0, buffer.Length);

            using (GZipStream output = new GZipStream(destinationFile,
                CompressionMode.Compress))
            {
                Console.WriteLine("Compressing {0} to {1}.", sourceFile.Name,
                    destinationFile.Name, false);

                output.Write(buffer, 0, buffer.Length);
            }

            // Close the files.
            sourceFile.Close();
            destinationFile.Close();
        }

        public static void ComprimirCarpeta(string RootFolder, string CurrentFolder, ICSharpCode.SharpZipLib.Zip.ZipOutputStream zStream)
        {
            string[] SubFolders = Directory.GetDirectories(CurrentFolder);

            //Llama de nuevo al metodo recursivamente para cada carpeta
            foreach (string Folder in SubFolders)
            {
                ComprimirCarpeta(RootFolder, Folder, zStream);
            }

            string relativePath = CurrentFolder.Substring(RootFolder.Length) + "/";

            //the path "/" is not added or a folder will be created
            //at the root of the file
            if (relativePath.Length > 1)
            {
                ICSharpCode.SharpZipLib.Zip.ZipEntry dirEntry;
                dirEntry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(relativePath);

            }

            //Añade todos los ficheros de la carpeta al zip
            foreach (string file in Directory.GetFiles(CurrentFolder))
            {
                AñadirFicheroaZip(zStream, relativePath, file);
            }
        }

        private static void AñadirFicheroaZip(ICSharpCode.SharpZipLib.Zip.ZipOutputStream zStream, string relativePath, string file)
        {
            byte[] buffer = new byte[4096];

            //the relative path is added to the file in order to place the file within
            //this directory in the zip
            string fileRelativePath = (relativePath.Length > 1 ? relativePath : string.Empty)
                                      + Path.GetFileName(file);

            ICSharpCode.SharpZipLib.Zip.ZipEntry entry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(fileRelativePath);

            zStream.PutNextEntry(entry);

            using (FileStream fs = File.OpenRead(file))
            {
                int sourceBytes;
                do
                {
                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                    zStream.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }
        }

        private void fnPegarAddendaXML(ref XmlDocument xComprobante, DataRow xdConsulta)
        {

            XmlDocument xAddenda = new XmlDocument();


            xAddenda.LoadXml(xdConsulta["addenda"].ToString());

            string AddendaNamespace = xdConsulta["namespace"].ToString();

            int idModulo = Convert.ToInt32(xdConsulta["id_modulo"]);



            if (AddendaNamespace != "")
            {
                string[] nombre = AddendaNamespace.Split('=');
                XmlAttribute xAttribute = xComprobante.CreateAttribute(nombre[0]);
                xAttribute.InnerText = AddendaNamespace;
                xComprobante.ChildNodes[1].Attributes.Append(xAttribute);
            }


            XmlNode childElement = xComprobante.CreateNode(XmlNodeType.Element, "cfdi:Addenda", xComprobante.DocumentElement.NamespaceURI);
            xComprobante.ChildNodes[1].AppendChild(childElement);

            // El Mudulo 6 es de la addenda de AHMSA por lo que al agregar el nodo, agregaremos los atributos y 
            // el primer nodo de la addenda 
            if (idModulo.Equals(6))
            {
                XmlAttribute atXmlns = xComprobante.CreateAttribute("xmlns:ahmsa", "http://www.w3.org/2000/xmlns/");
                atXmlns.Value = "http://www.ahmsa.com/xsd/AddendaAHM1";
                childElement.Attributes.SetNamedItem(atXmlns);

                XmlAttribute atXsi = xComprobante.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                atXsi.Value = "http://www.ahmsa.com/xsd/AddendaAHM1 http://www.ahmsa.com/xsd/AddendaAHM1/AddendaAHM.xsd";
                childElement.Attributes.SetNamedItem(atXsi);

                childElement.InnerXml = xAddenda.FirstChild.InnerXml;
            }
            else if (idModulo.Equals(7))
            {
                XmlElement xeAutoZone = xComprobante.CreateElement("ADDENDA20");
                XPathNavigator navAutoZone = xAddenda.CreateNavigator();

                xeAutoZone.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xeAutoZone.SetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "http://azfest.autozone.com/fssit91/XSD/Addenda_Non_Merch_32.xsd");
                xeAutoZone.SetAttribute("VERSION", navAutoZone.SelectSingleNode("/ADDENDA20/@VERSION").Value);
                xeAutoZone.SetAttribute("VENDOR_ID", navAutoZone.SelectSingleNode("/ADDENDA20/@VENDOR_ID").Value);
                xeAutoZone.SetAttribute("DEPTID", navAutoZone.SelectSingleNode("/ADDENDA20/@DEPTID").Value);
                xeAutoZone.SetAttribute("BUYER", navAutoZone.SelectSingleNode("/ADDENDA20/@BUYER").Value);
                xeAutoZone.SetAttribute("EMAIL", navAutoZone.SelectSingleNode("/ADDENDA20/@EMAIL").Value);

                childElement.AppendChild(xeAutoZone);
            }
            else
            {
                childElement.InnerXml = xAddenda.OuterXml;
            }

        }

        private DataTable fnObtenerComprobantesDescargaThread(string sComprobantes)
        {


            #region Comentado por Hector Portillo 2014-01-20.

            /*Razon = Intentamos Evitar usar la libreria de Utilerias de SQL ya que genera
        Errores al no cerrar las conexiones*/

            //Utilerias.SQL.InterfazSQL giSql = clsComun.fnCrearConexion(conSucursal);

            // Verificamos los demás parametros para decidir
            // si se usarán como filtros


            //giSql.AgregarParametro("nId_Usuario", idUsuario);
            //giSql.AgregarParametro("dFecha_Inicio", pdFechaIni);
            //giSql.AgregarParametro("dFecha_Fin", pdFechaFin);

            //if (psIdSucursal != "0")
            //    giSql.AgregarParametro("nId_Estructura", psIdSucursal);

            //if (psIdDocumento != "0")
            //    giSql.AgregarParametro("nId_Tipo_Documento", psIdDocumento);

            //if (psEstatus != "0")
            //    giSql.AgregarParametro("sEstatus", psEstatus);

            //if (psRfc != "0")
            //    giSql.AgregarParametro("sRfc_Receptor", psRfc);

            //if (psSerie != Resources.resCorpusCFDIEs.VarDropTodos)
            //    giSql.AgregarParametro("sSerie", psSerie);

            //if (!string.IsNullOrEmpty(psFolioIni))
            //    giSql.AgregarParametro("nFolio_Inicio", psFolioIni);

            //if (!string.IsNullOrEmpty(psFolioFin))
            //    giSql.AgregarParametro("nFolio_Fin", psFolioFin);

            //if (!string.IsNullOrEmpty(sUUID))
            //    giSql.AgregarParametro("nUUID", sUUID);

            //if (psIdUsuario_Filtro != "0")
            //    giSql.AgregarParametro("nId_Usuario_Filtro", psIdUsuario_Filtro);

            //giSql.Query("usp_Cfd_ComprobantesAsincrona_Sel_Cobro", true, ref dtAuxiliar);
            #endregion

            string conSucursal = "conConsultas";
            DataTable dtAuxiliar = new DataTable();


            bool berror = false;
            string sMensajeError = string.Empty;

            using (SqlConnection scConexion = new SqlConnection())
            {

                try
                {


                    scConexion.ConnectionString = PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSucursal].ConnectionString);

                    scConexion.Open();

                    using (SqlCommand scoComando = new SqlCommand())
                    {
                        scoComando.Connection = scConexion;
                        scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                        scoComando.CommandText = "usp_cfd_DescargaComprobantes";


                        scoComando.Parameters.AddWithValue("@sCsv", sComprobantes);

                        using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                        {

                            sdaAdaptador.Fill(dtAuxiliar);

                        }

                    }

                }
                catch (Exception ex)
                {
                    berror = true;
                    sMensajeError = Resources.resCorpusCFDIEs.varErrorConsulta;
                }
                finally
                {

                    scConexion.Close();

                    if (berror)
                    {
                        throw new Exception(sMensajeError);

                    }


                }



            }



            return dtAuxiliar;
        }

        private string fnObtenerXml(string sComprobantes)
        {




            string conSucursal = "conConsultas";
            string xml = "";


            bool berror = false;
            string sMensajeError = string.Empty;

            using (SqlConnection scConexion = new SqlConnection())
            {

                try
                {


                    scConexion.ConnectionString = PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSucursal].ConnectionString);

                    scConexion.Open();

                    using (SqlCommand scoComando = new SqlCommand())
                    {
                        scoComando.Connection = scConexion;
                        scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                        scoComando.CommandText = "usp_Cfd_Comprobante_Sel_Cobro";


                        scoComando.Parameters.AddWithValue("@nId_Cfd", sComprobantes);
                        scoComando.Parameters.AddWithValue("@nId_Contribuyente", 0);

                        xml = scoComando.ExecuteScalar().ToString();

                    }

                }
                catch (Exception ex)
                {
                    berror = true;
                    sMensajeError = Resources.resCorpusCFDIEs.varErrorConsulta;
                }
                finally
                {

                    scConexion.Close();

                    if (berror)
                    {
                        throw new Exception(sMensajeError);

                    }


                }



            }



            return xml;
        }

        #endregion
    }

    #region clases auxiliares

    class ParametrosThreadDescargaNomina
    {
        public object Session { get; set; }
        public HttpContext Conext { get; set; }
        //public ParametroConsulta ParametrosConsulta { get; set; }
        public string sComprobantes { set; get; }
        public clsInicioSesionUsuario SesionUsuario { get; set; }
    }

    class ParametroConsultaNomina
    {
        public string Receptor { get; set; }
        public string Estatus { get; set; }
        public string Sucursal { get; set; }
        public string Documento { get; set; }
        public string Serie { get; set; }
        public string FolioIni { get; set; }
        public string FolioFin { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public string UUID { get; set; }
        public string Usuario { get; set; }
    }

    #endregion
