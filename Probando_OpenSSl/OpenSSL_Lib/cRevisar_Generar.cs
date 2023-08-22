using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using OpenSSL_Lib.Properties;

namespace OpenSSL_Lib
{
    public class cRevisar_Generar
    {


        static string sRutaExe = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "openssl" + Path.DirectorySeparatorChar; 

        public static string sRutaEXE { get { return sRutaExe; } }

        public static void fnInstalar()
        {


            if (!cRevisar_Generar.fnExistenArchivosOpenSSL())
            {
               

                if (!cRevisar_Generar.fnExisteCarpetaOpenssl())
                    cRevisar_Generar.fnCrearCarpetaOpenssl();

                if (!cRevisar_Generar.fnGenerarTodoslosArchivos())
                {
                    throw new Exception("No se pueden Instalar los Archvos Openssl");


                }

            }
        
        }

        public static void fnInstalar(String psRutaExe) 
        {
            sRutaExe = psRutaExe;

            fnInstalar();
        
        }

        /// <summary>
        /// Funcion que se encarga de revisar si los Archivos OpenSSl estan instalados
        /// </summary>
        /// <returns></returns>
        public static bool fnExistenArchivosOpenSSL()
        {
            FileStream fsArchivo;
            
            try
            {
                
              return  File.Exists(sRutaExe + "openssl.exe");
            }
            catch(Exception ex)
            {
               
                return false; 
            }
           

            return true;

        }

        /// <summary>
        /// Funcion que genera todos los archivos OpenSSl
        /// </summary>
        /// <returns></returns>
        public static bool fnGenerarTodoslosArchivos()
        {

            FileStream fsArchivo;
           
            try
            {
                fsArchivo = File.Open(sRutaExe + "openssl.exe", FileMode.Create, FileAccess.ReadWrite);
                fsArchivo.Write(Resources.openssl, 0, Resources.openssl.Length);
                fsArchivo.Close();
            }
            catch { return false; }
 
            return true;


        }

        public static bool fnExisteCarpetaOpenssl() 
        {

          return  Directory.Exists(sRutaExe);

        
        }

        public static bool fnCrearCarpetaOpenssl() 
        {

            try
            {
                Directory.CreateDirectory(sRutaExe);


            }
            catch { return false; }
            return true;
        }
    }
}
