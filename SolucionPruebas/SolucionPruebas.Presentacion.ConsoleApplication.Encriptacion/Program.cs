using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SolucionPruebas.Presentacion.ConsoleApplication.Encriptacion
{
    class Program
    {
        static void Main(string[] args)
        {
            string sEncriptado = string.Empty;
            string sEncriptado_2 = string.Empty;

            sEncriptado = PAXCrypto.CryptoAES.EncriptarAES64("Data Source=DB-DESAROLLO;Initial Catalog=CFDI_Crypto_Test;Persist Security Info=True;User ID=Configuracion;Password=F4cturax10n_CnF");
            Console.WriteLine(sEncriptado);
            Console.ReadLine();

            sEncriptado_2 = PAXCrypto.CryptoAES.DesencriptaAES64("EE/IijvBioZPzNHHiOCcoIlY6FWZp9Pjv0fmARYr/pjxpelUabtgwDS76QnX5eO9eBax2vh9fGoyqsRkhNoHuL3TcGTb9E0mJ0OFJtB/e+6EVBwx9PQeRkKYfGyW4M5sd1Qjga6af+7tBfo61tlkWkQc3J8uEJsJghY0ggVbW8JuBMOM7esv+Fy2O30iIJSKF242bgsZHilpvUnunbMTcaSNtiy69hTIFW47fOJFpMx+WX5Tt3igMAuxnjvYgU2ebwUY6Lb9lv+nUIx8wqYFAy5nOw9b5FN4naDwAXKzNjb4oRvlAHFW9RM1gpl3q7zeA3VBASx2sfjyglnHs8kT1GHEB9p/qkEhNbEDFMwa0kONqWN5Hw6q0KalUrjhLhlr");
            Console.WriteLine(sEncriptado_2);
            Console.ReadLine();

            //Assembly cCryptoAESAssemble = Assembly.LoadFrom(@"D:\Proyectos\Practicas\SolucionPruebas\SolucionPruebas.Presentacion.ConsoleApplication.Encriptacion\bin\Debug\PAXCrypto.dll");
            //Type cCryptoAES = cCryptoAESAssemble.GetTypes()[4];
            //ConstructorInfo cContructorCryptoAES = cCryptoAES.GetConstructor(Type.EmptyTypes);
            //object cCryptoAESObj = cContructorCryptoAES.Invoke(new object[] { });

            //MethodInfo cCryptoAESMethod = cCryptoAES.GetMethod("EncriptarAES64");
            //sEncriptado = Convert.ToString(cCryptoAESMethod.Invoke(cCryptoAESObj, new object[] {"Data Source=DB-DESAROLLO;Initial Catalog=CFDI_Crypto_Test;Persist Security Info=True;User ID=Configuracion;Password=F4cturax10n_CnF"}));
            //sEncriptado_2 = PAXCrypto.CryptoAES.DesencriptaAES64(sEncriptado);

            //Console.WriteLine("Termine");
            //Console.ReadLine();

            //byte[] llave = {(byte) 97, (byte) 67, (byte) 65, (byte) 11, (byte) 69, (byte) 1, (byte) 76, (byte) 4, (byte) 54, (byte) 131, (byte) 34, (byte) 209, (byte) 222, (byte) 136, (byte) 35, (byte) 86};
            byte[] llave1 = { 0xD1, 0x61, 0x56, 0x41, 0xB, 0x45, 0x1, 0x4, 0x88, 0x36, 0x83, 0x22, 0xDE, 0x4C, 0x23, 0x43 };

            string sllave = Convert.ToBase64String(llave1);

            //if (llave == llave1)
            //    Console.WriteLine("Iguales");
        }
    }
}
