using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;

namespace PAXConectorFTPGTCFDI33
{
    public static class clsExtensionMethod
    {
        public static IEnumerable<List<cfdi>> Partition<cfdi>(this IList<cfdi> source, Int32 size)
        {
            for (int i = 0; i < Math.Ceiling(source.Count / (Double)size); i++)
                yield return new List<cfdi>(source.Skip(size * i).Take(size));
        }

        public static clsColeccionCFDI[] fnParticion(List<clsCfdi> lCFDI, int nHilos)
        {
            int nTamaño = lCFDI.Count / nHilos;

            List<clsColeccionCFDI> lColecciondeColecciones = new List<clsColeccionCFDI>();

            // cuando la cantidad de facturas es menor a la cantidad de Hilos
            if (nTamaño == 0)
            {
                for (int i = 0; i < lCFDI.Count; i++)
                {

                    clsColeccionCFDI cColeccionAux = new clsColeccionCFDI();

                    clsCfdi[] cfdis = new clsCfdi[] { lCFDI[i] };

                    cColeccionAux.Collecion = cfdis;

                    lColecciondeColecciones.Add(cColeccionAux);
                }

                return lColecciondeColecciones.ToArray();
            }

            //cuando la cantidad de facturas es Mayor o igual a la cantidad de Hilos

            int nPaquetesAlojados = 1;
            for (int i = 0; i < lCFDI.Count; i += nTamaño)
            {
                clsColeccionCFDI cColeccionAux = new clsColeccionCFDI();
                clsCfdi[] cfdis;

                /*
                 * Si la division de la cantidad de comprobantes entre la cantidad de Hilos no es exacta (osea su residuo es mayor a 0)
                 * Y el indice de Paquetes que ya fueron alojados es igual a la cantidad de Hilos (osea es el ultimo paquete a alojar)
                 * 
                 * Entonces se toman todos los paquetes faltantes
                 */
                if (lCFDI.Count % nHilos > 0 && nPaquetesAlojados == nHilos)
                {

                    cfdis = lCFDI.Skip(i).Take(lCFDI.Count - i).ToArray();
                    cColeccionAux.Collecion = cfdis;
                    lColecciondeColecciones.Add(cColeccionAux);

                    break;
                }

                cfdis = lCFDI.Skip(i).Take(nTamaño).ToArray();
                cColeccionAux.Collecion = cfdis;
                lColecciondeColecciones.Add(cColeccionAux);

                nPaquetesAlojados++;
            }

            return lColecciondeColecciones.ToArray();

        }


        public static clsColeccionCFDI[] fnParticion1(List<clsCfdi> lCFDI, int nHilos)
        {
            int nTamaño = lCFDI.Count / nHilos;

            clsColeccionCFDI[] cColecciondeColecciones;

            //Se revisa el número de comprobantes por Hilo
            if (nTamaño == 0)
            {
                // Si es 0, el numero de comprobantes es menor al tamaño por Hilo configurado
                // por lo que se genera un hilo por comprobante

                cColecciondeColecciones = new clsColeccionCFDI[lCFDI.Count];

                for (int i = 0; i < lCFDI.Count; i++)
                {
                    clsColeccionCFDI cColeccionAux = new clsColeccionCFDI();

                    clsCfdi[] cfdiComprobantes = new clsCfdi[1];

                    cfdiComprobantes[0] = lCFDI[i];

                    cColeccionAux.Collecion = cfdiComprobantes;

                    cColecciondeColecciones[i] = cColeccionAux;
                }

                return cColecciondeColecciones;
            }
            else
            {
                //Si no, se dividen los comprobantes es estructura por Hilo
                int nEstructuraComprobantes = 0;
                int nContador = 0;
                cColecciondeColecciones = new clsColeccionCFDI[nHilos];

                for (int i = 0; i < nHilos; i++)
                {
                    clsColeccionCFDI cColeccionAux = new clsColeccionCFDI();
                    clsCfdi[] cfdiComprobantes;

                    // Se inicializan el tamaño que cada hilo va tener de comprobantes                   
                    if (nHilos.Equals(i + 1))
                    {
                        // Si el hilo es el ultimo, puede tener el mismo tamaño que los primero 4, pero puede tener un número mayor de comprobantes
                        nEstructuraComprobantes = lCFDI.Count - (nTamaño * (nHilos - 1));
                        cfdiComprobantes = new clsCfdi[nEstructuraComprobantes];
                    }
                    else
                    {
                        // Si no, el tamaño de comprobantes es igual a la división de comprobantes entre el número de hilos
                        nEstructuraComprobantes = nTamaño;
                        cfdiComprobantes = new clsCfdi[nEstructuraComprobantes];
                    }

                    // Se recorre la lista de Comprobantes y se asigna por estructura
                    for (int j = 0; j < nEstructuraComprobantes; j++)
                    {
                        // Cuando nContador llegue al numero de comprobantes en la lista, se sale del ciclo, terminando de repartir los comprobantes en los hilos
                        if (nContador.Equals(lCFDI.Count))
                            break;

                        cfdiComprobantes[j] = lCFDI[nContador];

                        // Esta variable nos va a servir para revisar cuantos comprobantes se han metido en las estructuras, para que
                        // cuando llegue al limite se salga
                        nContador++;
                    }

                    cColeccionAux.Collecion = cfdiComprobantes;
                    cColecciondeColecciones[i] = cColeccionAux;
                }
            }
            return cColecciondeColecciones;
        }

        public static String fnComparaListas(List<clsCfdi> lCFDI, List<string> lSalida, string sNombreZip)
        {
            List<string> lOriginal = new List<string>();

            foreach (clsCfdi cfdi in lCFDI)
            {
                lOriginal.Add(cfdi.FileName);
            }

            List<string> lRes = new List<string>();

            foreach (string sCadena in lOriginal)
            {
                if (!lSalida.Contains(sCadena))
                    lRes.Add(sCadena);
            }
            if (lRes.Count > 0)
            {
                String sRes = "Hubo un incidente con el archivo: " + sNombreZip + ", no se pudieron procesar " + lRes.Count + " de " + lOriginal.Count + " comprobantes favor de reenviarlas";
                return sRes;
            }
            return "";
        }

        public static ICSharpCode.SharpZipLib.Zip.ZipOutputStream zip;
        public static MemoryStream bos;
        public static ZipOutputStream zipfile;
        public static ZipEntry zipentry;
        public static Hashtable htHilosRepetidos = new Hashtable();
        public static XmlDocument xdHilos;
        public static int nHilos;
        public static List<string> lArchivosProcesados;
        public static Semaphore Semaforo;
        public static string sRutaRFCCambio = string.Empty;
    }
}
