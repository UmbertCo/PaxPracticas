using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;

namespace OpenSSL_Lib.PFX
{
   public class cValidacionCertificados
    {

        public cPEM pCerPEM;

        public cPEM pKeyPEM;

        public string _sPass;

        public string _sRutaArchivoPFX = string.Empty;

        public string _sRutaGeneracionArchivos = string.Empty;

        public string _sRutaOpenSSL = string.Empty;

        public string sRutaArchivoPFX { get { return _sRutaArchivoPFX; } }

        public cPEM CERPEM { get { return pCerPEM; } }

        public cPEM KEYPEM { get { return pKeyPEM; } }

        public String sMensajeErr { set; get; }

       public enuEstatus eEstatusRevision { set; get; }


       public cValidacionCertificados(string psRutaCer, string psRutaKey, string psContraseña) 
       {
           eEstatusRevision = enuEstatus.DESCONOCIDO;

           _sPass = psContraseña;

           try
           {
               pCerPEM = new cPEM(psRutaCer, DocTipo.CER);

               pCerPEM.fnGenerarPEM();
           }
           catch (OpenSSLException osslex)
           {
               sMensajeErr = osslex.Message;
               eEstatusRevision = osslex.eError;



           }
           catch (Exception ex) 
           {
               sMensajeErr = ex.Message;
           
           }

           try
           {
               pKeyPEM = new cPEM(psRutaKey, DocTipo.KEY, _sPass);

               pKeyPEM.fnGenerarPEM();
           }
           catch (OpenSSLException osslex)
           {
               sMensajeErr = osslex.Message;
               eEstatusRevision = osslex.eError;



           }
           catch (Exception ex)
           {
               sMensajeErr = ex.Message;

           }

           _sRutaGeneracionArchivos = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

           _sRutaOpenSSL = _sRutaGeneracionArchivos + @"\openssl\";
       }

       public cValidacionCertificados(byte[] bCertificado, byte[] bLlave, string psContraseña) 
       {

           _sPass = psContraseña;

           try
           {
               pCerPEM = new cPEM(bCertificado, DocTipo.CER);

               pCerPEM.fnGenerarPEM();

           }
           catch (OpenSSLException osslex)
           {
               sMensajeErr = osslex.Message;
               eEstatusRevision = osslex.eError;



           }
           catch (Exception ex)
           {
               sMensajeErr = ex.Message;

           }

           try
           {
               pKeyPEM = new cPEM(bLlave, DocTipo.KEY, _sPass);

               pKeyPEM.fnGenerarPEM();
           }
           catch (OpenSSLException osslex)
           {
               sMensajeErr = osslex.Message;
               eEstatusRevision = osslex.eError;



           }
           catch (Exception ex)
           {
               sMensajeErr = ex.Message;

           }


           _sRutaGeneracionArchivos = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

           _sRutaOpenSSL = _sRutaGeneracionArchivos + @"\openssl\";

       
       }

        static string fnMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public bool fnValidarCerKey()
        {
            ProcessStartInfo psiEjecucion = null;
       
            psiEjecucion = new ProcessStartInfo(_sRutaOpenSSL + "openssl.exe", "x509 -noout -modulus -in \"" + pCerPEM.sRutaArchivoPem + "\" ");

            psiEjecucion.CreateNoWindow = true;
            psiEjecucion.UseShellExecute = false;
            psiEjecucion.RedirectStandardOutput = true;

            // Inicia el proceso ya inicializado y espera a que termine su ejecucion
            Process pProceso = Process.Start(psiEjecucion);
            pProceso.WaitForExit();

            String sMD5CER = "";
            try
            {
                sMD5CER = fnMd5Hash(MD5.Create(), pProceso.StandardOutput.ReadToEnd());
            }
            catch(Exception ex)
            {
                sMensajeErr = ex.Message;
                eEstatusRevision = enuEstatus.VALIDACIONERR;
                return false;
            }
            
            pProceso.Dispose();


            psiEjecucion = new ProcessStartInfo(_sRutaOpenSSL + "openssl.exe", "x509 -noout -modulus -in \"" + pCerPEM.sRutaArchivoPem + "\" ");

            psiEjecucion.CreateNoWindow = true;
            psiEjecucion.UseShellExecute = false;
            psiEjecucion.RedirectStandardOutput = true;

            // Inicia el proceso ya inicializado y espera a que termine su ejecucion
            pProceso = Process.Start(psiEjecucion);
            pProceso.WaitForExit();

            String sMD5Key = "";
            try
            {
                sMD5Key = fnMd5Hash(MD5.Create(), pProceso.StandardOutput.ReadToEnd());
            }
            catch (Exception ex)
            {
                sMensajeErr = ex.Message;
                eEstatusRevision = enuEstatus.VALIDACIONERR;
                return false;
            }

            pProceso.Dispose();

            if (!sMD5Key.Equals(sMD5CER)) 
            {

                eEstatusRevision = enuEstatus.CER_LLAVE_INC;
                return false;
            
            }


            eEstatusRevision = enuEstatus.OK;
            return true;
        }


    }
}
