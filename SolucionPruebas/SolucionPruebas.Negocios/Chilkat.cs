using Chilkat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SolucionPruebas.Negocios
{
    public class Chilkat
    {
        public Chilkat()
        { 
        
        }



        /// <summary>
        /// Función que se encarga de crear la pfx.
        /// </summary>
        /// <param name="psRutaPfx">Ruta donde se va a crear el pfx</param>
        /// <param name="psCertificado">PEM del Certificado</param>
        /// <param name="psLlave">PEM de la llave privada</param>
        /// <param name="psPassword">Password de la llave privada</param>
        /// <returns></returns>
        public bool fnGenerarPfxPem(string psRutaPfx, string psCertificado, string psLlave, string psPassword, bool pbIncludeCertsInChain)
        {
            Cert certificado = new Cert();
            bool bSuccess = false;
            bool bResultado = true;
            string sLlaveRSA = string.Empty;
            try
            {
                bSuccess = certificado.LoadFromBase64(psCertificado);
                if (!bSuccess)
                    throw new Exception("No se cargo la pem del certificado. " + certificado.LastErrorText);

                bSuccess = certificado.SetPrivateKeyPem(psLlave);
                if (!bSuccess)
                    throw new Exception("No se pudo asignar la llave al certificado. " + certificado.LastErrorText);

                bSuccess = certificado.ExportToPfxFile(psRutaPfx + "pfx_0.pfx", psPassword, pbIncludeCertsInChain);
                if (!bSuccess)
                    throw new Exception("No se pudo crear el pfx. " + certificado.LastErrorText);

                bResultado = false;
            }
            catch (Exception ex)
            {
                throw new Exception("No es posible generar el pfx. " + ex.Message);
            }
            return bResultado;
        }

        /// <summary>
        /// Función que se encarga de crear la pfx en base a las rutas de la pem del certificado y a la ruta de la 
        /// llave privada.
        /// </summary>
        /// <param name="psRutaPfx">Ruta donde se va a crear el pfx</param>
        /// <param name="psRutaCertificado">Ruta PEM del Certificado</param>
        /// <param name="psRutaLlave">Ruta PEM de la llave privada</param>
        /// <param name="psPassword">Password de la llave privada</param>
        /// <returns></returns>
        public bool fnGenerarPfxBytes(string psRutaPfx, byte[] paCertificado, byte[] paLlave, string psPassword, bool pbIncludeCertsInChain)
        {
            Cert certificado = new Cert();
            PrivateKey llave = new PrivateKey();
            PrivateKey llavepem = new PrivateKey();
            PrivateKey pem = new PrivateKey();
            Rsa oRsa = new Rsa();
            bool bSuccess = false;
            bool bResultado = false;
            try
            {
                bSuccess = certificado.LoadFromBinary(paCertificado);
                if (!bSuccess)
                    throw new Exception("No se cargo la pem del certificado. " + certificado.LastErrorText);

                bSuccess = llave.LoadPkcs8Encrypted(paLlave, psPassword);
                if (!bSuccess)
                    throw new Exception("No se cargo la llave privada. " + llave.LastErrorText);

                bSuccess = llavepem.LoadPem(llave.GetPkcs8Pem());
                if (!bSuccess)
                    throw new Exception("No se cargo la pem de la llave privada. " + llave.LastErrorText);

                bSuccess = certificado.SetPrivateKey(llavepem);
                if (!bSuccess)
                    throw new Exception("No se pudo asignar la llave al certificado. " + certificado.LastErrorText);

                bSuccess = certificado.ExportToPfxFile(psRutaPfx + "pfx_1.pfx", psPassword, pbIncludeCertsInChain);
                if (!bSuccess)
                    throw new Exception("No se pudo crear el pfx. " + certificado.LastErrorText);

                bResultado = true;
            }
            catch (Exception ex)
            {
                throw new Exception("No es posible generar el pfx. " + ex.Message);
            }
            return bResultado;
        }

        /// <summary>
        /// Funcón que se encarga de crear la pfx en base a los archivos a la llave pública (.cer), la llave privada (.key) y el password
        /// </summary>
        /// <param name="psRutaPfx">Ruta donde se va generar el archivo pfx</param>
        /// <param name="psRutaCertificado">Ruta del Certificado</param>
        /// <param name="psRutaLlave">Ruta la llave privada</param>
        /// <param name="psPassword">Password de la llave privada</param>
        /// <param name="pbIncludeCertsInChain">Incluir certificados en la cadena de autoridad</param>
        /// <returns></returns>
        public bool fnGenerarPfxRuta(string psRutaPfx, string psRutaCertificado, string psRutaLlave, string psPassword, bool pbIncludeCertsInChain)
        {
            bool bSuccess = false;
            bool bResultado = false;
            byte[] aLlavePrivada;
            Cert certificado = new Cert();
            PrivateKey llave = new PrivateKey();
            PrivateKey llavepem = new PrivateKey();
            Rsa oRsa = new Rsa();
            try
            {
                Stream streamkey = File.Open(psRutaLlave, FileMode.Open);
                StreamReader srkey = new StreamReader(streamkey, System.Text.Encoding.UTF8, true);
                using (BinaryReader br = new BinaryReader(streamkey))
                {
                    aLlavePrivada = br.ReadBytes(Convert.ToInt32(streamkey.Length));
                }

                bSuccess = certificado.LoadFromFile(psRutaCertificado);
                if (!bSuccess)
                    throw new Exception("No se cargo la pem del certificado. " + certificado.LastErrorText);

                bSuccess = llave.LoadPkcs8Encrypted(aLlavePrivada, psPassword);
                if (!bSuccess)
                    throw new Exception("No se cargo la llave privada. " + llave.LastErrorText);

                bSuccess = llavepem.LoadPem(llave.GetPkcs8Pem());
                if (!bSuccess)
                    throw new Exception("No se cargo la pem de la llave privada. " + llave.LastErrorText);

                bSuccess = certificado.SetPrivateKey(llavepem);
                if (!bSuccess)
                    throw new Exception("No se pudo asignar la llave al certificado. " + certificado.LastErrorText);

                bSuccess = certificado.ExportToPfxFile(psRutaPfx + "pfx_2.pfx", psPassword, pbIncludeCertsInChain);
                if (!bSuccess)
                    throw new Exception("No se pudo crear el pfx. " + certificado.LastErrorText);

                bResultado = true;
            }
            catch (Exception ex)
            {
                throw new Exception("No es posible generar el pfx. " + ex.Message);
            }
            return bResultado;
        }


        public byte[] fnGenerarPfxRutasByte(string psRutaPfx, string psRutaCertificado, string psRutaLlave, string psPassword, bool pbIncludeCertsInChain)
        {
            bool bSuccess = false;
            byte[] bResultado;
            byte[] aLlavePrivada;
            Cert certificado = new Cert();
            string sKeyXml = string.Empty;
            PrivateKey llave = new PrivateKey();
            PrivateKey llavepem = new PrivateKey();
            X509Certificate2 certificadoPfx;
            try
            {
                Stream streamkey = File.Open(psRutaLlave, FileMode.Open);
                StreamReader srkey = new StreamReader(streamkey, System.Text.Encoding.UTF8, true);
                using (BinaryReader br = new BinaryReader(streamkey))
                {
                    aLlavePrivada = br.ReadBytes(Convert.ToInt32(streamkey.Length));
                }

                bSuccess = certificado.LoadFromFile(psRutaCertificado);
                if (!bSuccess)
                    throw new Exception("No se cargo la pem del certificado. " + certificado.LastErrorText);

                bSuccess = llave.LoadPkcs8Encrypted(aLlavePrivada, psPassword);
                if (!bSuccess)
                    throw new Exception("No se cargo la llave privada. " + llave.LastErrorText);

                bSuccess = llavepem.LoadPem(llave.GetPkcs8Pem());
                if (!bSuccess)
                    throw new Exception("No se cargo la pem de la llave privada. " + llave.LastErrorText);

                bSuccess = certificado.SetPrivateKey(llavepem);
                if (!bSuccess)
                    throw new Exception("No se pudo asignar la llave al certificado. " + certificado.LastErrorText);

                certificadoPfx = new X509Certificate2(System.Text.Encoding.UTF8.GetBytes(certificado.ExportCertPem()), psPassword);
                sKeyXml = certificado.ExportPrivateKey().GetXml();

                RSACryptoServiceProvider rsKey = new RSACryptoServiceProvider();
                rsKey.FromXmlString(sKeyXml);

                certificadoPfx.PrivateKey = (AsymmetricAlgorithm)rsKey;

                bResultado = certificadoPfx.Export(X509ContentType.Pfx, psPassword);
            }
            catch (Exception ex)
            {
                throw new Exception("No es posible generar el pfx. " + ex.Message);
            }
            return bResultado;
        }
    }
}
