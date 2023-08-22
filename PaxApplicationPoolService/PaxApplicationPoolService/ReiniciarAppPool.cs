using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.Xml;
using System.IO;
using System.Reflection;

namespace PaxApplicationPoolService
{
    class ReiniciarAppPool
    {
        public static void ChecarEstatusIIS()
        {

            //tomamos el directorio donde está siendo ejecutado el servicio
            String dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(dir, "ListaAppPool.xml");
             
            /**********************CREANDO LISTA DE APPLICATION POOL POR REVISAR******************************/
            //esta lista se carga a partir de un archivo xml ubicado en Resource1.resx
            //para agregar elementos a esta lista se debe modificar el archivo xml

            List<string> lista = new List<string>();
            XmlDocument xmlAppPool = new XmlDocument();
            try
            {
                xmlAppPool.Load(path);
            }
            catch (Exception ex)
            {
                clsLog.EscribirLog("Error al cargar XML: "+ex);
            }

            XmlNodeList xApplications_pools = xmlAppPool.GetElementsByTagName("applications_pools");
            XmlNodeList xApp_pool = ((XmlElement)xApplications_pools[0]).GetElementsByTagName("app_pool");

            foreach (XmlElement app_pool in xApp_pool)
            {
                lista.Add(app_pool.InnerText);
            }
            /*************************FIN CREACIÓN LISTA APPLICATION POOL******************************/


            //definimos las constantes dcon los posibles estados de un ApplicationPool
            const int MD_APPPOOL_STATE_STARTING = 1;
            const int MD_APPPOOL_STATE_STARTED = 2;
            const int MD_APPPOOL_STATE_STOPPING = 3;
            const int MD_APPPOOL_STATE_STOPPED = 4;

            //Al menos en IIS 6 la raíz de Pools se llama IISAplicationPools
            //por lo que debemos entrar a este nodo para acceder a sus nodos 'hijos'
            const string WebServerSchema = "IISAPPLICATIONPOOLS";

            try
            {
                //Nos conectamos al servidor local
                DirectoryEntry W3SVC = new DirectoryEntry("IIS://localhost/w3svc", "", "");
                //Recorremos los nodos buscando el nodo que contiene los AppPools
                foreach (DirectoryEntry Site in W3SVC.Children)
                {
                    if (Site.SchemaClassName.ToUpper() == WebServerSchema)
                    {
                        //Una vez encontrado el nodo debemos recorrer la lista de ApplicationPools en IIS para saber su estado
                        foreach (DirectoryEntry child in Site.Children)
                        {
                            foreach (String appPool in lista)
                            {
                                //coinciden el elemento de la lista cargada del xml con el elemento de la lista que trae el app pool?
                                if (appPool.Equals(child.Name.ToString()))
                                {
                                    //Accedemos a las propiedades del nodo y verificamos el estado, ya revisamos más arriba los posibles estados
                                    System.DirectoryServices.PropertyCollection appPoolProps = child.Properties;

                                    if (int.Parse(child.Properties["AppPoolState"].Value.ToString()) == MD_APPPOOL_STATE_STOPPED)
                                    {
                                        DateTime fecha = DateTime.Now;              // Use current time
                                        string formato = "dd/MM/yyyy";    // Use this format
                                        string formatoHora = "HH:mm:ss";    // Use this format
                                        try
                                        {
                                            //si está detenido debemos reiniciarlo con el comando START y podríamos avisar a los admins
                                            System.Threading.Thread.Sleep(10000);
                                            child.Invoke("START", null);
                                            String mensaje = "Application Pool IIS: " + child.Name.ToString() + " se encontraba detenido. Se ha vuelto a iniciar el día " + fecha.ToString(formato) + " a las " + fecha.ToString(formatoHora) + ".";
                                            EnvioCorreo.fnEnviarCorreo(PaxApplicationPoolService.Properties.Settings.Default.emailTo, "Application Pool IIS ''" + child.Name.ToString() + "'' reiniciado [" + fecha.ToString(formato) + " a las " + fecha.ToString(formatoHora) + "]", mensaje, PaxApplicationPoolService.Properties.Settings.Default.emailCC, PaxApplicationPoolService.Properties.Settings.Default.emailCCo);
                                        }
                                        catch (Exception ex)
                                        {
                                            String mensaje = "Error al intentar iniciar Application Pool IIS: " + child.Name.ToString() + ". Error devuelto: ''" + ex + "''.";
                                            EnvioCorreo.fnEnviarCorreo(PaxApplicationPoolService.Properties.Settings.Default.emailTo, "Error al intentar iniciar Application Pool IIS ''" + child.Name.ToString() + "'' [" + fecha.ToString(formato) + " a las " + fecha.ToString(formatoHora) + "]", mensaje, PaxApplicationPoolService.Properties.Settings.Default.emailCC, PaxApplicationPoolService.Properties.Settings.Default.emailCCo);
                                            clsLog.EscribirLog(mensaje);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.EscribirLog("Error al conectarse a servidor local: "+ex);
            }
        }
    }
}
