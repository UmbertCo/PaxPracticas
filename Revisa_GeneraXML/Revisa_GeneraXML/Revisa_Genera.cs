using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Schema;

namespace Revisa_GeneraXML
{
    public partial class Revisa_Genera : ServiceBase
    {
       static string[]  lArchivos_Busqueda;

       static string[] lArchivos_Busqueda_aux;



       static string sNuevo_Archivo { set; get; }

        Thread thHilo_secundario;

        StreamWriter swEscritor_Archivo_Log;

        GeneraXML gGenerador { set; get; }

        FileSystemWatcher fswObservador;
      
        public Revisa_Genera()
        {


            Debugger.Launch();

            dfnDelegado dApuntador = new dfnDelegado(fnMonitorear_Ruta);

            thHilo_secundario = new Thread(new ThreadStart(dApuntador));


            InitializeComponent();

         

            fswObservador = new FileSystemWatcher(Properties.Settings.Default.sRuta_Busqueda);

            fswObservador.NotifyFilter = NotifyFilters.FileName |NotifyFilters.DirectoryName;
            
            fswObservador.EnableRaisingEvents = true;

            fswObservador.Created+=new FileSystemEventHandler(fswObservador_Created);

           // Cambio += new dEventoCambio(fnRevisa_Genera_Cambio);
        
        
        }

      

    
        
        

    
        protected override void OnStart(string[] pargs)
        {
            lArchivos_Busqueda = fnObtenerArchivosCarpeta
                                        (Properties.Settings.Default.sRuta_Busqueda);

            thHilo_secundario.Start();
                    
        }

        protected override void OnStop()
        {
        }

        #region Funciones de Eventos

        void fswObservador_Created(object sender, FileSystemEventArgs e)
        {
            FileInfo archivo = new FileInfo(e.FullPath);

            fnEscribirLog("se Agrego: " + Path.GetFileName(e.FullPath), true);

            gGenerador = new GeneraXML(archivo.FullName);

            Validador vValidar = new Validador(gGenerador.sDocumento);

            vValidar.fnRevisar();

            if (vValidar.bErrorAdvertencia)
            {
                fnEscribirLog("Se Encontraron Errores en Archivo: " + e.FullPath, true);

                fnEscribirLog(vValidar.sErrores + vValidar.sWarnings, false);

                File.Copy(archivo.FullName, Properties.Settings.Default.sRuta_Erroneos + Path.GetFileName(e.FullPath), true);
                File.Delete(archivo.FullName);
                return;
            }

            File.Copy(archivo.FullName, Properties.Settings.Default.sRuta_Validados + Path.GetFileName(e.FullPath), true);

            File.Delete(archivo.FullName);

            gGenerador.fnGuardarXML(Properties.Settings.Default.sRuta_Validados + Path.GetFileNameWithoutExtension(archivo.Name) + ".xml");

            fnEscribirLog("Se Valido Correctamente sin Errores archivo: " + archivo.FullName, true);

        }



        #endregion


        /// <summary>
        /// Funcion 
        /// </summary>
        /// <param name="sMensaje"></param>
        /// <param name="bFecha"></param>
        public void fnEscribirLog(string sMensaje, bool bFecha)
        {

            swEscritor_Archivo_Log = File.AppendText(Properties.Settings.Default.sRuta_Log);

            if (bFecha)

                swEscritor_Archivo_Log.WriteLine(sMensaje + " " + DateTime.Now.ToShortDateString());

            else
                swEscritor_Archivo_Log.WriteLine(sMensaje);


            swEscritor_Archivo_Log.Close();

        }

        /// <summary>
        /// Proceso de hilo secundario que se encarga de monitorear si existe algun cambio
        /// en la carpeta seleccionada
        /// </summary>
         void fnMonitorear_Ruta ()
        {
            //Debugger.Launch();

            while (true)
            {
                //if (fnExisteCambio())
                //{
                //    lArchivos_Busqueda = fnObtenerArchivosCarpeta
                //                            (Properties.Settings.Default.sRuta_Busqueda);

                //    Cambio(new Cambio_Args(sNuevo_Archivo));
                //}


                Thread.Sleep(5);

            }
                
        
        
        }

        /// <summary>
        /// Obtiene los archivos que existen en la carpeta seleccionada
        /// </summary>
        /// <param name="pruta">carpeta seleccionada</param>
        /// <returns>arreglo de nombres de archivos</returns>
       static public string [] fnObtenerArchivosCarpeta(string pruta) 
        {


            return Directory.GetFiles(pruta);
          
        }


        /// <summary>
        /// Anliza si existe alguna diferencia entre los archivos que en ese momento estan
        /// y los que anteriormente se leyeron
        /// </summary>
        /// <returns> true : si existe algun cambio
        ///           false : si no existe ningun cambio
        /// </returns>
       static public bool fnExisteCambio()
        {
           //obtiene lista auxiliar para comparar los  nuevos archivos con los viejos
           lArchivos_Busqueda_aux = fnObtenerArchivosCarpeta(Properties.Settings.Default.sRuta_Busqueda);

           //si en primera instancia los arreglos son de diferente tamaño se entiende que
           //son diferentes de los archivos anteriores
           if (lArchivos_Busqueda_aux.Length > lArchivos_Busqueda.Length)
           {
               sNuevo_Archivo = lArchivos_Busqueda_aux[lArchivos_Busqueda_aux.Length-1];
               return true;
           }
           //se compara nombre por nombre de archivo para saber si existe alguna diferencia
           for (int i = 0; i < lArchivos_Busqueda.Length; i++) 
           {
               if (!lArchivos_Busqueda[i].Equals(lArchivos_Busqueda[i])) 
               {
                   sNuevo_Archivo = lArchivos_Busqueda[i];
                   return true;
               }
           }

            return false;
        }

    
    }


    #region Delegados
    /// <summary>
    /// Delegado para hilo externo en Servicio de windows
    /// </summary>
    public delegate void dfnDelegado();


    //public delegate void dEventoAgregoArchivo();

    //public delegate void dEventoBorroArchivo();
    #endregion



}
