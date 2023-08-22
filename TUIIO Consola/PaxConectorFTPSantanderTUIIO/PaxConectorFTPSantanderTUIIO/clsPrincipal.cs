using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAXConectorFTPGTCFDI33
{
    class clsPrincipal
    {

        public static void fnPrincipal()
        {
            clsUtiles objclsUtiles = new clsUtiles();
            
            String sArchivoProcesar = clsUtiles.fnSiguienteArchivo();
            if (String.IsNullOrEmpty(sArchivoProcesar)) return;

            

            clsUtiles.fnAgregarCertificado();
            clsUtiles.fnProcesoPrincipal(sArchivoProcesar);
        }
    }
}
