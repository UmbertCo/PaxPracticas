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


        /// <summary>
        /// Funcion que se encarga de revisar si los Archivos OpenSSl estan instalados
        /// </summary>
        /// <returns></returns>
        public static bool fnExistenArchivosOpenSSL()
        {
            FileStream fsArchivo;

            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "bftest.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "bntest.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "casttest.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "destest.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "dhtest.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "dsatest.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "ecdhtest.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "ecdsatest.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "enginetest.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "evp_test.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "exptest.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "hmactest.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "ideatest.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "libeay32.dll", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "libssl32.dll", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "md2test.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "md4test.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "md5test.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            try
            {
                fsArchivo = File.Open(sRutaExe + "openssl.exe", FileMode.Open, FileAccess.ReadWrite);
                fsArchivo.Close();
            }
            catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "randtest.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "rc2test.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "rc4test.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "rmdtest.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "rsa_test.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "sha1test.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "sha512t.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "sha256t.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "shatest.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "ssltest.exe", FileMode.Open, FileAccess.ReadWrite);
            //    fsArchivo.Close();
            //}
            //catch { return false; }

            return true;

        }

        /// <summary>
        /// Funcion que genera todos los archivos OpenSSl
        /// </summary>
        /// <returns></returns>
        public static bool fnGenerarTodoslosArchivos()
        {

            FileStream fsArchivo;

            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "bftest.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.bftest, 0, Resources.bftest.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "bntest.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.bntest, 0, Resources.bntest.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "casttest.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.casttest, 0, Resources.casttest.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "destest.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.destest, 0, Resources.destest.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "dhtest.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.dhtest, 0, Resources.dhtest.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "dsatest.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.dhtest, 0, Resources.dhtest.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "ecdhtest.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.ecdhtest, 0, Resources.ecdhtest.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "ecdsatest.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.ecdsatest, 0, Resources.ecdsatest.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "enginetest.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.enginetest, 0, Resources.enginetest.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "evp_test.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.evp_test, 0, Resources.evp_test.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "exptest.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.exptest, 0, Resources.exptest.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "hmactest.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.hmactest, 0, Resources.hmactest.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "ideatest.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.ideatest, 0, Resources.ideatest.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "libeay32.dll", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.libeay32, 0, Resources.libeay32.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "libssl32.dll", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.libssl32, 0, Resources.libssl32.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "md2test.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.md2test, 0, Resources.md2test.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "md4test.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.md4test, 0, Resources.md4test.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "md5test.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.md5test, 0, Resources.md5test.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            try
            {
                fsArchivo = File.Open(sRutaExe + "openssl.exe", FileMode.Create, FileAccess.ReadWrite);
                fsArchivo.Write(Resources.openssl, 0, Resources.openssl.Length);
                fsArchivo.Close();
            }
            catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "randtest.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.randtest, 0, Resources.randtest.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "rc2test.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.rc2test, 0, Resources.rc2test.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "rc4test.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.rc4test, 0, Resources.rc4test.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "rmdtest.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.rmdtest, 0, Resources.rmdtest.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "rsa_test.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.rsa_test, 0, Resources.rsa_test.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "sha1test.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.sha1test, 0, Resources.sha1test.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "sha512t.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.sha512t, 0, Resources.sha512t.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "sha256t.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.sha256t, 0, Resources.sha256t.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "shatest.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.shatest, 0, Resources.shatest.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }
            //try
            //{
            //    fsArchivo = File.Open(sRutaExe + "ssltest.exe", FileMode.Create, FileAccess.ReadWrite);
            //    fsArchivo.Write(Resources.ssltest, 0, Resources.ssltest.Length);
            //    fsArchivo.Close();
            //}
            //catch { return false; }

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
