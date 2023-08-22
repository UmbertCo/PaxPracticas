using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
namespace P_PruebasConPaxDescargaWS
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //  Application.Run(new webForms.frmSplash());
             Application.Run(new frmConsulta());
            //  Application.Run(new webForms.frmLogin());
            //Debugger.Launch();
        }
    }
}
