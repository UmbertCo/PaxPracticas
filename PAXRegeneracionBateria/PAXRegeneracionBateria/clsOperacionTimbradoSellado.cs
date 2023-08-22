using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace PAXRegeneracionBateria
{
    public class clsOperacionTimbradoSellado: IDisposable
    {
        private byte[] gbLlave;
        private string gsPassword;

        public enum AlgoritmoSellado
        {
            MD5,
            SHA1
        }

        void IDisposable.Dispose()
        {
            fnDestruirLlave();
        }

        public void fnDestruirLlave()
        {
            try
            {
                Array.Clear(gbLlave, 0, gbLlave.Length);
                gbLlave = null;
            } catch { }
        }

        public clsOperacionTimbradoSellado(byte[] pbLlave, string psPassword)
        {
            gbLlave = pbLlave;
            gsPassword = psPassword;
        }

        public string fnGenerarSello(string psCadenaOriginal, AlgoritmoSellado pAlgoritmo)
        {
            return fnGenerarSello(psCadenaOriginal, pAlgoritmo, false);
        }

        static private byte[] fnDesencriptarLlave(byte[] pbLlave)
        {
            return Utilerias.Encriptacion.DES3.Desencriptar(pbLlave);
        }

        static private string fnDesencriptarPassword(string psPassword)
        {
            return Encoding.UTF8.GetString(Utilerias.Encriptacion.DES3.Desencriptar(Convert.FromBase64String(psPassword)));
        }

        public string fnGenerarSello(string psCadenaOriginal, AlgoritmoSellado pAlgoritmo, bool pbDesencriptar)
        {
            try
            {
                //Llave privada original
                Chilkat.PrivateKey key = new Chilkat.PrivateKey();

                if (pbDesencriptar)
                    key.LoadPkcs8Encrypted(fnDesencriptarLlave(gbLlave), fnDesencriptarPassword(gsPassword));

                else
                    key.LoadPkcs8Encrypted(gbLlave, gsPassword);

                //Llave privada PEM
                Chilkat.PrivateKey pem = new Chilkat.PrivateKey();
                pem.LoadPem(key.GetPkcs8Pem());

                string pkeyXml = pem.GetXml();

                Chilkat.Rsa rsa = new Chilkat.Rsa();

                bool bSuccess;
                bSuccess = rsa.UnlockComponent("INTERMRSA_78UJEvED0IwK");
                bSuccess = rsa.GenerateKey(1024);

                rsa.LittleEndian = false;
                rsa.EncodingMode = "base64";
                rsa.Charset = "utf-8";
                rsa.ImportPrivateKey(pkeyXml);

                //Definimos el algoritmo
                string sAlgoritmo = string.Empty;
                if (pAlgoritmo == AlgoritmoSellado.SHA1)
                    sAlgoritmo = "sha-1";
                else
                    sAlgoritmo = "md5";

                string sello = rsa.SignStringENC(psCadenaOriginal, sAlgoritmo);

                //destruimos los objetos por seguridad
                try
                {
                    key = new Chilkat.PrivateKey();
                    key.Dispose();
                    pem = new Chilkat.PrivateKey();
                    pem.Dispose();
                    rsa = new Chilkat.Rsa();
                    rsa.Dispose();
                } catch {  }

                return sello;
            }
            catch
            {
                return null;
            }
        }

        public XmlDocument fnGenerarXML(TimbreFiscalDigital datos)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);
            XmlDocument xXml = new XmlDocument();
            XmlSerializerNamespaces sns = new XmlSerializerNamespaces();
            sns.Add("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            XmlSerializer serializador = new XmlSerializer(typeof(TimbreFiscalDigital));
            try
            {
                serializador.Serialize(sw, datos, sns);
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);

                xXml.LoadXml(sr.ReadToEnd());
                XmlAttribute att = xXml.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                att.Value = "http://www.sat.gob.mx/TimbreFiscalDigital http://www.sat.gob.mx/timbrefiscaldigital/TimbreFiscalDigital.xsd";
                xXml.DocumentElement.SetAttributeNode(att);

                return xXml;
            }
            catch (Exception)
            {
                return xXml;
            }
        }

    }
}