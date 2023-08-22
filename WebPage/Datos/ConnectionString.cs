using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace Datos
{
    class ConnectionString
    {
        public string conexionString()
        {
            string cadena = ConfigurationManager.ConnectionStrings["webstring"].ConnectionString;
            return cadena;
        }
    }
}
