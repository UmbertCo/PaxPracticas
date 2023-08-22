using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using OpenSSL_Lib;
using OpenSSL_Lib.Properties;
using OpenSSL_Lib.PFX;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Threading;


namespace PruebaOpenSSL
{
    class Program
    {

        static void Main(string[] args)
        {


            //cPEM pemCert = new cPEM(File.ReadAllBytes(@"C:\Users\Ismael Hidalgo\Desktop\AAA010101AAA\aaa010101aaa__csd_01.cer"), DocTipo.CER);
            //cPEM pemKey = new cPEM(File.ReadAllBytes(@"C:\Users\Ismael Hidalgo\Desktop\AAA010101AAA\aaa010101aaa__csd_01.key"), DocTipo.KEY);

            //cValidacionCertificados pfx = new cValidacionCertificados(@"C:\Users\Ismael Hidalgo\Desktop\AAA010101AAA\aaa010101aaa__csd_01.cer", @"C:\Users\Ismael Hidalgo\Desktop\AAA010101AAA\aaa010101aaa__csd_01.key", "12345678a");

            Double tTicksOpensslib = 0;

            Double tTicksChilkat = 0;

            //for (int i = 0; i < 1000; i++)
            //{
                
      
            //DateTime tTick = DateTime.Now;

            //cSello Sello = new cSello("12345678a", File.ReadAllBytes(@"C:\Users\Ismael Hidalgo\Desktop\AAA010101AAA\aaa010101aaa__csd_01.key"), @"C:\Users\Ismael Hidalgo\Documents\Visual Studio 2010\Projects\Probando_OpenSSl\PruebaOpenSSL\bin\Debug\");

            //Sello.sCadenaOriginal = "asd";

            //String s = Sello.sSello;

            //TimeSpan tchk = DateTime.Now.Subtract(tTick);

            //tTicksOpensslib += tchk.TotalMilliseconds;

            //tTick = DateTime.Now;

            //s = fnGenerarSello("asd", File.ReadAllBytes(@"C:\Users\Ismael Hidalgo\Desktop\AAA010101AAA\aaa010101aaa__csd_01.key"), "12345678a");

            //tchk = DateTime.Now.Subtract(tTick);

            //tTicksChilkat += tchk.TotalMilliseconds;
            //}

            Object _locker1 = new object();

            Object _locker2 = new object();

            int nHilos = 1000;

            Thread[] tHilosOpenSSLLib = new Thread[nHilos];

            Thread[] tHilosChilkat = new Thread[nHilos];

            for (int i = 0; i < tHilosChilkat.Length; i++)
            {
                tHilosOpenSSLLib[i] = new Thread(new ThreadStart(delegate
                    {
                        DateTime tTick = DateTime.Now;

                        cSello Sello = new cSello("12345678a", File.ReadAllBytes(@"C:\Users\Ismael Hidalgo\Desktop\AAA010101AAA\aaa010101aaa__csd_01.key"), @"C:\Users\Ismael Hidalgo\Documents\Visual Studio 2010\Projects\Probando_OpenSSl\PruebaOpenSSL\bin\Debug\");

                        Sello.sCadenaOriginal = "asd";

                        String s = Sello.sSello;

                        TimeSpan tchk = DateTime.Now.Subtract(tTick);


                        lock (_locker1)
                        {
                            tTicksOpensslib += tchk.TotalMilliseconds;
                        
                        }

                    
                    }));
                tHilosChilkat[i] = new Thread(new ThreadStart(delegate
                    {
                        
                        DateTime tTick = DateTime.Now;

                        String s = fnGenerarSello("asd", File.ReadAllBytes(@"C:\Users\Ismael Hidalgo\Desktop\AAA010101AAA\aaa010101aaa__csd_01.key"), "12345678a");


                        TimeSpan tchk = DateTime.Now.Subtract(tTick);

                        lock (_locker2)
                        {
                            tTicksChilkat += tchk.TotalMilliseconds;

                        }
                    
                    }));


                tHilosChilkat[i].Start();
                tHilosOpenSSLLib[i].Start();

            }

            for (int i = 0; i < nHilos; i++)
            {
                tHilosOpenSSLLib[i].Join();
                tHilosChilkat[i].Join();
            }


            Console.WriteLine("Chilkat " + tTicksChilkat / nHilos + " OpenSLL_Lib " + tTicksOpensslib / nHilos + " Diff " + (tTicksOpensslib - tTicksChilkat) / nHilos);
            //pfx.fnValidarCerKey();

          


            Console.ReadLine();

        }

        public static string fnGenerarSello(string psCadenaOriginal, byte[] pbKey,string psPass)
        {
            try
            {
                //Llave privada original
                Chilkat.PrivateKey key = new Chilkat.PrivateKey();
                             
                key.LoadPkcs8Encrypted(pbKey, psPass);
              

                //Llave privada PEM
                Chilkat.PrivateKey pem = new Chilkat.PrivateKey();
                pem.LoadPem(key.GetPkcs8Pem());

                string pkeyXml = pem.GetXml();

                Chilkat.Rsa rsa = new Chilkat.Rsa();

                bool bSuccess;
                bSuccess = rsa.UnlockComponent("INTERMRSA_78UJEvED0IwK");
                bSuccess = rsa.GenerateKey(1024);

                rsa.LittleEndian = false;
                rsa.EncodingMode = "base64";
                rsa.Charset = "utf-8";
                rsa.ImportPrivateKey(pkeyXml);
                
                string sello = rsa.SignStringENC(psCadenaOriginal, "sha-1");

             

                return sello;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
