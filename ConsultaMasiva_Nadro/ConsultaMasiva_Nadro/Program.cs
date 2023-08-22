using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace ConsultaMasiva_Nadro
{
    class Program
    {
        static string nConsultasProcesadas = "0";

        static DataTable dtResultados;

        #region Funciones
        static void Main(string[] args)
        {

            string[] sQuerys = Properties.Settings.Default.sQuerys.Split("\n".ToArray());

            dtResultados = new DataTable();

            dtResultados.Columns.Add("Hilo");

            dtResultados.Columns.Add("Tiempo");

            dtResultados.Columns.Add("Registros");

            dtResultados.Columns.Add("Consulta");

            dtResultados.Columns.Add("Memoria Antes (MB)");

            dtResultados.Columns.Add("Memoria Despues (MB)");


            Console.WriteLine("Son " + sQuerys.Length + " Consultas");

            List<Thread> tHilos = new List<Thread>();

            int nHilo = 1;

            foreach (string squery in sQuerys)
            {


                Thread tHilo = new Thread(fnRealizarConsulta);

                tHilo.Name = "t" + nHilo;

                Consulta cConsulta = new Consulta(Properties.Settings.Default.sCadenaConexion, Properties.Settings.Default.sCadenaConexionSA, squery.Trim(), Properties.Settings.Default.sQueryMemoria);

                tHilo.Start(cConsulta);

                tHilos.Add(tHilo);

                nHilo++;


            }

            foreach (Thread thilo in tHilos)
            {

                thilo.Join();

            }

            ExportarExc(dtResultados);

            Console.WriteLine("ya terimine");



            Console.ReadLine();
        }

        /// <summary>
        /// Funcion dentro del Hilo que realiza la consulta
        /// </summary>
        /// <param name="cConsulta"></param>
        static void fnRealizarConsulta(Object cCon)
        {
            Consulta cConsulta = (Consulta)cCon;



            cConsulta.realizarConsulta();

            lock (nConsultasProcesadas)
            {
                int nconsultas = int.Parse(nConsultasProcesadas);
                
                Console.WriteLine(Thread.CurrentThread.Name + " | " + cConsulta.sOut + " | " + nconsultas + " Consultas");

                nconsultas++;

                nConsultasProcesadas = "" + nconsultas;

                dtResultados.Rows.Add(Thread.CurrentThread.Name, cConsulta.sTiempo, cConsulta.sRegistros, cConsulta.sQuery,cConsulta.sMemoria_Antes,cConsulta.sMemoria_Despues);



            }
        }


        /// <summary>
        /// Exporta Datatables a Excel con formato HTML
        /// </summary>
        /// <param name="datos"></param>
        public static void ExportarExc( DataTable datos)
        {
            try
            {
                Guid Giud = Guid.NewGuid();
                String dlDir = @"Exceles/";
                string rutas = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Consultas"+ DateTime.Now.Day+"_"+DateTime.Now.Hour+"_"+DateTime.Now.Minute+"_"+DateTime.Now.Second+"_"+DateTime.Now.Millisecond+ ".xls";
                FileStream fs = new FileStream(rutas, FileMode.Create,
                                               FileAccess.ReadWrite);
                StreamWriter w = new StreamWriter(fs);
                string comillas = char.ConvertFromUtf32(34);
                StringBuilder html = new StringBuilder();
                html.Append(@"<!DOCTYPE html PUBLIC" + comillas +
                "-//W3C//DTD XHTML 1.0 Transitional//EN" + comillas +
                " " + comillas
                + "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" + comillas + ">");
                html.Append(@"<html xmlns=" + comillas
                             + "http://www.w3.org/1999/xhtml"
                             + comillas + ">");
                html.Append(@"<head>");
                html.Append(@"<meta http-equiv=" + comillas + "Content-Type"
                             + comillas + "content=" + comillas
                             + "text/html; charset=utf-8" + comillas + "/>");
                html.Append(@"<title>Untitled Document</title>");
                html.Append(@"</head>");
                html.Append(@"<body>");


                //Generando encabezados del archivo 
                //(aquí podemos dar el formato como a una tabla de HTML)
                html.Append(@"<table WIDTH=730 CELLSPACING=0 CELLPADDING=10 
                    border=8 BORDERCOLOR=" + comillas + "#333366"
                            + comillas + " bgcolor=" + comillas + "#FFFFFF"
                            + comillas + ">");
                html.Append(@"<tr> <b>");

                foreach (DataColumn dcColumna in datos.Columns)
                {
                    html.Append(@"<th>" + dcColumna.ColumnName + "</th>");
                }
                html.Append(@"</b> </tr>");

                //Generando datos del archivo
                for (int i = 0; i < datos.Rows.Count; i++)
                {
                    html.Append(@"<tr>");
                    for (int j = 0; j < datos.Columns.Count; j++)
                    {
                        
                            html.Append(@"<td>" +
                                        datos.Rows[i][j].ToString() + "</td>");
                        

                    }
                    html.Append(@"</tr>");
                }
                html.Append(@"</body>");
                html.Append(@"</html>");
                w.Write(html.ToString());
                w.Close();

                //Reporta progreso (20% - 90%)

         
                //fnDescargaArchivo(rutas);

            }
            catch (Exception ex)
            {
                
            }

        }
        
        
        #endregion



        
    }
    
    /// <summary>
    /// Clase que se encarga de hacer la consulta
    /// </summary>
    class Consulta
    {


        #region Variables
        /// <summary>
        /// Cadena de conexion con la que se crea la consulta
        /// </summary>
        private string sCadenaConexion;

        private string sCadenaConexion_sa;

        /// <summary>
        /// Query dado para la consulta
        /// </summary>
        string squery;
        public string sQuery { get { return squery; } }

        string squeryMemoria;

        /// <summary>
        /// Cadena que se usa para determinar el tiempo que duro la consulta en darse
        /// </summary>
        string stiempo = "";
        public string sTiempo { get { return stiempo; } }


        /// <summary>
        /// Cadena que se imprime en la consola
        /// </summary>
        string sout = "";
        public string sOut { get { return stiempo; } }

        string sregistros;
        public string sRegistros { get { return sregistros; } }

        string smemoria_antes;
        public string sMemoria_Antes { get { return smemoria_antes; } }

        string smemoria_despues;
        public string sMemoria_Despues { get { return smemoria_despues; } }

        #endregion


        #region Constructor
        /// <summary>
        /// Constructor que recibe las cadenas de conexion y los querys a ejecturar
        /// </summary>
        /// <param name="pscadena_conexion"></param>
        /// <param name="pscadena_query"></param>
        public Consulta(string pscadena_conexion, string pscadena_conexion_sa, string pscadena_query, string psqueryMemoria) { this.sCadenaConexion = pscadena_conexion; this.squery = pscadena_query; this.sCadenaConexion_sa = pscadena_conexion_sa; this.squeryMemoria = psqueryMemoria; }
        
        #endregion

        #region Funciones

        /// <summary>
        ///  Metodo que hace la consulta a la base de datos
        /// </summary>
        public void realizarConsulta()
        {

            try
            {
                using (SqlConnection scConexionMemoria = new SqlConnection(sCadenaConexion_sa))
                {

                    using (SqlConnection scConexion = new SqlConnection(sCadenaConexion))
                    {

                        using (SqlCommand scoComandoMemoria = new SqlCommand(squeryMemoria))
                        {

                            using (SqlCommand scoComando = new SqlCommand(squery))
                            {
                                using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComandoMemoria))
                                {
                                  
                                        try
                                        {
                                            DataTable dtDatosMemoria;

                                            #region Obtener Memoria Antes de la Consulta

                                            using (SqlDataAdapter sdaAdaptadorMemoria = new SqlDataAdapter(scoComando))
                                            {
                                                try
                                                {
                                                    scoComandoMemoria.Connection = scConexionMemoria;

                                                    scoComandoMemoria.CommandTimeout = 360;

                                                    scoComandoMemoria.CommandType = CommandType.Text;



                                                    dtDatosMemoria = new DataTable();


                                                    sdaAdaptadorMemoria.Fill(dtDatosMemoria);


                                                    smemoria_antes = dtDatosMemoria.Rows[0]["Total Server Memory (MB)"].ToString();
                                                }
                                                catch
                                                {
                                                    
                                                    sdaAdaptadorMemoria.Dispose();
                                                
                                                }
                                            }


                                            #endregion

                                            #region Consulta
                                            scoComando.Connection = scConexion;

                                            scoComando.CommandTimeout = 360;

                                            scoComando.CommandType = CommandType.Text;

                                            DataTable dtDatos;

                                            dtDatos = new DataTable();

                                            DateTime t1 = DateTime.Now;

                                            sdaAdaptador.Fill(dtDatos);

                                            DateTime t2 = DateTime.Now;

                                            TimeSpan tx = t2.Subtract(t1);

                                            sregistros = dtDatos.Rows.Count.ToString();

                                            stiempo = tx.ToString();

                                            squery = scoComando.CommandText;

                                            
                                            #endregion

                                            #region Obtener Memoria Despues de la Consulta


                                            using (SqlDataAdapter sdaAdaptadorMemoria = new SqlDataAdapter(scoComando))
                                            {
                                                try
                                                {
                                                    scoComandoMemoria.Connection = scConexionMemoria;

                                                    scoComandoMemoria.CommandTimeout = 360;

                                                    scoComandoMemoria.CommandType = CommandType.Text;



                                                    dtDatosMemoria = new DataTable();


                                                    sdaAdaptadorMemoria.Fill(dtDatosMemoria);


                                                    smemoria_despues = dtDatosMemoria.Rows[0]["Total Server Memory (MB)"].ToString();
                                                }
                                                catch
                                                {

                                                    sdaAdaptadorMemoria.Dispose();

                                                }
                                            }

                                            #endregion

                                            #region Imprimir en sOut
                                            sout = tx.ToString() + " | Registros: " + dtDatos.Rows.Count + " | " + scoComando.CommandText;
                                            #endregion

                                        }
                                        catch (Exception ex)
                                        {

                                            sout = "No Completado" + ex.Message + " | " + scoComando.CommandText;

                                            stiempo = "No Completado";

                                            squery = scoComando.CommandText;
                                        }

                                        finally
                                        {

                                            scConexion.Close();
                                            scConexion.Dispose();
                                            sdaAdaptador.Dispose();
                                            scoComandoMemoria.Dispose();
                                            scConexionMemoria.Close();
                                          
                                            scConexionMemoria.Dispose();
                                        }

                                    
                                }
                            }
                        }
                    }

                }
            }
            catch { }



        } 
        #endregion
    
    
    }
 
}


