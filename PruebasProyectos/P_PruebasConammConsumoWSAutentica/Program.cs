using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P_PruebasConammConsumoWSAutentica
{
    class Program
    {
        static void Main(string[] args)
        {


            wsPAXAutentificaConamm.PAXAutentificaConamm url = new wsPAXAutentificaConamm.PAXAutentificaConamm();

            url.Url = "http://192.168.3.106:5050/PAXAutentificaConamm.asmx?wsdl";

            bool asd = url.fnEmpleadoExiste("10", "AAA010101AAA", "AAAA010101MCHDRR09", "DIR", "PAX", "Prueba1+");

        }
    }
}
