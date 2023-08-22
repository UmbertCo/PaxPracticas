using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P_PruebaConsultaSAT
{
    class Program
    {
        static void Main(string[] args)
        {
            wsPAXConsultaEnvioSAT.wsPAXConsultaEnvioSAT SOAP = new wsPAXConsultaEnvioSAT.wsPAXConsultaEnvioSAT();

            string sUsuarioPAX = "WSDL_PAX";

            string sContraseña = "wqfCr8O3xLfEhMOHw4nEjMSrxJnvv7bvvr4cVcKuKkBEM++/ke+/gCPvv4nvvrfvvaDvvb/vvqTvvoA=";

            string sRFCEmisor = "POVH580318BH6";

            string sRFCReceptor="IAR8006273A3";

            int nTotal = 4066;

            string sUUID = "1ae9846f-0159-4bf0-a036-ddda4313aeec";


           String sRes = SOAP.fnConsultaEnvioSAT(sUsuarioPAX, sContraseña, sRFCEmisor, sRFCReceptor, nTotal, sUUID);

        }
    }
}
