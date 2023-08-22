using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneradorPruebasSantander.Properties;
using System.IO;
using System.Diagnostics;

namespace GeneradorPruebasSantander
{
    class clsGeneracionPaquetes
    {
        public static string fnDameYYYYMMDD(DateTime pdtFecha)
        {

            return pdtFecha.ToString("yyyyMMdd");
        
        }

        public static DateTime fnDamefecha72()
        {
            DateTime dtLimite = DateTime.Now.AddHours(-1);

            Random rRand = new Random((int)dtLimite.Ticks);


            dtLimite = dtLimite.AddDays(-rRand.Next(0, 4));
            dtLimite = dtLimite.AddHours((double)-rRand.Next(0, dtLimite.Hour));
            dtLimite = dtLimite.AddMinutes(-dtLimite.Minute).AddMinutes(rRand.Next(0, 60));
            dtLimite = dtLimite.AddSeconds(-dtLimite.Second).AddSeconds(rRand.Next(0, 60));


            return dtLimite;
        }

        public static string fnObtenerRFC(double pnPorcentaje)
        {
            System.Threading.Thread.Sleep(5);
        
            Random rRand = new Random((int)DateTime.Now.Ticks);

            if (rRand.NextDouble() * 100 > pnPorcentaje)
            {
                return fnDameUnRfcMalo();
                
            }
            else
            {
                return "AAA010101AAA";
            }

            return "";
        
        }

        public static string fnDameUnRfcMalo() 
        {
            string[] rfcsListaNegra = Resources.RFCListaNegra.Split(",".ToArray());

            string[] rfcsInexistentes = Resources.RFCInexsistente.Split(",".ToArray());

            List<string> rfcsMalos = rfcsListaNegra.Union(rfcsInexistentes).ToList();

            Random rRandomRFC = new Random();



            return rfcsMalos[rRandomRFC.Next(0,rfcsMalos.Count-1)].Trim();
        }

        public static string fnGenerarArchivo( int pnRegistros,double pnPorcerntajeMalo) 
        {
            string sArchivo = string.Empty;

            DateTime dtFechaRandom; 

            for (int i = 0; i < pnRegistros; i++)
            {
                dtFechaRandom =fnDamefecha72();
                sArchivo += Resources.sRegistro.Replace("$FECHA$", dtFechaRandom.ToString("yyyy-MM-ddTHH:mm:ss")).Replace("$RFC$", fnObtenerRFC(pnPorcerntajeMalo)) + Environment.NewLine;
            }

            sArchivo.Substring(0, sArchivo.Length - Environment.NewLine.Length);
            

            return sArchivo;
        }

        public static string fnGenerarArchivoFin(int nArchivos, int nTotalRegistros, int nRegistrosArchivo) 
        {

            string sTxtFin = string.Empty;

            sTxtFin += nArchivos + "|Archivos"+ Environment.NewLine;
            sTxtFin += nTotalRegistros + "|Total de registros"+Environment.NewLine;
            sTxtFin += nRegistrosArchivo + "|Registros por archivo"+ Environment.NewLine;


            return sTxtFin;
        
        }

        public static void fnEscribirArchivo(string psRutaArchivo, string psTxtArchivo) 
        {
            File.WriteAllText(psRutaArchivo, psTxtArchivo);
            
        
        }

        public static void fnGenerarGZIP(string psNombreArchivo,string psRutaExe) 
        {
           ProcessStartInfo psiEjecucion
                                       = new ProcessStartInfo(psRutaExe, psNombreArchivo);

           psiEjecucion.CreateNoWindow = true;
           psiEjecucion.UseShellExecute = false;
           psiEjecucion.RedirectStandardError = true;

           // Inicia el proceso ya inicializado y espera a que termine su ejecucion
           Process pProceso = Process.Start(psiEjecucion);
           pProceso.WaitForExit();

           //if (pProceso.ExitCode != 0) throw new Exception("No se pudo crear el archivo BIN: " + _sNombreArchivoBin);
           pProceso.Dispose();

        }

        public static void fnGenerarPrueba(string psRutaGuardar)
        {
            fnGenerarPrueba(psRutaGuardar, Settings.Default.sInterface,
                Settings.Default.nArchivos, Settings.Default.nTotalRegistros, Settings.Default.nRegistrosArch
                , Settings.Default.bAcompletar, Settings.Default.nRfcMalo);
        }

        public static void fnGenerarPrueba(string psRutaGuardar,string psInterface, int pnArchivos,
            int pnTotalRegistros,int pnRegistrosArch, bool pbAcompletar,double pnRfcMalo) 
        {


            String sInterface = psInterface;

            DateTime dtRandom = fnDamefecha72();


            string psRutaArchivoFin = sInterface +fnDameYYYYMMDD(dtRandom) + ".fin";

           fnEscribirArchivo(psRutaGuardar + Path.DirectorySeparatorChar + psRutaArchivoFin,
               fnGenerarArchivoFin(pnArchivos, pnTotalRegistros, pnRegistrosArch));

           int nTotalGenerados = 0;

           for (int i = 0; i < pnArchivos; i++) 
            {
                String sTxtArchivo;

                string sRutaAux;

                int nRegistros = pnRegistrosArch;

                if (i == pnArchivos - 1)
                {
                    if (pbAcompletar)
                    {

                        nRegistros = pnTotalRegistros - nTotalGenerados;

                    }

                    nTotalGenerados += nRegistros;


                }
                else
                {

                    nTotalGenerados += nRegistros;
                }



                sTxtArchivo = fnGenerarArchivo(nRegistros, pnRfcMalo);

                sRutaAux = psRutaGuardar + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(psRutaArchivoFin) + "_PARTE_" + (i + 1) + ".txt";

                fnEscribirArchivo(sRutaAux,sTxtArchivo);

                fnGenerarGZIP(sRutaAux, Settings.Default.srutaGZIPEXE);

                
            
            }
        
        }
    }
}
