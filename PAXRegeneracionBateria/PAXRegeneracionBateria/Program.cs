//--------------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Xml;
using System.Net;
using System.Web;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Threading;
using System.Net.Security;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Security.Cryptography;
using PAXRegeneracionBateria.Properties;
using System.Security.Cryptography.X509Certificates;
using PAXRegeneracionBateria.HSM;
//--------------------------------------------------------------------------------------------------
namespace PAXRegeneracionBateria {
    
    //----------------------------------------------------------------------------------------------
    class Program {
        
        //------------------------------------------------------------------------------------------
        public enum AlgoritmoSellado {
            /*MD5,*/ SHA1, SHA256
        }
        
        //------------------------------------------------------------------------------------------
        public static String retorno() {
            string x;

            try {
                x = "try";
                return x;
            }
            finally {
                x = "finally";
            }
        }
        
        //------------------------------------------------------------------------------------------
        private static xmCryptoService hsm;
        private static XmlDocument docNodoTimbre;
        private static clsValCertificado gCertificado;
        private static clsOperacionTimbradoSellado gTimbrado;
        private static TimbreFiscalDigital gNodoTimbre = new TimbreFiscalDigital();
        
        //------------------------------------------------------------------------------------------
        private static String[] SOAP_Methods = { "SOAP(ASMX)", "SOAP12(ASMX)", "SOAP(ASPEL)", "SOAP12(ASPEL)", "SOAP12(SVC)" };
     
        //------------------------------------------------------------------------------------------
        static void Main(String[] args)
        {
            //--------------------------------------------------------------------------------------
            retorno();
            //--------------------------------------------------------------------------------------
            Console.WriteLine("Timbrar Bateria: [S]/[N]");
            String timbrado = Console.ReadLine();
            //--------------------------------------------------------------------------------------
            XmlDocument xDocument = new XmlDocument();
            X509Certificate2 certCSD = new X509Certificate2();
            XPathNavigator navNodoTimbre = xDocument.CreateNavigator();
            //--------------------------------------------------------------------------------------
            String sello = String.Empty;
            String sCadenaOriginal = String.Empty;
            //--------------------------------------------------------------------------------------
            XslCompiledTransform xslt = null;
            XPathNavigator navDocGenera = null;
            XmlNamespaceManager nsmComprobante = null;
            XsltArgumentList argss = new XsltArgumentList();
            //--------------------------------------------------------------------------------------
            String SoapSVC = String.Empty;
            String SoapASMX = String.Empty;
            String SoapASPEL = String.Empty;
            String Soap12ASMX = String.Empty;
            String Soap12ASPEL = String.Empty;
            //--------------------------------------------------------------------------------------
            String ContextoGeN = "/RecepcionGeneracionTimbrado";
            String ContextoTiM = "/RecepcionTimbrado";
            String ContextoVaL = "/RecepcionValidacion";
            //--------------------------------------------------------------------------------------
            #region Jmeter
            String Jmeter = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><jmeterTestPlan version=\"1.2\" properties=\"2.7\" jmeter=\"2.12 r1636949\"><hashTree><TestPlan guiclass=\"TestPlanGui\" testclass=\"TestPlan\" testname=\"Plan de Pruebas\" enabled=\"true\">" +
                "<stringProp name=\"TestPlan.comments\"></stringProp><boolProp name=\"TestPlan.functional_mode\">false</boolProp><boolProp name=\"TestPlan.serialize_threadgroups\">false</boolProp><elementProp name=\"TestPlan.user_defined_variables\" elementType=\"Arguments\" guiclass=\"ArgumentsPanel\" testclass=\"Arguments\" testname=\"Variables definidas por el Usuario\" enabled=\"true\">" +
                "<collectionProp name=\"Arguments.arguments\"/></elementProp><stringProp name=\"TestPlan.user_define_classpath\"></stringProp></TestPlan><hashTree><ThreadGroup guiclass=\"ThreadGroupGui\" testclass=\"ThreadGroup\" testname=\"Grupo de Hilos 10.54.134.171-10.54.57.207\" enabled=\"true\">" +
                "<stringProp name=\"ThreadGroup.on_sample_error\">continue</stringProp><elementProp name=\"ThreadGroup.main_controller\" elementType=\"LoopController\" guiclass=\"LoopControlPanel\" testclass=\"LoopController\" testname=\"Controlador Bucle\" enabled=\"true\"><boolProp name=\"LoopController.continue_forever\">false</boolProp>" +
                "<stringProp name=\"LoopController.loops\">1</stringProp></elementProp><stringProp name=\"ThreadGroup.num_threads\">1</stringProp><stringProp name=\"ThreadGroup.ramp_time\">1</stringProp><longProp name=\"ThreadGroup.start_time\">1375295756000</longProp><longProp name=\"ThreadGroup.end_time\">1375295756000</longProp>" +
                "<boolProp name=\"ThreadGroup.scheduler\">false</boolProp><stringProp name=\"ThreadGroup.duration\"></stringProp><stringProp name=\"ThreadGroup.delay\"></stringProp></ThreadGroup><hashTree><ResultCollector guiclass=\"SummaryReport\" testclass=\"ResultCollector\" testname=\"Reporte resumen\" enabled=\"true\">" +
                "<boolProp name=\"ResultCollector.error_logging\">false</boolProp><objProp><name>saveConfig</name><value class=\"SampleSaveConfiguration\"><time>true</time><latency>true</latency><timestamp>true</timestamp><success>true</success><label>true</label><code>true</code>" +
                "<message>true</message><threadName>true</threadName><dataType>true</dataType><encoding>false</encoding><assertions>true</assertions><subresults>true</subresults><responseData>false</responseData><samplerData>false</samplerData><xml>false</xml>" +
                "<fieldNames>false</fieldNames><responseHeaders>false</responseHeaders><requestHeaders>false</requestHeaders><responseDataOnError>false</responseDataOnError><saveAssertionResultsFailureMessage>false</saveAssertionResultsFailureMessage><assertionsResultsToSave>0</assertionsResultsToSave>" +
                "<bytes>true</bytes><threadCounts>true</threadCounts></value></objProp><stringProp name=\"filename\"></stringProp></ResultCollector><hashTree/><ResultCollector guiclass=\"ViewResultsFullVisualizer\" testclass=\"ResultCollector\" testname=\"Ver Árbol de Resultados\" enabled=\"true\">" +
                "<boolProp name=\"ResultCollector.error_logging\">false</boolProp><objProp><name>saveConfig</name><value class=\"SampleSaveConfiguration\"><time>true</time><latency>true</latency><timestamp>true</timestamp><success>true</success><label>true</label><code>true</code>" +
                "<message>true</message><threadName>true</threadName><dataType>true</dataType><encoding>false</encoding><assertions>true</assertions><subresults>true</subresults><responseData>false</responseData><samplerData>false</samplerData><xml>false</xml><fieldNames>false</fieldNames>" +
                "<responseHeaders>false</responseHeaders><requestHeaders>false</requestHeaders><responseDataOnError>false</responseDataOnError><saveAssertionResultsFailureMessage>false</saveAssertionResultsFailureMessage><assertionsResultsToSave>0</assertionsResultsToSave>" +
                "<bytes>true</bytes><threadCounts>true</threadCounts></value></objProp><stringProp name=\"filename\"></stringProp></ResultCollector><hashTree/>";
            //--------------------------------------------------------------------------------------
            String JmeterSello = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><jmeterTestPlan version=\"1.2\" properties=\"2.7\" jmeter=\"2.12 r1636949\"><hashTree><TestPlan guiclass=\"TestPlanGui\" testclass=\"TestPlan\" testname=\"Plan de Pruebas\" enabled=\"true\">" +
                "<stringProp name=\"TestPlan.comments\"></stringProp><boolProp name=\"TestPlan.functional_mode\">false</boolProp><boolProp name=\"TestPlan.serialize_threadgroups\">false</boolProp><elementProp name=\"TestPlan.user_defined_variables\" elementType=\"Arguments\" guiclass=\"ArgumentsPanel\" testclass=\"Arguments\" testname=\"Variables definidas por el Usuario\" enabled=\"true\">" +
                "<collectionProp name=\"Arguments.arguments\"/></elementProp><stringProp name=\"TestPlan.user_define_classpath\"></stringProp></TestPlan><hashTree><ThreadGroup guiclass=\"ThreadGroupGui\" testclass=\"ThreadGroup\" testname=\"Grupo de Hilos 10.54.134.171-10.54.57.207\" enabled=\"true\">" +
                "<stringProp name=\"ThreadGroup.on_sample_error\">continue</stringProp><elementProp name=\"ThreadGroup.main_controller\" elementType=\"LoopController\" guiclass=\"LoopControlPanel\" testclass=\"LoopController\" testname=\"Controlador Bucle\" enabled=\"true\"><boolProp name=\"LoopController.continue_forever\">false</boolProp>" +
                "<stringProp name=\"LoopController.loops\">1</stringProp></elementProp><stringProp name=\"ThreadGroup.num_threads\">1</stringProp><stringProp name=\"ThreadGroup.ramp_time\">1</stringProp><longProp name=\"ThreadGroup.start_time\">1375295756000</longProp><longProp name=\"ThreadGroup.end_time\">1375295756000</longProp>" +
                "<boolProp name=\"ThreadGroup.scheduler\">false</boolProp><stringProp name=\"ThreadGroup.duration\"></stringProp><stringProp name=\"ThreadGroup.delay\"></stringProp></ThreadGroup><hashTree><ResultCollector guiclass=\"SummaryReport\" testclass=\"ResultCollector\" testname=\"Reporte resumen\" enabled=\"true\">" +
                "<boolProp name=\"ResultCollector.error_logging\">false</boolProp><objProp><name>saveConfig</name><value class=\"SampleSaveConfiguration\"><time>true</time><latency>true</latency><timestamp>true</timestamp><success>true</success><label>true</label><code>true</code>" +
                "<message>true</message><threadName>true</threadName><dataType>true</dataType><encoding>false</encoding><assertions>true</assertions><subresults>true</subresults><responseData>false</responseData><samplerData>false</samplerData><xml>false</xml>" +
                "<fieldNames>false</fieldNames><responseHeaders>false</responseHeaders><requestHeaders>false</requestHeaders><responseDataOnError>false</responseDataOnError><saveAssertionResultsFailureMessage>false</saveAssertionResultsFailureMessage><assertionsResultsToSave>0</assertionsResultsToSave>" +
                "<bytes>true</bytes><threadCounts>true</threadCounts></value></objProp><stringProp name=\"filename\"></stringProp></ResultCollector><hashTree/><ResultCollector guiclass=\"ViewResultsFullVisualizer\" testclass=\"ResultCollector\" testname=\"Ver Árbol de Resultados\" enabled=\"true\">" +
                "<boolProp name=\"ResultCollector.error_logging\">false</boolProp><objProp><name>saveConfig</name><value class=\"SampleSaveConfiguration\"><time>true</time><latency>true</latency><timestamp>true</timestamp><success>true</success><label>true</label><code>true</code>" +
                "<message>true</message><threadName>true</threadName><dataType>true</dataType><encoding>false</encoding><assertions>true</assertions><subresults>true</subresults><responseData>false</responseData><samplerData>false</samplerData><xml>false</xml><fieldNames>false</fieldNames>" +
                "<responseHeaders>false</responseHeaders><requestHeaders>false</requestHeaders><responseDataOnError>false</responseDataOnError><saveAssertionResultsFailureMessage>false</saveAssertionResultsFailureMessage><assertionsResultsToSave>0</assertionsResultsToSave>" +
                "<bytes>true</bytes><threadCounts>true</threadCounts></value></objProp><stringProp name=\"filename\"></stringProp></ResultCollector><hashTree/>";
            //--------------------------------------------------------------------------------------
            String JmeterValidacion = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><jmeterTestPlan version=\"1.2\" properties=\"2.7\" jmeter=\"2.12 r1636949\"><hashTree><TestPlan guiclass=\"TestPlanGui\" testclass=\"TestPlan\" testname=\"Plan de Pruebas\" enabled=\"true\">" +
                "<stringProp name=\"TestPlan.comments\"></stringProp><boolProp name=\"TestPlan.functional_mode\">false</boolProp><boolProp name=\"TestPlan.serialize_threadgroups\">false</boolProp><elementProp name=\"TestPlan.user_defined_variables\" elementType=\"Arguments\" guiclass=\"ArgumentsPanel\" testclass=\"Arguments\" testname=\"Variables definidas por el Usuario\" enabled=\"true\">" +
                "<collectionProp name=\"Arguments.arguments\"/></elementProp><stringProp name=\"TestPlan.user_define_classpath\"></stringProp></TestPlan><hashTree><ThreadGroup guiclass=\"ThreadGroupGui\" testclass=\"ThreadGroup\" testname=\"Grupo de Hilos 10.54.134.171-10.54.57.207\" enabled=\"true\">" +
                "<stringProp name=\"ThreadGroup.on_sample_error\">continue</stringProp><elementProp name=\"ThreadGroup.main_controller\" elementType=\"LoopController\" guiclass=\"LoopControlPanel\" testclass=\"LoopController\" testname=\"Controlador Bucle\" enabled=\"true\"><boolProp name=\"LoopController.continue_forever\">false</boolProp>" +
                "<stringProp name=\"LoopController.loops\">1</stringProp></elementProp><stringProp name=\"ThreadGroup.num_threads\">1</stringProp><stringProp name=\"ThreadGroup.ramp_time\">1</stringProp><longProp name=\"ThreadGroup.start_time\">1375295756000</longProp><longProp name=\"ThreadGroup.end_time\">1375295756000</longProp>" +
                "<boolProp name=\"ThreadGroup.scheduler\">false</boolProp><stringProp name=\"ThreadGroup.duration\"></stringProp><stringProp name=\"ThreadGroup.delay\"></stringProp></ThreadGroup><hashTree><ResultCollector guiclass=\"SummaryReport\" testclass=\"ResultCollector\" testname=\"Reporte resumen\" enabled=\"true\">" +
                "<boolProp name=\"ResultCollector.error_logging\">false</boolProp><objProp><name>saveConfig</name><value class=\"SampleSaveConfiguration\"><time>true</time><latency>true</latency><timestamp>true</timestamp><success>true</success><label>true</label><code>true</code>" +
                "<message>true</message><threadName>true</threadName><dataType>true</dataType><encoding>false</encoding><assertions>true</assertions><subresults>true</subresults><responseData>false</responseData><samplerData>false</samplerData><xml>false</xml>" +
                "<fieldNames>false</fieldNames><responseHeaders>false</responseHeaders><requestHeaders>false</requestHeaders><responseDataOnError>false</responseDataOnError><saveAssertionResultsFailureMessage>false</saveAssertionResultsFailureMessage><assertionsResultsToSave>0</assertionsResultsToSave>" +
                "<bytes>true</bytes><threadCounts>true</threadCounts></value></objProp><stringProp name=\"filename\"></stringProp></ResultCollector><hashTree/><ResultCollector guiclass=\"ViewResultsFullVisualizer\" testclass=\"ResultCollector\" testname=\"Ver Árbol de Resultados\" enabled=\"true\">" +
                "<boolProp name=\"ResultCollector.error_logging\">false</boolProp><objProp><name>saveConfig</name><value class=\"SampleSaveConfiguration\"><time>true</time><latency>true</latency><timestamp>true</timestamp><success>true</success><label>true</label><code>true</code>" +
                "<message>true</message><threadName>true</threadName><dataType>true</dataType><encoding>false</encoding><assertions>true</assertions><subresults>true</subresults><responseData>false</responseData><samplerData>false</samplerData><xml>false</xml><fieldNames>false</fieldNames>" +
                "<responseHeaders>false</responseHeaders><requestHeaders>false</requestHeaders><responseDataOnError>false</responseDataOnError><saveAssertionResultsFailureMessage>false</saveAssertionResultsFailureMessage><assertionsResultsToSave>0</assertionsResultsToSave>" +
                "<bytes>true</bytes><threadCounts>true</threadCounts></value></objProp><stringProp name=\"filename\"></stringProp></ResultCollector><hashTree/>";
            #endregion
            //--------------------------------------------------------------------------------------
            var rutaAbsolutaAcuse = String.Empty;
            XmlDocument xmldoc = new XmlDocument();
            String Nombre = String.Empty;
            String Status = String.Empty;

            var rutaGen = String.Empty;
            //--------------------------------------------------------------------------------------
            Boolean Prod = Settings.Default.Productivos;

            //--------------------------------------------------------------------------------------
            #region Version 3.2
            //--------------------------------------------------------------------------------------            

            #region ComercioExterior11

            //#region

            //Nombre = "CCE_11-Correcta"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-Correcta.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-Correcta", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-Correcta_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-101"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-101.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-101", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-101_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-102"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-102.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("17:05:23T1090-03-14");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-102", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-102_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-103"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-103.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-103", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-103_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-104"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-104.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-104", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-104_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-105"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-105.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-105", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-105_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-106"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-106.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-106", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-106_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-107"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-107.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-107", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-107_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-108"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-108.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-108", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-108_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-109"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-109.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-109", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-109_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-110"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-110.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-110", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-110_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-111"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-111.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-111", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-111_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-112"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-112.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-112", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-112_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-113"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-113.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-113", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-113_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-114"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-114.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-114", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-114_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-115"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-115.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-115", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-115_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-116"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-116.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-116", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-116_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-117"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-117.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-117", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-117_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-118"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-118.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-118", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-118_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-119"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-119.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-119", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-119_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-120"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-120.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-120", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-120_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-121"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-121.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-121", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-121_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-122"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-122.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-122", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-122_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-123"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-123.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-123", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-123_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-124"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-124.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-124", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-124_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-125"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-125.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-125", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-125_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-126"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-126.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-126", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-126_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-127"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-127.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-127", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-127_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-128"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-128.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-128", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-128_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-129"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-129.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-129", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-129_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-130"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-130.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-130", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-130_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-131"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-131.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-131", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-131_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-132"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-132.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-132", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-132_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-133"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-133.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-133", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-133_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-134"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-134.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-134", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-134_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-135"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-135.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-135", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-135_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-136"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-136.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-136", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-136_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-137"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-137.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-137", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-137_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-138"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-138.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-138", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-138_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-139"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-139.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-139", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-139_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-140"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-140.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-140", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-140_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-141_1"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-141_1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-141_1", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-141_1_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-141_2"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-141_2.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-141_2", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-141_2_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-142_1"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-142_1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-142_1", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-142_1_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-142_2"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-142_2.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-142_2", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-142_2_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-143_1"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-143_1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-143_1", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-143_1_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-143_2"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-143_2.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-143_2", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-143_2_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-144_1"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-144_1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-144_1", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-144_1_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-144_2"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-144_2.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-144_2", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-144_2_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-153"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-153.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-153", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-153_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-154"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-154.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-154", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-154_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            ////Nombre = "CCE_11-155-1"; Status = "_OK";
            //////----------------------------------------
            ////foreach (string var in SOAP_Methods)
            ////{
            ////    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-155-1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            ////    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            ////    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-155-1", "1.0", false, "N", "N", "NA", false, var, false, false);
            ////    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-155-1_Tim", "false", DateTime.Now.ToString(), Prod);
            ////} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-155"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-155.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-155", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-155_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            ////Nombre = "CCE_11-155-2"; Status = "_OK";
            //////----------------------------------------
            ////foreach (string var in SOAP_Methods)
            ////{
            ////    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-155-2.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            ////    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            ////    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-155-2", "1.0", false, "N", "N", "NA", false, var, false, false);
            ////    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-155-2_Tim", "false", DateTime.Now.ToString(), Prod);
            ////} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-156"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-156.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-156", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-156_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-158"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-158.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-158", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-158_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-159"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-159.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-159", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-159_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-160"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-160.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-160", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-160_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-161"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-161.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-161", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-161_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-161(Correcto)"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-161(Correcto).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-161(Correcto)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-161(Correcto)_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-162"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-162.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-162", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-162_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-163"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-163.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-163", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-163_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-164"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-164.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-164", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-164_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-165"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-165.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-165", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-165_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-166"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-166.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-166", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-166_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-174"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-174.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-174", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-174_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-175"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-175.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-175", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-175_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-177"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-177.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-177", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-177_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-178"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-178.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-178", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-178_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-179"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-179.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-179", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-179_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-180"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-180.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-180", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-180_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-188"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-188.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-188", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-188_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-189"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-189.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-189", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-189_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-190"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-190.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-190", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-190_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-191"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-191.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-191", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-191_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-192"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-192.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-192", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-192_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-193"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-193.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-193", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-193_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-194"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-194.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-194", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-194_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-195"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-195.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-195", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-195_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-196"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-196.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-196", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-196_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-197"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-197.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-197", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-197_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-198"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-198.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-198", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-198_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-199"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-199.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-199", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-199_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-200"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-200.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-200", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-200_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-201"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-201.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-201", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-201_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-202"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-202.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-202", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-202_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-203"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-203.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-203", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-203_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-204"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-204.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-204", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-204_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-205"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-205.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-205", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-205_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-206"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-206.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-206", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-206_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-207"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-207.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-207", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-207_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-208"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-208.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-208", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-208_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-209"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-209.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-209", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-209_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-210"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-210.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-210", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-210_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-211"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-211.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-211", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-211_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-213"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-213.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-213", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-213_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-214"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-214.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-214", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-214_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-215"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-215.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-215", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-215_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-216"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-216.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-216", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-216_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-217"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-217.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-217", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-217_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "CCE_11-218"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, @"CCE11\CCE_11-218.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE_11-218", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE_11-218_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //#endregion

            #endregion

            #region ComercioExterior
            //Nombre = "ComercioExterior"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ComercioExterior.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ComercioExterior", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ComercioExterior_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "ComercioExterior_01";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ComercioExterior_01.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ComercioExterior_01", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ComercioExterior_01_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "ComercioExterior_02";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ComercioExterior_02.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ComercioExterior_02", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ComercioExterior_02_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "ComercioExterior_03";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ComercioExterior_03.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ComercioExterior_03", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ComercioExterior_03_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "ComercioExterior_04";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ComercioExterior_04.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ComercioExterior_04", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ComercioExterior_04_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "ComercioExterior_05";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ComercioExterior_05.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ComercioExterior_05", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ComercioExterior_05_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);


            //Nombre = "CCE140";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE140.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE140", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE140_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE141";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE141.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE141", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE141_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE142";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE142.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE142", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE142_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE143";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE143.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE143", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE143_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE144";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE144.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE144", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE144_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE145";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE145.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE145", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE145_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE146";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE146.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE146", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE146_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE147";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE147.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE147", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE147_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE148";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE148.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE148", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE148_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE149";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE149.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE149", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE149_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE150";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE150.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE150", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE150_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE151";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE151.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE151", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE151_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE152";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE152.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE152", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE152_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE153";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE153.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE153", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE153_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE154";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE154.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE154", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE154_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE155";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE155.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE155", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE155_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE156";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE156.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE156", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE156_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE157";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE157.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE157", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE157_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE158";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE158.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE158", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE158_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE159";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE159.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE159", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE159_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE160";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE160.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE160", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE160_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE161";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE161.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE161", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE161_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE162";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE162.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE162", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE162_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE163";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE163.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE163", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE163_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE164";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE164.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE164", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE164_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE165";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE165.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE165", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE165_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE166";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE166.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE166", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE166_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE167";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE167.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE167", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE167_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE168";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE168.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE168", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE168_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE169";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE169.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE169", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE169_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE170";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE170.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE170", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE170_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE171";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE171.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE171", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE171_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE172";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE172.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE172", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE172_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE173";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE173.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE173", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE173_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE174";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE174.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE174", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE174_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE175";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE175.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE175", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE175_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE176";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE176.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE176", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE176_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE177";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE177.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE177", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE177_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE178";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE178.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE178", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE178_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE179";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE179.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE179", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE179_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE180";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE180.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE180", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE180_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE181";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE181.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE181", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE181_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE182";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE182.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE182", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE182_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE183";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE183.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE183", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE183_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE184";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE184.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE184", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE184_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE185";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE185.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE185", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE185_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE186";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE186.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE186", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE186_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE187";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE187.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE187", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE187_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE188";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE188.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE188", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE188_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE189";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE189.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE189", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE189_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE190";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE190.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE190", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE190_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            //Nombre = "CCE191";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CCE191.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CCE191", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CCE191_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            #endregion

            #region INE
            //Nombre = "INE_Nemecio_2"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE_Nemecio_2.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE_Nemecio_2", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE_Nemecio_2_Tim", "false", DateTime.Now.ToString(), Prod);
            //}

            //Nombre = "INE_SINDICO"; Status = "_OK";
            //----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE_SINDICO.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE_SINDICO", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE_SINDICO_Tim", "false", DateTime.Now.ToString(), Prod);
            //}

            //Nombre = "INE_Ernesto_Murgia_2"; Status = "_OK";
            //----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE_Ernesto_Murgia_2.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE_Ernesto_Murgia_2", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE_Ernesto_Murgia_2_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            #endregion

            #region ECC11
            //Nombre = "ECC11(121)"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ECC11(121).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(1.08).ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ECC11(121)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ECC11(121)_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "ECC11(122)"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ECC11(122).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(1.08).ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ECC11(122)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ECC11(122)_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "ECC11(123)"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ECC11(123).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(1.08).ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ECC11(123)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ECC11(123)_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "ECC11(124)"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ECC11(124).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(1.08).ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ECC11(124)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ECC11(124)_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "ECC11(125)"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ECC11(125).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(1.08).ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ECC11(125)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ECC11(125)_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "ECC11(126)"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ECC11(126).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(1.08).ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ECC11(126)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ECC11(126)_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);
            #endregion

            #region INE

            //Nombre = "INE(2)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(2).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2016-08-25T12:00:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(2)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(2)_Tim", "ine10", "2016-08-25T12:00:00", Prod);
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "INE(180)_1";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(180)_1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(180)_1", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(180)_1_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "INE(181)_1";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(181)_1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(181)_1", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(181)_1_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);


            //Nombre = "INE(182)_1";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(182)_1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(182)_1", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(182)_1_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "INE(183)_1";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(183)_1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(183)_1", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(183)_1_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "INE(184)_1";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(184)_1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(184)_1", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(184)_1_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "INE(185)_1";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(185)_1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(185)_1", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(185)_1_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "INE(186)_1";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(186)_1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(186)_1", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(186)_1_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "INE(187)_1";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(187)_1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(187)_1", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(187)_1_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "INE(180)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(180).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(180)", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(180)_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "INE(181)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(181).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(181)", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(181)_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);


            //Nombre = "INE(182)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(182).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(182)", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(182)_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "INE(183)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(183).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(183)", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(183)_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "INE(184)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(184).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(184)", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(184)_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "INE(185)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(185).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(185)", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(185)_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "INE(186)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(186).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(186)", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(186)_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "INE(187)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(187).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(187)", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(187)_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            #endregion

            #region Pago Incorrecto

            //Nombre = "MetodoPago"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "MetodoPago.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "MetodoPago", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "MetodoPago_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "MetodoPagoIncorrecto"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "MetodoPagoIncorrecto.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "MetodoPagoIncorrecto", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "MetodoPagoIncorrecto_Tim", "false", DateTime.Now.ToString(), Prod);
            //} GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);


            //Nombre = "INE_INE"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE_INE.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE_INE", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE_INE_Tim", "false", DateTime.Now.ToString(), Prod);
            //}

            #endregion

            #region Nomina Bateria
            //#region Bateria Datapower

            //Nombre = "Nomina12(TipoRegimen)"; Status = "_OK";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(TipoRegimen).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(TipoRegimen)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(TipoRegimen)_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "NOM176_PruebaCorrecta"; Status = "_OK";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "NOM176_PruebaCorrecta.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "NOM176_PruebaCorrecta", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "NOM176_PruebaCorrecta_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "NOM176_PruebaIncorrecta"; Status = "_OK";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "NOM176_PruebaIncorrecta.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "NOM176_PruebaIncorrecta", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "NOM176_PruebaIncorrecta_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12"; Status = "_OK";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(Productiva)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(Productiva).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(Productiva)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(Productiva)_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(NoCuenta000000)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(NoCuenta000000).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(NoCuenta000000)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(NoCuenta000000)_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);


            //Nombre = "Nomina12(NoAntiguedad)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(NoAntiguedad).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(NoAntiguedad)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(NoAntiguedad)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(NoRiesgoPuesto)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(NoRiesgoPuesto).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(NoRiesgoPuesto)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(NoRiesgoPuesto)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(NoTipoJornada)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(NoTipoJornada).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(NoTipoJornada)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(NoTipoJornada)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(NoFechaInicioRel)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(NoFechaInicioRel).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(NoFechaInicioRel)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(NoFechaInicioRel)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(PercRepetida)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(PercRepetida).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(PercRepetida)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(PercRepetida)_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(Rep)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(Rep).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(Rep)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(Rep)_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Replica(189)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Replica(189).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Replica(189)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Replica(189)_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Replica(193)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Replica(193).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Replica(193)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Replica(193)_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);


            ////Nombre = "Nomina12Correcta";
            //////--------------------------------------------------------------------------------------
            ////foreach (string var in SOAP_Methods)
            ////{
            ////    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12Correcta.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            ////    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            ////    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12Correcta", "1.0", false, "N", "N", "NA", false, var, false, false);
            ////    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12Correcta_Tim", "false", DateTime.Now.ToString(), Prod);
            ////    break;
            ////}
            //////--------------------------------------------------------------------------------------
            ////GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12Cliente";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12Cliente.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12Cliente", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12Cliente_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(NumDiasPagados)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(NumDiasPagados).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(NumDiasPagados)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(NumDiasPagados)_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(NumAñosServicio)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(NumAñosServicio).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(NumAñosServicio)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(NumAñosServicio)_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(PeriodicidadPago10)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(PeriodicidadPago10).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(PeriodicidadPago10)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(PeriodicidadPago10)_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(TipoDeduccion)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(TipoDeduccion).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(TipoDeduccion)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(TipoDeduccion)_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(TipoPercepcion)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(TipoPercepcion).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(TipoPercepcion)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(TipoPercepcion)_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(Antiguedad)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(Antiguedad).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(Antiguedad)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(Antiguedad)_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(Asimilados)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(Asimilados).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(Asimilados)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(Asimilados)_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(NoCuentaBancaria)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(NoCuentaBancaria).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(NoCuentaBancaria)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(NoCuentaBancaria)_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(211Bug)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(211Bug).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(211Bug)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(211Bug)_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(221Bug)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(221Bug).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(221Bug)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(221Bug)_Tim", "false", DateTime.Now.ToString(), Prod);

            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(622)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(622).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(622)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(622)_Tim", "false", DateTime.Now.ToString(), Prod);
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(101)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(101).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s") + "Z");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(101)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(101)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(102)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(102).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(102)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(102)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(103)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(103).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(103)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(103)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(104)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(104).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(104)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(104)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(105)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(105).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(105)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(105)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(106)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(106).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(106)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(106)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(107)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(107).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(107)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(107)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(108)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(108).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(108)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(108)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(109)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(109).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(109)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(109)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(110)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(110).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(110)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(110)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(111)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(111).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(111)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(111)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(112-1)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(112-1).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(112-1)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(112-1)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(112-2)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(112-2).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(112-2)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(112-2)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(112-3)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(112-3).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(112-3)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(112-3)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(112-4)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(112-4).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(112-4)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(112-4)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(112-5)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(112-5).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(112-5)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(112-5)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(112-6)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(112-6).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(112-6)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(112-6)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(113)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(113).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(113)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(113)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(114)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(114).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(114)", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(114)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(115)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(115).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(115)", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(115)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(116-1)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(116-1).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(116-1)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(116-1)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(116-2)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(116-2).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(116-2)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(116-2)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(117)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(117).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(117)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(117)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(118)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(118).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(118)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(118)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(119)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(119).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(119)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(119)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(120)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(120).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(120)", "1.0", false, "N", "N", "BAJ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(120)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(121)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(121).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(121)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(121)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(122)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(122).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(122)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(122)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(123)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(123).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(123)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(123)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(124)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(124).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(124)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(124)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(125)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(125).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(125)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(125)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(126)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(126).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(126)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(126)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(127)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(127).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(127)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(127)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(128)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(128).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(128)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(128)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(129)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(129).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(129)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(129)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(130)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(130).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(130)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(130)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(131-1)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(131-1).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(131-1)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(131-1)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(131-2)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(131-2).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(131-2)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(131-2)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(150)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(150).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(150)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(150)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(151)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(151).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(151)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(151)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(152)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(152).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(152)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(152)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(153)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(153).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(153)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(153)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(154)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(154).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(154)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(154)_Tim", "false", DateTime.Now.ToString(), Prod); break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(155)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(155).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(155)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(155)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(156)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(156).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(156)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(156)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(157)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(157).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(157)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(157)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(158)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(158).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(158)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(158)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(159)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(159).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(159)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(159)_Tim", "false", DateTime.Now.ToString(), Prod); break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(160)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(160).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(160)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(160)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(161)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(161).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(161)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(161)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(162)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(162).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(162)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(162)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(163)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(163).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(163)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(163)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(164)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(164).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(164)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(164)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(165)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(165).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(165)", "1.0", false, "N", "N", "LAN", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(165)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(166)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(166).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(166)", "1.0", false, "N", "N", "PZA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(166)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(167)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(167).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(167)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(167)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(168)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(168).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(168)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(168)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(169)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(169).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(169)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(169)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(170)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(170).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(170)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(170)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(171)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(171).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(171)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(171)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(172)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(172).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(172)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(172)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(173)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(173).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(173)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(173)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(174)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(174).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(174)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(174)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(175)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(175).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(175)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(175)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(176)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(176).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(176)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(176)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(177)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(177).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(177)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(177)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(178)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(178).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(178)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(178)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(179)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(179).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(179)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(179)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(180)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(180).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(180)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(180)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(181)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(181).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(181)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(181)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(182)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(182).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(182)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(182)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(183)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(183).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(183)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(183)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(184)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(184).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(184)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(184)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(185)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(185).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(185)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(185)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(186)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(186).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(186)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(186)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(187)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(187).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(187)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(187)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(188)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(188).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(188)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(188)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(189)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(189).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(189)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(189)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(190)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(190).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(190)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(190)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(191)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(191).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(191)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(191)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(192)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(192).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(192)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(192)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(193)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(193).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(193)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(193)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(194)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(194).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(194)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(194)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(195)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(195).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(195)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(195)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(196)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(196).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(196)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(196)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(197)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(197).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(197)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(197)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(198)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(198).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(198)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(198)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(199)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(199).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(199)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(199)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(200)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(200).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(200)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(200)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(201)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(201).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(201)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(201)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(202)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(202).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(202)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(202)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(203)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(203).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(203)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(203)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(204)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(204).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(204)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(204)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(205)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(205).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(205)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(205)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(206)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(206).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(206)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(206)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(207)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(207).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(207)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(207)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(208)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(208).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(208)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(208)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(209)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(209).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(209)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(209)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(210)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(210).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(210)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(210)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(211)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(211).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(211)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(211)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(212)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(212).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(212)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(212)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(213)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(213).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(213)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(213)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(214)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(214).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(214)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(214)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(215)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(215).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(215)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(215)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(216)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(216).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(216)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(216)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(217)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(217).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(217)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(217)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(218)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(218).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(218)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(218)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(219)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(219).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(219)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(219)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(220)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(220).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(220)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(220)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(221)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(221).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(221)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(221)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(222)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(222).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(222)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(222)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(223)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(223).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(223)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(223)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(224)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(224).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(224)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(224)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(225)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(225).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(225)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(225)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(225-1)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(225-1).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(225-1)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(225-1)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(225-2)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(225-2).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(225-2)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(225-2)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Nomina12(225-3)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12(225-3).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12(225-3)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12(225-3)_Tim", "false", DateTime.Now.ToString(), Prod);
            //    break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //#endregion
            #endregion

            #region DP

            #region Bateria Datapower

            Nombre = "65Minutos"; Status = "_OK";
            //----------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Normal.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(1.08).ToString("s"));
                //navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2017-12-31T23:59:59");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "65Minutos", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "65Minutos_Tim", "false", DateTime.Now.ToString(), Prod); 
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "PAX11731"; Status = "_OK";
            //----------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "PAX11731.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(0).ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "PAX11731", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "PAX11731_Tim", "false", DateTime.Now.ToString(), Prod); 
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "PAX32"; Status = "_OK";
            //----------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "PAX32.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(0).ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "PAX32", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "PAX32_Tim", "false", DateTime.Now.ToString(), Prod);
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "PAX11732"; Status = "_OK";
            //----------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "PAX11732.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(0).ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "PAX11732", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "PAX11732_Tim", "false", DateTime.Now.ToString(), Prod);
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "PAX11733"; Status = "_OK";
            //----------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "PAX11733.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(0).ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "PAX11733", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "PAX11733_Tim", "false", DateTime.Now.ToString(), Prod);
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "PAX11734"; Status = "_OK";
            //----------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "PAX11734.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(0).ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "PAX11734", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "PAX11734_Tim", "false", DateTime.Now.ToString(), Prod);
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Comprobante268"; Status = "_OK";
            ////----------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Comprobante268.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(0).ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Comprobante268", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Comprobante268_Tim", "false", DateTime.Now.ToString(), Prod); break;
            //}ñ
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "65Minutos+";
            //----------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Normal.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(02.08).ToString("s"));
                //navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(01.58).ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "65Minutos+", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "65Minutos+_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Addenda";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Addenda.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Addenda", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Addenda_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "INE(1.1)";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(1).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(1.1)", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(1.1)_Tim", "false", DateTime.Now.ToString(), Prod); break;
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "INE(1.0)";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "INE(2).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "INE(1.0)", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "INE(1.0)_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Aerolineas";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Aerolineas.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Aerolineas", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Aerolineas_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Timbrados/", "CSD con MegaPAC");
            xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "CSD con MegaPAC" + "_OK", ContextoVaL);

            rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Timbrados/", "Fiel Con MegaPAC");
            xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "Fiel Con MegaPAC" + "_OK", ContextoVaL);

            Nombre = "CertificadoDestruccion";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CertificadoDestruccion.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CertificadoDestruccion", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CertificadoDestruccion_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "ComercioExterior(cliente)";
            //----------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ComercioExterior(cliente).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ComercioExterior(cliente)", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ComercioExterior(cliente)_Tim", "false", DateTime.Now.ToString(), Prod);
            } GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "ComercioExterior";
            //----------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ComercioExterior.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ComercioExterior", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ComercioExterior_Tim", "false", DateTime.Now.ToString(), Prod); break;
            } GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);


            Nombre = "ConsumoCombustibles";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ConsumoCombustibles.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(0.10).ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ConsumoCombustibles", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ConsumoCombustibles_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Detallista";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Detallista.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Detallista", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Detallista_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Divisas";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Divisas.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Divisas", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Divisas_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Donatarias1.1";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Donatarias1.1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Donatarias1.1", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Donatarias1.1_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "ECC11";
            //----------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ECC11.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(0.10).ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ECC11", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ECC11_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Factura_AMP";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Factura&.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Factura_AMP", "1.0", false, "N", "N", "F&", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Factura_AMP_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "FacturaÑ";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "FacturaÑ.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "FacturaÑ", "1.0", false, "N", "N", "FÑ", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "FacturaÑ_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "IEDU";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "IEDU.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(0.10).ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "IEDU", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "IEDU_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "ImpLocal";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ImpLocal.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ImpLocal", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ImpLocal_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "LeyendasFiscales";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "LeyendasFiscales.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "LeyendasFiscales", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "LeyendasFiscales_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Nomina";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddDays(0).ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Nomina12";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina12.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina12", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina12_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Normal";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Normal.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Normal", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Normal_Tim", "false", DateTime.Now.ToString(), Prod);
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Normal.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "SectorPrimario", "1.0", false, "N", "N", "NA", false, var, true, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "SectorPrimario_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "NotariosPublicos";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "NotariosPublicos.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "NotariosPublicos", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "NotariosPublicos_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "ObrasArte";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ObrasArte.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddHours(0.10).ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ObrasArte", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ObrasArte_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "PagoEnEspecie";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "PagoEnEspecie.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "PagoEnEspecie", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "PagoEnEspecie_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "PFIntegranteCoordinado";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "PFIntegranteCoordinado.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "PFIntegranteCoordinado", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "PFIntegranteCoordinado_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "SPEI";
            //--------------------------------------------------------------------------------------l
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "SPEI.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "SPEI", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "SPEI_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "RegistroFiscal";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "RegistroFiscal.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "RegistroFiscal", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "RegistroFiscal_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "RenovacionSeminuevos";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "RenovacionSeminuevos.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "RenovacionSeminuevos", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "RenovacionSeminuevos_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "ServicioParcialConstruccion";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ServicioParcialConstruccion.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ServicioParcialConstruccion", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ServicioParcialConstruccion_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "SustitucionSeminuevos";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "SustitucionSeminuevos.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "SustitucionSeminuevos", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "SustitucionSeminuevos_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "TercerosInfoAduanera";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "TercerosInfoAduanera.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "TercerosInfoAduanera", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "TercerosInfoAduanera_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "TercerosParte";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "TercerosParte.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "TercerosParte", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "TercerosParte_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "TercerosPredial";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "TercerosPredial.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "TercerosPredial", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "TercerosPredial_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "MultiplesComplementos";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "MultiplesComplementos.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "MultiplesComplementos", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "MultiplesComplementos_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "InfoAduanera";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "InfoAduanera.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "InfoAduanera", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "InfoAduanera_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "IEPS";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "IEPS.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "IEPS", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "IEPS_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "CuentaPredial";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CuentaPredial.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CuentaPredial", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CuentaPredial_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Turista";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Turista.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Turista", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Turista_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "ValesDespensa";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ValesDespensa.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ValesDespensa", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ValesDespensa_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "VehiculoUsado";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "VehiculoUsado.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "VehiculoUsado", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "VehiculoUsado_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "VentaVehiculos_1_0(Vigente)";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "VentaVehiculos_1_0.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2013-11-25T18:15:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "VentaVehiculos_1_0(Vigente)", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "VentaVehiculos_1_0(Vigente)_Tim", "veh10", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "VentaVehiculos_1_1";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "VentaVehiculos_1_1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "VentaVehiculos_1_1", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "VentaVehiculos_1_1_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "VentaVehiculos_1_1PTE";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "VentaVehiculos_1_1PTE.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "VentaVehiculos_1_1PTE", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "VentaVehiculos_1_1PTE_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Fiel(Vigente)";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Fiel(Persona Fisica).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2015-02-08T10:25:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Fiel(Vigente)", "1.0", false, "N", "N", "FF", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Fiel(Vigente)_Tim", "fiel", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAPVal(ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoVaL, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, ref rutaGen, Status);

            Nombre = "EsquemaIncorrecto";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "EsquemaIncorrecto.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "EsquemaIncorrecto", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "EsquemaIncorrecto_Tim", "false", DateTime.Now.ToString("s"), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "200KB(Menos)";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "200KB(Menos).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "200KB(Menos)", "1.1", false, "N", "N", "NA", false, var, false, false);
                //Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "200KB(Menos)", "1.1", false, "N", "N", "FÑ", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "200KB(Menos)_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "475KB(Menos)";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {

                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "475KB(Menos).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "475KB(Menos)", "1.1", false, "N", "N", "NA", false, var, false, false);
                //Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "475KB(Menos)", "1.1", false, "N", "N", "FÑ", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "475KB(Menos)_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "1000KB(Menos)";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "1000KB(Menos).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                //Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "1000KB(Menos)", "1.1", false, "N", "N", "AQ", false, var, false, false);
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "1000KB(Menos)", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "1000KB(Menos)_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);



            Nombre = "Default(Menos)";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Default(Menos).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                //Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Default(Menos)", "1.1", false, "N", "N", "SL", false, var, false, false);
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Default(Menos)", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Default(Menos)_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "2000KB(Menos)"; Status = "_Error";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "2000KB(Menos).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "2000KB(Menos)", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "2000KB(Menos)_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Aerolineas";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Aerolineas.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2014-01-01T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Aerolineas", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Aerolineas_Tim", "dyna", "2014-01-01T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "CertificadoDestruccion";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "CertificadoDestruccion.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2015-05-01T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "CertificadoDestruccion", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "CertificadoDestruccion_Tim", "dyna", "2015-05-01T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "ConsumoCombustibles";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ConsumoCombustibles.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2014-01-05T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ConsumoCombustibles", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ConsumoCombustibles_Tim", "dyna", "2014-01-05T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Detallista";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Detallista.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2010-12-01T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Detallista", "1.1", false, "N", "N", "2012", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Detallista_Tim", "2010", "2010-12-01T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Divisas";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Divisas.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2010-12-01T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Divisas", "1.1", false, "N", "N", "2012", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Divisas_Tim", "2010", "2010-12-01T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Donatarias1.1";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Donatarias1.1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2010-12-01T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Donatarias1.1", "1.1", false, "N", "N", "2012", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Donatarias1.1_Tim", "2010", "2010-12-01T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "EdoCuentaCombustibles";
            ////--------------------------------------------------------------------------------------
            //foreach (String var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "EdoCuentaCombustibles.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2015-11-16T12:00:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "EdoCuentaCombustibles", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "EdoCuentaCombustibles_Tim", "dyna", "2015-11-16T12:00:00"); break;
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "EdoCuentaCombustibles";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "EdoCuentaCombustibles.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2015-11-16T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "EdoCuentaCombustibles", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "EdoCuentaCombustibles_Tim", "dyna", "2015-11-16T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "ECC11";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ECC11.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2015-10-01T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ECC11", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ECC11_Tim", "dyna", "2015-10-01T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "IEDU";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "IEDU.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2010-12-01T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "IEDU", "1.1", false, "N", "N", "2012", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "IEDU_Tim", "2010", "2010-12-01T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "ImpLocal";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ImpLocal.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2010-12-01T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ImpLocal", "1.1", false, "N", "N", "2012", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ImpLocal_Tim", "2010", "2010-12-01T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "LeyendasFiscales";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "LeyendasFiscales.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2011-12-01T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "LeyendasFiscales", "1.1", false, "N", "N", "2012", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "LeyendasFiscales_Tim", "2010", "2011-12-01T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Nomina";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Nomina.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2013-12-01T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Nomina", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Nomina_Tim", "dyna", "2013-12-01T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "NotariosPublicos";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "NotariosPublicos.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2014-03-05T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "NotariosPublicos", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "NotariosPublicos_Tim", "dyna", "2014-03-05T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "ObrasArte";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ObrasArte.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2015-03-28T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ObrasArte", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ObrasArte_Tim", "dyna", "2015-03-28T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "PagoEnEspecie";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "PagoEnEspecie.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2013-12-01T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "PagoEnEspecie", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "PagoEnEspecie_Tim", "dyna", "2013-12-01T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "PFIntegranteCoordinado";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "PFIntegranteCoordinado.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2010-12-01T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "PFIntegranteCoordinado", "1.1", false, "N", "N", "2012", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "PFIntegranteCoordinado_Tim", "2010", "2010-12-01T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "SPEI";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "SPEI.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2012-05-16T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "SPEI", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "SPEI_Tim", "dyna", "2012-05-16T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "RegistroFiscal";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "RegistroFiscal.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2013-11-01T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "RegistroFiscal", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "RegistroFiscal_Tim", "dyna", "2013-11-01T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "RenovacionSeminuevos";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "RenovacionSeminuevos.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2015-02-28T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "RenovacionSeminuevos", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "RenovacionSeminuevos_Tim", "dyna", "2015-02-28T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "ServicioParcialConstruccion";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ServicioParcialConstruccion.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2015-01-15T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ServicioParcialConstruccion", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ServicioParcialConstruccion_Tim", "dyna", "2015-01-15T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "IEPS";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "IEPS.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2015-10-01T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "IEPS", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "IEPS_Tim", "dyna", "2015-10-01T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Turista";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Turista.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2011-12-01T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Turista", "1.1", false, "N", "N", "2012", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Turista_Tim", "2010", "2011-12-01T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "ValesDespensa";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ValesDespensa.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2014-01-05T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ValesDespensa", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ValesDespensa_Tim", "dyna", "2014-01-05T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "VehiculoUsado";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "VehiculoUsado.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2014-08-16T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "VehiculoUsado", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "VehiculoUsado_Tim", "dyna", "2014-08-16T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "VentaVehiculos_1_1";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "VentaVehiculos_1_1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2013-12-01T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "VentaVehiculos_1_1", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "VentaVehiculos_1_1_Tim", "dyna", "2013-12-01T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "72HorasMas";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Normal.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddDays(4).ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "72HorasMas", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "72HorasMas_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "72HorasMenos";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Normal.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.AddDays(-4).ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "72HorasMenos", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "72HorasMenos_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            //Nombre = "Addenda";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Addenda.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Addenda", "1.0", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Addenda_Tim", "false", DateTime.Now.ToString()); break;
            //}
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Certificado";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Normal.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Certificado", "1.0", false, "N", "S", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Certificado_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "ComplementomalFormado";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "ComplementomalFormado.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "ComplementomalFormado", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "ComplementomalFormado_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Esquema";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Normal.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Esquema", "1.0", false, "S", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Esquema_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "FielPFisica";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Fiel(Persona Fisica).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "FielPFisica", "1.0", false, "N", "N", "FF", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "FielPFisica_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAPFiel(ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoTiM, ContextoVaL, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, ref rutaGen, Status);

            Nombre = "FielPMoral";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Fiel(Persona Moral).xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "FielPMoral", "1.0", false, "N", "N", "FM", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "FielPMoral_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAPFiel(ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoTiM, ContextoVaL, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, ref rutaGen, Status);

            Nombre = "Pipes";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Pipes.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Pipes", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Pipes_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "RFC_Emisor";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Normal.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).SetValue(string.Empty);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "RFC_Emisor", "1.0", false, "N", "N", "NA", true, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "RFC_Emisor_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "RFC_Receptor";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Normal.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).SetValue("HEPR860402CVN");
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "RFC_Receptor", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "RFC_Receptor_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "SelloAlterado";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Normal.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "SelloAlterado", "1.0", true, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "SelloAlterado_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAPFiel(ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoTiM, ContextoVaL, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, ref rutaGen, Status);

            Nombre = "SelloComilla";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Normal.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "SelloComilla", "1.0", false, "N", "N", "NA", false, var, false, true);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "SelloComilla_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAPFiel(ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoTiM, ContextoVaL, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, ref rutaGen, Status);

            Nombre = "Timbre_Previo";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Normal_Tim.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Timbre_Previo", "1.0", false, "N", "N", "NA", false, var, false, false);
                Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Timbre_Previo_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            GenerarSOAPPrevio(ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoTiM, ref JmeterSello, ref rutaAbsolutaAcuse, xmldoc, Nombre, ref rutaGen, Status);

            Nombre = "TimbreDoble";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "TimbreDoble.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "TimbreDoble", "1.1", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "TimbreDoble_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "VentaVehiculos_1_0";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "VentaVehiculos_1_0.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "VentaVehiculos_1_0", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "VentaVehiculos_1_0_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "Version";
            //--------------------------------------------------------------------------------------
            foreach (string var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Version.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Version", "3.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Version_Tim", "false", DateTime.Now.ToString(), Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            Nombre = "305";
            //--------------------------------------------------------------------------------------
            foreach (String var in SOAP_Methods)
            {
                Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Normal.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2013-05-06T12:00:00");
                Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "305", "1.0", false, "N", "N", "NA", false, var, false, false);
                if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "305_Tim", "dyna", "2013-05-06T12:00:00", Prod); break;
            }
            //--------------------------------------------------------------------------------------
            GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, Status);

            #endregion

            #endregion

            #region Schema Restriction

            //Nombre = "200KB_Error";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "200KB.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "200KB_Error", "1.1", false, "N", "N", "FÑ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "200KB_Error_Tim", "false");
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre);

            //Nombre = "475KB_Error";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "475KB.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "475KB_Error", "1.1", false, "N", "N", "F&", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "475KB_Error_Tim", "false");
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre);

            //Nombre = "1000KB_Error";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "1000KB.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "1000KB_Error", "1.1", false, "N", "N", "AQ", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "1000KB_Error_Tim", "false");
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre);

            //Nombre = "2000KB_Error";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "2000KB.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "2000KB_Error", "1.1", false, "N", "N", "NA", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "2000KB_Error_Tim", "false");
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre);

            //Nombre = "Default_Error";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Default.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Default_Error", "1.1", false, "N", "N", "SL", false, var, false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Default_Error_Tim", "false");
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAP(ref rutaGen, ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoGeN, ContextoTiM, ContextoVaL, ref Jmeter, ref JmeterSello, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre);

            //Nombre = "&AGP500110M19";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "&AGP500110M19.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(("2015-11-25T16:24:07"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "&AGP500110M19", "1.1", false, "N", "N", "AGP500110M19", false, var, true, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "&AGP500110M19_Tim", "SP");
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAPVal(ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoVaL, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, ref rutaGen);

            //Nombre = "DUDB290712KN0";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "DUDB290712KN0.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(("2015-11-25T16:13:13"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "DUDB290712KN0", "1.1", false, "N", "N", "DUDB290712KN0", false, var, true, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "DUDB290712KN0_Tim", "SP");
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAPVal(ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoVaL, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, ref rutaGen);


            //Nombre = "GA&R6606141Q1";
            ////--------------------------------------------------------------------------------------
            //foreach (string var in SOAP_Methods)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "GA&R6606141Q1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "GA&R6606141Q1", "1.1", false, "N", "N", "GAR6606141Q1", false, var, true, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "GA&R6606141Q1_Tim", "SP");
            //}
            ////--------------------------------------------------------------------------------------
            //GenerarSOAPVal(ref SoapSVC, ref SoapASMX, ref SoapASPEL, ref Soap12ASMX, ref Soap12ASPEL, ContextoVaL, ref JmeterValidacion, ref rutaAbsolutaAcuse, xmldoc, Nombre, ref rutaGen);
            
            #endregion

            #endregion
            //--------------------------------------------------------------------------------------
            #region Version 3.0

            //#region

            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "30_Comprobante.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2012-05-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "30_Comprobante", "3.0", false, "N", "N", "2012", false, "NA", false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "30_Comprobante_Tim", "true", DateTime.Now.ToString(), Prod);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Timbrados/", "30_Comprobante" + "_Tim");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "30_Comprobante", ContextoVaL);
            //}

            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "30_Donatarias.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2012-05-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "30_Donatarias", "3.0", false, "N", "N", "2012", false, "NA", false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "30_Donatarias_Tim", "true", DateTime.Now.ToString(), Prod);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Timbrados/", "30_Donatarias" + "_Tim");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "30_Donatarias", ContextoVaL);
            //}

            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "30_ImpuestosLocales.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2012-05-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "30_ImpuestosLocales", "3.0", false, "N", "N", "2012", false, "NA", false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "30_ImpuestosLocales_Tim", "true", DateTime.Now.ToString(), Prod);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Timbrados/", "30_ImpuestosLocales" + "_Tim");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "30_ImpuestosLocales", ContextoVaL);
            //}

            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "30_Detallista.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2012-05-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "30_Detallista", "3.0", false, "N", "N", "2012", false, "NA", false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "30_Detallista_Tim", "true", DateTime.Now.ToString(), Prod);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Timbrados/", "30_Detallista" + "_Tim");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "30_Detallista", ContextoVaL);
            //}

            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "30_Divisas.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2012-05-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "30_Divisas", "3.0", false, "N", "N", "2012", false, "NA", false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "30_Divisas_Tim", "true", DateTime.Now.ToString(), Prod);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Timbrados/", "30_Divisas" + "_Tim");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "30_Divisas", ContextoVaL);
            //}

            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "30_ECC.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2012-05-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "30_ECC", "3.0", false, "N", "N", "2012", false, "NA", false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "30_ECC_Tim", "true", DateTime.Now.ToString(), Prod);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Timbrados/", "30_ECC" + "_Tim");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "30_ECC", ContextoVaL);
            //}

            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "30_IEDU.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2012-05-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "30_IEDU", "3.0", false, "N", "N", "2012", false, "NA", false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "30_IEDU_Tim", "true", DateTime.Now.ToString(), Prod);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Timbrados/", "30_IEDU" + "_Tim");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "30_IEDU", ContextoVaL);
            //}

            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "30_Comprobante.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue("2012-05-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "30_SelloInvalido", "3.0", true, "N", "N", "2012", false, "NA", false, false);
            //    if (timbrado == "S" || timbrado == "s") Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "30_SelloInvalido_Tim", "true", DateTime.Now.ToString(), Prod);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Timbrados/", "30_SelloInvalido" + "_Tim");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "30_SelloInvalido", ContextoVaL);
            //}

            //#endregion

            #endregion
            //--------------------------------------------------------------------------------------
            #region Version 2.0

            //#region
            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "20_Comprobante.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2011-10-08T12:00:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "20_Comprobante", "2.0", false, "N", "N", "2012", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "20_Comprobante");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "20_Comprobante", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "20_Detallista.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2011-10-08T12:00:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "20_Detallista", "2.0", false, "N", "N", "2012", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "20_Detallista");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "20_Detallista", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "20_Divisas.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2011-10-08T12:00:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "20_Divisas", "2.0", false, "N", "N", "2012", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "20_Divisas");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "20_Divisas", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "20_Donatarias.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2011-10-08T12:00:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "20_Donatarias", "2.0", false, "N", "N", "2012", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "20_Donatarias");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "20_Donatarias", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "20_IEDU.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2011-10-08T12:00:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "20_IEDU", "2.0", false, "N", "N", "2012", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "20_IEDU");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "20_IEDU", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "20_ImpuestoLocales.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2011-10-08T12:00:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "20_ImpuestoLocales", "2.0", false, "N", "N", "2012", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "20_ImpuestoLocales");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "20_ImpuestoLocales", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "20_ECC.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2011-10-08T12:00:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "20_ECC", "2.0", false, "N", "N", "2012", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "20_ECC");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "20_ECC", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "20_Comprobante.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2011-10-08T12:00:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "20_SelloInvalido", "2.0", true, "N", "N", "2012", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "20_SelloInvalido");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "20_SelloInvalido", ContextoVaL);
            //}

            //#endregion

            #endregion
            //--------------------------------------------------------------------------------------
            #region Version 2.2

            //#region
            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "22_Comprobante.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2013-10-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "22_Comprobante", "2.2", false, "N", "N", "NA", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "22_Comprobante");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "22_Comprobante", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "22_Comprobante.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2013-10-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "22_SelloInvalido", "2.2", true, "N", "N", "NA", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "22_SelloInvalido");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "22_SelloInvalido", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "22_Detallista.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2013-10-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "22_Detallista", "2.2", false, "N", "N", "NA", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "22_Detallista");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "22_Detallista", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "22_Divisas.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2013-10-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "22_Divisas", "2.2", false, "N", "N", "NA", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "22_Divisas");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "22_Divisas", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "22_Donatarias1.1.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2013-10-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "22_Donatarias1.1", "2.2", false, "N", "N", "NA", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "22_Donatarias1.1");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "22_Donatarias1.1", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "22_ECC.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2013-10-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "22_ECC", "2.2", false, "N", "N", "NA", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "22_ECC");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "22_ECC", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "22_Divisas.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2013-10-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "22_Divisas", "2.2", false, "N", "N", "NA", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "22_Divisas");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "22_Divisas", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "22_IEDU.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2013-10-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "22_IEDU", "2.2", false, "N", "N", "NA", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "22_IEDU");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "22_IEDU", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "22_ImpuestosLocales.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2013-10-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "22_ImpuestosLocales", "2.2", false, "N", "N", "NA", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "22_ImpuestosLocales");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "22_ImpuestosLocales", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "22_LeyendasFiscales.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2013-10-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "22_LeyendasFiscales", "2.2", false, "N", "N", "NA", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "22_LeyendasFiscales");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "22_LeyendasFiscales", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "22_PFIC.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2013-10-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "22_PFIC", "2.2", false, "N", "N", "NA", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "22_PFIC");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "22_PFIC", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "22_Turista.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2013-10-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "22_Turista", "2.2", false, "N", "N", "NA", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "22_Turista");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "22_Turista", ContextoVaL);
            //}

            //{
            //    Cargar_XML20(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "22_VentaVehicular1.0.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).SetValue("2013-10-08T12:30:00");
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "22_VentaVehicular1.0", "2.2", false, "N", "N", "NA", false, "NA", false, false);
            //    rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", "22_VentaVehicular1.0");
            //    xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, "22_VentaVehicular1.0", ContextoVaL);
            //}

            //#endregion

            #endregion
            //--------------------------------------------------------------------------------------
            Jmeter = Jmeter + "</hashTree></hashTree></hashTree></jmeterTestPlan>";
            JmeterSello = JmeterSello + "</hashTree></hashTree></hashTree></jmeterTestPlan>";
            JmeterValidacion = JmeterValidacion + "</hashTree></hashTree></hashTree></jmeterTestPlan>";
            //--------------------------------------------------------------------------------------
            #region XMLProject

            String XMLProject =
              "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +

              "<con:soapui-project activeEnvironment=\"Default\" name=\"TIM_ASMX\" resourceRoot=\"\" soapui-version=\"4.6.2\" abortOnError=\"false\" runType=\"SEQUENTIAL\" xmlns:con=\"http://eviware.com/soapui/config\"><con:settings/>" +

              "<con:interface xsi:type=\"con:WsdlInterface\" wsaVersion=\"NONE\" name=\"wcfRecepcionASPELSoap\" type=\"wsdl\" bindingName=\"{https://www.paxfacturacion.com.mx:453}wcfRecepcionASPELSoap\" soapVersion=\"1_1\" anonymous=\"optional\" definition=\"http://10.54.57.208:7080/webservices/wcfRecepcionaspel.asmx?wsdl\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><con:settings/><con:definitionCache type=\"TEXT\" rootPart=\"http://10.54.57.208:7080/webservices/wcfRecepcionaspel.asmx?wsdl\"><con:part><con:url>http://10.54.57.208:7080/webservices/wcfRecepcionaspel.asmx?wsdl</con:url><con:content><![CDATA[<wsdl:definitions targetNamespace=\"https://www.paxfacturacion.com.mx:453\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\" xmlns:soap11=\"http://schemas.xmlsoap.org/wsdl/soap/\" xmlns:soap12=\"http://schemas.xmlsoap.org/wsdl/soap12/\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http/\" xmlns:mime=\"http://schemas.xmlsoap.org/wsdl/mime/\" xmlns:wsp=\"http://www.w3.org/ns/ws-policy\" xmlns:wsp200409=\"http://schemas.xmlsoap.org/ws/2004/09/policy\" xmlns:wsp200607=\"http://www.w3.org/2006/07/ws-policy\" xmlns:ns0=\"https://www.paxfacturacion.com.mx:453\">" +
              "<wsdl:types xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
              "<xsd:schema>" +
              "<xsd:import schemaLocation=\"wcfRecepcionaspel.asmx.xsd1.xsd\" namespace=\"https://www.paxfacturacion.com.mx:453\"/>" +
              "</xsd:schema>" +
              "</wsdl:types>" +
              "<wsdl:message name=\"fnEnviarXMLSoapIn\">" +
              "<wsdl:part name=\"parameters\" element=\"xsns:fnEnviarXML\" xmlns:xsns=\"https://www.paxfacturacion.com.mx:453\"/>" +
              "</wsdl:message>" +
              "<wsdl:message name=\"fnEnviarXMLSoapOut\">" +
              "<wsdl:part name=\"parameters\" element=\"xsns:fnEnviarXMLResponse\" xmlns:xsns=\"https://www.paxfacturacion.com.mx:453\"/>" +
              "</wsdl:message>" +
              "<wsdl:portType name=\"wcfRecepcionASPELSoap\">" +
              "<wsdl:operation name=\"fnEnviarXML\">" +
              "<wsdl:input name=\"fnEnviarXMLRequest\" message=\"ns0:fnEnviarXMLSoapIn\"/>" +
              "<wsdl:output name=\"fnEnviarXMLResponse\" message=\"ns0:fnEnviarXMLSoapOut\"/>" +
              "</wsdl:operation>" +
              "</wsdl:portType>" +
              "<wsdl:binding name=\"wcfRecepcionASPELSoap\" type=\"ns0:wcfRecepcionASPELSoap\">" +
              "<soap11:binding transport=\"http://schemas.xmlsoap.org/soap/http\" style=\"document\"/>" +
              "<wsdl:operation name=\"fnEnviarXML\">" +
              "<soap11:operation soapAction=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\" style=\"document\"/>" +
              "<wsdl:input name=\"fnEnviarXMLRequest\">" +
              "<soap11:body use=\"literal\"/>" +
              "</wsdl:input>" +
              "<wsdl:output name=\"fnEnviarXMLResponse\">" +
              "<soap11:body use=\"literal\"/>" +
              "</wsdl:output>" +
              "</wsdl:operation>" +
              "</wsdl:binding>" +
              "<wsdl:binding name=\"wcfRecepcionASPELSoap12\" type=\"ns0:wcfRecepcionASPELSoap\">" +
              "<soap12:binding transport=\"http://schemas.xmlsoap.org/soap/http\" style=\"document\"/>" +
              "<wsdl:operation name=\"fnEnviarXML\">" +
              "<soap12:operation soapAction=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\" soapActionRequired=\"false\" style=\"document\"/>" +
              "<wsdl:input name=\"fnEnviarXMLRequest\">" +
              "<soap12:body use=\"literal\"/>" +
              "</wsdl:input>" +
              "<wsdl:output name=\"fnEnviarXMLResponse\">" +
              "<soap12:body use=\"literal\"/>" +
              "</wsdl:output>" +
              "</wsdl:operation>" +
              "</wsdl:binding>" +
              "<wsdl:service name=\"wcfRecepcionASPEL\">" +
              "<wsdl:port name=\"wcfRecepcionASPELSoap\" binding=\"ns0:wcfRecepcionASPELSoap\">" +
              "<soap11:address location=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcionaspel.asmx\"/>" +
              "</wsdl:port>" +
              "<wsdl:port name=\"wcfRecepcionASPELSoap12\" binding=\"ns0:wcfRecepcionASPELSoap12\">" +
              "<soap12:address location=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcionaspel.asmx\"/>" +
              "</wsdl:port>" +
              "</wsdl:service>" +
              "</wsdl:definitions>]]></con:content><con:type>http://schemas.xmlsoap.org/wsdl/</con:type></con:part><con:part><con:url>http://10.54.57.208:7080/webservices/wcfRecepcionaspel.asmx.xsd1.xsd</con:url><con:content><![CDATA[<s:schema elementFormDefault=\"qualified\" targetNamespace=\"https://www.paxfacturacion.com.mx:453\" xmlns:s=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://schemas.xmlsoap.org/wsdl/soap12/\" xmlns:mime=\"http://schemas.xmlsoap.org/wsdl/mime/\" xmlns:tns=\"https://www.paxfacturacion.com.mx:453\" xmlns:soap=\"http://schemas.xmlsoap.org/wsdl/soap/\" xmlns:tm=\"http://microsoft.com/wsdl/mime/textMatching/\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http/\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\">" +
              "<s:element name=\"fnEnviarXML\">" +
              "<s:complexType>" +
              "<s:sequence>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"psComprobante\" type=\"s:string\"/>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"psTipoDocumento\" type=\"s:string\"/>" +
              "<s:element minOccurs=\"1\" maxOccurs=\"1\" name=\"pnId_Estructura\" type=\"s:int\"/>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sNombre\" type=\"s:string\"/>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sContrasena\" type=\"s:string\"/>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sVersion\" type=\"s:string\"/>" +
              "</s:sequence>" +
              "</s:complexType>" +
              "</s:element>" +
              "<s:element name=\"fnEnviarXMLResponse\">" +
              "<s:complexType>" +
              "<s:sequence>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"fnEnviarXMLResult\" type=\"s:string\"/>" +
              "</s:sequence>" +
              "</s:complexType>" +
              "</s:element>" +
              "</s:schema>]]></con:content><con:type>http://www.w3.org/2001/XMLSchema</con:type></con:part></con:definitionCache><con:endpoints><con:endpoint>http://10.54.57.208:7080/webservices/wcfRecepcionaspel.asmx</con:endpoint><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionaspel.asmx</con:endpoint></con:endpoints>" +

              "<con:operation isOneWay=\"false\" action=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\" name=\"fnEnviarXML\" bindingOperationName=\"fnEnviarXML\" type=\"Request-Response\" outputName=\"fnEnviarXMLResponse\" inputName=\"fnEnviarXMLRequest\" receivesAttachments=\"false\" sendsAttachments=\"false\" anonymous=\"optional\"><con:settings/>" +
            #endregion
            //--------------------------------------------------------------------------------------
            #region SoapASPEL

                    SoapASPEL +

              "</con:operation></con:interface>" +

              "<con:interface xsi:type=\"con:WsdlInterface\" wsaVersion=\"NONE\" name=\"wcfRecepcionASPELSoap12\" type=\"wsdl\" bindingName=\"{https://www.paxfacturacion.com.mx:453}wcfRecepcionASPELSoap12\" soapVersion=\"1_2\" anonymous=\"optional\" definition=\"http://10.54.57.208:7080/webservices/wcfRecepcionaspel.asmx?wsdl\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><con:settings/><con:definitionCache type=\"TEXT\" rootPart=\"http://10.54.57.208:7080/webservices/wcfRecepcionaspel.asmx?wsdl\"><con:part><con:url>http://10.54.57.208:7080/webservices/wcfRecepcionaspel.asmx?wsdl</con:url><con:content><![CDATA[<wsdl:definitions targetNamespace=\"https://www.paxfacturacion.com.mx:453\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\" xmlns:soap11=\"http://schemas.xmlsoap.org/wsdl/soap/\" xmlns:soap12=\"http://schemas.xmlsoap.org/wsdl/soap12/\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http/\" xmlns:mime=\"http://schemas.xmlsoap.org/wsdl/mime/\" xmlns:wsp=\"http://www.w3.org/ns/ws-policy\" xmlns:wsp200409=\"http://schemas.xmlsoap.org/ws/2004/09/policy\" xmlns:wsp200607=\"http://www.w3.org/2006/07/ws-policy\" xmlns:ns0=\"https://www.paxfacturacion.com.mx:453\">" +
              "<wsdl:types xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
              "<xsd:schema>" +
              "<xsd:import schemaLocation=\"wcfRecepcionaspel.asmx.xsd1.xsd\" namespace=\"https://www.paxfacturacion.com.mx:453\"/>" +
              "</xsd:schema>" +
              "</wsdl:types>" +
              "<wsdl:message name=\"fnEnviarXMLSoapIn\">" +
              "<wsdl:part name=\"parameters\" element=\"xsns:fnEnviarXML\" xmlns:xsns=\"https://www.paxfacturacion.com.mx:453\"/>" +
              "</wsdl:message>" +
              "<wsdl:message name=\"fnEnviarXMLSoapOut\">" +
              "<wsdl:part name=\"parameters\" element=\"xsns:fnEnviarXMLResponse\" xmlns:xsns=\"https://www.paxfacturacion.com.mx:453\"/>" +
              "</wsdl:message>" +
              "<wsdl:portType name=\"wcfRecepcionASPELSoap\">" +
              "<wsdl:operation name=\"fnEnviarXML\">" +
              "<wsdl:input name=\"fnEnviarXMLRequest\" message=\"ns0:fnEnviarXMLSoapIn\"/>" +
              "<wsdl:output name=\"fnEnviarXMLResponse\" message=\"ns0:fnEnviarXMLSoapOut\"/>" +
              "</wsdl:operation>" +
              "</wsdl:portType>" +
              "<wsdl:binding name=\"wcfRecepcionASPELSoap\" type=\"ns0:wcfRecepcionASPELSoap\">" +
              "<soap11:binding transport=\"http://schemas.xmlsoap.org/soap/http\" style=\"document\"/>" +
              "<wsdl:operation name=\"fnEnviarXML\">" +
              "<soap11:operation soapAction=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\" style=\"document\"/>" +
              "<wsdl:input name=\"fnEnviarXMLRequest\">" +
              "<soap11:body use=\"literal\"/>" +
              "</wsdl:input>" +
              "<wsdl:output name=\"fnEnviarXMLResponse\">" +
              "<soap11:body use=\"literal\"/>" +
              "</wsdl:output>" +
              "</wsdl:operation>" +
              "</wsdl:binding>" +
              "<wsdl:binding name=\"wcfRecepcionASPELSoap12\" type=\"ns0:wcfRecepcionASPELSoap\">" +
              "<soap12:binding transport=\"http://schemas.xmlsoap.org/soap/http\" style=\"document\"/>" +
              "<wsdl:operation name=\"fnEnviarXML\">" +
              "<soap12:operation soapAction=\"https://www.paxfacturacion.com.mx:453/fnEnviarXML\" soapActionRequired=\"false\" style=\"document\"/>" +
              "<wsdl:input name=\"fnEnviarXMLRequest\">" +
              "<soap12:body use=\"literal\"/>" +
              "</wsdl:input>" +
              "<wsdl:output name=\"fnEnviarXMLResponse\">" +
              "<soap12:body use=\"literal\"/>" +
              "</wsdl:output>" +
              "</wsdl:operation>" +
              "</wsdl:binding>" +
              "<wsdl:service name=\"wcfRecepcionASPEL\">" +
              "<wsdl:port name=\"wcfRecepcionASPELSoap\" binding=\"ns0:wcfRecepcionASPELSoap\">" +
              "<soap11:address location=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcionaspel.asmx\"/>" +
              "</wsdl:port>" +
              "<wsdl:port name=\"wcfRecepcionASPELSoap12\" binding=\"ns0:wcfRecepcionASPELSoap12\">" +
              "<soap12:address location=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcionaspel.asmx\"/>" +
              "</wsdl:port>" +
              "</wsdl:service>" +
              "</wsdl:definitions>]]></con:content><con:type>http://schemas.xmlsoap.org/wsdl/</con:type></con:part><con:part><con:url>http://10.54.57.208:7080/webservices/wcfRecepcionaspel.asmx.xsd1.xsd</con:url><con:content><![CDATA[<s:schema elementFormDefault=\"qualified\" targetNamespace=\"https://www.paxfacturacion.com.mx:453\" xmlns:s=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://schemas.xmlsoap.org/wsdl/soap12/\" xmlns:mime=\"http://schemas.xmlsoap.org/wsdl/mime/\" xmlns:tns=\"https://www.paxfacturacion.com.mx:453\" xmlns:soap=\"http://schemas.xmlsoap.org/wsdl/soap/\" xmlns:tm=\"http://microsoft.com/wsdl/mime/textMatching/\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http/\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\">" +
              "<s:element name=\"fnEnviarXML\">" +
              "<s:complexType>" +
              "<s:sequence>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"psComprobante\" type=\"s:string\"/>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"psTipoDocumento\" type=\"s:string\"/>" +
              "<s:element minOccurs=\"1\" maxOccurs=\"1\" name=\"pnId_Estructura\" type=\"s:int\"/>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sNombre\" type=\"s:string\"/>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sContrasena\" type=\"s:string\"/>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sVersion\" type=\"s:string\"/>" +
              "</s:sequence>" +
              "</s:complexType>" +
              "</s:element>" +
              "<s:element name=\"fnEnviarXMLResponse\">" +
              "<s:complexType>" +
              "<s:sequence>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"fnEnviarXMLResult\" type=\"s:string\"/>" +
              "</s:sequence>" +
              "</s:complexType>" +
              "</s:element>" +
              "</s:schema>]]></con:content><con:type>http://www.w3.org/2001/XMLSchema</con:type></con:part></con:definitionCache><con:endpoints><con:endpoint>http://10.54.57.208:7080/webservices/wcfRecepcionaspel.asmx</con:endpoint><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionaspel.asmx</con:endpoint></con:endpoints><con:operation isOneWay=\"false\" action=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\" name=\"fnEnviarXML\" bindingOperationName=\"fnEnviarXML\" type=\"Request-Response\" outputName=\"fnEnviarXMLResponse\" inputName=\"fnEnviarXMLRequest\" receivesAttachments=\"false\" sendsAttachments=\"false\" anonymous=\"optional\"><con:settings/>" +
            #endregion
            //--------------------------------------------------------------------------------------
            #region Soap12ASPEL

                    Soap12ASPEL +

              "</con:operation></con:interface>" +

              "<con:interface xsi:type=\"con:WsdlInterface\" wsaVersion=\"NONE\" name=\"wcfRecepcionASMXSoap12\" type=\"wsdl\" bindingName=\"{https://www.paxfacturacion.com.mx:453}wcfRecepcionASMXSoap12\" soapVersion=\"1_2\" anonymous=\"optional\" definition=\"http://10.54.57.208:7080/webservices/wcfRecepcionasmx.asmx?wsdl\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><con:settings/><con:definitionCache type=\"TEXT\" rootPart=\"http://10.54.57.208:7080/webservices/wcfRecepcionasmx.asmx?wsdl\"><con:part><con:url>http://10.54.57.208:7080/webservices/wcfRecepcionasmx.asmx?wsdl</con:url><con:content><![CDATA[<wsdl:definitions targetNamespace=\"https://www.paxfacturacion.com.mx:453\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\" xmlns:soap11=\"http://schemas.xmlsoap.org/wsdl/soap/\" xmlns:soap12=\"http://schemas.xmlsoap.org/wsdl/soap12/\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http/\" xmlns:mime=\"http://schemas.xmlsoap.org/wsdl/mime/\" xmlns:wsp=\"http://www.w3.org/ns/ws-policy\" xmlns:wsp200409=\"http://schemas.xmlsoap.org/ws/2004/09/policy\" xmlns:wsp200607=\"http://www.w3.org/2006/07/ws-policy\" xmlns:ns0=\"https://www.paxfacturacion.com.mx:453\">" +
              "<wsdl:types xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
              "<xsd:schema>" +
              "<xsd:import schemaLocation=\"wcfRecepcionasmx.asmx.xsd1.xsd\" namespace=\"https://www.paxfacturacion.com.mx:453\"/>" +
              "</xsd:schema>" +
              "</wsdl:types>" +
              "<wsdl:message name=\"fnEnviarXMLSoapIn\">" +
              "<wsdl:part name=\"parameters\" element=\"xsns:fnEnviarXML\" xmlns:xsns=\"https://www.paxfacturacion.com.mx:453\"/>" +
              "</wsdl:message>" +
              "<wsdl:message name=\"fnEnviarXMLSoapOut\">" +
              "<wsdl:part name=\"parameters\" element=\"xsns:fnEnviarXMLResponse\" xmlns:xsns=\"https://www.paxfacturacion.com.mx:453\"/>" +
              "</wsdl:message>" +
              "<wsdl:portType name=\"wcfRecepcionASMXSoap\">" +
              "<wsdl:operation name=\"fnEnviarXML\">" +
              "<wsdl:input name=\"fnEnviarXMLRequest\" message=\"ns0:fnEnviarXMLSoapIn\"/>" +
              "<wsdl:output name=\"fnEnviarXMLResponse\" message=\"ns0:fnEnviarXMLSoapOut\"/>" +
              "</wsdl:operation>" +
              "</wsdl:portType>" +
              "<wsdl:binding name=\"wcfRecepcionASMXSoap\" type=\"ns0:wcfRecepcionASMXSoap\">" +
              "<soap11:binding transport=\"http://schemas.xmlsoap.org/soap/http\" style=\"document\"/>" +
              "<wsdl:operation name=\"fnEnviarXML\">" +
              "<soap11:operation soapAction=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\" style=\"document\"/>" +
              "<wsdl:input name=\"fnEnviarXMLRequest\">" +
              "<soap11:body use=\"literal\"/>" +
              "</wsdl:input>" +
              "<wsdl:output name=\"fnEnviarXMLResponse\">" +
              "<soap11:body use=\"literal\"/>" +
              "</wsdl:output>" +
              "</wsdl:operation>" +
              "</wsdl:binding>" +
              "<wsdl:binding name=\"wcfRecepcionASMXSoap12\" type=\"ns0:wcfRecepcionASMXSoap\">" +
              "<soap12:binding transport=\"http://schemas.xmlsoap.org/soap/http\" style=\"document\"/>" +
              "<wsdl:operation name=\"fnEnviarXML\">" +
              "<soap12:operation soapAction=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\" soapActionRequired=\"false\" style=\"document\"/>" +
              "<wsdl:input name=\"fnEnviarXMLRequest\">" +
              "<soap12:body use=\"literal\"/>" +
              "</wsdl:input>" +
              "<wsdl:output name=\"fnEnviarXMLResponse\">" +
              "<soap12:body use=\"literal\"/>" +
              "</wsdl:output>" +
              "</wsdl:operation>" +
              "</wsdl:binding>" +
              "<wsdl:service name=\"wcfRecepcionASMX\">" +
              "<wsdl:port name=\"wcfRecepcionASMXSoap\" binding=\"ns0:wcfRecepcionASMXSoap\">" +
              "<soap11:address location=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcionasmx.asmx\"/>" +
              "</wsdl:port>" +
              "<wsdl:port name=\"wcfRecepcionASMXSoap12\" binding=\"ns0:wcfRecepcionASMXSoap12\">" +
              "<soap12:address location=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcionasmx.asmx\"/>" +
              "</wsdl:port>" +
              "</wsdl:service>" +
              "</wsdl:definitions>]]></con:content><con:type>http://schemas.xmlsoap.org/wsdl/</con:type></con:part><con:part><con:url>http://10.54.57.208:7080/webservices/wcfRecepcionasmx.asmx.xsd1.xsd</con:url><con:content><![CDATA[<s:schema elementFormDefault=\"qualified\" targetNamespace=\"https://www.paxfacturacion.com.mx:453\" xmlns:s=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://schemas.xmlsoap.org/wsdl/soap12/\" xmlns:mime=\"http://schemas.xmlsoap.org/wsdl/mime/\" xmlns:tns=\"https://www.paxfacturacion.com.mx:453\" xmlns:soap=\"http://schemas.xmlsoap.org/wsdl/soap/\" xmlns:tm=\"http://microsoft.com/wsdl/mime/textMatching/\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http/\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\">" +
              "<s:element name=\"fnEnviarXML\">" +
              "<s:complexType>" +
              "<s:sequence>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"psComprobante\" type=\"s:string\"/>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"psTipoDocumento\" type=\"s:string\"/>" +
              "<s:element minOccurs=\"1\" maxOccurs=\"1\" name=\"pnId_Estructura\" type=\"s:int\"/>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sNombre\" type=\"s:string\"/>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sContraseña\" type=\"s:string\"/>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sVersion\" type=\"s:string\"/>" +
              "</s:sequence>" +
              "</s:complexType>" +
              "</s:element>" +
              "<s:element name=\"fnEnviarXMLResponse\">" +
              "<s:complexType>" +
              "<s:sequence>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"fnEnviarXMLResult\" type=\"s:string\"/>" +
              "</s:sequence>" +
              "</s:complexType>" +
              "</s:element>" +
              "</s:schema>]]></con:content><con:type>http://www.w3.org/2001/XMLSchema</con:type></con:part></con:definitionCache><con:endpoints><con:endpoint>http://10.54.57.208:7080/webservices/wcfRecepcionasmx.asmx</con:endpoint><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionasmx.asmx</con:endpoint></con:endpoints><con:operation isOneWay=\"false\" action=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\" name=\"fnEnviarXML\" bindingOperationName=\"fnEnviarXML\" type=\"Request-Response\" outputName=\"fnEnviarXMLResponse\" inputName=\"fnEnviarXMLRequest\" receivesAttachments=\"false\" sendsAttachments=\"false\" anonymous=\"optional\"><con:settings/>" +
            #endregion
            //--------------------------------------------------------------------------------------
            #region Soap12ASMX

                    Soap12ASMX +

              "</con:operation></con:interface>" +

              "<con:interface xsi:type=\"con:WsdlInterface\" wsaVersion=\"NONE\" name=\"wcfRecepcionASMXSoap\" type=\"wsdl\" bindingName=\"{https://www.paxfacturacion.com.mx:453}wcfRecepcionASMXSoap\" soapVersion=\"1_1\" anonymous=\"optional\" definition=\"http://10.54.57.208:7080/webservices/wcfRecepcionasmx.asmx?wsdl\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><con:settings/><con:definitionCache type=\"TEXT\" rootPart=\"http://10.54.57.208:7080/webservices/wcfRecepcionasmx.asmx?wsdl\"><con:part><con:url>http://10.54.57.208:7080/webservices/wcfRecepcionasmx.asmx?wsdl</con:url><con:content><![CDATA[<wsdl:definitions targetNamespace=\"https://www.paxfacturacion.com.mx:453\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\" xmlns:soap11=\"http://schemas.xmlsoap.org/wsdl/soap/\" xmlns:soap12=\"http://schemas.xmlsoap.org/wsdl/soap12/\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http/\" xmlns:mime=\"http://schemas.xmlsoap.org/wsdl/mime/\" xmlns:wsp=\"http://www.w3.org/ns/ws-policy\" xmlns:wsp200409=\"http://schemas.xmlsoap.org/ws/2004/09/policy\" xmlns:wsp200607=\"http://www.w3.org/2006/07/ws-policy\" xmlns:ns0=\"https://www.paxfacturacion.com.mx:453\">" +
              "<wsdl:types xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
              "<xsd:schema>" +
              "<xsd:import schemaLocation=\"wcfRecepcionasmx.asmx.xsd1.xsd\" namespace=\"https://www.paxfacturacion.com.mx:453\"/>" +
              "</xsd:schema>" +
              "</wsdl:types>" +
              "<wsdl:message name=\"fnEnviarXMLSoapIn\">" +
              "<wsdl:part name=\"parameters\" element=\"xsns:fnEnviarXML\" xmlns:xsns=\"https://www.paxfacturacion.com.mx:453\"/>" +
              "</wsdl:message>" +
              "<wsdl:message name=\"fnEnviarXMLSoapOut\">" +
              "<wsdl:part name=\"parameters\" element=\"xsns:fnEnviarXMLResponse\" xmlns:xsns=\"https://www.paxfacturacion.com.mx:453\"/>" +
              "</wsdl:message>" +
              "<wsdl:portType name=\"wcfRecepcionASMXSoap\">" +
              "<wsdl:operation name=\"fnEnviarXML\">" +
              "<wsdl:input name=\"fnEnviarXMLRequest\" message=\"ns0:fnEnviarXMLSoapIn\"/>" +
              "<wsdl:output name=\"fnEnviarXMLResponse\" message=\"ns0:fnEnviarXMLSoapOut\"/>" +
              "</wsdl:operation>" +
              "</wsdl:portType>" +
              "<wsdl:binding name=\"wcfRecepcionASMXSoap\" type=\"ns0:wcfRecepcionASMXSoap\">" +
              "<soap11:binding transport=\"http://schemas.xmlsoap.org/soap/http\" style=\"document\"/>" +
              "<wsdl:operation name=\"fnEnviarXML\">" +
              "<soap11:operation soapAction=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\" style=\"document\"/>" +
              "<wsdl:input name=\"fnEnviarXMLRequest\">" +
              "<soap11:body use=\"literal\"/>" +
              "</wsdl:input>" +
              "<wsdl:output name=\"fnEnviarXMLResponse\">" +
              "<soap11:body use=\"literal\"/>" +
              "</wsdl:output>" +
              "</wsdl:operation>" +
              "</wsdl:binding>" +
              "<wsdl:binding name=\"wcfRecepcionASMXSoap12\" type=\"ns0:wcfRecepcionASMXSoap\">" +
              "<soap12:binding transport=\"http://schemas.xmlsoap.org/soap/http\" style=\"document\"/>" +
              "<wsdl:operation name=\"fnEnviarXML\">" +
              "<soap12:operation soapAction=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\" soapActionRequired=\"false\" style=\"document\"/>" +
              "<wsdl:input name=\"fnEnviarXMLRequest\">" +
              "<soap12:body use=\"literal\"/>" +
              "</wsdl:input>" +
              "<wsdl:output name=\"fnEnviarXMLResponse\">" +
              "<soap12:body use=\"literal\"/>" +
              "</wsdl:output>" +
              "</wsdl:operation>" +
              "</wsdl:binding>" +
              "<wsdl:service name=\"wcfRecepcionASMX\">" +
              "<wsdl:port name=\"wcfRecepcionASMXSoap\" binding=\"ns0:wcfRecepcionASMXSoap\">" +
              "<soap11:address location=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcionasmx.asmx\"/>" +
              "</wsdl:port>" +
              "<wsdl:port name=\"wcfRecepcionASMXSoap12\" binding=\"ns0:wcfRecepcionASMXSoap12\">" +
              "<soap12:address location=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcionasmx.asmx\"/>" +
              "</wsdl:port>" +
              "</wsdl:service>" +
              "</wsdl:definitions>]]></con:content><con:type>http://schemas.xmlsoap.org/wsdl/</con:type></con:part><con:part><con:url>http://10.54.57.208:7080/webservices/wcfRecepcionasmx.asmx.xsd1.xsd</con:url><con:content><![CDATA[<s:schema elementFormDefault=\"qualified\" targetNamespace=\"https://www.paxfacturacion.com.mx:453\" xmlns:s=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://schemas.xmlsoap.org/wsdl/soap12/\" xmlns:mime=\"http://schemas.xmlsoap.org/wsdl/mime/\" xmlns:tns=\"https://www.paxfacturacion.com.mx:453\" xmlns:soap=\"http://schemas.xmlsoap.org/wsdl/soap/\" xmlns:tm=\"http://microsoft.com/wsdl/mime/textMatching/\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http/\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\">" +
              "<s:element name=\"fnEnviarXML\">" +
              "<s:complexType>" +
              "<s:sequence>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"psComprobante\" type=\"s:string\"/>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"psTipoDocumento\" type=\"s:string\"/>" +
              "<s:element minOccurs=\"1\" maxOccurs=\"1\" name=\"pnId_Estructura\" type=\"s:int\"/>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sNombre\" type=\"s:string\"/>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sContraseña\" type=\"s:string\"/>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"sVersion\" type=\"s:string\"/>" +
              "</s:sequence>" +
              "</s:complexType>" +
              "</s:element>" +
              "<s:element name=\"fnEnviarXMLResponse\">" +
              "<s:complexType>" +
              "<s:sequence>" +
              "<s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"fnEnviarXMLResult\" type=\"s:string\"/>" +
              "</s:sequence>" +
              "</s:complexType>" +
              "</s:element>" +
              "</s:schema>]]></con:content><con:type>http://www.w3.org/2001/XMLSchema</con:type></con:part></con:definitionCache><con:endpoints><con:endpoint>http://10.54.57.208:7080/webservices/wcfRecepcionasmx.asmx</con:endpoint><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionasmx.asmx</con:endpoint></con:endpoints><con:operation isOneWay=\"false\" action=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\" name=\"fnEnviarXML\" bindingOperationName=\"fnEnviarXML\" type=\"Request-Response\" outputName=\"fnEnviarXMLResponse\" inputName=\"fnEnviarXMLRequest\" receivesAttachments=\"false\" sendsAttachments=\"false\" anonymous=\"optional\"><con:settings/>" +
            #endregion
            //--------------------------------------------------------------------------------------
            #region SoapASMX

                    SoapASMX +

              "</con:operation></con:interface>" +

              "<con:interface xsi:type=\"con:WsdlInterface\" wsaVersion=\"NONE\" name=\"BasicHttpBinding_IwcfRecepcion\" type=\"wsdl\" bindingName=\"{http://www.datapower.com/extensions/http://schemas.xmlsoap.org/wsdl/soap12/}BasicHttpBinding_IwcfRecepcion\" soapVersion=\"1_2\" anonymous=\"optional\" definition=\"http://10.54.57.208:7080/webservices/wcfRecepcion.svc?wsdl\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><con:settings/><con:definitionCache type=\"TEXT\" rootPart=\"http://10.54.57.208:7080/webservices/wcfRecepcion.svc?wsdl\"><con:part><con:url>http://10.54.57.208:7080/webservices/wcfRecepcion.svc?wsdl</con:url><con:content><![CDATA[<wsdl:definitions targetNamespace=\"http://www.datapower.com/extensions/http://schemas.xmlsoap.org/wsdl/soap12/\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\" xmlns:soap11=\"http://schemas.xmlsoap.org/wsdl/soap/\" xmlns:soap12=\"http://schemas.xmlsoap.org/wsdl/soap12/\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http/\" xmlns:mime=\"http://schemas.xmlsoap.org/wsdl/mime/\" xmlns:wsp=\"http://www.w3.org/ns/ws-policy\" xmlns:wsp200409=\"http://schemas.xmlsoap.org/ws/2004/09/policy\" xmlns:wsp200607=\"http://www.w3.org/2006/07/ws-policy\" xmlns:ns0=\"http://www.datapower.com/extensions/http://schemas.xmlsoap.org/wsdl/soap12/\" xmlns:ns1=\"https://www.paxfacturacion.com.mx:453\">" +
              "<wsdl:import location=\"wcfRecepcion.svc?ns1.wsdl\" namespace=\"https://www.paxfacturacion.com.mx:453\"/>" +
              "<wsdl:binding name=\"BasicHttpBinding_IwcfRecepcion\" type=\"ns1:IwcfRecepcion\">" +
              "<soap12:binding transport=\"http://schemas.xmlsoap.org/soap/http\" style=\"document\"/>" +
              "<wsdl:operation name=\"fnEnviarXML\">" +
              "<soap12:operation soapAction=\"https://ws.paxfacturacion.com.mx:453/IwcfRecepcion/fnEnviarXML\" soapActionRequired=\"true\" style=\"document\"/>" +
              "<wsdl:input name=\"fnEnviarXMLRequest\">" +
              "<soap12:body use=\"literal\"/>" +
              "</wsdl:input>" +
              "<wsdl:output name=\"fnEnviarXMLResponse\">" +
              "<soap12:body use=\"literal\"/>" +
              "</wsdl:output>" +
              "</wsdl:operation>" +
              "</wsdl:binding>" +
              "</wsdl:definitions>]]></con:content><con:type>http://schemas.xmlsoap.org/wsdl/</con:type></con:part><con:part><con:url>http://10.54.57.208:7080/webservices/wcfRecepcion.svc?ns1.wsdl</con:url><con:content><![CDATA[<wsdl:definitions targetNamespace=\"https://www.paxfacturacion.com.mx:453\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\" xmlns:soap11=\"http://schemas.xmlsoap.org/wsdl/soap/\" xmlns:soap12=\"http://schemas.xmlsoap.org/wsdl/soap12/\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http/\" xmlns:mime=\"http://schemas.xmlsoap.org/wsdl/mime/\" xmlns:wsp=\"http://www.w3.org/ns/ws-policy\" xmlns:wsp200409=\"http://schemas.xmlsoap.org/ws/2004/09/policy\" xmlns:wsp200607=\"http://www.w3.org/2006/07/ws-policy\" xmlns:ns0=\"http://www.datapower.com/extensions/http://schemas.xmlsoap.org/wsdl/soap12/\" xmlns:ns1=\"https://www.paxfacturacion.com.mx:453\">" +
              "<wsdl:types xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
              "<xsd:schema>" +
              "<xsd:import schemaLocation=\"wcfRecepcion.svc.xsd2.xsd\" namespace=\"https://www.paxfacturacion.com.mx:453\"/>" +
              "</xsd:schema>" +
              "</wsdl:types>" +
              "<wsdl:message name=\"IwcfRecepcion_fnEnviarXML_InputMessage\">" +
              "<wsdl:part name=\"parameters\" element=\"xsns:fnEnviarXML\" xmlns:xsns=\"https://www.paxfacturacion.com.mx:453\"/>" +
              "</wsdl:message>" +
              "<wsdl:message name=\"IwcfRecepcion_fnEnviarXML_OutputMessage\">" +
              "<wsdl:part name=\"parameters\" element=\"xsns:fnEnviarXMLResponse\" xmlns:xsns=\"https://www.paxfacturacion.com.mx:453\"/>" +
              "</wsdl:message>" +
              "<wsdl:portType name=\"IwcfRecepcion\">" +
              "<wsdl:operation name=\"fnEnviarXML\">" +
              "<wsdl:input name=\"fnEnviarXMLRequest\" message=\"ns1:IwcfRecepcion_fnEnviarXML_InputMessage\"/>" +
              "<wsdl:output name=\"fnEnviarXMLResponse\" message=\"ns1:IwcfRecepcion_fnEnviarXML_OutputMessage\"/>" +
              "</wsdl:operation>" +
              "</wsdl:portType>" +
              "<wsdl:binding name=\"BasicHttpBinding_IwcfRecepcion\" type=\"ns1:IwcfRecepcion\">" +
              "<soap11:binding transport=\"http://schemas.xmlsoap.org/soap/http\" style=\"document\"/>" +
              "<wsdl:operation name=\"fnEnviarXML\">" +
              "<soap11:operation soapAction=\"https://ws.paxfacturacion.com.mx:453/IwcfRecepcion/fnEnviarXML\" style=\"document\"/>" +
              "<wsdl:input name=\"fnEnviarXMLRequest\">" +
              "<soap11:body use=\"literal\"/>" +
              "</wsdl:input>" +
              "<wsdl:output name=\"fnEnviarXMLResponse\">" +
              "<soap11:body use=\"literal\"/>" +
              "</wsdl:output>" +
              "</wsdl:operation>" +
              "</wsdl:binding>" +
              "<wsdl:service name=\"wcfRecepcion\">" +
              "<wsdl:port name=\"BasicHttpBinding_IwcfRecepcion\" binding=\"ns1:BasicHttpBinding_IwcfRecepcion\">" +
              "<wsp:PolicyReference URI=\"#policy0\" xmlns:wsp=\"http://schemas.xmlsoap.org/ws/2004/09/policy\"/>" +
              "<soap11:address location=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcion.svc\"/>" +
              "</wsdl:port>" +
              "<wsdl:port name=\"BasicHttpBinding_IwcfRecepcion.0\" binding=\"ns0:BasicHttpBinding_IwcfRecepcion\">" +
              "<wsp:PolicyReference URI=\"#policy0\" xmlns:wsp=\"http://schemas.xmlsoap.org/ws/2004/09/policy\"/>" +
              "<soap12:address location=\"https://ws.paxfacturacion.com.mx:454/webservices/wcfRecepcion.svc\"/>" +
              "</wsdl:port>" +
              "</wsdl:service>" +
              "<wsp:Policy wsu:Id=\"policy0\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:wsp=\"http://schemas.xmlsoap.org/ws/2004/09/policy\">" +
              "<wsp:ExactlyOne>" +
              "<wsp:All>" +
              "<sp:TransportBinding xmlns:sp=\"http://schemas.xmlsoap.org/ws/2005/07/securitypolicy\" xmlns:wsx=\"http://schemas.xmlsoap.org/ws/2004/09/mex\" xmlns:wsa10=\"http://www.w3.org/2005/08/addressing\" xmlns:tns=\"https://www.paxfacturacion.com.mx:453\" xmlns:wsap=\"http://schemas.xmlsoap.org/ws/2004/08/addressing/policy\" xmlns:msc=\"http://schemas.microsoft.com/ws/2005/12/wsdl/contract\" xmlns:wsa=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsam=\"http://www.w3.org/2007/05/addressing/metadata\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:soap=\"http://schemas.xmlsoap.org/wsdl/soap/\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\">" +
              "<wsp:Policy>" +
              "<wsp:ExactlyOne>" +
              "<wsp:All>" +
              "<sp:TransportToken>" +
              "<wsp:Policy>" +
              "<wsp:ExactlyOne>" +
              "<wsp:All>" +
              "<sp:HttpsToken RequireClientCertificate=\"false\"/>" +
              "</wsp:All>" +
              "</wsp:ExactlyOne>" +
              "</wsp:Policy>" +
              "</sp:TransportToken>" +
              "<sp:AlgorithmSuite>" +
              "<wsp:Policy>" +
              "<wsp:ExactlyOne>" +
              "<wsp:All>" +
              "<sp:Basic256/>" +
              "</wsp:All>" +
              "</wsp:ExactlyOne>" +
              "</wsp:Policy>" +
              "</sp:AlgorithmSuite>" +
              "<sp:Layout>" +
              "<wsp:Policy>" +
              "<wsp:ExactlyOne>" +
              "<wsp:All>" +
              "<sp:Strict/>" +
              "</wsp:All>" +
              "</wsp:ExactlyOne>" +
              "</wsp:Policy>" +
              "</sp:Layout>" +
              "</wsp:All>" +
              "</wsp:ExactlyOne>" +
              "</wsp:Policy>" +
              "</sp:TransportBinding>" +
              "</wsp:All>" +
              "</wsp:ExactlyOne>" +
              "</wsp:Policy>" +
              "</wsdl:definitions>]]></con:content><con:type>http://schemas.xmlsoap.org/wsdl/</con:type></con:part><con:part><con:url>http://10.54.57.208:7080/webservices/wcfRecepcion.svc.xsd2.xsd</con:url><con:content><![CDATA[<xs:schema elementFormDefault=\"qualified\" targetNamespace=\"https://www.paxfacturacion.com.mx:453\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:tns=\"https://www.paxfacturacion.com.mx:453\">" +
              "<xs:element name=\"fnEnviarXML\">" +
              "<xs:complexType>" +
              "<xs:sequence>" +
              "<xs:element minOccurs=\"0\" name=\"psComprobante\" nillable=\"true\" type=\"xs:string\"/>" +
              "<xs:element minOccurs=\"0\" name=\"psTipoDocumento\" nillable=\"true\" type=\"xs:string\"/>" +
              "<xs:element minOccurs=\"0\" name=\"pnId_Estructura\" type=\"xs:int\"/>" +
              "<xs:element minOccurs=\"0\" name=\"sNombre\" nillable=\"true\" type=\"xs:string\"/>" +
              "<xs:element minOccurs=\"0\" name=\"sContraseña\" nillable=\"true\" type=\"xs:string\"/>" +
              "<xs:element minOccurs=\"0\" name=\"sVersion\" nillable=\"true\" type=\"xs:string\"/>" +
              "</xs:sequence>" +
              "</xs:complexType>" +
              "</xs:element>" +
              "<xs:element name=\"fnEnviarXMLResponse\">" +
              "<xs:complexType>" +
              "<xs:sequence>" +
              "<xs:element minOccurs=\"0\" name=\"fnEnviarXMLResult\" nillable=\"true\" type=\"xs:string\"/>" +
              "</xs:sequence>" +
              "</xs:complexType>" +
              "</xs:element>" +
              "</xs:schema>]]></con:content><con:type>http://www.w3.org/2001/XMLSchema</con:type></con:part></con:definitionCache><con:endpoints><con:endpoint>http://10.54.57.208:7080/webservices/wcfRecepcion.svc</con:endpoint><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcion.svc</con:endpoint></con:endpoints><con:operation isOneWay=\"false\" action=\"https://ws.paxfacturacion.com.mx:453/IwcfRecepcion/fnEnviarXML\" name=\"fnEnviarXML\" bindingOperationName=\"fnEnviarXML\" type=\"Request-Response\" outputName=\"fnEnviarXMLResponse\" inputName=\"fnEnviarXMLRequest\" receivesAttachments=\"false\" sendsAttachments=\"false\" anonymous=\"optional\"><con:settings/>" +

              SoapSVC +

              "</con:operation></con:interface>" + "<con:properties/><con:wssContainer/><con:sensitiveInformation/></con:soapui-project>";

            #endregion
            //--------------------------------------------------------------------------------------
            xmldoc.LoadXml(XMLProject);
            xmldoc.Save(String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAPUI/", "Proyecto"));
            ////------------------------------------------------------------------------------------
            xmldoc.LoadXml(Jmeter);
            xmldoc.Save(String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Jmeter/", "Grupo de Hilos_POST_Generacion.xml"));
            //--------------------------------------------------------------------------------------
            xmldoc.LoadXml(JmeterSello);
            xmldoc.Save(String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Jmeter/", "Grupo de Hilos_POST_Sellado.xml"));
            //--------------------------------------------------------------------------------------
            xmldoc.LoadXml(JmeterValidacion);
            xmldoc.Save(String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Jmeter/", "Grupo de Hilos_POST_Validacion.xml"));
            //--------------------------------------------------------------------------------------
            #region Comentada
            //for (int i = 0; i < 50; i++)
            //{
            //    Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Grande.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).SetValue("3.2");
            //    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //    Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Grande"+i, "1.1", false, "N", "N", "NA");
            //    Thread.Sleep(1000);
            //}
            //---------------------------------------------------------------------------------------------
            //Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "Mediano.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //navDocGenera.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).SetValue("3.2");
            //navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "Mediano", "1.1", false, "N", "N", "NA");
            //if (res == "S" || res == "s")
            //    Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "Mediano_Tim");
            //---------------------------------------------------------------------------------------------
            //Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "NominaMensual.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            //navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            //Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "NominaMensual", "1.0", false, "N", "N", "NA");
            //if (res == "S" || res == "s")
            //    Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "NominaMensual_Tim");
            // --------------------------------------------------------------------------------------------
            // Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "100KB-Menos.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).SetValue("3.2");
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            // Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "100KB-Menos", "1.1", false, "N", "N", "NA");
            // if (res == "S" || res == "s")
            //     Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "100KB-Menos_Tim");
            // --------------------------------------------------------------------------------------------
            // Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "100KB.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).SetValue("3.2");
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            // Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "100KB", "1.1", false, "N", "N", "NA");
            // if (res == "S" || res == "s")
            //     Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "100KB_Tim");
            // --------------------------------------------------------------------------------------------
            // Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "100KB-Mas.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).SetValue("3.2");
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            // Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "100KB-Mas", "1.1", false, "N", "N", "NA");
            // if (res == "S" || res == "s")
            //     Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "100KB-Mas_Tim");
            // --------------------------------------------------------------------------------------------
            // Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "200KB.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).SetValue("3.2");
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            // Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "200KB", "1.1", false, "N", "N", "NA");
            // if (res == "S" || res == "s")
            //     Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "200KB_Tim");
            // --------------------------------------------------------------------------------------------
            // Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "200KB-Mas.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).SetValue("3.2");
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            // Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "200KB-Mas", "1.1", false, "N", "N", "NA");
            // if (res == "S" || res == "s")
            //     Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "200KB-Mas_Tim");    
            // --------------------------------------------------------------------------------------------
            // Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "300KB.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).SetValue("3.2");
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            // Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "300KB", "1.1", false, "N", "N", "NA");
            // if (res == "S" || res == "s")
            //     Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "300KB_Tim");
            // --------------------------------------------------------------------------------------------
            // Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "300KB-Mas.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).SetValue("3.2");
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            // Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "300KB-Mas", "1.1", false, "N", "N", "NA");
            // if (res == "S" || res == "s")
            //     Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "300KB-Mas_Tim");
            // --------------------------------------------------------------------------------------------
            // Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "400KB.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).SetValue("3.2");
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            // Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "400KB", "1.1", false, "N", "N", "NA");
            // if (res == "S" || res == "s")
            //     Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "400KB_Tim");
            // --------------------------------------------------------------------------------------------
            // Cargar_XML(ref certCSD, ref xDocument, ref navNodoTimbre, ref sello, ref sCadenaOriginal, ref argss, "400KB-Mas.xml", ref nsmComprobante, ref navDocGenera, ref xslt);
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).SetValue("3.2");
            // navDocGenera.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).SetValue(DateTime.Now.ToString("s"));
            // Sellar_Factura(certCSD, xDocument, navNodoTimbre, ref sello, ref sCadenaOriginal, argss, nsmComprobante, navDocGenera, xslt, "400KB-Mas", "1.1", false, "N", "N", "NA");
            // if (res == "S" || res == "s")
            //     Timbrar_XML(sello, sCadenaOriginal, xDocument, nsmComprobante, "400KB-Mas_Tim");
            // --------------------------------------------------------------------------------------------
            #endregion
            //--------------------------------------------------------------------------------------
        }

        //------------------------------------------------------------------------------------------
        private static void GenerarSOAPPrevio(ref String SoapSVC, ref String SoapASMX, ref String SoapASPEL, ref String Soap12ASMX, ref String Soap12ASPEL, String ContextoTiM, ref String JmeterSello, ref string rutaAbsolutaAcuse, XmlDocument xmldoc, String Nombre, ref String rutaGen, String Estatus)
        {
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP/", "SOAP(ASPEL)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASPEL = SoapASPEL +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionaspel.asmx</con:endpoint>" +
              "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(ASPEL)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASPEL = Soap12ASPEL +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionaspel.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/wcfRecepcionASPELSoap/fnEnviarXMLRequest\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(ASMX)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASMX = Soap12ASMX +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/wcfRecepcionASMXSoap/fnEnviarXMLRequest\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP/", "SOAP(ASMX)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASMX = SoapASMX +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(SVC)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapSVC = SoapSVC +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcion.svc</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"http://www.datapower.com/extensions/http://schemas.xmlsoap.org/wsdl/soap12/IwcfRecepcion/fnEnviarXMLRequest\"/><con:wsrmConfig version=\"1.2\"/></con:call>";

            //rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Generados/", Nombre + "_Gen");
            //xmldoc.Load(rutaGen); Jmeter = GenerarJmeter(xmldoc, Jmeter, Nombre + "_OK", ContextoGeN);

            rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", Nombre);
            xmldoc.Load(rutaGen); JmeterSello = GenerarJmeter(xmldoc, JmeterSello, Nombre + Estatus, ContextoTiM);

            //rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Timbrados/", Nombre + "_Tim");
            //xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, Nombre + "_OK", ContextoVaL);
        }

        //------------------------------------------------------------------------------------------
        private static void GenerarSOAPFiel(ref String SoapSVC, ref String SoapASMX, ref String SoapASPEL, ref String Soap12ASMX, ref String Soap12ASPEL, String ContextoTiM, String ContextoVaL, ref String JmeterSello, ref String JmeterValidacion, ref String rutaAbsolutaAcuse, XmlDocument xmldoc, String Nombre, ref String rutaGen, String Estatus)
        {
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP/", "SOAP(ASPEL)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASPEL = SoapASPEL +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionaspel.asmx</con:endpoint>" +
              "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(ASPEL)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASPEL = Soap12ASPEL +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionaspel.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/wcfRecepcionASPELSoap/fnEnviarXMLRequest\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(ASMX)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASMX = Soap12ASMX +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/wcfRecepcionASMXSoap/fnEnviarXMLRequest\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP/", "SOAP(ASMX)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASMX = SoapASMX +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(SVC)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapSVC = SoapSVC +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcion.svc</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"http://www.datapower.com/extensions/http://schemas.xmlsoap.org/wsdl/soap12/IwcfRecepcion/fnEnviarXMLRequest\"/><con:wsrmConfig version=\"1.2\"/></con:call>";

            //rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Generados/", Nombre + "_Gen");
            //xmldoc.Load(rutaGen); Jmeter = GenerarJmeter(xmldoc, Jmeter, Nombre + "_OK", ContextoGeN);

            rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", Nombre);
            xmldoc.Load(rutaGen); JmeterSello = GenerarJmeter(xmldoc, JmeterSello, Nombre + Estatus, ContextoTiM);

            rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Timbrados/", Nombre + "_Tim");
            xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, Nombre + Estatus, ContextoVaL);
        }

        //------------------------------------------------------------------------------------------
        private static void GenerarSOAPTim(ref String SoapSVC, ref String SoapASMX, ref String SoapASPEL, ref String Soap12ASMX, ref String Soap12ASPEL, String ContextoGeN, String ContextoTiM, ref String Jmeter, ref String JmeterSello, ref String rutaAbsolutaAcuse, XmlDocument xmldoc, String Nombre, ref String rutaGen, String Estatus)
        {
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP/", "SOAP(ASPEL)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASPEL = SoapASPEL +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionaspel.asmx</con:endpoint>" +
              "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(ASPEL)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASPEL = Soap12ASPEL +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionaspel.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/wcfRecepcionASPELSoap/fnEnviarXMLRequest\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(ASMX)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASMX = Soap12ASMX +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/wcfRecepcionASMXSoap/fnEnviarXMLRequest\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP/", "SOAP(ASMX)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASMX = SoapASMX +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(SVC)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapSVC = SoapSVC +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcion.svc</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"http://www.datapower.com/extensions/http://schemas.xmlsoap.org/wsdl/soap12/IwcfRecepcion/fnEnviarXMLRequest\"/><con:wsrmConfig version=\"1.2\"/></con:call>";

            rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Generados/", Nombre + "_Gen");
            xmldoc.Load(rutaGen); Jmeter = GenerarJmeter(xmldoc, Jmeter, Nombre + Estatus, ContextoGeN);

            rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", Nombre);
            xmldoc.Load(rutaGen); JmeterSello = GenerarJmeter(xmldoc, JmeterSello, Nombre + Estatus, ContextoTiM);

            //rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Timbrados/", Nombre + "_Tim");
            //xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, Nombre + "_OK", ContextoVaL);
        }

        //------------------------------------------------------------------------------------------
        private static void GenerarSOAPVal(ref String SoapSVC, ref String SoapASMX, ref String SoapASPEL, ref String Soap12ASMX, ref String Soap12ASPEL, String ContextoVaL, ref String JmeterValidacion, ref String rutaAbsolutaAcuse, XmlDocument xmldoc, String Nombre, ref String rutaGen, String Estatus)
        {
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP/", "SOAP(ASPEL)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASPEL = SoapASPEL +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionaspel.asmx</con:endpoint>" +
              "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(ASPEL)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASPEL = Soap12ASPEL +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionaspel.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/wcfRecepcionASPELSoap/fnEnviarXMLRequest\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(ASMX)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASMX = Soap12ASMX +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/wcfRecepcionASMXSoap/fnEnviarXMLRequest\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP/", "SOAP(ASMX)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASMX = SoapASMX +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionasmx.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(SVC)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapSVC = SoapSVC +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcion.svc</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"http://www.datapower.com/extensions/http://schemas.xmlsoap.org/wsdl/soap12/IwcfRecepcion/fnEnviarXMLRequest\"/><con:wsrmConfig version=\"1.2\"/></con:call>";

            rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Timbrados/", Nombre + "_Tim");
            xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, Nombre + Estatus, ContextoVaL);
        }

        //------------------------------------------------------------------------------------------
        private static void GenerarSOAP(ref String rutaGen, ref String SoapSVC, ref String SoapASMX, ref String SoapASPEL, ref String Soap12ASMX, ref String Soap12ASPEL, String ContextoGeN, String ContextoTiM, String ContextoVaL, ref String Jmeter, ref String JmeterSello, ref String JmeterValidacion, ref String rutaAbsolutaAcuse, XmlDocument xmldoc, String Nombre, String Estatus) {

            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP/", "SOAP(ASPEL)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASPEL = SoapASPEL +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionaspel.asmx</con:endpoint>" +
              "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(ASPEL)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASPEL = Soap12ASPEL +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcionaspel.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/wcfRecepcionASPELSoap/fnEnviarXMLRequest\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(ASMX)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            Soap12ASMX = Soap12ASMX +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://test.paxfacturacion.com.mx:453/webservices/wcfRecepcionASMX.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/wcfRecepcionASMXSoap/fnEnviarXMLRequest\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP/", "SOAP(ASMX)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapASMX = SoapASMX +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://test.paxfacturacion.com.mx:453/webservices/wcfRecepcionASMX.asmx</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"https://ws.paxfacturacion.com.mx:453/fnEnviarXML\"/><con:wsrmConfig version=\"1.2\"/></con:call>";
            //--------------------------------------------------------------------------------------
            rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(SVC)+" + Nombre + "");
            xmldoc.Load(rutaAbsolutaAcuse);

            SoapSVC = SoapSVC +
            "<con:call name=\"" + Nombre + "\" outgoingWss=\"\" incomingWss=\"\"><con:settings><con:setting id=\"com.eviware.soapui.impl.wsdl.WsdlRequest@request-headers\">&lt;xml-fragment/></con:setting></con:settings><con:encoding>UTF-8</con:encoding><con:endpoint>https://ws.paxfacturacion.com.mx:453/webservices/wcfRecepcion.svc</con:endpoint>" +
            "<con:request><![CDATA[" + xmldoc.InnerXml + "]]></con:request><con:credentials><con:username xsi:nil=\"true\"/><con:password xsi:nil=\"true\"/><con:domain xsi:nil=\"true\"/><con:authType>Global HTTP Settings</con:authType></con:credentials><con:jmsConfig JMSDeliveryMode=\"PERSISTENT\"/><con:jmsPropertyConfig/><con:wsaConfig mustUnderstand=\"NONE\" version=\"200508\" action=\"http://www.datapower.com/extensions/http://schemas.xmlsoap.org/wsdl/soap12/IwcfRecepcion/fnEnviarXMLRequest\"/><con:wsrmConfig version=\"1.2\"/></con:call>";

            rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Generados/", Nombre + "_Gen");
            xmldoc.Load(rutaGen); Jmeter = GenerarJmeter(xmldoc, Jmeter, Nombre + Estatus, ContextoGeN);

            rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", Nombre);
            xmldoc.Load(rutaGen); JmeterSello = GenerarJmeter(xmldoc, JmeterSello, Nombre + Estatus, ContextoTiM);

            rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Timbrados/", Nombre + "_Tim");
            xmldoc.Load(rutaGen); JmeterValidacion = GenerarJmeter(xmldoc, JmeterValidacion, Nombre + Estatus, ContextoVaL);
 
        }

        //------------------------------------------------------------------------------------------
        private static String GenerarJmeter(XmlDocument xDocument, String Jmeter, String Name, String Contexto) {

            Jmeter = Jmeter + "<HTTPSamplerProxy guiclass=\"HttpTestSampleGui\" testclass=\"HTTPSamplerProxy\" testname=\"" + Name + "\" enabled=\"true\">" +
            "<boolProp name=\"HTTPSampler.postBodyRaw\">true</boolProp>" +
            "<elementProp name=\"HTTPsampler.Arguments\" elementType=\"Arguments\">" +
             "<collectionProp name=\"Arguments.arguments\">" +
               "<elementProp name=\"\" elementType=\"HTTPArgument\">" +
                 "<boolProp name=\"HTTPArgument.always_encode\">false</boolProp>" +
                 "<stringProp name=\"Argument.value\">" + xDocument.OuterXml.Replace("<", "&lt;").Replace(">", "&gt;").Replace("amp;", "amp;amp;") + "</stringProp>" +
                 "<stringProp name=\"Argument.metadata\">=</stringProp>" +
               "</elementProp>" +
             "</collectionProp>" +
            "</elementProp>" +
            "<stringProp name=\"HTTPSampler.domain\">" + Settings.Default.servicio + "</stringProp><stringProp name=\"HTTPSampler.port\">" + Settings.Default.puerto + "</stringProp>" +
            "<stringProp name=\"HTTPSampler.connect_timeout\"></stringProp><stringProp name=\"HTTPSampler.response_timeout\"></stringProp>" +
            "<stringProp name=\"HTTPSampler.protocol\"/><stringProp name=\"HTTPSampler.contentEncoding\">utf-8</stringProp>".Trim().TrimEnd().TrimStart() +
            "<stringProp name=\"HTTPSampler.path\">" + Contexto + "</stringProp><stringProp name=\"HTTPSampler.method\">POST</stringProp>" +
            "<boolProp name=\"HTTPSampler.follow_redirects\">true</boolProp><boolProp name=\"HTTPSampler.auto_redirects\">false</boolProp>" +
            "<boolProp name=\"HTTPSampler.use_keepalive\">true</boolProp><boolProp name=\"HTTPSampler.DO_MULTIPART_POST\">false</boolProp>" +
            "<stringProp name=\"HTTPSampler.implementation\">HttpClient4</stringProp><boolProp name=\"HTTPSampler.monitor\">false</boolProp><stringProp name=\"HTTPSampler.embedded_url_re\"></stringProp>" +
            "</HTTPSamplerProxy><hashTree/>";

            return Jmeter;
        }

        //------------------------------------------------------------------------------------------
        private static void Timbrar_XML(String sello, String sCadenaOriginal, XmlDocument xDocTimbrado, XmlNamespaceManager nsmComprobante, String name, String Timbrar2012, String Fecha, Boolean Productivo) {
            
            docNodoTimbre = null;
            gNodoTimbre.UUID = Guid.NewGuid().ToString();
            //sello = "aqsGwyuUHhqlqnX6ShLHEaiWC/noEFLR4ZsJng2hZFtit3uSrZYRG43PjxQa1oFhjv/zxvnz9FLGGYVWljmxMmvg7phr48o40eXcZp99zhBBvP2KZuwrIsRo0e0h0N5detGnZbhLT07GlbM4TksH1KKl/VwX2Zp2fTnTZUA40xI=";
            //sCadenaOriginal = "||3.2|2016-04-07T14:13:58|egreso||CONTADO|879.12|0.00|1|MXN|1019.78|TRANSFERENCIA ELECTRONICA DE FONDOS|San Luis Potosi,SLP|NO IDENTIFICADO|GAC951005FZ9|GRUPO ACERERO SA DE CV|EJE 108 S/N|COL. ZONA INDUSTRIAL|SAN LUIS POTOSI|SAN LUIS POTOSI|SAN LUIS POTOSI|MEXICO|78395|EJE 108 S/N|COL. ZONA INDUSTRIAL|SAN LUIS POTOSI|SAN LUIS POTOSI|MEXICO|78395|REGIMEN GENERAL DE LEY PERSONAS MORALES|CAP9306284H8|CRUZ AZUL PACIFICO, S.A. DE C. V.|BLVD JOSE MARIA LAFRAGUA 9164|SAN FRANCISCO TOTIMEHUACAN|PUEBLA|PUEBLA|PUEBLA|MEXICO|72960|8.140|TM|PTVA-XDE1-G001|VARILLA CORRUGADA 3/8 GAX DOBLADA 1a a 12 Mts|108.0000|879.12|0.00|IVA|16.00|140.66|140.66||";
            //gNodoTimbre.UUID = "621b6392-9a26-4a7c-9b6a-dadf9c19e12a";
            //gNodoTimbre.FechaTimbrado = Convert.ToDateTime("2016-04-07T14:14:59");


            if (Timbrar2012 == "false")
                gNodoTimbre.FechaTimbrado = Convert.ToDateTime(DateTime.Now.ToString("s"));
            if (Timbrar2012 == "true")
                gNodoTimbre.FechaTimbrado = Convert.ToDateTime("2012-05-08T12:30:00");
            if (Timbrar2012 == "fiel")
                gNodoTimbre.FechaTimbrado = Convert.ToDateTime("2015-02-08T10:25:00");
            if (Timbrar2012 == "veh10")
                gNodoTimbre.FechaTimbrado = Convert.ToDateTime("2013-11-25T18:15:00");

            if (Timbrar2012 == "ine10")
                gNodoTimbre.FechaTimbrado = Convert.ToDateTime("2016-08-25T18:15:00");

            if (Timbrar2012 == "dyna")
            {
                gNodoTimbre.FechaTimbrado = Convert.ToDateTime(Fecha);
            }

            if (Timbrar2012 == "2010")
            {
                Timbrar2012 = "true";

                gNodoTimbre.FechaTimbrado = Convert.ToDateTime(Fecha);
            }

            gNodoTimbre.selloCFD = sello;
            fnGenerarSelloPAC(ref docNodoTimbre, gCertificado, ref sCadenaOriginal, Timbrar2012, Productivo);
            docNodoTimbre = gTimbrado.fnGenerarXML(gNodoTimbre);
            XmlNode Complemento = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento", nsmComprobante);
            if (Complemento == null)
                Complemento = xDocTimbrado.CreateElement("cfdi", "Complemento", nsmComprobante.LookupNamespace("cfdi"));

            Complemento.InnerXml = docNodoTimbre.DocumentElement.OuterXml + Complemento.InnerXml;
            xDocTimbrado.DocumentElement.AppendChild(Complemento);
            gTimbrado.fnDestruirLlave();

            var rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos+"/Ruta Timbrados/", name);
            AccesoDisco.GuardarArchivoTexto(rutaAbsolutaAcuse, xDocTimbrado.OuterXml);
        }
        
        //------------------------------------------------------------------------------------------
        private static void Cargar_XML(ref X509Certificate2 certCSD, ref XmlDocument xDocument, ref XPathNavigator navNodoTimbre, ref String sello, ref String sCadenaOriginal, ref XsltArgumentList argss, String nombre, ref XmlNamespaceManager nsmComprobante, ref XPathNavigator navDocGenera, ref XslCompiledTransform xslt) {

            certCSD = new X509Certificate2();
            xDocument = new XmlDocument();
            xDocument.Load(Settings.Default.archivoXML + nombre);
            navNodoTimbre = xDocument.CreateNavigator();            
            sello = string.Empty;
            sCadenaOriginal = string.Empty;
            argss = new XsltArgumentList();

            nsmComprobante = new XmlNamespaceManager(xDocument.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            navDocGenera = xDocument.CreateNavigator();
            xslt = new XslCompiledTransform();
        }

        //------------------------------------------------------------------------------------------
        private static void Cargar_XML20(ref X509Certificate2 certCSD, ref XmlDocument xDocument, ref XPathNavigator navNodoTimbre, ref String sello, ref String sCadenaOriginal, ref XsltArgumentList argss, String nombre, ref XmlNamespaceManager nsmComprobante, ref XPathNavigator navDocGenera, ref XslCompiledTransform xslt) {
          
            certCSD = new X509Certificate2();
            xDocument = new XmlDocument();
            xDocument.Load(Settings.Default.archivoXML + nombre);
            navNodoTimbre = xDocument.CreateNavigator();
            sello = string.Empty;
            sCadenaOriginal = string.Empty;
            argss = new XsltArgumentList();

            nsmComprobante = new XmlNamespaceManager(xDocument.NameTable);
            nsmComprobante.AddNamespace("cfd","http://www.sat.gob.mx/cfd/2");
            //nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            navDocGenera = xDocument.CreateNavigator();
            xslt = new XslCompiledTransform();
        }
        
        //------------------------------------------------------------------------------------------
        private static void Sellar_Factura(X509Certificate2 certEmisor, XmlDocument xDocument, XPathNavigator navNodoTimbre, ref String sello, ref String sCadenaOriginal, XsltArgumentList argss, XmlNamespaceManager nsmComprobante, XPathNavigator navDocGenera, XslCompiledTransform xslt, String name, String versionComp, Boolean alterarSello, String removerNodo, String alterarCert, String TipoCer, Boolean modificarEmisor, String soap_type, Boolean sectorprimario, Boolean alterarCadena) {

            String numeroCertificado = String.Empty;
            Byte[] bCertificado = null;
            Byte[] bLlave = null;
            String sPassword = String.Empty;            

            if (TipoCer == "NA") {
                if (Settings.Default.certificadosSHA256 == "false") {
                    bCertificado = File.ReadAllBytes(Settings.Default.certificado);
                    bLlave = File.ReadAllBytes(Settings.Default.llave);
                    sPassword = Settings.Default.password;
                } else {
                    bCertificado = File.ReadAllBytes(Settings.Default.certificado256);
                    bLlave = File.ReadAllBytes(Settings.Default.llave256);
                    sPassword = Settings.Default.password256;
                }
            }

            if (TipoCer == "BAJ") {
                bCertificado = File.ReadAllBytes(Settings.Default.certificadoBAJ);
                bLlave = File.ReadAllBytes(Settings.Default.llaveBAJ);
                sPassword = Settings.Default.password;
            }

            if (TipoCer == "PZA")
            {
                bCertificado = File.ReadAllBytes(Settings.Default.certificadoPZA);
                bLlave = File.ReadAllBytes(Settings.Default.llavePZA);
                sPassword = Settings.Default.password;
            }

            if (TipoCer == "LAN")
            {
                bCertificado = File.ReadAllBytes(Settings.Default.certificadoLAN);
                bLlave = File.ReadAllBytes(Settings.Default.llaveLAN);
                sPassword = Settings.Default.password;
            }

            if (TipoCer == "FF") {
                bCertificado = File.ReadAllBytes(Settings.Default.certificadoFiel_F);
                bLlave = File.ReadAllBytes(Settings.Default.llaveFiel_F);
                sPassword = Settings.Default.passwordFiel_F;
            }

            if (TipoCer == "2012") {
                bCertificado = File.ReadAllBytes(Settings.Default.certificado2012);
                bLlave = File.ReadAllBytes(Settings.Default.llave2012);
                sPassword = Settings.Default.password2012;
            }
            
            if (TipoCer == "FM") {
                bCertificado = File.ReadAllBytes(Settings.Default.certificadoFiel_M);
                bLlave = File.ReadAllBytes(Settings.Default.llaveFiel_M);
                sPassword = Settings.Default.passwordFiel_M;
            }

            if (TipoCer == "F&") {
                bCertificado = File.ReadAllBytes(Settings.Default.certificadoAMP);
                bLlave = File.ReadAllBytes(Settings.Default.llaveAMP);
                sPassword = Settings.Default.passwordAMP;
            }

            if (TipoCer == "FÑ") {
                bCertificado = File.ReadAllBytes(Settings.Default.certificadoñ);
                bLlave = File.ReadAllBytes(Settings.Default.llaveñ);
                sPassword = Settings.Default.passwordñ;
            }

            if (TipoCer == "AQ") {
                bCertificado = File.ReadAllBytes(Settings.Default.certificadoAAQ);
                bLlave = File.ReadAllBytes(Settings.Default.llaveAAQ);
                sPassword = Settings.Default.passwordñ;
            }

            if (TipoCer == "SL") {
                bCertificado = File.ReadAllBytes(Settings.Default.certificadoSLU);
                bLlave = File.ReadAllBytes(Settings.Default.llaveSLU);
                sPassword = Settings.Default.passwordñ;
            }

            if (TipoCer == "DUDB290712KN0" || TipoCer == "GAR6606141Q1" || TipoCer == "AGP500110M19") {
                bCertificado = File.ReadAllBytes(Settings.Default.certificado);
                bLlave = File.ReadAllBytes(Settings.Default.llave);
                //bCertificado = File.ReadAllBytes(Settings.Default.certificado);
                //bLlave = File.ReadAllBytes(Settings.Default.llave);
                sPassword = Settings.Default.password;
            }
            
            certEmisor.Import(bCertificado);
            String sCertificado = Convert.ToBase64String(certEmisor.GetRawCertData());
            String rfc = String.Empty;
            numeroCertificado = ObtenerNoCertificado(certEmisor);
            String certName = Convert.ToString(certEmisor.SubjectName.Name.Trim());
            List<String> names = certName.Split(',').ToList<String>();

            foreach (String str in names) {
                List<String> res = str.Split('=').ToList<String>();
                if (res[0].Trim() == "OID.2.5.4.45") {
                    List<String> rest = res[1].Split('/').ToList<String>();
                    //rfc = rest[0].Replace("&", "&amp;").Trim();
                    rfc = rest[0].Trim();
                }
            }

            if (modificarEmisor == true) {
                navDocGenera.SelectSingleNode("cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).SetValue("AAA010101AAB");
            } else {
                if (versionComp == "2.0" || versionComp == "2.2") {
                    navDocGenera.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/@rfc", nsmComprobante).SetValue(rfc);
                } else {
                    //rfc = "DUDB290712KN0";
                    //rfc = "AHO0505163C7"; //NOM166
                    navDocGenera.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).SetValue(rfc);
                }
            }

            if (sectorprimario == true)
            {
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).SetValue("&AGP500110M19");
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).SetValue(rfc);
            }

            if (TipoCer == "DUDB290712KN0") {
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).SetValue("DUDB290712KN0");
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).SetValue(rfc);
            }

            if (TipoCer == "GAR6606141Q1") {
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).SetValue("GA&R6606141Q1");
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).SetValue(rfc);
            }

            if (TipoCer == "AGP500110M19") {
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).SetValue("&AGP500110M19");
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).SetValue(rfc);
            }

            if (versionComp == "1.0")
                xslt.Load(typeof(CaOri.V32));
            if (versionComp == "1.1")
                xslt.Load(typeof(CaOri.V3211));
            if (versionComp == "3.0")
                xslt.Load(typeof(CaOri.V30));

            if(versionComp == "2.0")
                xslt.Load(typeof(cadenaoriginal_2_0));
            if(versionComp == "2.2")
                xslt.Load(typeof(cadenaoriginal_2_2));

            MemoryStream ms = new MemoryStream();
            StreamReader srDll;

            xslt.Transform(navNodoTimbre, argss, ms);
            ms.Seek(0, SeekOrigin.Begin);
            srDll = new StreamReader(ms);

            

            if (versionComp == "2.0" || versionComp == "2.2") {
                navDocGenera.SelectSingleNode("/cfd:Comprobante/@noCertificado", nsmComprobante).SetValue(numeroCertificado);
            } else {
                if(name == "Nomina12(103)") numeroCertificado = "AAAAAAAAAAAAAA";
                else navDocGenera.SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).SetValue(numeroCertificado);
            }

            sCadenaOriginal = srDll.ReadToEnd();
            sello = fnGenerarSello(sCadenaOriginal, AlgoritmoSellado.SHA1, false, bLlave, sPassword, alterarCadena, alterarSello);

            if (removerNodo == "S")
                xDocument.DocumentElement.RemoveAttribute("total");

            var rutaGen = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Generados/", name + "_Gen"); AccesoDisco.GuardarArchivoTexto(rutaGen, xDocument.OuterXml);

            if (versionComp == "2.0" || versionComp == "2.2") {
                navDocGenera.SelectSingleNode("/cfd:Comprobante/@sello", nsmComprobante).SetValue(sello);
            } else {
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).SetValue(sello);
            }

            if (removerNodo != "S") {
                if (versionComp == "2.0" || versionComp == "2.2") {
                    navDocGenera.SelectSingleNode("/cfd:Comprobante/@certificado", nsmComprobante).SetValue(sCertificado);
                    if (alterarCert == "S")
                        navDocGenera.SelectSingleNode("/cfd:Comprobante/@certificado", nsmComprobante).SetValue("D" + sCertificado);
                } else {
                    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@certificado", nsmComprobante).SetValue(sCertificado);
                    if (alterarCert == "S")
                        navDocGenera.SelectSingleNode("/cfdi:Comprobante/@certificado", nsmComprobante).SetValue("D" + sCertificado);
                }
            } else {
                    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@certificado", nsmComprobante).SetValue(sCertificado);
            }            

            if (soap_type == "SOAP(ASMX)") {
                String SOAP =
                "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:test=\"https://test.paxfacturacion.com.mx:453\">" +
                    "<soapenv:Header/>" +
                        "<soapenv:Body>" +
                            "<test:fnEnviarXML>" +
                                "<test:psComprobante>" + xDocument.OuterXml.Replace("<", "&lt;").Replace(">", "&gt;").Replace("amp;", "amp;amp;") + "</test:psComprobante>" +
                                "<test:psTipoDocumento>factura</test:psTipoDocumento>" +
                                "<test:pnId_Estructura>0</test:pnId_Estructura>" +
                                "<test:sNombre>wsdl_pax</test:sNombre>" +
                                "<test:sContraseña>wqfCr8O3xLfEhMOHw4nEjMSrxJnvv7bvvr4cVcKuKkBEM++/ke+/gCPvv4nvvrfvvaDvvb/vvqTvvoA=</test:sContraseña>" +
                                "<test:sVersion>3.2</test:sVersion>" +
                            "</test:fnEnviarXML>" +
                        "</soapenv:Body>" +
                "</soapenv:Envelope>";

                var rutaAbsolutaAcuse = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP/", "SOAP(ASMX)+" + name);
                AccesoDisco.GuardarArchivoTexto(rutaAbsolutaAcuse, SOAP);
            }

            if (soap_type == "SOAP12(ASMX)") {
                String SOAP12 =
                    "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:test=\"https://test.paxfacturacion.com.mx:453\">" +
                        "<soap:Header/>" +
                            "<soap:Body>" +
                                "<test:fnEnviarXML>" +
                                    "<test:psComprobante>" + xDocument.OuterXml.Replace("<", "&lt;").Replace(">", "&gt;").Replace("amp;", "amp;amp;") + "</test:psComprobante>" +
                                    "<test:psTipoDocumento>factura</test:psTipoDocumento>" +
                                    "<test:pnId_Estructura>0</test:pnId_Estructura>" +
                                    "<test:sNombre>wsdl_pax</test:sNombre>" +
                                    "<test:sContraseña>wqfCr8O3xLfEhMOHw4nEjMSrxJnvv7bvvr4cVcKuKkBEM++/ke+/gCPvv4nvvrfvvaDvvb/vvqTvvoA=</test:sContraseña>" +
                                    "<test:sVersion>3.2</test:sVersion>" +
                                "</test:fnEnviarXML>" +
                            "</soap:Body>" +
                    "</soap:Envelope>";

                var rutaAbsolutaAcuse = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(ASMX)+" + name);
                AccesoDisco.GuardarArchivoTexto(rutaAbsolutaAcuse, SOAP12);
            }

            if (soap_type == "SOAP12(SVC)") {
                String SOAP12 =
                    "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:pax=\"https://www.paxfacturacion.com.mx:453\">" +
                        "<soap:Header/>" +
                            "<soap:Body>" +
                                "<pax:fnEnviarXML>" +
                                    "<pax:psComprobante>" + xDocument.OuterXml.Replace("<", "&lt;").Replace(">", "&gt;").Replace("amp;","amp;amp;") + "</pax:psComprobante>" + 
                                    "<pax:psTipoDocumento>factura</pax:psTipoDocumento>" +
                                    "<pax:pnId_Estructura>0</pax:pnId_Estructura>" +
                                    "<pax:sNombre>paxgeneracion</pax:sNombre>" +
                                    "<pax:sContraseña>wpTCnMSfxI/CvsOlxKnEnsSDxJEJIcO1wr4pW8K6wqF4wpbvvpvvvqbvvrbvvorvvZ7vvZrvvLfvvLrvvbY=</pax:sContraseña>" +
                                    "<pax:sVersion>3.2</pax:sVersion>" +
                                "</pax:fnEnviarXML>" +
                            "</soap:Body>" +
                    "</soap:Envelope>";

                var rutaAbsolutaAcuse = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(SVC)+" + name);
                AccesoDisco.GuardarArchivoTexto(rutaAbsolutaAcuse, SOAP12);
            }

            if (soap_type == "SOAP(ASPEL)") {
                String SOAP =
                "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:pax=\"https://www.paxfacturacion.com.mx:453\">" +
                    "<soapenv:Header/>" +
                        "<soapenv:Body>" +
                            "<pax:fnEnviarXML>" +
                                "<pax:psComprobante>" + xDocument.OuterXml.Replace("<", "&lt;").Replace(">", "&gt;").Replace("amp;","amp;amp;") + "</pax:psComprobante>" + 
                                "<pax:psTipoDocumento>factura</pax:psTipoDocumento>" +
                                "<pax:pnId_Estructura>0</pax:pnId_Estructura>" +
                                "<pax:sNombre>paxgeneracion</pax:sNombre>" +
                                "<pax:sContrasena>UHJ1ZWJhMTIr</pax:sContrasena>" +
                                "<pax:sVersion>3.2</pax:sVersion>" +
                            "</pax:fnEnviarXML>" +
                        "</soapenv:Body>" +
                "</soapenv:Envelope>";

                var rutaAbsolutaAcuse = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP/", "SOAP(ASPEL)+" + name);
                AccesoDisco.GuardarArchivoTexto(rutaAbsolutaAcuse, SOAP);
            }

            if (soap_type == "SOAP12(ASPEL)") {
                String SOAP12 =
                    "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:pax=\"https://www.paxfacturacion.com.mx:453\">" +
                        "<soap:Header/>" +
                            "<soap:Body>" +
                                "<pax:fnEnviarXML>" +
                                    "<pax:psComprobante>" + xDocument.OuterXml.Replace("<", "&lt;").Replace(">", "&gt;").Replace("amp;","amp;amp;") + "</pax:psComprobante>" + 
                                    "<pax:psTipoDocumento>factura</pax:psTipoDocumento>" +
                                    "<pax:pnId_Estructura>0</pax:pnId_Estructura>" +
                                    "<pax:sNombre>paxgeneracion</pax:sNombre>" +
                                    "<pax:sContrasena>UHJ1ZWJhMTIr</pax:sContrasena>" +
                                    "<pax:sVersion>3.2</pax:sVersion>" +
                                "</pax:fnEnviarXML>" +
                            "</soap:Body>" +
                    "</soap:Envelope>";

                var rutaAbsolutaAcuse = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/SOAP12/", "SOAP12(ASPEL)+" + name);
                AccesoDisco.GuardarArchivoTexto(rutaAbsolutaAcuse, SOAP12);
            }

            var rutaAcuse = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Sellados/", name);
            AccesoDisco.GuardarArchivoTexto(rutaAcuse, xDocument.OuterXml);

            //rutaAcuse = String.Format("{0}\\{1}.xml", Settings.Default.rutaArchivos + "/Ruta Generados/", name + "_Gen");
            //AccesoDisco.GuardarArchivoTexto(rutaAcuse, xDocument.OuterXml);
        }
        
        //------------------------------------------------------------------------------------------
        static String ObtenerNoCertificado(X509Certificate2 certEmisor) {
            Byte[] bCertificadoInvertido = certEmisor.GetSerialNumber().Reverse().ToArray();
            return Encoding.Default.GetString(bCertificadoInvertido);
        }
        
        //------------------------------------------------------------------------------------------
        static private Byte[] fnDesencriptarLlave(Byte[] pbLlave) {
            return Utilerias.Encriptacion.DES3.Desencriptar(pbLlave);
        }
        
        //------------------------------------------------------------------------------------------
        static private String fnDesencriptarPassword(String psPassword) {
            return Encoding.UTF8.GetString(Utilerias.Encriptacion.DES3.Desencriptar(Convert.FromBase64String(psPassword)));
        }
        
        //------------------------------------------------------------------------------------------
        static String fnGenerarSello(String psCadenaOriginal, AlgoritmoSellado pAlgoritmo, Boolean pbDesencriptar, Byte[] bLlave, String sPassword, Boolean alterarSello, Boolean alterarCadenaOriginal) {

            try {
                //Llave privada original
                Chilkat.PrivateKey key = new Chilkat.PrivateKey();

                DataTable certificado = new DataTable();

                if (pbDesencriptar)
                    key.LoadPkcs8Encrypted(fnDesencriptarLlave(bLlave), fnDesencriptarPassword(sPassword));
                else
                    key.LoadPkcs8Encrypted(bLlave, sPassword);

                // Llave privada PEM
                Chilkat.PrivateKey pem = new Chilkat.PrivateKey();
                pem.LoadPem(key.GetPkcs8Pem());
                String pkeyXml = pem.GetXml();
                Chilkat.Rsa rsa = new Chilkat.Rsa();

                Boolean bSuccess;
                bSuccess = rsa.UnlockComponent("INTERMRSA_78UJEvED0IwK");
                //if (pAlgoritmo == AlgoritmoSellado.SHA1)
                bSuccess = rsa.GenerateKey(1024);
                //else
                //bSuccess = rsa.GenerateKey(2048);

                rsa.LittleEndian = false;
                rsa.EncodingMode = "base64";
                rsa.Charset = "utf-8";
                rsa.ImportPrivateKey(pkeyXml);               

                // Definimos el algoritmo
                String sAlgoritmo = String.Empty;
                if (pAlgoritmo == AlgoritmoSellado.SHA1)
                    sAlgoritmo = "sha-1";
                else
                    sAlgoritmo = "SHA-256";

                String sello = String.Empty;
                if (alterarCadenaOriginal == false)
                    sello = rsa.SignStringENC(psCadenaOriginal, sAlgoritmo);
                else
                    sello = rsa.SignStringENC(psCadenaOriginal+"D", sAlgoritmo);

                if (alterarSello == true) sello = sello + "'";

                //  Destruimos los objetos por seguridad
                try {
                    key = new Chilkat.PrivateKey();
                    key.Dispose();
                    pem = new Chilkat.PrivateKey();
                    pem.Dispose();
                    rsa = new Chilkat.Rsa();
                    rsa.Dispose();
                } catch (Exception) {
                } return sello;
            } catch (Exception) {
                return null;
            }
        }
        
        //------------------------------------------------------------------------------------------
        public XmlDocument fnGenerarXML(TimbreFiscalDigital datos) {

            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);
            XmlDocument xXml = new XmlDocument();
            XmlSerializerNamespaces sns = new XmlSerializerNamespaces();
            sns.Add("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            XmlSerializer serializador = new XmlSerializer(typeof(TimbreFiscalDigital));
            try {
                serializador.Serialize(sw, datos, sns);
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);

                xXml.LoadXml(sr.ReadToEnd());
                XmlAttribute att = xXml.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                att.Value = "http://www.sat.gob.mx/TimbreFiscalDigital http://www.sat.gob.mx/timbrefiscaldigital/TimbreFiscalDigital.xsd";
                xXml.DocumentElement.SetAttributeNode(att);

                return xXml;
            } catch (Exception) {
                return xXml;
            }
        }
        
        //------------------------------------------------------------------------------------------
        public static Boolean fnGenerarSelloPAC(ref XmlDocument docNodoTimbre, clsValCertificado vValidadorCertificado, ref String sCadenaOriginal, String Timbrar2012, Boolean Productivo) {
            
            Byte[] bLlave;
            Byte[] bCertificado;
            Boolean bvalidacion = false;

            String selloPAC;
            String sPassword;
            String noCertificadoPAC;

            try {
                if (Timbrar2012 == "true") {
                    bCertificado = 
                        File.ReadAllBytes(Settings.Default.certificadoPAC2012);
                    bLlave = File.ReadAllBytes(Settings.Default.llavePAC2012);
                    sPassword = Settings.Default.passwordPAC2012;
                } else {
                    bCertificado = File.ReadAllBytes(Settings.Default.certificadoPac);
                    bLlave = File.ReadAllBytes(Settings.Default.llavePac);
                    sPassword = Settings.Default.password;
                }

                if (Timbrar2012 == "SP") {
                    bCertificado = File.ReadAllBytes(Settings.Default.certificadoPac);
                    bLlave = File.ReadAllBytes(Settings.Default.llavePac);
                    sPassword = Settings.Default.password;
                }

                gTimbrado = new clsOperacionTimbradoSellado(bLlave, sPassword);
                gCertificado = new clsValCertificado(bCertificado);
                noCertificadoPAC = gCertificado.ObtenerNoCertificado();
                gNodoTimbre.noCertificadoSAT = noCertificadoPAC;

                // Generamos el primer XML necesario para generar la cadena original
                docNodoTimbre = gTimbrado.fnGenerarXML(gNodoTimbre);

                // Generamos la cadena original
                MemoryStream ms;
                StreamReader srDll;
                XsltArgumentList args;
                XslCompiledTransform xslt;
                XPathNavigator navNodoTimbre = docNodoTimbre.CreateNavigator();

                // Load the type of the class.
                xslt = new XslCompiledTransform();
                xslt.Load(typeof(Timbrado.V3.TFDXSLT));

                ms = new MemoryStream();
                args = new XsltArgumentList();

                xslt.Transform(navNodoTimbre, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);

                sCadenaOriginal = srDll.ReadToEnd();

                selloPAC = gTimbrado.fnGenerarSello(sCadenaOriginal, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1);
                gNodoTimbre.selloSAT = selloPAC;


                if (Productivo)
                {
                    ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);
                    clsHSMComunicacion gHSM = new clsHSMComunicacion();
                    docNodoTimbre = fnFirmaHSMPrincipal(gHSM, sCadenaOriginal, Timbrar2012);
                }
                // if (vValidadorCertificado.fnVerificarSelloPAC(sCadenaOriginal, selloPAC, noCertificadoPAC))
                // {
                //     bvalidacion = true;
                // }
            } catch (Exception) {
                noCertificadoPAC = String.Empty;
                bvalidacion = false;
                // sError = ex.Message;
            }
                return bvalidacion;
        }
        
        //-------------------------------------------------------------------------------------------------
        public static Boolean AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
            return true;
        }

        //-------------------------------------------------------------------------------------------------
        public static XmlDocument fnFirmaHSMPrincipal(clsHSMComunicacion gHSM, string sCadenaOriginal, String Timbrar2012)
        {
            //-- HSM1 ----------------------------------------------------------------------------
            string noCertificadoPAC = string.Empty;
            string selloPAC = string.Empty;
            bool bvalidacion = false;
            //------------------------------------------------------------------------------------
            try
            {
                //--------------------------------------------------------------------------------
                hsm = new xmCryptoService();
                hsm.Url = "https://" + ("10.54.57.203") + ":8443/xmc/services/xmCryptoService";
                //--------------------------------------------------------------------------------
                noCertificadoPAC = gHSM.fnObtenerNumeroCertificado(gHSM.fnHSMLogin(hsm), hsm);
                noCertificadoPAC = "00001000000301251152";
                gNodoTimbre.noCertificadoSAT = noCertificadoPAC;

                if (Timbrar2012 == "veh10")
                    gNodoTimbre.FechaTimbrado = Convert.ToDateTime("2013-11-25T18:15:00");

                if (Timbrar2012 == "ine10")
                    gNodoTimbre.FechaTimbrado = Convert.ToDateTime("2016-08-25T18:15:00");

                //-- Generamos el primer XML necesario para generar la cadena original -----------
                docNodoTimbre = gTimbrado.fnGenerarXML(gNodoTimbre);

                //-- Generamos la cadena original ------------------------------------------------            
                XPathNavigator navNodoTimbre = docNodoTimbre.CreateNavigator();
                //--------------------------------------------------------------------------------
                MemoryStream ms;
                StreamReader srDll;
                XsltArgumentList args;
                XslCompiledTransform xslt;
                //-- Load the type of the class --------------------------------------------------
                xslt = new XslCompiledTransform();
                xslt.Load(typeof(Timbrado.V3.TFDXSLT));
                //--------------------------------------------------------------------------------
                ms = new MemoryStream();
                args = new XsltArgumentList();
                //--------------------------------------------------------------------------------
                xslt.Transform(navNodoTimbre, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);
                //--------------------------------------------------------------------------------
                sCadenaOriginal = srDll.ReadToEnd();
                //--------------------------------------------------------------------------------
                selloPAC = gHSM.fnRecuperaFirma(sCadenaOriginal, hsm);
                gNodoTimbre.selloSAT = selloPAC;
                gHSM.fnHSMLogOut(hsm);

                return docNodoTimbre;
                //--------------------------------------------------------------------------------
                //if (vValidadorCertificado.fnVerificarSelloPAC(sCadenaOriginal, selloPAC, noCertificadoPAC))
                //{
                //    //----------------------------------------------------------------------------
                //    bvalidacion = true;
                //    //----------------------------------------------------------------------------
                //}
                ////--------------------------------------------------------------------------------
                //else
                //{
                //    //----------------------------------------------------------------------------
                //    sError = "CadenaOriginal: " + sCadenaOriginal + "SelloPAC: " + selloPAC + "Certificado: " + noCertificadoPAC;
                //    //----------------------------------------------------------------------------
                //}
                //--------------------------------------------------------------------------------
            }
            //------------------------------------------------------------------------------------
            catch (Exception ex)
            {
                return docNodoTimbre;
                //--------------------------------------------------------------------------------
                //noCertificadoPAC = string.Empty;
                //bvalidacion = false;
                //sError = ex.Message;
                //--------------------------------------------------------------------------------
            }
            //------------------------------------------------------------------------------------
           // return bvalidacion;
            //------------------------------------------------------------------------------------
        }
    }
    //--------------------------------------------------------------------------------------------
}
//------------------------------------------------------------------------------------------------

