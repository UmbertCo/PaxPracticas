using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace BateriaRetencionesGeneracionYTimbrado
{
    public partial class Service1 : ServiceBase
    {
        FilesManager FM = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
           FM = new FilesManager();     
        }

        protected override void OnStop()
        {

        }
    }
}
