using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using OpenSSL_Lib.Properties;

namespace OpenSSL_Lib
{
    public class cSello
    {
        #region Variables de Clase
       


        string _sRutaKey = String.Empty;

        string _sRutaGeneracionArchivos = string.Empty;

        string _sRutaPass = string.Empty;

        string _sNombreArchivoPemLlave = string.Empty;

        string _sNombreArchivoLlave = string.Empty;

        string _sNombreArchivoCadenaOriginal = string.Empty;

        string _sNombreArchivoBin = string.Empty;

        string _sNombreArchivoSello = string.Empty;

        string _sPass = string.Empty;

        string _sCadenaOriginal = string.Empty;

        string _sSello = string.Empty;

        byte[] _bBytesLlave;

        byte[] _bBytesLlavePem;


        string _sRutaOpenSSL;

        #endregion
        
        #region Atributos
        public enuMetodoDigestion Digestion { set; get; }

        public string sCadenaOriginal
        {
            set { _sCadenaOriginal = value; }

        }

        public byte[] bBytesLlavePEM { get { return _bBytesLlavePem; } }

        public string sSello
        {
            get
            {
                try
                { 
                    fnCrearArchivoPem();
                    fnCrearArchivoCadenaOriginal();
                    fnCrearArchivoBin();
                    fnCrearSello();
                    fnObtenerSello();

                    if (string.IsNullOrEmpty(_sCadenaOriginal)) 
                    {
                        throw new Exception("Sello No Generado");
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    fnBorrarArchivosGeneracionSello();
                }
                return _sSello;
            }
        } 
        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que lee las rutas de la Llave y la contraseña y genera en Memoria el Archivo PEM de la Llave
        /// </summary>
        /// <param name="psRutaKey">Ruta Llave</param>
        /// <param name="psRutaPass">Ruta Pass</param>
        public cSello(string psRutaKey, string psRutaPass,string psRutaGeneracion)
        {
            _sRutaKey = psRutaKey;
            _sRutaPass = psRutaPass;
            _sRutaGeneracionArchivos = psRutaGeneracion;

            Digestion = enuMetodoDigestion.SHA1;


            _sRutaOpenSSL = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)+@"\openssl\";

            if (!cRevisar_Generar.fnExistenArchivosOpenSSL()) 
            {
                if (!cRevisar_Generar.fnExisteCarpetaOpenssl())
                    cRevisar_Generar.fnCrearCarpetaOpenssl();

                if (!cRevisar_Generar.fnGenerarTodoslosArchivos()) 
                {
                    throw new Exception("No se pueden Instalar los Archvos Openssl");
                
                
                }
            
            }

            fnLeerContraseña();
            fnCrearPemLlave();
            fnObtenerArchivoPem();
            fnBorrarArchivosGeneracionPEM();
        }

        /// <summary>
        /// Constructor que acepta la contraseña como cadena y un arreglo de bytes de la Llave (Archivo .Key)
        /// </summary>
        /// <param name="psPass">Contraseña</param>
        /// <param name="pbBytesLlave">Arreglo de Bytes de la Llave</param>
        public cSello(string psPass, byte[] pbBytesLlave, string psRutaGeneracion)
        {
            Digestion = enuMetodoDigestion.SHA1;

            _sPass = psPass;

            _bBytesLlave = pbBytesLlave;

            _sRutaGeneracionArchivos = psRutaGeneracion;

            _sRutaOpenSSL = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\openssl\";

            if (!cRevisar_Generar.fnExistenArchivosOpenSSL())
            {
                if (!cRevisar_Generar.fnExisteCarpetaOpenssl())
                    cRevisar_Generar.fnCrearCarpetaOpenssl();

                if (!cRevisar_Generar.fnGenerarTodoslosArchivos())
                {
                    throw new Exception("No se pueden Instalar los Archvos Openssl");


                }

            }

            fnCrearArchivoKey();
            fnCrearPemLlave();
            fnObtenerArchivoPem();
            fnBorrarArchivosGeneracionPEM();
        }

        public cSello(string psPass, byte[] pbBytesLlave, string psRutaGeneracion, enuMetodoDigestion pmdDigestion)
        {
            Digestion = pmdDigestion;

            _sPass = psPass;

            _bBytesLlave = pbBytesLlave;

            _sRutaGeneracionArchivos = psRutaGeneracion;

            _sRutaOpenSSL = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\openssl\";

            if (!cRevisar_Generar.fnExistenArchivosOpenSSL())
            {
                if (!cRevisar_Generar.fnExisteCarpetaOpenssl())
                    cRevisar_Generar.fnCrearCarpetaOpenssl();

                if (!cRevisar_Generar.fnGenerarTodoslosArchivos())
                {
                    throw new Exception("No se pueden Instalar los Archvos Openssl");


                }

            }

            fnCrearArchivoKey();
            fnCrearPemLlave();
            fnObtenerArchivoPem();
            fnBorrarArchivosGeneracionPEM();
        }
        #endregion

        #region Obtener PEM
        /// <summary>
        /// 
        /// </summary>
        private void fnCrearArchivoKey() 
        {
          
                _sNombreArchivoLlave =_sRutaGeneracionArchivos+ "Key" + DateTime.Now.ToFileTime() + ".key";

                File.WriteAllBytes(_sNombreArchivoLlave, _bBytesLlave);

                //_sRutaKey =_sRutaGeneracionArchivos+ _sNombreArchivoLlave;
                _sRutaKey = _sNombreArchivoLlave;

         }

        /// <summary>
        /// Crea el Archivo PEM con la Llave y el Password
        /// </summary>
        private void fnCrearPemLlave()
        {
            //Crea Nombre del Archivo PEM con una fecha especifica
            _sNombreArchivoPemLlave =_sRutaGeneracionArchivos+ "LlavePEM" + DateTime.Now.ToFileTime()+ ".pem";
          
                //Inicializa un proceso para llamar el Ejecutable Openssl.exe
                ProcessStartInfo psiEjecucion = new ProcessStartInfo(_sRutaOpenSSL+"openssl.exe", "pkcs8 -inform DER -in \"" + _sRutaKey + "\" -passin pass:\"" + _sPass + "\" -out \"" + _sNombreArchivoPemLlave + "\"");

                psiEjecucion.CreateNoWindow = true;
                psiEjecucion.UseShellExecute = false;

                // Inicia el proceso ya inicializado y espera a que termine su ejecucion
                Process pProceso = Process.Start(psiEjecucion);
                pProceso.WaitForExit();
            if(pProceso.ExitCode != 0)throw new Exception("No se pudo crear el archivo PEM: "+_sNombreArchivoPemLlave);
                pProceso.Dispose();
           
        }

        private void fnLeerPemLlave()
        {

            _bBytesLlavePem = File.ReadAllBytes(_sNombreArchivoPemLlave);

        }
          
        /// <summary>
        /// Lee el Archivo contraseña y lo almacena en Memoria
        /// </summary>
        private void fnLeerContraseña()
        {
        
            _sPass = File.ReadAllText(_sRutaPass);
        
        }
                         
        /// <summary>
        /// Lee el Archivo LlavePEM  y lo almacena en memoria. Finalmente Elimina el Archivo
        /// </summary>
        private void fnObtenerArchivoPem()
        {

            _bBytesLlavePem = File.ReadAllBytes(_sNombreArchivoPemLlave);


        }

        /// <summary>
        /// Borra Archivos que fueron creados al Generar el PEM
        /// </summary>
        private void fnBorrarArchivosGeneracionPEM()
        {


            try
            {

                File.Delete(_sNombreArchivoLlave);
            }
            catch { }

            try
            {
                File.Delete(_sNombreArchivoPemLlave);

            }
            catch { }

         


        }

        #endregion

        #region Obtener Sello


        /// <summary>
        /// Crea Archivo Pem con los bytes entregados en el segundo Constructor
        /// </summary>
        private void fnCrearArchivoPem()
        {

            //Crea Nombre del Archivo PEM con una fecha especifica
            _sNombreArchivoPemLlave =_sRutaGeneracionArchivos+ "LlavePEM" + DateTime.Now.ToFileTime() + ".pem";

            File.WriteAllBytes(_sNombreArchivoPemLlave, _bBytesLlavePem);

        }

        /// <summary>
        /// Crea Archivo con la cadena Original para ser procesada despues
        /// </summary>
        private void fnCrearArchivoCadenaOriginal()
        {

            _sNombreArchivoCadenaOriginal =_sRutaGeneracionArchivos+ "CO" + DateTime.Now.ToFileTime() + ".txt";

            File.WriteAllText(_sNombreArchivoCadenaOriginal, _sCadenaOriginal);


        }

        /// <summary>
        /// Crea Archivo Bin con el Archivo CadenaOriginal.txt y el Archivo PEM.txt
        /// </summary>
        private void fnCrearArchivoBin() 
        {
            //Crea Nombre del Archivo Bin con una fecha especifica
            _sNombreArchivoBin=_sRutaGeneracionArchivos+ "BIN" + DateTime.Now.ToFileTime() + ".txt";

            ProcessStartInfo psiEjecucion = null;

            //Inicializa un proceso para llamar el Ejecutable Openssl.exe

            switch (Digestion)
            {

                case enuMetodoDigestion.SHA1:
                    psiEjecucion = new ProcessStartInfo(_sRutaOpenSSL + "openssl.exe", "dgst -sha1 -sign \"" + _sNombreArchivoPemLlave + "\" -out \"" + _sNombreArchivoBin + "\" \"" + _sNombreArchivoCadenaOriginal + "\"");
                    psiEjecucion.CreateNoWindow = true;
                    psiEjecucion.UseShellExecute = false;

                    break;
                case enuMetodoDigestion.SHA256:
                       psiEjecucion = new ProcessStartInfo(_sRutaOpenSSL + "openssl.exe", "dgst -sha256 -sign \"" + _sNombreArchivoPemLlave + "\" -out \"" + _sNombreArchivoBin + "\" \"" + _sNombreArchivoCadenaOriginal + "\"");
                    psiEjecucion.CreateNoWindow = true;
                    psiEjecucion.UseShellExecute = false;

                    break;
            }
            // Inicia el proceso ya inicializado y espera a que termine su ejecucion
            Process pProceso = Process.Start(psiEjecucion);
            pProceso.WaitForExit();
            if (pProceso.ExitCode != 0) throw new Exception("No se pudo crear el archivo BIN: " + _sNombreArchivoBin);
            pProceso.Dispose();

        
        }

        /// <summary>
        /// Crea Archivo Sello con el archivo Bin.txt en Base64
        /// </summary>
        private void fnCrearSello() 
        {

            _sNombreArchivoSello =_sRutaGeneracionArchivos+ "Sello" + DateTime.Now.ToFileTime() + ".txt";

            //Inicializa un proceso para llamar el Ejecutable Openssl.exe
            ProcessStartInfo psiEjecucion = new ProcessStartInfo(_sRutaOpenSSL + "openssl.exe", "enc -base64 -in \"" + _sNombreArchivoBin + "\" -out \"" + _sNombreArchivoSello + "\"");
            psiEjecucion.CreateNoWindow = true;
            psiEjecucion.UseShellExecute = false;

            // Inicia el proceso ya inicializado y espera a que termine su ejecucion
            Process pProceso = Process.Start(psiEjecucion);
            pProceso.WaitForExit();
            if (pProceso.ExitCode != 0) throw new Exception("No se pudo crear el archivo Sello: " + _sNombreArchivoSello);
            pProceso.Dispose();
        }

        /// <summary>
        /// Obtiene y guarda en memoria el Sello del archivo Sello.txt
        /// </summary>
        private void fnObtenerSello()
        {
            string sSelloGenerado = string.Empty;
            sSelloGenerado = File.ReadAllText(_sNombreArchivoSello);
            _sSello = "";

            foreach (string elemento in sSelloGenerado.Split('\n'))
            {
                _sSello += elemento.Trim().TrimStart().TrimEnd();
            }
        }

        /// <summary>
        /// Borra los Archivos que fueron creados para la generacion del Sello
        /// </summary>
        private void fnBorrarArchivosGeneracionSello()
        {
           

            try
            {
                File.Delete(_sNombreArchivoPemLlave);
            }
            catch { }

            try
            {
                File.Delete( _sNombreArchivoCadenaOriginal);
            }
            catch { }

            try
            {
                File.Delete( _sNombreArchivoBin);
            }
            catch { }

            try
            {
                File.Delete( _sNombreArchivoSello);
            }
            catch { }
        }

        #endregion
    }



       
    
    
}
