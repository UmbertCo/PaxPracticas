using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections;
using System.Timers;
using System.Net.Mail;
using System.Net.Mime;
using System.Xml.Linq;

namespace XmlWindowsService
{
    public partial class XmlWindowsService : ServiceBase
    {
        public Timer timerWS = null;
        ArrayList arrNombres = new ArrayList();
        public XmlWindowsService()
        {
            InitializeComponent();

            if (!System.Diagnostics.EventLog.SourceExists("MySource"))//////////////////////////////LOG
            {
                System.Diagnostics.EventLog.CreateEventSource("MySource", "MyNewLog");
            }
            eventLog1.Source = "MySource";
            eventLog1.Log = "MyNewLog";//////////////////////////////////////////////////////////////LOG

            timerWS = new Timer(6000);
            timerWS.Elapsed += new ElapsedEventHandler(timerSW_Elapsed);
        }

        void timerSW_Elapsed(object sender, ElapsedEventArgs e)//////////////////////////////////////HANDLER DEL TIMER
        {
            try
            {
                timerWS.Enabled = false;                         
                XmlDocument xmlEnviar = GeneraXml();
                localhost.Service1 servicio = new localhost.Service1();
                servicio.ValidarXml(xmlEnviar);
                timerWS.Enabled = true;
                              
            }
            catch (Exception)
            {

                throw;
            }
        }/////////////////////////////////////////////////////////////////////////////////////////////HANDLER DEL TIMER


        public XmlDocument GeneraXml()//////////////////////////////////////////////////////////////////LEER ARCHIVO TXT Y GUARDARLO EN ARRAY
        {

            String pathRead = @"C:\WS3\Info.txt";// LEER ACHIVO .TXT    
            StreamReader sr = new StreamReader(pathRead, true);
            
            String line = "";
            int cont = 0;
            while ((line = sr.ReadLine()) != null)
            {
                cont++;
                if (line != null)
                {
                    arrNombres.Add(line);
                }

            }
            sr.Close();                      
     
            XmlDocument doc = new XmlDocument(); // CREAR DOC XML 
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode usuariosNode = doc.CreateElement("Usuarios"); // ASIGNARLE EL CONJUNTO DE USUARIOS
            doc.AppendChild(usuariosNode);
           
            
            foreach (string a in arrNombres) // EN BASE A CADA ELEMENTO DE TIPO STRING EN ARRNOMBRES, CREAR UN NUEVO ELEMENTO CON ESE NODO
            {
                XmlNode usuarioNode = doc.CreateElement("Usuario"); // CREAR PRIMER ELEMENTO DEL CONJUNTO DE USUARIOS
                usuariosNode.AppendChild(usuarioNode);              // DEFINE QUE EL NODOUSUARIO RECIEN CREADO DEPENDE DE EL NODO "USUARIOS"
                XmlNode nombreNode = doc.CreateElement("Nombre"); // CREA EL PRIMER NODO NOMBRE DE LOS "USUARIO"
                nombreNode.AppendChild(doc.CreateTextNode(a.ToString())); // ASIGNA AL PRIMERO NODO NOMBRE EL VALOR CONTENIDO EN A
                usuarioNode.AppendChild(nombreNode);                // DEFINE QUE EL NODO NOMBRE DEPENDE DEL NODO USUARIO
                XmlNode rfcNode = doc.CreateElement("RFC");
                rfcNode.AppendChild(doc.CreateTextNode("MOTC851006BA"));
                usuarioNode.AppendChild(rfcNode);

            }
           
            doc.Save(@"C:\WS3\Info3.xml"); // GUARDARLO EN ESTA UBICACION
            return doc;                    // REGRESAR EL DOCUMENTO PARA SER ENVIADO AL WEBSERVICE
           
           
            
        }///////////////////////////////////////////////////////////////////////////////////////////LEER ARCHIVO TXT Y GUARDARLO EN ARRAY
        protected override void OnStart(string[] args)
        {
            if (args.GetLength(0) > 0 && args[0].Equals("DEBUG"))
            {
                System.Diagnostics.Debugger.Launch();
            }
            timerWS.Start();
        }

        protected override void OnStop()
        {
            timerWS.Stop();
        }
    }
}
