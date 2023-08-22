using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using mshtml;
using System.Text.RegularExpressions;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;

namespace P_DescargarComprobantesBOT.webForms
{


    public partial class frmLogin : Form
    {
      public String sScriptAccessLogin;
      public String sScriptSaveLogin;
      public String sScriptNavigateFecha;
      public String sScriptBuscarInfo;
      public String sScriptDescarga;
      public String sJQuery_2_2_4;
      public String sJQuery_3_0_0;                
      public String sRutaExe = "";
      public String sRutaLog = "";
      public String sRutaXML = "";
      public String sRfc = //"POVH580318BH6";
       "CFA110411FW5";
      public String sPass = "";
      public clsConsulta cConsulta;
      public XmlDocument xdLog;
      public XPathNavigator xpnNodoRaiz;
      public bool bModoLogin = true;
      public bool bConsulta = false;
      public DataTable tblResultado;

        DateTime dtFechaIni = DateTime.Parse("2016-01-01");
        DateTime dtFechaFin = DateTime.Parse("2017-01-01");

        string sEntrada = DateTime.Now.ToString("HHmm");

        public void fnCargaScripts() 
        {

           sScriptAccessLogin = File.ReadAllText(sRutaExe + Path.DirectorySeparatorChar + "Scripts/sScriptAccessLogin.js", Encoding.UTF8);
           sScriptSaveLogin = File.ReadAllText(sRutaExe + Path.DirectorySeparatorChar + "Scripts/sScriptSaveLogin.js", Encoding.UTF8);
           sScriptNavigateFecha = File.ReadAllText(sRutaExe + Path.DirectorySeparatorChar + "Scripts/sScriptNavigateFecha.js", Encoding.UTF8);
           sScriptBuscarInfo = File.ReadAllText(sRutaExe + Path.DirectorySeparatorChar + "Scripts/sScriptBuscarInfo.js", Encoding.UTF8);
           sScriptDescarga = File.ReadAllText(sRutaExe + Path.DirectorySeparatorChar + "Scripts/sScriptDescarga.js", Encoding.UTF8);
           sJQuery_2_2_4 = File.ReadAllText(sRutaExe + Path.DirectorySeparatorChar + "Scripts/jquery-2.2.4.min.js", Encoding.UTF8);
           sJQuery_3_0_0 = File.ReadAllText(sRutaExe + Path.DirectorySeparatorChar + "Scripts/jquery-3.0.0.min.js", Encoding.UTF8);
        
        }

        public void fnInjectScripts() 
        {
            if(!String.IsNullOrEmpty(wbLogin.Url.ToString()) ){
            HtmlElement htmlBody = wbLogin.Document.GetElementsByTagName("body")[0];

            HtmlElement htmlScript = wbLogin.Document.CreateElement("script");

            htmlScript.SetAttribute("type", "text/javascript");
            IHTMLScriptElement iScript = (IHTMLScriptElement)htmlScript.DomElement;

            iScript.text = txtScript.Text;

            htmlBody.AppendChild(htmlScript);

            //wbLogin.Document.InvokeScript(
            }
        
        }

        public void fnInjectScripts(String psTexto)
        {
            if (!String.IsNullOrEmpty(wbLogin.Url.ToString()))
            {
                HtmlElement htmlBody = wbLogin.Document.GetElementsByTagName("body")[0];

                HtmlElement htmlScript = wbLogin.Document.CreateElement("script");

                htmlScript.SetAttribute("type", "text/javascript");
                IHTMLScriptElement iScript = (IHTMLScriptElement)htmlScript.DomElement;

                iScript.text = psTexto; 

                htmlBody.AppendChild(htmlScript);

                //wbLogin.Document.InvokeScript(
            }

        }

        public frmLogin()
        {
            InitializeComponent();

            sRutaExe = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sRutaXML = sRutaExe + Path.DirectorySeparatorChar + "XMLs";
            fnCargaScripts();
  
            bConsulta = false;

        }

        public frmLogin(clsConsulta pcConsulta)
        {
            InitializeComponent();

          cConsulta = pcConsulta;
          sRutaExe = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
          fnCargaScripts();
          dtFechaIni = pcConsulta.dtFechaIni;
          dtFechaFin = pcConsulta.dtFechaFin;
          bConsulta = pcConsulta.bConsulta;


        }

        public static void fnAgregarEntrada(ref XPathNavigator xpniNodoLocal,string sUrl,string sDoc) 
        {

            xpniNodoLocal.AppendChild("<nav hora='' doc=''></nav>");

            xpniNodoLocal.SelectSingleNode("nav[@hora='']/@hora").SetValue(DateTime.Now.ToString("HH:mm:ss"));

            xpniNodoLocal.SelectSingleNode("nav[@doc='']/@doc").SetValue(sDoc);
        
        }

        private void wbLogin_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {


            if (e.Url.ToString().Contains("https://cfdiau.sat.gob.mx/"))
            {
                if (bModoLogin)
                {
                    String sLogin = sScriptAccessLogin;

                    String stxtLogin = File.ReadAllText(sRutaExe + Path.DirectorySeparatorChar + sRfc.ToUpper() + ".txt", Encoding.UTF8);

                    // String saux = fnObtenerValoresRegex(@"Pass=.+", stxtLogin);

                    sPass = PAXCrypto.CryptoAES.DesencriptaAES64(fnObtenerValoresRegex(@"Pass=.+", stxtLogin).Split('=')[1]);


                    sLogin = sLogin.Replace("@rfc", sRfc);

                    sLogin = sLogin.Replace("@pass", sPass);

                    fnInjectScripts(sLogin);


                }
                else
                {
                    fnInjectScripts(sScriptSaveLogin);
                }


            }

            if (e.Url.ToString().Contains("https://portalcfdi.facturaelectronica.sat.gob.mx/ConsultaEmisor.aspx")) 
            {

                fnNavegarxFecha(dtFechaIni, dtFechaFin);
            // fnInjectScripts(sScriptRecolectarInfo);
            
            }

            //else 
            //{

            //    fnNavegarxFecha(dtFechaIni, dtFechaFin);

            
            //}
            
        }

        private void wbLogin_DockChanged(object sender, EventArgs e)
        {
          //  if (!Directory.Exists(sRutaExe + Path.DirectorySeparatorChar + "prueba"))
          //      Directory.CreateDirectory(sRutaExe + Path.DirectorySeparatorChar + "prueba");
          //
          //  string sDoc = DateTime.Now.ToString("HHmmss") + ".html";
          //
          //  File.WriteAllText(sRutaExe + Path.DirectorySeparatorChar + "prueba" + Path.DirectorySeparatorChar + sDoc, wbLogin.DocumentText);
          //
          //  fnAgregarEntrada(ref xpnNodoRaiz, wbLogin.Url.ToString(), sDoc);
          //



                        

        }

        private void wbLogin_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (wbLogin.Url.ToString().Contains("https://cfdiau.sat.gob.mx/"))
            {
                if (bModoLogin)
                {

                
                
                }
                else
                {

                    HtmlElementCollection Inputs = wbLogin.Document.GetElementsByTagName("input");

                    foreach (HtmlElement htmlInput in Inputs)
                    {

                        String sName = htmlInput.GetAttribute("name");

                        if (sName.Contains("Ecom_Password"))
                        {
                            HtmlElement htmlPass = wbLogin.Document.GetElementById("pass");
                            HtmlElement htmlRFC = wbLogin.Document.GetElementById("rfc");

                            if (htmlPass == null || htmlRFC == null)
                                continue;

                            //XmlDocument xdPass = new XmlDocument();
                            //xdPass.LoadXml(htmlPass.OuterHtml);
                            //
                            //XmlDocument xdRFC = new XmlDocument();
                            //xdRFC.LoadXml(htmlRFC.OuterHtml);

                            String Pass = "", RFC = "";

                            RFC = fnObtenerValoresRegex(@"value=\w+", htmlRFC.OuterHtml).Split('=')[1]; //xdRFC.CreateNavigator().SelectSingleNode("/input/@value").Value;
                            Pass = fnObtenerValoresRegex(@"value=\w+", htmlPass.OuterHtml).Split('=')[1];// xdPass.CreateNavigator().SelectSingleNode("/input/@value").Value;

                            File.WriteAllText(sRutaExe + Path.DirectorySeparatorChar + RFC + ".txt", "RFC=" + RFC + Environment.NewLine + "Pass=" + PAXCrypto.CryptoAES.EncriptarAES64(sPass));
                        }


                    }
                }


            }


            if (e.Url.ToString().Contains("https://www.google.com.mx/")) 
            {
                e.Cancel = true;

                HtmlElement htmlDivPaginas = wbLogin.Document.GetElementById("DivPaginas");
     

                    tblResultado = fnProcesarDivPaginas(htmlDivPaginas);




                    foreach (DataRow drRenglon in tblResultado.Rows)
                    {

                        string sUrl = drRenglon[0].ToString();
                        string sRutaArchivoAux = sRutaExe + "\\archivoAux.xml";
                        // wbLogin.Url = new Uri(sUrl);

                       FileInfo fiArchivo =   DownloadFile(sUrl,sRutaArchivoAux);

                       String sXml =  File.ReadAllText(sRutaArchivoAux);


                       try
                       {
                           XmlDocument xdDoc = new XmlDocument();

                           xdDoc.LoadXml(sXml);

                          // XmlNamespaceManager xnsm = new XmlNamespaceManager(xdDoc.NameTable);
                          // xnsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                          // xnsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                           String sNombreArchivo = fnObtenerValoresRegex("[a-f0-9]{8}-[a-f0-9]{4}-4[a-f0-9]{3}-[89aAbB][a-f0-9]{3}-[a-f0-9]{12}", sXml);

                           if (String.IsNullOrEmpty(sNombreArchivo)) { continue; }

                           xdDoc.Save(sRutaXML + Path.DirectorySeparatorChar + sNombreArchivo + ".xml");

                       }
                       catch 
                       {
                       
                       
                       
                       }


                       Thread.Sleep(100);

                       File.Delete(sRutaArchivoAux);
                    }



                return;
            }

            if (e.Url.ToString().Contains("cargarligas")) 
            {
                e.Cancel = true;

                HtmlElement htmlRutasArchivos = wbLogin.Document.GetElementById("RutasArchivos");
            
              HttpWebRequest httpReq = (HttpWebRequest)HttpWebRequest.Create(e.Url);
            
              httpReq.CookieContainer = new CookieContainer();
            
              httpReq.CookieContainer.SetCookies(e.Url, wbLogin.Document.Cookie);
            
             HttpWebResponse httpResponse = (HttpWebResponse)  httpReq.GetResponse();

                

              // WebClient wb = new WebClient();
              //
              // 
              //
              // wb.Headers.Add(HttpRequestHeader.Cookie, wbLogin.Document.Cookie);
              // wb.DownloadDataCompleted += new DownloadDataCompletedEventHandler(wb_DownloadDataCompleted);
              //
              // wb.DownloadDataAsync(e.Url);
            }

        }

        void wb_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            File.WriteAllBytes(sRutaExe + "\\" + DateTime.Now.ToString("HHmmssfff") + ".txt", e.Result);
            
        }

        public FileInfo DownloadFile(string url, string destinationFullPathWithName)
        {
            URLDownloadToFile(null, url, destinationFullPathWithName, 0, IntPtr.Zero);
            return new FileInfo(destinationFullPathWithName);
        }

        [DllImport("urlmon.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern Int32 URLDownloadToFile(
            [MarshalAs(UnmanagedType.IUnknown)] object callerPointer,
            [MarshalAs(UnmanagedType.LPWStr)] string url,
            [MarshalAs(UnmanagedType.LPWStr)] string filePathWithName,
            Int32 reserved,
            IntPtr callBack);

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookieEx(
            string url,
            string cookieName,
            StringBuilder cookieData,
            ref int size,
            Int32 dwFlags,
            IntPtr lpReserved);

        private const Int32 InternetCookieHttponly = 0x2000;

        /// <summary>
        /// Gets the URI cookie container.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static CookieContainer GetUriCookieContainer(Uri uri)
        {
            CookieContainer cookies = null;
            // Determine the size of the cookie
            int datasize = 8192 * 16;
            StringBuilder cookieData = new StringBuilder(datasize);
            if (!InternetGetCookieEx(uri.ToString(), null, cookieData, ref datasize, InternetCookieHttponly, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;
                // Allocate stringbuilder large enough to hold the cookie
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookieEx(
                    uri.ToString(),
                    null, cookieData,
                    ref datasize,
                    InternetCookieHttponly,
                    IntPtr.Zero))
                    return null;
            }
            if (cookieData.Length > 0)
            {
                cookies = new CookieContainer();
                cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
            }
            return cookies;
        }

        public DataTable fnProcesarDivPaginas(HtmlElement phtmlDivPaginas) 
        {
            DataTable tblConsulta = new DataTable();

            tblConsulta.Columns.Add("LinkDescarga");
            tblConsulta.Columns.Add("Folio Fiscal");
            tblConsulta.Columns.Add("RFC Emisor");
            tblConsulta.Columns.Add("Nombre o Razón Social del Emisor");
            tblConsulta.Columns.Add("RFC Receptor");
            tblConsulta.Columns.Add("Nombre o Razón Social del Receptor");
            tblConsulta.Columns.Add("Fecha de Emisión");
            tblConsulta.Columns.Add("Fecha de Certificación");
            tblConsulta.Columns.Add("PAC que Certificó");
            tblConsulta.Columns.Add("Total");
            tblConsulta.Columns.Add("Efecto del Comprobante");
            tblConsulta.Columns.Add("Estado del Comprobante");

            HtmlElementCollection  htmlRenglones =   phtmlDivPaginas.GetElementsByTagName("tr");


                foreach (HtmlElement htmlTR in htmlRenglones)  
                {

                    HtmlElementCollection htmlRenTD = htmlTR.GetElementsByTagName("td");

                    tblConsulta.Rows.Add(fnObtenerLinkDescarga(htmlRenTD[0].InnerHtml)
                   ,htmlRenTD[1].InnerText
                   ,htmlRenTD[2].InnerText
                   ,htmlRenTD[3].InnerText
                   ,htmlRenTD[4].InnerText
                   ,htmlRenTD[5].InnerText
                   ,htmlRenTD[6].InnerText
                   ,htmlRenTD[7].InnerText
                   ,htmlRenTD[8].InnerText
                   ,htmlRenTD[9].InnerText
                   ,htmlRenTD[10].InnerText
                   ,htmlRenTD[11].InnerText);
             
                
                }

                return tblConsulta;
            
            }
       
        public string fnObtenerLinkDescarga(String sHtmlOnclick)
        {
            String sLink = "";

            try
            {
                sLink = fnObtenerValoresRegex("'RecuperaCfdi\\.aspx\\?.+',", sHtmlOnclick);

                sLink = sLink.Replace("'RecuperaCfdi.aspx","https://portalcfdi.facturaelectronica.sat.gob.mx/RecuperaCfdi.aspx");

               sLink = sLink.Replace("',", "");
            }
            catch 
            { }
            return sLink;
        }

        public String fnObtenerValoresRegex(String psExpresion,String psCadena) 
        {

            Regex reg = new Regex(psExpresion, RegexOptions.IgnoreCase);

            Match mMatch = reg.Match(psCadena);


            if (mMatch.Success) return mMatch.Value;
            


            return "";  
        }

        public void fnNavegarxFecha(DateTime pdtFechaIni, DateTime pdtFechafin) 
        {
            String sNavegarFecha = sScriptNavigateFecha;

            sNavegarFecha = sNavegarFecha.Replace("@fechaini", pdtFechaIni.ToString("dd/MM/yyyy"));
            sNavegarFecha = sNavegarFecha.Replace("@fechafin", pdtFechafin.ToString("dd/MM/yyyy"));

            fnInjectScripts(sNavegarFecha);
            fnInjectScripts(sScriptBuscarInfo);
        }

        private void wbLogin_FileDownload(object sender, EventArgs e)
        {

        }

        private void wbLogin_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {

        }

        private void wbLogin_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void wbLogin_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url.ToString().Contains("https://cfdiau.sat.gob.mx/"))
            {


                HtmlElementCollection Inputs = wbLogin.Document.GetElementsByTagName("input");

                

                //HtmlElement input =
                //e.Cancel = true ;
            }


   
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void wbLogin_RegionChanged(object sender, EventArgs e)
        {  

        }

        private void wbLogin_Validating(object sender, CancelEventArgs e)
        {
           
           

        }

        private void txtScript_KeyUp(object sender, KeyEventArgs e)
       {
            if (e.KeyCode == Keys.Enter) 
            {

                fnInjectScripts();
            
            }
        }

        private void txtUrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                wbLogin.Navigate(txtUrl.Text);

                txtUrl.Text = wbLogin.Url.ToString();
            }
        }

        private void frmLogin_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.H)
            {
                txtScript.Visible = !txtScript.Visible;
            
            }
        }

   

    }
}
