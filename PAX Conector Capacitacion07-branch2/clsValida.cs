using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Cryptography.X509Certificates;


namespace PAX_Conector_Capacitacion06
{
    class clsValida
    {
        static clsLog sLog = new clsLog(ConfigurationManager.AppSettings["sLog"]);

        public bool fnValidarXML(string sArchivoXML)
        {
            bool bValidarXML = false;

            try
            {
                /* Lectura de cfdi */
                //string sPathXML = @".\xml\cfdi_1.xml"; //sArchivoXML
                byte[] yXml = File.ReadAllBytes(sArchivoXML);

                /* Llamado al método y verificación de resultado */
                string sResultado = Valida_con_XSD(yXml);
                if (string.IsNullOrWhiteSpace(sResultado))
                {
                    sLog.fnAgregarLog("XML Valido con XSD");
                    bValidarXML = true;

                    return bValidarXML;
                }

                sLog.fnAgregarLog("Validacion contra XSD con la siguiente incidencia: \n" + sResultado);
                return bValidarXML;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static string Valida_con_XSD(byte[] yXML)
        {
            MemoryStream oMemoStream = null;
            XmlReader oXmlReader = null;
            XmlReaderSettings oXmlReadSettings = null;

            try
            {
                /* Configuración del validador */
                oXmlReadSettings = new XmlReaderSettings();
                oXmlReadSettings.ValidationType = ValidationType.Schema;

                /* Colección de archivos XSD */
                XmlSchemaSet oSchemaSet = new XmlSchemaSet();
                oSchemaSet.XmlResolver = null;
                //oSchemaSet.Add(null, @".\xsd\cfdv33.xsd");
                //oSchemaSet.Add(null, @".\xsd\catCFDI.xsd");
                //oSchemaSet.Add(null, @".\xsd\tdCFDI.xsd");
                oSchemaSet.Add(null, "http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv33.xsd");
                oSchemaSet.Add(null, "http://www.sat.gob.mx/sitio_internet/cfd/catalogos/catCFDI.xsd");
                oSchemaSet.Add(null, "http://www.sat.gob.mx/sitio_internet/cfd/tipoDatos/tdCFDI/tdCFDI.xsd");
                oSchemaSet.Compile();

                /* Asignación de shcemas */
                oXmlReadSettings.Schemas = oSchemaSet;

                /* Lectura y validación del CFDI*/
                oMemoStream = new MemoryStream(yXML);
                oXmlReader = XmlReader.Create(oMemoStream, oXmlReadSettings);
                while (oXmlReader.Read()) { }

                return string.Empty;
            }
            catch (XmlSchemaException XmlSchemaEx)
            {
                /* Captura de mensajes */
                return XmlSchemaEx.Message;
            }
            catch (XmlException XmlEx)
            {
                /* Captura de mensajes */
                return XmlEx.Message;
            }
            catch (Exception ex)
            {
                /* Algo salio muy mal 7n7 */
                throw;
            }
        }

        public static bool fnLeerCER(string NombreArchivo, out string Inicio, out string Final, out string Serie, out string numeroCertificado)
        {

            if (NombreArchivo.Length < 1)
            {
                Inicio = "";
                Final = "";
                Serie = "INVALIDO";
                numeroCertificado = "";
                return false;
            }

            //byte[] bCertificado = File.ReadAllBytes(NombreArchivo);

          
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
                numeroCertificado = "";
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
                numeroCertificado = rNumero;

            }

            if (DateTime.Now < objCert.NotAfter && DateTime.Now > objCert.NotBefore)
            {
                return true;
            }



            return false;
        }


        public static string Base64_Encode(byte[] str)
        {
            return Convert.ToBase64String(str);
        }



    }


}
