using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Data;
using System.IO;
using System.Reflection;


namespace P_ConammReporteArchivo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
         

            DataTable tblReporte = new DataTable();

            fnLlenarReporte(tblReporte);

          
   
            String sHtml = "<html><body>" +ConvertDataTableToHTML(tblReporte)+"</body></html>";

            File.WriteAllText("Reporte " + DateTime.Now.ToString("ffff") +".xml", sHtml, Encoding.UTF8);
        }

        public static void fnGenerarHTML(DataTable ptblReporte) 
        {
            DataTable tblSecciones = ptblReporte.DefaultView.ToTable(true, "Directorio");
        
        }

        public static void fnLlenarReporte(DataTable tblReporte)
        {
            String sDirEjectuable = @"C:\PAXConamm_20160301\Cambios";
            //Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            String[] sCarpetas = Directory.GetDirectories(sDirEjectuable);

            tblReporte.Columns.Add("Directorio");

            tblReporte.Columns.Add("Archivo");

            tblReporte.Columns.Add("Borrado", typeof(bool));

            tblReporte.Columns.Add("Renombrado", typeof(bool));

            foreach (String sCarpeta in sCarpetas)
            {
                String[] sBorrados = Directory.GetDirectories(sCarpeta);

                foreach (String sAux in sBorrados)
                {
                    if (Path.GetFileNameWithoutExtension(sAux).ToUpper().Equals("BORRADO") || Path.GetFileNameWithoutExtension(sAux).ToUpper().Equals("BORRADOS"))
                    {
                        String[] sArchivosBorrados = Directory.GetFiles(sAux);

                        foreach (string sArchivo in sArchivosBorrados)
                        {
                            tblReporte.Rows.Add(Path.GetFileNameWithoutExtension(sCarpeta), Path.GetFileNameWithoutExtension(sArchivo), true, false);

                        }

                    }

                }

                String[] sArchivosCambios = Directory.GetFiles(sCarpeta);

                foreach (String sArchivo in sArchivosCambios)
                {
                    tblReporte.Rows.Add(Path.GetFileNameWithoutExtension(sCarpeta), Path.GetFileNameWithoutExtension(sArchivo), false, false);

                }
            }
        
        
        }

        public static string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table>";
            //add header row
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<th>" + dt.Columns[i].ColumnName + "</th>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }
    }
}
