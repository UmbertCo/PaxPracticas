using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace OpenSSL_Lib.PFX
{
   public class cPFX
   {
       byte[] bPFX;

       
       
      public cPEM pCerPEM;
   
      public cPEM pKeyPEM;
    
      public string _sPass;
      
      public string _sRutaArchivoPFX = string.Empty;
  
      public string _sRutaGeneracionArchivos = string.Empty;
    
      public string _sRutaOpenSSL = string.Empty;

       public string sRutaArchivoPFX { get { return _sRutaArchivoPFX; } }

       public cPEM CERPEM { get { return pCerPEM; } }

       public cPEM KEYPEM { get { return pKeyPEM; } }

       public X509Certificate2 x509PFX { get { return new X509Certificate2(bPFX,_sPass); } }

       public cPFX(string psRutaCer, string psRutaKey, string psContraseña) 
       {
           _sPass = psContraseña;

          pCerPEM = new cPEM(psRutaCer, DocTipo.CER);

           pCerPEM.fnGenerarPEM();

           pKeyPEM = new cPEM(psRutaKey, DocTipo.KEY, _sPass);

           pKeyPEM.fnGenerarPEM();

           _sRutaGeneracionArchivos = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

           _sRutaOpenSSL = _sRutaGeneracionArchivos + @"\openssl\";
       }

       public cPFX(byte[] bCertificado, byte[] bLlave, string psContraseña) 
       {

           _sPass = psContraseña;

           pCerPEM = new cPEM(bCertificado, DocTipo.CER);

           pCerPEM.fnGenerarPEM();

           pKeyPEM = new cPEM(bLlave, DocTipo.KEY, _sPass);

           pKeyPEM.fnGenerarPEM();

           _sRutaGeneracionArchivos = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

           _sRutaOpenSSL = _sRutaGeneracionArchivos + @"\openssl\";

       
       }

       public void fnGenerarPFX() 
       {

           fnCrearPFX();
           bPFX = File.ReadAllBytes(_sRutaArchivoPFX);

           fnBorrarArchivos();
       
       }


       private void fnCrearPFX()
       {
           ProcessStartInfo psiEjecucion = null;
           //Crea Nombre del Archivo PEM con una fecha especifica
           _sRutaArchivoPFX = _sRutaGeneracionArchivos + Path.DirectorySeparatorChar + "PEM" + Guid.NewGuid().ToString().Replace("-","").ToUpper() + ".pfx";

           //Inicializa un proceso para llamar el Ejecutable Openssl.exe

           psiEjecucion = new ProcessStartInfo(_sRutaOpenSSL + "openssl.exe", "pkcs12 -export -in \"" + pCerPEM.sRutaArchivoPem + "\" -inkey \"" + pKeyPEM.sRutaArchivoPem + "\" -passout pass:\"" + _sPass + "\" -out \"" + _sRutaArchivoPFX + "\"");
               
           psiEjecucion.CreateNoWindow = true;
           psiEjecucion.UseShellExecute = false;

           // Inicia el proceso ya inicializado y espera a que termine su ejecucion
           Process pProceso = Process.Start(psiEjecucion);
           pProceso.WaitForExit();
           if (!File.Exists(_sRutaArchivoPFX))
           {
               pProceso.Dispose();
               throw new OpenSSLException(enuEstatus.PERMISOS,"No se pudo crear el archivo PFX (fnCrearPFX): " + _sRutaArchivoPFX);


           }

           if (pProceso.ExitCode != 0)
           {
               pProceso.Dispose();
               throw new OpenSSLException(enuEstatus.ERROPENSSL,"Existio un error en la creacion del archivo PFX (fnCrearPFX): " + _sRutaArchivoPFX);
           }

           pProceso.Dispose();

       }

       public void fnBorrarArchivos()
       {
           try
           { File.Delete(_sRutaArchivoPFX); }
           catch { }

           pCerPEM.fnBorrarArchivos();

           pKeyPEM.fnBorrarArchivos();
       
       }
   }
}
