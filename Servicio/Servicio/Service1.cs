using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.IO;

namespace Servicio
{
    public partial class Servicio : ServiceBase
    {
        Timer tmServicio = null;
        public Servicio()
        {
            InitializeComponent();
            
            
            
            
        }

        void tmServicio_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            try
            {
                tmServicio.Enabled = false;
                string strPathLog = "C:\\logs\\logs.txt";
               
                    StreamWriter tw = new StreamWriter(strPathLog,true);
                    tw.WriteLine(DateTime.Now.ToString() + " - Se ejecutó la tarea.");
                    tw.Close();
                
                
            }
            catch (Exception ex)
            {
                try
                {
                    System.Diagnostics.EventLog.WriteEntry("Application", "Ocurrió el siguiente error: " + ex.Message);
                    StreamWriter tw = new StreamWriter("C:\\logs\\error.txt", true);
                    tw.WriteLine(DateTime.Now.ToString() + " - Error. " + ex.Message);
                    tw.Close();
                }
                catch (Exception exx) { }
            }
            finally
            {
                tmServicio.Enabled = true;
            }
        }
        
        protected override void OnStart(string[] args)
        {
            //tmServicio.Start();
            tmServicio = new Timer();
            tmServicio.Interval = 1000;
            tmServicio.Elapsed += new ElapsedEventHandler(tmServicio_Elapsed);
            tmServicio.Enabled = true;
            
        }

        protected override void OnStop()
        {
            //tmServicio.Stop();
            tmServicio.Enabled = false;
        }
    }
}
