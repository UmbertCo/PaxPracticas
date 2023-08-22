using OpenSSL_Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SolucionPruebas.Negocios
{
    public class OpenSSL
    {
        public OpenSSL()
        { }

        public string fnGenerarSello(string psRutaCertificado, string psRutaLlave, string psPassword, string psNombreCertificado, string psNombreLlave, string psCadenaOriginal, string psRutaPfx)
        {
            string sResultado = string.Empty;
            ProcessStartInfo info;
            Process proceso;
            ProcessStartInfo infoSello;
            Process procesoSello;
            string sInstruccion = string.Empty;
            string sNombreCadenaOriginal = string.Empty;
            string sArchivo = string.Empty;
            try
            {
                fnCrearCertificadoPEM(psRutaCertificado, psNombreCertificado, psRutaPfx);
                fnCrearLlavePrivadaPEM(psRutaLlave, psPassword, psNombreLlave, psRutaPfx);

                sArchivo = Guid.NewGuid().ToString();

                sNombreCadenaOriginal = psRutaPfx + "CadenaOriginal_" + sArchivo + ".txt";
                File.WriteAllText(sNombreCadenaOriginal, psCadenaOriginal);

                //sInstruccion = "pkcs12 -export -out " + psRutaPfx + psNombreCertificado + ".pfx -inkey " + psRutaPfx + psNombreLlave + ".pem -in " + psRutaPfx + psNombreCertificado + ".pem -passout pass:" + psPassword;
                sInstruccion = "dgst -sha1 -sign " + psRutaPfx + psNombreLlave + ".pem -out " + psRutaPfx + "BIN_" + sArchivo + ".txt " + sNombreCadenaOriginal;

                info = new ProcessStartInfo(VariablesGlobales.RutaOpenSSL, sInstruccion);
                info.CreateNoWindow = true;
                info.UseShellExecute = false;
                proceso = Process.Start(info);
                proceso.WaitForExit();
                proceso.Dispose();

                sInstruccion = "enc -base64 -in " + psRutaPfx + "BIN_" + sArchivo + ".txt -out " + psRutaPfx + "SELLO_" + sArchivo + ".txt";

                infoSello = new ProcessStartInfo(VariablesGlobales.RutaOpenSSL, sInstruccion);
                infoSello.CreateNoWindow = true;
                infoSello.UseShellExecute = false;
                procesoSello = Process.Start(infoSello);
                procesoSello.WaitForExit();
                procesoSello.Dispose();


                using (Stream stream = File.Open(psRutaPfx + "SELLO_" + sArchivo + ".txt", FileMode.Open))
                {
                    StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8, true);
                    sResultado = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
            finally
            { 
                if(File.Exists(psRutaPfx + psNombreCertificado + ".pem"))
                    File.Delete(psRutaPfx + psNombreCertificado + ".pem");

                if (File.Exists(psRutaPfx + psNombreLlave + ".pem"))
                    File.Delete(psRutaPfx + psNombreLlave + ".pem");

                if (File.Exists(sNombreCadenaOriginal))
                    File.Delete(sNombreCadenaOriginal);

                if (File.Exists(psRutaPfx + "BIN_" + sArchivo + ".txt"))
                    File.Delete(psRutaPfx + "BIN_" + sArchivo + ".txt");

                if (File.Exists(psRutaPfx + "SELLO_" + sArchivo + ".txt"))
                    File.Delete(psRutaPfx + "SELLO_" + sArchivo + ".txt");
            }
            return sResultado;
        }

        private void fnCrearCertificadoPEM(string psRutaCertificado, string psNombreCertificado, string psRutaPfx)
        {
            Process proceso;
            ProcessStartInfo info;
            string sInstruccion = string.Empty;
            try
            {
                sInstruccion = "x509 -inform DER -in " + psRutaCertificado + " -out " + psRutaPfx + psNombreCertificado + ".pem";
                info = new ProcessStartInfo(VariablesGlobales.RutaOpenSSL, sInstruccion);

                info.CreateNoWindow = true;
                info.UseShellExecute = false;
                
                proceso = Process.Start(info);
                proceso.WaitForExit();
                proceso.Dispose();
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        private void fnCrearLlavePrivadaPEM(string psRutaLlave, string psPassword, string psNombreLlave, string psRutaPfx)
        {
            Process proceso;
            ProcessStartInfo info;
            string sInstruccion = string.Empty;
            try
            {
                sInstruccion = "pkcs8 -inform DER -in " + psRutaLlave + " -passin pass:" + psPassword + " -out " + psRutaPfx + psNombreLlave + ".pem";
                info = new ProcessStartInfo(VariablesGlobales.RutaOpenSSL, sInstruccion);

                info.CreateNoWindow = true;
                info.UseShellExecute = false;

                proceso = Process.Start(info);
                proceso.WaitForExit();
                proceso.Dispose();
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public string fnGenerarSello(string psRutaPEM, string psCadenaOriginal, byte[] psLlave, string psPassword)
        {
            string sSello = string.Empty;
            try
            {
                cSello Sello = new cSello(psPassword, psLlave, psRutaPEM);
                Sello.sCadenaOriginal = psCadenaOriginal;
                sSello = Sello.sSello;
            }
            catch (Exception ex)
            {
                throw new Exception("No es posible generar el sello. " + ex.Message);
            }
            return sSello;
        }

        public string fnGenerarSello(string psRutaPEM, string psCadenaOriginal, string psLlave, string psPassword)
        {
            string sSello = string.Empty;
            try
            {
                cSello Sello = new cSello(psLlave, psPassword, psRutaPEM);
                Sello.sCadenaOriginal = psCadenaOriginal;
                sSello = Sello.sSello;
            }
            catch (Exception ex)
            {
                throw new Exception("No es posible generar el sello. " + ex.Message);
            }
            return sSello;
        }

        //public object fnGenerarSello(string psCadenaOriginal, byte[] psLlave, string psPassword)
        //{ 
        //    int nValor = 0;
        //    Validacion.Validacion bValidacion = new Validacion.Validacion();
        //    bValidacion.ValidarId(nValor);
        //}
    }
}
