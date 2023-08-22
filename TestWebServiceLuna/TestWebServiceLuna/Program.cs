using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestWebServiceLuna
{
    class Program
    {
        private static String rutaArchivoLog = @"D:\PruebasWebServiceLunaSA\log.log";
        private static int horaStop = 0;
        private static int dayStop = 0;
        private static int tiempoEspera = 0;
        private static int contIntentos = 0;
        private static int maxIntentos = 20;

        static WSLunaSA.TestLunaWebServiceClient clientLuna = new WSLunaSA.TestLunaWebServiceClient();
        
        static void Main(string[] args)
        {
            Console.WriteLine(">> Inicia aplicación >>");

            Console.WriteLine(">>> Parámetros de entrada:");

            try
            {
                if (args != null && args.Length > 0)
                {
                    if (args[0] != "0")
                        rutaArchivoLog = args[0];

                    if (args.Length > 1)
                    {
                        dayStop = int.Parse(args[1]);

                        if (args.Length > 2)
                        {
                            horaStop = int.Parse(args[2]);
                        }

                        if (args.Length > 3)
                        {
                            tiempoEspera = int.Parse(args[3]);
                        }
                    }
                }

            Console.WriteLine("Ruta archivo: " + rutaArchivoLog);
            Console.WriteLine("Día: " + dayStop);
            Console.WriteLine("Hora: " + horaStop);
            Console.WriteLine("Tiempo de espera entre firmas: " + tiempoEspera);

            //Console.WriteLine(">>> Login");
            iniciarAplicacion();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error: " + ex.Message);
            }

            Console.WriteLine("Terminó aplicación");

            Console.ReadLine();
        }

        private static void iniciarAplicacion()
        {
            if (login("", ""))
            {
                Console.WriteLine(">>> Login completado");
                Console.WriteLine(">>> Firmando");
                uint cont = 0;
                while (true)
                {
                    String cadenaOriginal = String.Empty;
                    String firma = String.Empty;
                    try
                    {
                        cadenaOriginal = "||1.0|cc44df63-90ff-4638-af9f-7a605106460e|" + fechaFormateada() + "|kjAEZHbTm9kHzNHF5t/TwBtwyI1IArPhSJtfzRN4iIcyHPJSJDlw/e6BfCH3H4oLgCNVFEzyiPMWkvUKOHcXFyupn7C9dN4LmXUQs+WtGDCGSdasKUZ2FWDSGHpmtJAyKjmTI3UunUkTNcxwn/G0stOoFzUatraKd6WWuzk+Who=|20001000000100004045||";
                        firma = firmar(cadenaOriginal);
                        cont++;

                        String entradaLog = "\r\n" + cont + " - [" + DateTime.Now + "]\r\nCadena: " + cadenaOriginal + "\r\nFirma: " + firma;
                        escribirEntradaLog(entradaLog);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ocurrió un error al realizar la firma:\r\nCadena: " + cadenaOriginal + "\r\nFirma" + firma + "\r\nError: " + ex.Message);
                    }

                    int nDia = DateTime.Now.Day;
                    int nHora = DateTime.Now.Hour;

                    //if (nDia > dayStop || ( nDia<= dayStop && nHora >= horaStop) )
                    if(nDia >= dayStop && nHora >= horaStop)
                    {
                        break;
                    }
                    

                    if (tiempoEspera > 0)
                    {
                        try
                        {
                            Console.WriteLine("Se duerme hilo por " + tiempoEspera + " minutos");
                            System.Threading.Thread.Sleep(tiempoEspera * 60000);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ocurrió un error al dormir el hilo principal: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                contIntentos++;
                Console.WriteLine("No se pudo realizar el login en el webservice, se intentará realizar el login de nuevo");

                if (contIntentos <= maxIntentos)
                {
                    System.Threading.Thread.Sleep(5000);
                    iniciarAplicacion();
                }
            }
        }

        private static bool login(String usuario, String password)
        {
         //   WSLunaSA.TestLunaWebServiceClient clientLuna = new WSLunaSA.TestLunaWebServiceClient();
            
            bool result = false;

            try
            {
                String resultWS = clientLuna.login(usuario, password);

                if (resultWS.Trim().ToLower().Contains("success"))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error al realizar el login en el webservice: " + ex.Message);
                result = false;
            }

            return result;
        }

        private static String firmar(String cadenaOriginal)
        {
            

            String resultFirma = String.Empty;

            try
            {
                resultFirma = clientLuna.firmar(cadenaOriginal);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error al firmar la cadena:\r\nCadena Original: " + cadenaOriginal + "\r\nFirma: " + resultFirma + "\r\nError: " + ex.Message);
                resultFirma = string.Empty;
            }

            return resultFirma;
        }

        private static void escribirEntradaLog(String entrada)
        {
            try
            {
                //if (!System.IO.File.Exists(rutaArchivoLog))
                //{
                //    System.IO.File.Create(rutaArchivoLog);
                //}

                System.IO.File.AppendAllText(rutaArchivoLog, entrada);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error al escribir la entrada en el log:\r\nEntrada: " + entrada + "\r\nError: " + ex.Message);
            }

        }

        private static String fechaFormateada()
        {
            DateTime fechaActual = DateTime.Now;

            return fechaActual.ToString("yyyy-MM-ddThh:mm:ss");
        }

        private static void TestWSLuna()
        {
            Console.WriteLine(">>> Login >>>");

            WSLunaSA.TestLunaWebServiceClient clientLuna = new WSLunaSA.TestLunaWebServiceClient();

            String result = clientLuna.login("", "");
            Console.WriteLine("Respuesta login: " + result);

            if (result.Trim().ToLower().Contains("success"))
            {
                Console.WriteLine(">>> Login correcto >>>");

                Console.Write(">>> Firmando ... >>>");
                String cadenaOriginal = "||1.0|cc44df63-90ff-4638-af9f-7a605106460e|2012-12-28T16:22:13|kjAEZHbTm9kHzNHF5t/TwBtwyI1IArPhSJtfzRN4iIcyHPJSJDlw/e6BfCH3H4oLgCNVFEzyiPMWkvUKOHcXFyupn7C9dN4LmXUQs+WtGDCGSdasKUZ2FWDSGHpmtJAyKjmTI3UunUkTNcxwn/G0stOoFzUatraKd6WWuzk+Who=|20001000000100004045||";
                Console.WriteLine("Datos a firmar: " + cadenaOriginal);
                String resultFirma = clientLuna.firmar(cadenaOriginal);

                Console.WriteLine("Firma: " + resultFirma);

                //Console.WriteLine(">>> Logout >>>");
                //String resultLogout = clientLuna.logout("", "");
                //Console.WriteLine("Logout: " + resultLogout);
            }
            else
            {
                Console.WriteLine("Ocurrió un error al realizar el login");
            }

            Console.ReadLine();
        }
    }
}
