using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace WcfService1
{


    /// <summary>
    /// Author:		Alejandro Martinez
    /// Date: 		14.04.2021
    /// Description:  Service1 : IService1
    /// </summary>
    /// Variables que están incluidas en las funciones
    /// <param name="sSalida">C:\PAXConectorCapacitacion06\Entrada\</param>
    /// <param name="sTxtProcesados">C:\PAXConectorCapacitacion06\TXT Procesados\</param>
    /// <param name="sLog">C:\PAXConectorCapacitacion06\Log\</param>
    /// <param name="sErrores">C:\PAXConectorCapacitacion06\Errores\</param>
    /// <param name="sCertificados">C:\PAXConectorCapacitacion06\Certificados\</param>
    /// <param name="sTemp">C:\PAXConectorCapacitacion06\Temp\</param>

    public class Service1 : IService1
    {

        string sDirTemp = @"C:\wsPAXConectorCapacitacion06\Temp\";
        //string sDirTemp = ConfigurationManager.AppSettings["sTemp"].ToString();
        string sDirOpenssl = @"C:\wsPAXconectorCapacitacion06\Openssl\";
        //string sDirOpenssl = ConfigurationManager.AppSettings["sOpenssl"].ToString();
        string sCadenaOriginalXSLT = @"C:\wsPAXConectorCapacitacion06\XSLT\cadenaoriginal_3_3.xslt";
        //string sCadenaOriginalXSLT = ConfigurationManager.AppSettings["sCadenaOriginalXSLT"].ToString();
        string sDirKey = @"C:\wsPAXConectorCapacitacion06\Key\";

        public bool fnLeerCER(string NombreArchivo, out string Inicio, out string Final, out string Serie, out string Numero)
        {

            if (NombreArchivo.Length < 1)
            {
                Inicio = "";
                Final = "";
                Serie = "INVALIDO";
                Numero = "";
                return false;
            }
            X509Certificate2 objCert = new X509Certificate2(NombreArchivo);
            StringBuilder objSB = new StringBuilder("Detalle del certificado: \n\n");

            //Detalle
            objSB.AppendLine("Persona = " + objCert.Subject);
            objSB.AppendLine("Emisor = " + objCert.Issuer);
            objSB.AppendLine("Válido desde = " + objCert.NotBefore.ToString());
            Inicio = objCert.NotBefore.ToString();
            objSB.AppendLine("Válido hasta = " + objCert.NotAfter.ToString());
            Final = objCert.NotAfter.ToString();
            objSB.AppendLine("Tamaño de la clave = " + objCert.PublicKey.Key.KeySize.ToString());
            objSB.AppendLine("Número de serie = " + objCert.SerialNumber);
            Serie = objCert.SerialNumber.ToString();

            objSB.AppendLine("Hash = " + objCert.Thumbprint);
            //Numero = "?";
            string tNumero = "", rNumero = "", tNumero2 = "";

            int X;
            if (Serie.Length < 2)
                Numero = "";
            else
            {
                foreach (char c in Serie)
                {
                    switch (c)
                    {
                        case '0': tNumero += c; break;
                        case '1': tNumero += c; break;
                        case '2': tNumero += c; break;
                        case '3': tNumero += c; break;
                        case '4': tNumero += c; break;
                        case '5': tNumero += c; break;
                        case '6': tNumero += c; break;
                        case '7': tNumero += c; break;
                        case '8': tNumero += c; break;
                        case '9': tNumero += c; break;
                    }
                }
                for (X = 1; X < tNumero.Length; X++)
                {
                    X += 1;
                    tNumero2 = tNumero.Substring(0, X);
                    rNumero = rNumero + tNumero2.Substring(tNumero2.Length - 1, 1);
                }
                Numero = rNumero;

            }

            if (DateTime.Now < objCert.NotAfter && DateTime.Now > objCert.NotBefore)
            {
                return true;
            }



            return false;
        }


        public string fnCertificado(string ArchivoCER)
        {
            byte[] Certificado = File.ReadAllBytes(ArchivoCER);
            return Base64_Encode(Certificado);
        }

        string Base64_Encode(byte[] str)
        {
            return Convert.ToBase64String(str);
        }


        /// <summary>
        /// Author:		Alejandro Martinez
        /// Date: 		14.04.2021
        /// Description:  fnCrearSello 
        /// </summary>
        /// Variables que están incluidas en las funciones
        /// <param name="sXMLsinSello">String del XML</param>
        /// <param name="sKey">Nombre de la key</param>
        /// <param name="sClavePrivada">Clave privada</param>
        /// <param name="sArchivoExtXML">Nombre del archivo XML</param>

        public string fnCrearSello(string sXMLsinSello, string sKey, string sClavePrivada, string sArchivoExtXML)
        {
            try
            {

                string sWSKey = sDirKey + sKey;
                
                string stDirTemp = sDirTemp  + sArchivoExtXML;

                

                string scadenaOriginal = fnCadenaOriginal(sXMLsinSello);

                File.WriteAllText(stDirTemp + "cadena.txt", scadenaOriginal);
                string sArchivoCadena = stDirTemp + "cadena.txt";

                string sArchivoPem = stDirTemp +  ".pem";

                //string SelloTxt =  "sello.txt";
                //string sArchivoSello = sDirTemp + SelloTxt;

                string FileSello ="sellobinario.txt";
                string sArchivoBinario = stDirTemp + FileSello;

                string sSello = string.Empty;


                //SE CREA EL .PEM DEL KEY
                Process process1 = new Process();
                process1.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                process1.StartInfo.FileName = @"openssl.exe";//la ruta del openssl.exe
                String argumento1 = "pkcs8 -inform DER -in " + sWSKey + " -passin pass:" + sClavePrivada + " -out " + sArchivoPem;
                process1.StartInfo.Arguments = argumento1;//se envía la instrucción
                //process1.StartInfo.WorkingDirectory = @"C:\OpenSSL-Win32\bin\";
                process1.StartInfo.WorkingDirectory = sDirOpenssl;
                process1.StartInfo.UseShellExecute = false;
                process1.StartInfo.RedirectStandardOutput = true;
                process1.Start();//'iniciamos el proceso
                process1.WaitForExit();


                //OBTENER EL SELLO
                Process process2 = new Process();
                process2.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                process2.StartInfo.FileName = @"openssl.exe";
                string argumento2 = "dgst -sha256 -sign " + sArchivoPem + " -out " + sArchivoBinario + " " + sArchivoCadena;
                process2.StartInfo.Arguments = argumento2;//se envía la instrucción.
                // process2.StartInfo.WorkingDirectory = @"C:\OpenSSL-Win32\bin\";
                process2.StartInfo.WorkingDirectory = sDirOpenssl;
                process2.StartInfo.UseShellExecute = false;
                process2.StartInfo.RedirectStandardOutput = true;//
                process2.Start();//'iniciamos el proceso
                process2.WaitForExit();

                File.Delete(sArchivoPem);

                byte[] bSello = File.ReadAllBytes(sArchivoBinario);

                File.Delete(sArchivoCadena);
                //File.Delete(sArchivoSello);
                File.Delete(sArchivoBinario);

                //return sSello;

                return Base64_Encode(bSello);

            

            }
            catch (Exception ex)
            {
                return "Error al sellar: " + ex.ToString();
                //return null;
            }
        }


        public  string fnCadenaOriginal(string sXMLsinSello)
        {
            try
            {

                //recibir el string y convertirlo en xml 

                //StreamReader reader = new StreamReader(sArchivoXML);//archivo es la ruta del archivo xml //comentar 

                XmlDocument xtDoc = new XmlDocument();
                xtDoc.LoadXml(sXMLsinSello);
                string stXML = sDirTemp + "temporal.xml";

               

                xtDoc.Save(stXML);

                //StreamReader reader = new StreamReader(@"C:\PAXConectorCapacitacion06\Temp\temporal.xml");//archivo es la ruta del archivo xml //comentar 
                StreamReader reader = new StreamReader(stXML);//archivo es la ruta del archivo xml //comentar 

                XPathDocument myXPathDoc = new XPathDocument(reader);
                XslCompiledTransform myXslTrans = new XslCompiledTransform();
                //String CadOrigXSLT = @"C:\PAXConectorCapacitacion06\xslt33\cadenaoriginal_3_3.xslt";
                string CadOrigXSLT = sCadenaOriginalXSLT;
                myXslTrans.Load(CadOrigXSLT);
                StringWriter str = new StringWriter();
                XmlTextWriter myWriter = new XmlTextWriter(str);

                //'Aplicando transformacion
                myXslTrans.Transform(myXPathDoc, null, myWriter);

                //'Resultado
                String result = str.ToString();
                result = result.Replace("\n", "");//quitamos los saltos de linea

                reader.Dispose();
                reader.Close();

                File.Delete(stXML);
                return result;//result trae la cadena original
            }
            catch (Exception ex)
            {
                return "Error al generar la cadena: " + ex.ToString();
            }
        }

    }
}
