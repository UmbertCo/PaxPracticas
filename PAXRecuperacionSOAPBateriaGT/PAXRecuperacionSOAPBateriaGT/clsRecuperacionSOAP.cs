using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace PAXRecuperacionSOAPBateriaGT
{
    class clsRecuperacionSOAP
    {
        #region Response
        private static string sResContentLength;
        private static string sResCacheControl;
        private static string sResContentType;
        private static string sResDate;
        private static string sResServer;
        private static string sResVersion;
        private static string sResPowered;
        private static string sResMvc;
        private static string sResBody;
        #endregion

        #region Request
        private static string sReqHeader;
        private static string sReqBody;
        #endregion

        private static string sUUID;
        private static XmlDocument xmlRequest;
        private static XmlDocument xmlResponse;
        private static string sNombreXML;

        public clsRecuperacionSOAP()
        {
        }

        public void recuperaNombre(string NombreXML)
        {
            sNombreXML = NombreXML;
        }

        public void recuperarResponse(string ContentLength, string CacheControl, string ContentType, string Date, string Server, string Version, string Powered, string Mvc, string Body)
        {
            sResContentLength = ContentLength;
            sResCacheControl = CacheControl;
            sResContentType = ContentType;
            sResDate = Date;
            sResServer = Server;
            sResVersion = Version;
            sResPowered = Powered;
            sResMvc = Mvc;
            sResBody = Body;
        }

        public void recuperaRequest(string Body)
        {
            sReqBody = Body;
        }

        public void recuperaRequestHeader(string Header)
        {
            sReqHeader = Header;
        }

        public void escribirSOAP()
        {
            string path = PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["RutaSOAP"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

            if (!File.Exists(path))
            {
                StreamWriter sr4 = new StreamWriter(path);
                sr4.WriteLine(sNombreXML + ".xml");
                sr4.WriteLine("************************************************************************");
                sr4.WriteLine("Request");
                sr4.WriteLine("************************************************************************");
                sr4.WriteLine("[Request Header]");
                sr4.WriteLine(sReqHeader);
                sr4.WriteLine("");
                sr4.WriteLine("[Request Body]");
                sr4.WriteLine(sReqBody);
                sr4.WriteLine("");
                sr4.WriteLine("************************************************************************");
                sr4.WriteLine("Response");
                sr4.WriteLine("************************************************************************");
                sr4.WriteLine("[Response Header]");

                if (!string.IsNullOrEmpty(sResContentLength))
                    sr4.WriteLine("Content-Length: " + sResContentLength);

                if (!string.IsNullOrEmpty(sResCacheControl))
                    sr4.WriteLine("Cache-Control: " + sResCacheControl);

                if (!string.IsNullOrEmpty(sResContentType))
                    sr4.WriteLine("Content-Type: " + sResContentType);

                if (!string.IsNullOrEmpty(sResDate))
                    sr4.WriteLine("Date: " + sResDate);

                if (!string.IsNullOrEmpty(sResServer))
                    sr4.WriteLine("Server: " + sResServer);

                if (!string.IsNullOrEmpty(sResVersion))
                    sr4.WriteLine("X-AspNet-Version: " + sResVersion);

                if (!string.IsNullOrEmpty(sResPowered))
                    sr4.WriteLine("X-Powered-By: " + sResPowered);

                if (!string.IsNullOrEmpty(sResMvc))
                    sr4.WriteLine("X-AspNetMvc-Version: " + sResMvc);

                sr4.WriteLine("");
                sr4.WriteLine("[Response Body]");
                sr4.WriteLine(sResBody);
                sr4.WriteLine("");
                sr4.WriteLine("//---------------------------------------------------------------------- //");
                sr4.WriteLine("//");
                sr4.WriteLine("//---------------------------------------------------------------------- //");
                sr4.WriteLine("");
                sr4.Close();
                Console.WriteLine(sNombreXML + " procesado.");
            }
            else
            {
                StreamWriter sr4 = new StreamWriter(path, true);
                sr4.WriteLine(sNombreXML + ".xml");
                sr4.WriteLine("************************************************************************");
                sr4.WriteLine("Request");
                sr4.WriteLine("************************************************************************");
                sr4.WriteLine("[Request Header]");
                sr4.WriteLine(sReqHeader);
                sr4.WriteLine("");
                sr4.WriteLine("[Request Body]");
                sr4.WriteLine(sReqBody);
                sr4.WriteLine("");
                sr4.WriteLine("************************************************************************");
                sr4.WriteLine("Response");
                sr4.WriteLine("************************************************************************");
                sr4.WriteLine("[Response Header]");

                if (!string.IsNullOrEmpty(sResContentLength))
                    sr4.WriteLine("Content-Length: " + sResContentLength);

                if (!string.IsNullOrEmpty(sResCacheControl))
                    sr4.WriteLine("Cache-Control: " + sResCacheControl);

                if (!string.IsNullOrEmpty(sResContentType))
                    sr4.WriteLine("Content-Type: " + sResContentType);

                if (!string.IsNullOrEmpty(sResDate))
                    sr4.WriteLine("Date: " + sResDate);

                if (!string.IsNullOrEmpty(sResServer))
                    sr4.WriteLine("Server: " + sResServer);

                if (!string.IsNullOrEmpty(sResVersion))
                    sr4.WriteLine("X-AspNet-Version: " + sResVersion);

                if (!string.IsNullOrEmpty(sResPowered))
                    sr4.WriteLine("X-Powered-By: " + sResPowered);

                if (!string.IsNullOrEmpty(sResMvc))
                    sr4.WriteLine("X-AspNetMvc-Version: " + sResMvc);

                sr4.WriteLine("");
                sr4.WriteLine("[Response Body]");
                sr4.WriteLine(sResBody);
                sr4.WriteLine("");
                sr4.WriteLine("//---------------------------------------------------------------------- //");
                sr4.WriteLine("//");
                sr4.WriteLine("//---------------------------------------------------------------------- //");
                sr4.WriteLine("");
                sr4.Close();
                Console.WriteLine(sNombreXML + " procesado.");
            }
        }       

        public void obtenerUUID(XmlDocument xml)
        {
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());

            try
            {
                nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                nsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                sUUID = xml.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsm).Value;
            }
            catch
            {
            }
        }

        public static void limpiarVariables()
        {
            sNombreXML = string.Empty;
            sResContentLength = string.Empty;
            sResCacheControl = string.Empty;
            sResContentType = string.Empty;
            sResDate = string.Empty;
            sResServer = string.Empty;
            sResVersion = string.Empty;
            sResPowered = string.Empty;
            sResMvc = string.Empty;
            sResBody = string.Empty;
            sReqHeader = string.Empty;
            sReqBody = string.Empty;
            sUUID = string.Empty;
            xmlRequest = null;
            xmlResponse = null;
        }
    }
}
