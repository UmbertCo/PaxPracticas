using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace P_PruebasArreglos
{
    class Program
    {
        static void Main(string[] args)
        {

            DataTable tbl = new DataTable();

            tbl.Columns.Add("a");

            tbl.Rows.Add("0");
            tbl.Rows.Add("1");
            //tbl.Rows.Add("1");
            tbl.Rows.Add("2");
            tbl.Rows.Add("3");
            tbl.Rows.Add("4");
            tbl.Rows.Add("5");
            tbl.Rows.Add("6");
            tbl.Rows.Add("7");
            tbl.Rows.Add("8");
            tbl.Rows.Add("9");


            DataTable[] tbls = fnParticion1(tbl, 11);

        }


        public static DataTable[] fnParticion1(DataTable lCFDI, int nHilos)
        {


            int nTamaño = lCFDI.Rows.Count / nHilos;

            DataTable[] cColecciondeColecciones;

            if (lCFDI.Rows.Count < nHilos) 
            {
                nTamaño = 1;
                nHilos = lCFDI.Rows.Count;
            
            
            }

            //Se revisa el número de comprobantes por Hilo
            if (nTamaño == 0)
            {
                // Si es 0, el numero de comprobantes es menor al tamaño por Hilo configurado
                // por lo que se genera un hilo por comprobante

                cColecciondeColecciones = new DataTable[1];

                cColecciondeColecciones[0] = lCFDI;

                return cColecciondeColecciones;
            }
            else
            {

                cColecciondeColecciones = new DataTable[nHilos];

                int nAcumulado = 0;

                for (int i = 0; i < nHilos; i++)
                {
                    DataTable cColeccionAux = new DataTable();


                    if (nHilos.Equals(i + 1))
                    {

                        cColeccionAux = lCFDI.Select().Skip(nAcumulado).Take(lCFDI.Rows.Count - nAcumulado).CopyToDataTable();
                    }
                    else
                    {
                        cColeccionAux = lCFDI.Select().Skip(nAcumulado).Take(nTamaño).CopyToDataTable();

                        nAcumulado += nTamaño;

                    }


                    cColecciondeColecciones[i] = cColeccionAux;
                }
            }
            return cColecciondeColecciones;
        }
    }
}
