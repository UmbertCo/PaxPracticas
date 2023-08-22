using PAXRegeneracionBateriaGT.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace PAXRegeneracionBateriaGT
{
    class Program
    {
        public static string retorno()
        {
            string x;
            try
            {
                x = "try";
                return x;
            }
            finally
            {
                x = "finally";
            }
        }

        static string[] SOAP_Methods = { "SOAP(ASMX)", "SOAP12(ASMX)", "SOAPPAX001(ASMX)", "SOAPPAX00112(ASMX)", "SOAP12(SVC)" };

        static void Main(string[] args)
        {
            retorno();
  
            String SoapASMX = String.Empty;
            String Soap12ASMX = String.Empty;
            String SoapPAX001ASMX = String.Empty;
            String SoapPAX00112ASMX = String.Empty;
            String SoapSVC = String.Empty;

            XmlDocument xmldoc = new XmlDocument();

            var rutaAbsolutaAcuse = string.Empty;

            foreach (string var in SOAP_Methods)
            {
                string sLayout = string.Empty;
                Cargar_TXT(ref sLayout, "Layout_3_0.txt", DateTime.Now.ToString("s"));
                Generar_TXT(sLayout, "Layout_3_0", var);
            }

            foreach (string var in SOAP_Methods)
            {
                string sLayout = string.Empty;
                Cargar_TXT(ref sLayout, "Layout_3_2.txt", DateTime.Now.ToString("s"));
                Generar_TXT(sLayout, "Layout_3_2", var);
            }

            foreach (string var in SOAP_Methods)
            {
                string sLayout = string.Empty;
                Cargar_TXT(ref sLayout, "Layout_72HorasMas.txt", DateTime.Now.AddDays(3).ToString("s"));
                Generar_TXT(sLayout, "Layout_72HorasMas", var);
            }

            foreach (string var in SOAP_Methods)
            {
                string sLayout = string.Empty;
                Cargar_TXT(ref sLayout, "Layout_72HorasMenos.txt", DateTime.Now.AddDays(-3).ToString("s"));
                Generar_TXT(sLayout, "Layout_72HorasMenos", var);
            }

            foreach (string var in SOAP_Methods)
            {
                string sLayout = string.Empty;
                Cargar_TXT(ref sLayout, "Layout_AMP.txt", DateTime.Now.ToString("s"));
                Generar_TXT(sLayout, "Layout_AMP", var);
            }

            foreach (string var in SOAP_Methods)
            {
                string sLayout = string.Empty;
                Cargar_TXT(ref sLayout, "Layout_Pipes.txt", DateTime.Now.ToString("s"));
                Generar_TXT(sLayout, "Layout_Pipes", var);
            }

            foreach (string var in SOAP_Methods)
            {
                string sLayout = string.Empty;
                Cargar_TXT(ref sLayout, "Layout_RFCEmisor.txt", DateTime.Now.ToString("s"));
                Generar_TXT(sLayout, "Layout_RFCEmisor", var);
            }

            foreach (string var in SOAP_Methods)
            {
                string sLayout = string.Empty;
                Cargar_TXT(ref sLayout, "Layout_RFCReceptor.txt", DateTime.Now.ToString("s"));
                Generar_TXT(sLayout, "Layout_RFCReceptor", var);
            }

            foreach (string var in SOAP_Methods)
            {
                string sLayout = string.Empty;
                Cargar_TXT(ref sLayout, "Nom11.txt", DateTime.Now.ToString("s"));
                Generar_TXT(sLayout, "Nom11", var);
            }

            //-----------------------------------------------------------SOAP12
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(ASMX)+Layout_3_0");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASMX = Soap12ASMX +
            "<con:call name=\"Request 30\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTRequest\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(ASMX)+Layout_3_2");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASMX = Soap12ASMX +
            "<con:call name=\"Request 32\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTRequest\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(ASMX)+Layout_72HorasMas");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASMX = Soap12ASMX +
            "<con:call name=\"Request 72 horas mas\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTRequest\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(ASMX)+Layout_72HorasMenos");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASMX = Soap12ASMX +
            "<con:call name=\"Request 72 horas menos\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTRequest\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(ASMX)+Layout_AMP");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASMX = Soap12ASMX +
            "<con:call name=\"Request AMP\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTRequest\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(ASMX)+Layout_Pipes");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASMX = Soap12ASMX +
            "<con:call name=\"Request Pipes\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTRequest\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(ASMX)+Layout_RFCEmisor");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASMX = Soap12ASMX +
            "<con:call name=\"Request RFC Emisor\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTRequest\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(ASMX)+Layout_RFCReceptor");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASMX = Soap12ASMX +
            "<con:call name=\"Request RFC Receptor\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTRequest\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(ASMX)+Nom11");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASMX = Soap12ASMX +
            "<con:call name=\"Request Nomina\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTRequest\"/></con:call>";
            //--------------------------------------------------------------------------------------

            //-----------------------------------------------------------SOAP(ASMX)
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAP(ASMX)+Layout_3_0");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASMX = SoapASMX +
            "<con:call name=\"Request 30\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAP(ASMX)+Layout_3_2");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASMX = SoapASMX +
            "<con:call name=\"Request 32\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAP(ASMX)+Layout_72HorasMas");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASMX = SoapASMX +
            "<con:call name=\"Request 72 horas mas\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAP(ASMX)+Layout_72HorasMenos");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASMX = SoapASMX +
            "<con:call name=\"Request 72 horas menos\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAP(ASMX)+Layout_AMP");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASMX = SoapASMX +
            "<con:call name=\"Request AMP\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAP(ASMX)+Layout_Pipes");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASMX = SoapASMX +
            "<con:call name=\"Request Pipes\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAP(ASMX)+Layout_RFCEmisor");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASMX = SoapASMX +
            "<con:call name=\"Request RFC Emisor\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAP(ASMX)+Layout_RFCReceptor");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASMX = SoapASMX +
            "<con:call name=\"Request RFC Receptor\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAP(ASMX)+Nom11");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASMX = SoapASMX +
            "<con:call name=\"Request Nomina\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------

            //-----------------------------------------------------------SOAP12 PAX001
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAPPAX00112(ASMX)+Layout_3_0");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX00112ASMX = SoapPAX00112ASMX +
            "<con:call name=\"Request 30\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAPPAX00112(ASMX)+Layout_3_2");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX00112ASMX = SoapPAX00112ASMX +
            "<con:call name=\"Request 32\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAPPAX00112(ASMX)+Layout_72HorasMas");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX00112ASMX = SoapPAX00112ASMX +
            "<con:call name=\"Request 72 horas mas\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAPPAX00112(ASMX)+Layout_72HorasMenos");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX00112ASMX = SoapPAX00112ASMX +
            "<con:call name=\"Request 72 horas menos\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAPPAX00112(ASMX)+Layout_AMP");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX00112ASMX = SoapPAX00112ASMX +
            "<con:call name=\"Request AMP\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAPPAX00112(ASMX)+Layout_Pipes");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX00112ASMX = SoapPAX00112ASMX +
            "<con:call name=\"Request Pipes\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAPPAX00112(ASMX)+Layout_RFCEmisor");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX00112ASMX = SoapPAX00112ASMX +
            "<con:call name=\"Request RFC Emisor\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAPPAX00112(ASMX)+Layout_RFCReceptor");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX00112ASMX = SoapPAX00112ASMX +
            "<con:call name=\"Request RFC Receptor\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAPPAX00112(ASMX)+Nom11");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX00112ASMX = SoapPAX00112ASMX +
            "<con:call name=\"Request Nomina\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------

            //-----------------------------------------------------------SOAP PAX001 (ASMX)
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAPPAX001(ASMX)+Layout_3_0");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX001ASMX = SoapPAX001ASMX +
            "<con:call name=\"Request 30\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAPPAX001(ASMX)+Layout_3_2");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX001ASMX = SoapPAX001ASMX +
            "<con:call name=\"Request 32\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAPPAX001(ASMX)+Layout_72HorasMas");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX001ASMX = SoapPAX001ASMX +
            "<con:call name=\"Request 72 horas mas\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAPPAX001(ASMX)+Layout_72HorasMenos");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX001ASMX = SoapPAX001ASMX +
            "<con:call name=\"Request 72 horas menos\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAPPAX001(ASMX)+Layout_AMP");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX001ASMX = SoapPAX001ASMX +
            "<con:call name=\"Request AMP\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAPPAX001(ASMX)+Layout_Pipes");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX001ASMX = SoapPAX001ASMX +
            "<con:call name=\"Request Pipes\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAPPAX001(ASMX)+Layout_RFCEmisor");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX001ASMX = SoapPAX001ASMX +
            "<con:call name=\"Request RFC Emisor\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAPPAX001(ASMX)+Layout_RFCReceptor");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX001ASMX = SoapPAX001ASMX +
            "<con:call name=\"Request RFC Receptor\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAPPAX001(ASMX)+Nom11");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapPAX001ASMX = SoapPAX001ASMX +
            "<con:call name=\"Request Nomina\"><con:settings/><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://www.paxfacturacion.com.mx:454/wcfRecepcionASMXSoap/fnEnviarTXTPAX001Request\"/></con:call>";
            //--------------------------------------------------------------------------------------

            //-----------------------------------------------------------SOAP(SVC)
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(SVC)+Layout_3_0");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapSVC = SoapSVC +
            "<con:call name=\"Request 30\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcion.svc</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:authType>No Authorization</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:454/IwcfRecepcion/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(SVC)+Layout_3_2");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapSVC = SoapSVC +
            "<con:call name=\"Request 32\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcion.svc</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:authType>No Authorization</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:454/IwcfRecepcion/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(SVC)+Layout_72HorasMas");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapSVC = SoapSVC +
            "<con:call name=\"Request 72 horas mas\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcion.svc</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:authType>No Authorization</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:454/IwcfRecepcion/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(SVC)+Layout_72HorasMenos");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapSVC = SoapSVC +
            "<con:call name=\"Request 72 horas menos\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcion.svc</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:authType>No Authorization</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:454/IwcfRecepcion/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(SVC)+Layout_AMP");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapSVC = SoapSVC +
            "<con:call name=\"Request AMP\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcion.svc</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:authType>No Authorization</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:454/IwcfRecepcion/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(SVC)+Layout_Pipes");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapSVC = SoapSVC +
            "<con:call name=\"Request Pipes\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcion.svc</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:authType>No Authorization</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:454/IwcfRecepcion/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(SVC)+Layout_RFCEmisor");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapSVC = SoapSVC +
            "<con:call name=\"Request RFC Emisor\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcion.svc</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:authType>No Authorization</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:454/IwcfRecepcion/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(SVC)+Layout_RFCReceptor");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapSVC = SoapSVC +
            "<con:call name=\"Request RFC Receptor\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcion.svc</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:authType>No Authorization</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:454/IwcfRecepcion/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(SVC)+Nom11");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapSVC = SoapSVC +
            "<con:call name=\"Request Nomina\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcion.svc</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:authType>No Authorization</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:454/IwcfRecepcion/fnEnviarTXT\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------

            

            String XMLProject =
              "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +

              "<con:soapui-project activeEnvironment=\"Default\" name=\"GT_ASMX\" resourceRoot=\"\" soapui-version=\"4.6.2\" abortOnError=\"false\" runType=\"SEQUENTIAL\" xmlns:con=\"http://eviware.com/soapui/config\"><con:settings/>" +

              "<con:interface xsi:type=\"con:WsdlInterface\" wsaVersion=\"NONE\" name=\"wcfRecepcionASMXSoap12\" type=\"wsdl\" bindingName=\"{https://www.paxfacturacion.com.mx:454}wcfRecepcionASMXSoap12\" soapVersion=\"1_2\" anonymous=\"optional\" definition=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx?wsdl\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><con:settings/><con:definitionCache/><con:endpoints><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint></con:endpoints><con:operation isOneWay=\"false\" action=\"https://ws.paxfacturacion.com.mx:454/fnEnviarTXT\" name=\"fnEnviarTXT\" bindingOperationName=\"fnEnviarTXT\" type=\"Request-Response\" outputName=\"fnEnviarTXTResponse\" inputName=\"fnEnviarTXTRequest\" receivesAttachments=\"false\" sendsAttachments=\"false\" anonymous=\"optional\"><con:settings/>" +

              Soap12ASMX +

              "</con:operation>" +
              
              "<con:operation isOneWay=\"false\" action=\"https://ws.paxfacturacion.com.mx:454/fnEnviarTXTPAX001\" name=\"fnEnviarTXTPAX001\" bindingOperationName=\"fnEnviarTXTPAX001\" type=\"Request-Response\" outputName=\"fnEnviarTXTPAX001Response\" inputName=\"fnEnviarTXTPAX001Request\" receivesAttachments=\"false\" sendsAttachments=\"false\" anonymous=\"optional\"><con:settings/>" +
               
              SoapPAX00112ASMX +

              "</con:operation></con:interface>" +

              "<con:interface xsi:type=\"con:WsdlInterface\" wsaVersion=\"NONE\" name=\"wcfRecepcionASMXSoap\" type=\"wsdl\" bindingName=\"{https://www.paxfacturacion.com.mx:454}wcfRecepcionASMXSoap\" soapVersion=\"1_1\" anonymous=\"optional\" definition=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx?wsdl\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><con:settings/><con:definitionCache type=\"TEXT\" rootPart=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx?wsdl\"><con:part><con:url>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx?wsdl</con:url><con:content><![CDATA[<wsdl:definitions targetNamespace=\"https://www.paxfacturacion.com.mx:454\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\" xmlns:soap11=\"http://schemas.xmlsoap.org/wsdl/soap/\" xmlns:soap12=\"http://schemas.xmlsoap.org/wsdl/soap12/\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http/\" xmlns:mime=\"http://schemas.xmlsoap.org/wsdl/mime/\" xmlns:wsp=\"http://www.w3.org/ns/ws-policy\" xmlns:wsp200409=\"http://schemas.xmlsoap.org/ws/2004/09/policy\" xmlns:wsp200607=\"http://www.w3.org/2006/07/ws-policy\" xmlns:ns0=\"https://www.paxfacturacion.com.mx:454\">" +
              "   <wsdl:types xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
              "     <xsd:schema>" +
              "       <xsd:import schemaLocation=\"wcfrecepcionasmx.asmx.xsd1.xsd\" namespace=\"https://www.paxfacturacion.com.mx:454\"/>" +
              "     </xsd:schema>" +
              "   </wsdl:types>" +
              "   <wsdl:message name=\"fnEnviarTXTPAX001SoapIn\">" +
              "     <wsdl:part name=\"parameters\" element=\"xsns:fnEnviarTXTPAX001\" xmlns:xsns=\"https://www.paxfacturacion.com.mx:454\"/>" +
              "   </wsdl:message>" +
              "   <wsdl:message name=\"fnEnviarTXTPAX001SoapOut\">" +
              "     <wsdl:part name=\"parameters\" element=\"xsns:fnEnviarTXTPAX001Response\" xmlns:xsns=\"https://www.paxfacturacion.com.mx:454\"/>" +
              "   </wsdl:message>" +
              "   <wsdl:message name=\"fnEnviarTXTSoapIn\">" +
              "     <wsdl:part name=\"parameters\" element=\"xsns:fnEnviarTXT\" xmlns:xsns=\"https://www.paxfacturacion.com.mx:454\"/>" +
              "   </wsdl:message>" +
              "   <wsdl:message name=\"fnEnviarTXTSoapOut\">" +
              "     <wsdl:part name=\"parameters\" element=\"xsns:fnEnviarTXTResponse\" xmlns:xsns=\"https://www.paxfacturacion.com.mx:454\"/>" +
              "   </wsdl:message>" +
              "   <wsdl:portType name=\"wcfRecepcionASMXSoap\">" +
              "     <wsdl:operation name=\"fnEnviarTXT\">" +
              "       <wsdl:input name=\"fnEnviarTXTRequest\" message=\"ns0:fnEnviarTXTSoapIn\"/>" +
              "       <wsdl:output name=\"fnEnviarTXTResponse\" message=\"ns0:fnEnviarTXTSoapOut\"/>" +
              "     </wsdl:operation>" +
              "     <wsdl:operation name=\"fnEnviarTXTPAX001\">" +
              "       <wsdl:input name=\"fnEnviarTXTPAX001Request\" message=\"ns0:fnEnviarTXTPAX001SoapIn\"/>" +
              "       <wsdl:output name=\"fnEnviarTXTPAX001Response\" message=\"ns0:fnEnviarTXTPAX001SoapOut\"/>" +
              "     </wsdl:operation>" +
              "   </wsdl:portType>" +
              "   <wsdl:binding name=\"wcfRecepcionASMXSoap\" type=\"ns0:wcfRecepcionASMXSoap\">" +
              "     <soap11:binding transport=\"http://schemas.xmlsoap.org/soap/http\" style=\"document\"/>" +
              "     <wsdl:operation name=\"fnEnviarTXT\">" +
              "       <soap11:operation soapAction=\"https://ws.paxfacturacion.com.mx:454/fnEnviarTXT\" style=\"document\"/>" +
              "       <wsdl:input name=\"fnEnviarTXTRequest\">" +
              "         <soap11:body use=\"literal\"/>" +
              "       </wsdl:input>" +
              "       <wsdl:output name=\"fnEnviarTXTResponse\">" +
              "         <soap11:body use=\"literal\"/>" +
              "       </wsdl:output>" +
              "     </wsdl:operation>" +
              "     <wsdl:operation name=\"fnEnviarTXTPAX001\">" +
              "       <soap11:operation soapAction=\"https://ws.paxfacturacion.com.mx:454/fnEnviarTXTPAX001\" style=\"document\"/>" +
              "       <wsdl:input name=\"fnEnviarTXTPAX001Request\">" +
              "         <soap11:body use=\"literal\"/>" +
              "       </wsdl:input>" +
              "       <wsdl:output name=\"fnEnviarTXTPAX001Response\">" +
              "         <soap11:body use=\"literal\"/>" +
              "       </wsdl:output>" +
              "     </wsdl:operation>" +
              "   </wsdl:binding>" +
              "   <wsdl:binding name=\"wcfRecepcionASMXSoap12\" type=\"ns0:wcfRecepcionASMXSoap\">" +
              "     <soap12:binding transport=\"http://schemas.xmlsoap.org/soap/http\" style=\"document\"/>" +
              "     <wsdl:operation name=\"fnEnviarTXT\">" +
              "       <soap12:operation soapAction=\"https://ws.paxfacturacion.com.mx:454/fnEnviarTXT\" soapActionRequired=\"false\" style=\"document\"/>" +
              "       <wsdl:input name=\"fnEnviarTXTRequest\">" +
              "         <soap12:body use=\"literal\"/>" +
              "       </wsdl:input>" +
              "       <wsdl:output name=\"fnEnviarTXTResponse\">" +
              "         <soap12:body use=\"literal\"/>" +
              "       </wsdl:output>" +
              "     </wsdl:operation>" +
              "     <wsdl:operation name=\"fnEnviarTXTPAX001\">" +
              "       <soap12:operation soapAction=\"https://ws.paxfacturacion.com.mx:454/fnEnviarTXTPAX001\" soapActionRequired=\"false\" style=\"document\"/>" +
              "       <wsdl:input name=\"fnEnviarTXTPAX001Request\">" +
              "         <soap12:body use=\"literal\"/>" +
              "       </wsdl:input>" +
              "       <wsdl:output name=\"fnEnviarTXTPAX001Response\">" +
              "         <soap12:body use=\"literal\"/>" +
              "       </wsdl:output>" +
              "     </wsdl:operation>" +
              "   </wsdl:binding>" +
              "   <wsdl:service name=\"wcfRecepcionASMX\">" +
              "     <wsdl:port name=\"wcfRecepcionASMXSoap\" binding=\"ns0:wcfRecepcionASMXSoap\">" +
              "       <soap11:address location=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx\"/>" +
              "     </wsdl:port>" +
              "     <wsdl:port name=\"wcfRecepcionASMXSoap12\" binding=\"ns0:wcfRecepcionASMXSoap12\">" +
              "       <soap12:address location=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx\"/>" +
              "     </wsdl:port>" +
              "   </wsdl:service>" +
              " </wsdl:definitions>]]></con:content><con:type>http://schemas.xmlsoap.org/wsdl/</con:type></con:part><con:part><con:url>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx.xsd1.xsd</con:url><con:content><![CDATA[<s:schema elementFormDefault=\"qualified\" targetNamespace=\"https://www.paxfacturacion.com.mx:454\" xmlns:s=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://schemas.xmlsoap.org/wsdl/soap12/\" xmlns:mime=\"http://schemas.xmlsoap.org/wsdl/mime/\" xmlns:tns=\"https://www.paxfacturacion.com.mx:454\" xmlns:soap=\"http://schemas.xmlsoap.org/wsdl/soap/\" xmlns:tm=\"http://microsoft.com/wsdl/mime/textMatching/\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http/\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\">" +
              "   <s:element name=\"fnEnviarTXT\">" +
              "     <s:complexType>" +
              "       <s:sequence>" +
              "         <s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"psComprobante\" type=\"s:string\"/>" +
              "         <s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"psTipoDocumento\" type=\"s:string\"/>" +
              "         <s:element minOccurs=\"1\" maxOccurs=\"1\" name=\"pnId_Estructura\" type=\"s:int\"/>" +
              "         <s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sNombre\" type=\"s:string\"/>" +
              "         <s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sContraseña\" type=\"s:string\"/>" +
              "         <s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sVersion\" type=\"s:string\"/>" +
              "         <s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sOrigen\" type=\"s:string\"/>" +
              "       </s:sequence>" +
              "     </s:complexType>" +
              "   </s:element>" +
              "   <s:element name=\"fnEnviarTXTResponse\">" +
              "     <s:complexType>" +
              "       <s:sequence>" +
              "         <s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"fnEnviarTXTResult\" type=\"s:string\"/>" +
              "       </s:sequence>" +
              "     </s:complexType>" +
              "   </s:element>" +
              "   <s:element name=\"fnEnviarTXTPAX001\">" +
              "     <s:complexType>" +
              "       <s:sequence>" +
              "         <s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"psComprobante\" type=\"s:string\"/>" +
              "         <s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"psTipoDocumento\" type=\"s:string\"/>" +
              "         <s:element minOccurs=\"1\" maxOccurs=\"1\" name=\"pnId_Estructura\" type=\"s:int\"/>" +
              "         <s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sNombre\" type=\"s:string\"/>" +
              "         <s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sContraseña\" type=\"s:string\"/>" +
              "         <s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sVersion\" type=\"s:string\"/>" +
              "         <s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sOrigen\" type=\"s:string\"/>" +
              "       </s:sequence>" +
              "     </s:complexType>" +
              "   </s:element>" +
              "   <s:element name=\"fnEnviarTXTPAX001Response\">" +
              "     <s:complexType>" +
              "       <s:sequence>" +
              "         <s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"fnEnviarTXTPAX001Result\" type=\"tns:ArrayOfAnyType\"/>" +
              "       </s:sequence>" +
              "     </s:complexType>" +
              "   </s:element>" +
              "   <s:complexType name=\"ArrayOfAnyType\">" +
              "     <s:sequence>" +
              "       <s:element minOccurs=\"0\" maxOccurs=\"unbounded\" name=\"anyType\" nillable=\"true\"/>" +
              "     </s:sequence>" +
              "   </s:complexType>" +
              " </s:schema>]]></con:content><con:type>http://www.w3.org/2001/XMLSchema</con:type></con:part></con:definitionCache><con:endpoints><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfrecepcionasmx.asmx</con:endpoint></con:endpoints><con:operation isOneWay=\"false\" action=\"https://ws.paxfacturacion.com.mx:454/fnEnviarTXT\" name=\"fnEnviarTXT\" bindingOperationName=\"fnEnviarTXT\" type=\"Request-Response\" outputName=\"fnEnviarTXTResponse\" inputName=\"fnEnviarTXTRequest\" receivesAttachments=\"false\" sendsAttachments=\"false\" anonymous=\"optional\"><con:settings/>" +

              SoapASMX +

              "</con:operation>" +

              "<con:operation isOneWay=\"false\" action=\"https://ws.paxfacturacion.com.mx:454/fnEnviarTXTPAX001\" name=\"fnEnviarTXTPAX001\" bindingOperationName=\"fnEnviarTXTPAX001\" type=\"Request-Response\" outputName=\"fnEnviarTXTPAX001Response\" inputName=\"fnEnviarTXTPAX001Request\" receivesAttachments=\"false\" sendsAttachments=\"false\" anonymous=\"optional\"><con:settings/>" +

              SoapPAX001ASMX +

              "</con:operation></con:interface>" + 

              "<con:interface xsi:type=\"con:WsdlInterface\" wsaVersion=\"NONE\" name=\"BasicHttpBinding_IwcfRecepcion\" type=\"wsdl\" bindingName=\"{https://www.paxfacturacion.com.mx:454}BasicHttpBinding_IwcfRecepcion\" soapVersion=\"1_1\" anonymous=\"optional\" definition=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcion.svc?wsdl\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><con:settings/><con:definitionCache type=\"TEXT\" rootPart=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcion.svc?wsdl\"><con:part><con:url>https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcion.svc?wsdl</con:url><con:content><![CDATA[<wsdl:definitions targetNamespace=\"https://www.paxfacturacion.com.mx:454\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\" xmlns:soap11=\"http://schemas.xmlsoap.org/wsdl/soap/\" xmlns:soap12=\"http://schemas.xmlsoap.org/wsdl/soap12/\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http/\" xmlns:mime=\"http://schemas.xmlsoap.org/wsdl/mime/\" xmlns:wsp=\"http://www.w3.org/ns/ws-policy\" xmlns:wsp200409=\"http://schemas.xmlsoap.org/ws/2004/09/policy\" xmlns:wsp200607=\"http://www.w3.org/2006/07/ws-policy\" xmlns:ns0=\"https://www.paxfacturacion.com.mx:454\">" + 
               "  <wsdl:types xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" + 
               "    <xsd:schema>" + 
               "      <xsd:import schemaLocation=\"wcfRecepcion.svc.xsd2.xsd\" namespace=\"https://www.paxfacturacion.com.mx:454\"/>" +
               "    </xsd:schema>" + 
               "  </wsdl:types>" + 
               "  <wsdl:message name=\"IwcfRecepcion_fnEnviarTXT_InputMessage\">" + 
               "    <wsdl:part name=\"parameters\" element=\"xsns:fnEnviarTXT\" xmlns:xsns=\"https://www.paxfacturacion.com.mx:454\"/>" +
               "  </wsdl:message>" + 
               "  <wsdl:message name=\"IwcfRecepcion_fnEnviarTXT_OutputMessage\">" + 
               "    <wsdl:part name=\"parameters\" element=\"xsns:fnEnviarTXTResponse\" xmlns:xsns=\"https://www.paxfacturacion.com.mx:454\"/>" +
               "  </wsdl:message>" + 
               "  <wsdl:portType name=\"IwcfRecepcion\">" + 
               "    <wsdl:operation name=\"fnEnviarTXT\">" + 
               "      <wsdl:input name=\"fnEnviarTXTRequest\" message=\"ns0:IwcfRecepcion_fnEnviarTXT_InputMessage\"/>" +
               "      <wsdl:output name=\"fnEnviarTXTResponse\" message=\"ns0:IwcfRecepcion_fnEnviarTXT_OutputMessage\"/>" +
               "    </wsdl:operation>" + 
               "  </wsdl:portType>" + 
               "  <wsdl:binding name=\"BasicHttpBinding_IwcfRecepcion\" type=\"ns0:IwcfRecepcion\">" + 
               "    <soap11:binding transport=\"http://schemas.xmlsoap.org/soap/http\" style=\"document\"/>" +
               "    <wsdl:operation name=\"fnEnviarTXT\">" + 
               "      <soap11:operation soapAction=\"https://ws.paxfacturacion.com.mx:454/IwcfRecepcion/fnEnviarTXT\" style=\"document\"/>" +
               "      <wsdl:input name=\"fnEnviarTXTRequest\">" + 
               "        <soap11:body use=\"literal\"/>" +
               "      </wsdl:input>" + 
               "      <wsdl:output name=\"fnEnviarTXTResponse\">" + 
               "        <soap11:body use=\"literal\"/>" +
               "      </wsdl:output>" + 
               "    </wsdl:operation>" + 
               "  </wsdl:binding>" + 
               "  <wsdl:service name=\"wcfRecepcion\">" + 
               "    <wsdl:port name=\"BasicHttpBinding_IwcfRecepcion\" binding=\"ns0:BasicHttpBinding_IwcfRecepcion\">" + 
               "      <wsp:PolicyReference URI=\"#policy0\" xmlns:wsp=\"http://schemas.xmlsoap.org/ws/2004/09/policy\"/>" +
               "      <soap11:address location=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcion.svc\"/>" +
               "    </wsdl:port>" + 
               "  </wsdl:service>" + 
               "  <wsp:Policy wsu:Id=\"policy0\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:wsp=\"http://schemas.xmlsoap.org/ws/2004/09/policy\">" + 
               "    <wsp:ExactlyOne>" + 
               "      <wsp:All>" + 
               "        <sp:TransportBinding xmlns:sp=\"http://schemas.xmlsoap.org/ws/2005/07/securitypolicy\" xmlns:wsx=\"http://schemas.xmlsoap.org/ws/2004/09/mex\" xmlns:wsa10=\"http://www.w3.org/2005/08/addressing\" xmlns:tns=\"https://www.paxfacturacion.com.mx:454\" xmlns:wsap=\"http://schemas.xmlsoap.org/ws/2004/08/addressing/policy\" xmlns:msc=\"http://schemas.microsoft.com/ws/2005/12/wsdl/contract\" xmlns:wsa=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsam=\"http://www.w3.org/2007/05/addressing/metadata\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:soap=\"http://schemas.xmlsoap.org/wsdl/soap/\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\">" + 
               "          <wsp:Policy>" + 
               "            <wsp:ExactlyOne>" + 
               "              <wsp:All>" + 
               "                <sp:TransportToken>" + 
               "                  <wsp:Policy>" + 
               "                    <wsp:ExactlyOne>" + 
               "                      <wsp:All>" + 
               "                        <sp:HttpsToken RequireClientCertificate=\"false\"/>" +
               "                      </wsp:All>" + 
               "                    </wsp:ExactlyOne>" + 
               "                  </wsp:Policy>" + 
               "                </sp:TransportToken>" + 
               "                <sp:AlgorithmSuite>" + 
               "                  <wsp:Policy>" + 
               "                    <wsp:ExactlyOne>" + 
               "                      <wsp:All>" + 
               "                        <sp:Basic256/>" +
               "                      </wsp:All>" + 
               "                    </wsp:ExactlyOne>" + 
               "                  </wsp:Policy>" + 
               "                </sp:AlgorithmSuite>" + 
               "                <sp:Layout>" + 
               "                  <wsp:Policy>" + 
               "                    <wsp:ExactlyOne>" + 
               "                      <wsp:All>" + 
               "                        <sp:Strict/>" +
               "                      </wsp:All>" + 
               "                    </wsp:ExactlyOne>" + 
               "                  </wsp:Policy>" + 
               "                </sp:Layout>" + 
               "              </wsp:All>" + 
               "            </wsp:ExactlyOne>" + 
               "          </wsp:Policy>" + 
               "        </sp:TransportBinding>" + 
               "      </wsp:All>" + 
               "    </wsp:ExactlyOne>" + 
               "  </wsp:Policy>" + 
               "</wsdl:definitions>]]></con:content><con:type>http://schemas.xmlsoap.org/wsdl/</con:type></con:part><con:part><con:url>https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcion.svc.xsd2.xsd</con:url><con:content><![CDATA[<xs:schema elementFormDefault=\"qualified\" targetNamespace=\"https://www.paxfacturacion.com.mx:454\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:tns=\"https://www.paxfacturacion.com.mx:454\">" + 
               "  <xs:element name=\"fnEnviarTXT\">" + 
               "    <xs:complexType>" + 
               "      <xs:sequence>" + 
               "        <xs:element minOccurs=\"0\" name=\"psComprobante\" nillable=\"true\" type=\"xs:string\"/>" +
               "        <xs:element minOccurs=\"0\" name=\"psTipoDocumento\" nillable=\"true\" type=\"xs:string\"/>" +
               "        <xs:element minOccurs=\"0\" name=\"pnId_Estructura\" type=\"xs:int\"/>" +
               "        <xs:element minOccurs=\"0\" name=\"sNombre\" nillable=\"true\" type=\"xs:string\"/>" +
               "        <xs:element minOccurs=\"0\" name=\"sContraseña\" nillable=\"true\" type=\"xs:string\"/>" +
               "        <xs:element minOccurs=\"0\" name=\"sVersion\" nillable=\"true\" type=\"xs:string\"/>" +
               "        <xs:element minOccurs=\"0\" name=\"sOrigen\" nillable=\"true\" type=\"xs:string\"/>" +
               "      </xs:sequence>" + 
               "    </xs:complexType>" + 
               "  </xs:element>" + 
               "  <xs:element name=\"fnEnviarTXTResponse\">" + 
               "    <xs:complexType>" + 
               "      <xs:sequence>" + 
               "        <xs:element minOccurs=\"0\" name=\"fnEnviarTXTResult\" nillable=\"true\" type=\"xs:string\"/>" +
               "      </xs:sequence>" + 
               "    </xs:complexType>" + 
               "  </xs:element>" + 
               "</xs:schema>]]></con:content><con:type>http://www.w3.org/2001/XMLSchema</con:type></con:part></con:definitionCache><con:endpoints><con:endpoint>https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcion.svc</con:endpoint></con:endpoints><con:operation isOneWay=\"false\" action=\"https://ws.paxfacturacion.com.mx:454/IwcfRecepcion/fnEnviarTXT\" name=\"fnEnviarTXT\" bindingOperationName=\"fnEnviarTXT\" type=\"Request-Response\" outputName=\"fnEnviarTXTResponse\" inputName=\"fnEnviarTXTRequest\" receivesAttachments=\"false\" sendsAttachments=\"false\" anonymous=\"optional\"><con:settings/>" +

              SoapSVC +

              "</con:operation></con:interface>" +
              
              "<con:properties/><con:wssContainer/><con:sensitiveInformation/></con:soapui-project>";

            xmldoc.LoadXml(XMLProject);
            xmldoc.Save(string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAPUI/", "Proyecto"));
          
        }

        private static void Cargar_TXT(ref string psDocument, string psNombre, string psFecha)
        {
            string sFecha = string.Empty;
            string[] aLayout = null;

            psDocument = System.IO.File.ReadAllText(Settings.Default.ArchivoTXT + psNombre);

            int nIndiceFecha = psDocument.IndexOf("fecha@");
            int nIndicePipeFecha = psDocument.IndexOf("|", nIndiceFecha);

            sFecha = psDocument.Substring(nIndiceFecha, nIndicePipeFecha - nIndiceFecha);

            aLayout = sFecha.Split('@');

            System.Threading.Thread.Sleep(1000);
            psDocument = psDocument.Replace(aLayout[1], psFecha);
        }

        private static void Generar_TXT(string psDocument, string psNombre, string psSoap_type)
        {
            if (psSoap_type == "SOAP(ASMX)")
            {                    
                String SOAP =
                "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:pax=\"https://www.paxfacturacion.com.mx:454\">" +
                    "<soapenv:Header/>" +
                        "<soapenv:Body>" +
                            "<pax:fnEnviarTXT>" +
                                "<pax:psComprobante>" + psDocument + "</pax:psComprobante>" +
                                "<pax:psTipoDocumento>factura</pax:psTipoDocumento>" +
                                "<pax:pnId_Estructura>0</pax:pnId_Estructura>" +
                                "<pax:sNombre>paxgeneracion</pax:sNombre>" +
                                "<pax:sContraseña>wpTCnMSfxI/CvsOlxKnEnsSDxJEJIcO1wr4pW8K6wqF4wpbvvpvvvqbvvrbvvorvvZ7vvZrvvLfvvLrvvbY=</pax:sContraseña>" +
                                "<pax:sVersion>3.2</pax:sVersion>" +
                                "<pax:sOrigen>GT</pax:sOrigen>" +
                            "</pax:fnEnviarTXT>" +
                        "</soapenv:Body>" +
                "</soapenv:Envelope>";

                if (psNombre.Equals("Layout_AMP"))
                    SOAP = SOAP.Replace("&", "&amp;");

                var rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAP(ASMX)+" + psNombre);
                GuardarArchivoTexto(rutaAbsolutaAcuse, SOAP);
            }

            if (psSoap_type == "SOAP12(ASMX)")
            {
                String SOAP12 =
                    "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:pax=\"https://www.paxfacturacion.com.mx:454\">" +
                        "<soap:Header/>" +
                            "<soap:Body>" +
                                "<pax:fnEnviarTXT>" +
                                    "<pax:psComprobante>" + psDocument + "</pax:psComprobante>" +
                                    "<pax:psTipoDocumento>factura</pax:psTipoDocumento>" +
                                    "<pax:pnId_Estructura>0</pax:pnId_Estructura>" +
                                    "<pax:sNombre>paxgeneracion</pax:sNombre>" +
                                    "<pax:sContraseña>wpTCnMSfxI/CvsOlxKnEnsSDxJEJIcO1wr4pW8K6wqF4wpbvvpvvvqbvvrbvvorvvZ7vvZrvvLfvvLrvvbY=</pax:sContraseña>" +
                                    "<pax:sVersion>3.2</pax:sVersion>" +
                                    "<pax:sOrigen>GT</pax:sOrigen>" +
                                "</pax:fnEnviarTXT>" +
                            "</soap:Body>" +
                    "</soap:Envelope>";

                if (psNombre.Equals("Layout_AMP"))
                    SOAP12 = SOAP12.Replace("&", "&amp;");

                var rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(ASMX)+" + psNombre);
                GuardarArchivoTexto(rutaAbsolutaAcuse, SOAP12);
            }

            if (psSoap_type == "SOAPPAX001(ASMX)")
            {
                String SOAPPAXX001 =
                "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:pax=\"https://www.paxfacturacion.com.mx:454\">" +
                    "<soapenv:Header/>" +
                        "<soapenv:Body>" +
                            "<pax:fnEnviarTXTPAX001>" +
                                "<pax:psComprobante>" + psDocument + "</pax:psComprobante>" +
                                "<pax:psTipoDocumento>factura</pax:psTipoDocumento>" +
                                "<pax:pnId_Estructura>0</pax:pnId_Estructura>" +
                                "<pax:sNombre>paxgeneracion</pax:sNombre>" +
                                "<pax:sContraseña>wpTCnMSfxI/CvsOlxKnEnsSDxJEJIcO1wr4pW8K6wqF4wpbvvpvvvqbvvrbvvorvvZ7vvZrvvLfvvLrvvbY=</pax:sContraseña>" +
                                "<pax:sVersion>3.2</pax:sVersion>" +
                                "<pax:sOrigen>GT</pax:sOrigen>" +
                            "</pax:fnEnviarTXTPAX001>" +
                        "</soapenv:Body>" +
                "</soapenv:Envelope>";

                if (psNombre.Equals("Layout_AMP"))
                    SOAPPAXX001 = SOAPPAXX001.Replace("&", "&amp;");

                var rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP/", "SOAPPAX001(ASMX)+" + psNombre);
                GuardarArchivoTexto(rutaAbsolutaAcuse, SOAPPAXX001);
            }

            if (psSoap_type == "SOAPPAX00112(ASMX)")
            {
                String SOAPPAX00112 =
                    "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:pax=\"https://www.paxfacturacion.com.mx:454\">" +
                        "<soap:Header/>" +
                            "<soap:Body>" +
                                "<pax:fnEnviarTXTPAX001>" +
                                    "<pax:psComprobante>" + psDocument + "</pax:psComprobante>" +
                                    "<pax:psTipoDocumento>factura</pax:psTipoDocumento>" +
                                    "<pax:pnId_Estructura>0</pax:pnId_Estructura>" +
                                    "<pax:sNombre>paxgeneracion</pax:sNombre>" +
                                    "<pax:sContraseña>wpTCnMSfxI/CvsOlxKnEnsSDxJEJIcO1wr4pW8K6wqF4wpbvvpvvvqbvvrbvvorvvZ7vvZrvvLfvvLrvvbY=</pax:sContraseña>" +
                                    "<pax:sVersion>3.2</pax:sVersion>" +
                                    "<pax:sOrigen>GT</pax:sOrigen>" +
                                "</pax:fnEnviarTXTPAX001>" +
                            "</soap:Body>" +
                    "</soap:Envelope>";

                if (psNombre.Equals("Layout_AMP"))
                    SOAPPAX00112 = SOAPPAX00112.Replace("&", "&amp;");

                var rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAPPAX00112(ASMX)+" + psNombre);
                GuardarArchivoTexto(rutaAbsolutaAcuse, SOAPPAX00112);
            }

            if (psSoap_type == "SOAP12(SVC)")
            {
                String SOAPSVC12 =
                    "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:pax=\"https://www.paxfacturacion.com.mx:454\">" +
                        "<soapenv:Header/>" +
                            "<soapenv:Body>" +
                                "<pax:fnEnviarTXT>" +
                                    "<pax:psComprobante>" + psDocument + "</pax:psComprobante>" +
                                    "<pax:psTipoDocumento>factura</pax:psTipoDocumento>" +
                                    "<pax:pnId_Estructura>0</pax:pnId_Estructura>" +
                                    "<pax:sNombre>paxgeneracion</pax:sNombre>" +
                                    "<pax:sContraseña>wpTCnMSfxI/CvsOlxKnEnsSDxJEJIcO1wr4pW8K6wqF4wpbvvpvvvqbvvrbvvorvvZ7vvZrvvLfvvLrvvbY=</pax:sContraseña>" +
                                    "<pax:sVersion>3.2</pax:sVersion>" +
                                    "<pax:sOrigen>GT</pax:sOrigen>" +
                                "</pax:fnEnviarTXT>" +
                            "</soapenv:Body>" +
                    "</soapenv:Envelope>";

                if (psNombre.Equals("Layout_AMP"))
                    SOAPSVC12 = SOAPSVC12.Replace("&", "&amp;");

                var rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaArchivos + "/SOAP12/", "SOAP12(SVC)+" + psNombre);
                GuardarArchivoTexto(rutaAbsolutaAcuse, SOAPSVC12);
            }

            var rutaAcuse = string.Format("{0}\\{1}.txt", Settings.Default.RutaArchivos, psNombre);
            GuardarArchivoTexto(rutaAcuse, psDocument);


        }

        static string ObtenerNoCertificado(X509Certificate2 certEmisor)
        {
            byte[] bCertificadoInvertido = certEmisor.GetSerialNumber().Reverse().ToArray();
            return Encoding.Default.GetString(bCertificadoInvertido);
        }

        public static void GuardarArchivoTexto(string rutaAbsoluta, string contenidoArchivo)
        {
            File.WriteAllText(rutaAbsoluta, contenidoArchivo);
        }
    }
}
