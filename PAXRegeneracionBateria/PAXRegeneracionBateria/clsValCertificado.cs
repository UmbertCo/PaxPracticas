using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace PAXRegeneracionBateria
{
    public class clsValCertificado
    {
        private X509Certificate2 certificado;
        private byte[] gbCertificado;

        public clsValCertificado(byte[] pbCertificado)
        {
            try
            {
                certificado = new X509Certificate2(pbCertificado);
                gbCertificado = pbCertificado;
            }
            catch
            {
                try
                {
                    certificado = new X509Certificate2(fnDesencriptarCertificado(pbCertificado));
                    gbCertificado = fnDesencriptarCertificado(pbCertificado);
                }
                catch (Exception)
                {
                    throw new CryptographicException("El certificado esta bloqueado");
                }
            }

            //if (certificado.Verify())
            // throw new CryptographicException("El certificado no pasó la verificación");
        }

        private byte[] fnDesencriptarCertificado(byte[] pbCertificadoEncriptado)
        {
            return Utilerias.Encriptacion.DES3.Desencriptar(pbCertificadoEncriptado);
        }

        public string ObtenerNoCertificado()
        {
            byte[] bCertificadoInvertido = certificado.GetSerialNumber().Reverse().ToArray();
            return Encoding.Default.GetString(bCertificadoInvertido);
        }

        public bool fnVerificarSelloPAC(string psCadenaOriginal, string psSello, string noCertificado)
        {
            try
            {
                // The path to the certificate.
                string Certificate = "D:\\CertificadosPAC\\" + noCertificado + "\\" + noCertificado + ".cer";
                // Load the certificate into an X509Certificate object.
                X509Certificate2 cert = new X509Certificate2(Certificate);
                byte[] certData = cert.Export(X509ContentType.Cert);
                certificado = new X509Certificate2(certData);
                RSACryptoServiceProvider publica = ((RSACryptoServiceProvider)certificado.PublicKey.Key);
                //Verificamos que el certificado obtenga el mismo resultado que el del sello
                byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(psCadenaOriginal));
                bool exito = publica.VerifyHash(
                        hash,
                        "sha1",
                        Convert.FromBase64String(psSello));

                return exito;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}