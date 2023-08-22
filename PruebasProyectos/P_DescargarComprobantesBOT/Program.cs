using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace P_DescargarComprobantesBOT
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
          //  Application.Run(new webForms.frmSplash());
          //  Application.Run(new webForms.frmConsulta());
          //  Application.Run(new webForms.frmLogin());
            //Debugger.Launch();
            
            fnBot();
          

        }


        public static void fnBot()
        {
            String sRFC = "CFA110411FW5";

            String sRutaExe = sRutaExe = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); 
            string sRutaXML = sRutaExe + Path.DirectorySeparatorChar + "XMLs";

            String stxtLogin = File.ReadAllText(sRutaExe + Path.DirectorySeparatorChar + sRFC.ToUpper() + ".txt", Encoding.UTF8);
            String sPass = PAXCrypto.CryptoAES.DesencriptaAES64(fnObtenerValoresRegex(@"Pass=.+", stxtLogin).Split('=')[1]);
            //clsBot bot = new clsBot(sRFC, sPass, sRutaXML,DateTime.Parse("2016-01-01"),DateTime.Parse("2017-01-01"));
        }

        public static String fnObtenerValoresRegex(String psExpresion, String psCadena)
        {

            Regex reg = new Regex(psExpresion, RegexOptions.IgnoreCase);

            Match mMatch = reg.Match(psCadena);


            if (mMatch.Success) return mMatch.Value;



            return "";
        }
    }
}
