using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace OpenSSL_Lib.PFX
{
   public class cPEM
    {
        byte[] bDoc;

        byte[] _bPEM;

        DocTipo dtTipo;

        string sRutaDoc;

        bool bDocExistia = false;

        string _sRutaGeneracionArchivos = string.Empty;

        string _sRutaOpenSSL = string.Empty;

        string _sPass = string.Empty;

        string _sNombreArchivoPem = string.Empty;

        public string sRutaArchivo { get { return sRutaDoc; } }

        public string sRutaArchivoPem { get { return _sNombreArchivoPem; } }

        public byte[] bDocumento { get { return bDoc; } }

        public byte[] bPEM { get { return _bPEM; } }

        enuEstatus eError;

        public cPEM(byte[] pbDoc, DocTipo pdtTipo):this(pbDoc,pdtTipo,"")
        { }

        public cPEM(string psRuta, DocTipo pdtTipo)
            : this(psRuta, pdtTipo, "")
        { }

        public cPEM(byte[] pbDoc, DocTipo pdtTipo, string psPass) 
        {
            cRevisar_Generar.fnInstalar();

            bDoc = pbDoc;

            dtTipo = pdtTipo;

            _sRutaGeneracionArchivos = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            _sRutaOpenSSL = _sRutaGeneracionArchivos + @"\openssl\";

            _sPass = psPass;

            fnGenerarDoc();
        }

        public cPEM(string psRuta, DocTipo pdtTipo, string psPass)
        {
            cRevisar_Generar.fnInstalar();

             sRutaDoc = psRuta;

             bDoc = File.ReadAllBytes(psRuta);

             dtTipo = pdtTipo;

            _sRutaGeneracionArchivos = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            _sRutaOpenSSL = _sRutaGeneracionArchivos + @"\openssl\";

            _sPass = psPass;

            bDocExistia = true;

        }

        public void fnGenerarPEM() 
        {
           fnCrearPem();

           _bPEM = File.ReadAllBytes(_sNombreArchivoPem);


        
        }

        private void fnCrearPem()
        {
            ProcessStartInfo psiEjecucion = null;
            //Crea Nombre del Archivo PEM con una fecha especifica
            _sNombreArchivoPem = _sRutaGeneracionArchivos+Path.DirectorySeparatorChar + "PEM" + Guid.NewGuid().ToString().Replace("-","").ToUpper() + ".pem";

            //Inicializa un proceso para llamar el Ejecutable Openssl.exe
            switch (dtTipo)
            {
                case DocTipo.KEY:
                    psiEjecucion = new ProcessStartInfo(_sRutaOpenSSL + "openssl.exe", "pkcs8 -inform DER -in \"" + sRutaDoc + "\" -passin pass:\"" + _sPass + "\" -out \"" + _sNombreArchivoPem + "\"");
                    break;
                case DocTipo.CER:
                    psiEjecucion = new ProcessStartInfo(_sRutaOpenSSL + "openssl.exe", "x509 -inform DER -in \"" + sRutaDoc + "\"  -out \"" + _sNombreArchivoPem + "\"");
                    break;
            }

            psiEjecucion.CreateNoWindow = true;
            psiEjecucion.UseShellExecute = false;

            // Inicia el proceso ya inicializado y espera a que termine su ejecucion
            Process pProceso = Process.Start(psiEjecucion);
            pProceso.WaitForExit();

            if (!File.Exists(_sNombreArchivoPem)) 
            {
                pProceso.Dispose();
                throw new OpenSSLException(enuEstatus.PERMISOS,"No se pudo crear el archivo PEM (fnCrearPem): " + _sNombreArchivoPem);
            
            
            }

            if (pProceso.ExitCode != 0)
            {
                pProceso.Dispose();
                if(dtTipo== DocTipo.KEY)
                    throw new OpenSSLException(enuEstatus.PASS, "Ocurrio un error en la creacion del archivo PEM (fnCrearPem): " + _sNombreArchivoPem);
            
                else
                throw new OpenSSLException(enuEstatus.DESCONOCIDO, "Ocurrio un error en la creacion del archivo PEM (fnCrearPem): " + _sNombreArchivoPem);
            }
            pProceso.Dispose();

        }

        private void fnGenerarDoc() 
        {
            switch (dtTipo) 
            {
                case DocTipo.KEY:
                    sRutaDoc = _sRutaGeneracionArchivos + Path.DirectorySeparatorChar + Guid.NewGuid().ToString().Replace("-","").ToUpper() + ".key";

                    File.WriteAllBytes(sRutaDoc, bDoc);
                    
                    break;

                case DocTipo.CER:
                    sRutaDoc = _sRutaGeneracionArchivos + Path.DirectorySeparatorChar + Guid.NewGuid().ToString().Replace("-","").ToUpper() + ".cer";
                    File.WriteAllBytes(sRutaDoc, bDoc);
                    break;
            
            }

        }

        public void fnBorrarArchivos() 
        {
            try { File.Delete(_sNombreArchivoPem); }
            catch { }

            try { 
                if(!bDocExistia)
                File.Delete(sRutaDoc); }
            catch { }
        }

    }
}
