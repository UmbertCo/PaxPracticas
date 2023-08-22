using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PAXServicioRecuperacionSOAP
{
    public class Logger
    {
        //método que se encargará de escribir
        //se pone static para que no sea necesario crear un objeto de tipo Logger para hacer uso del método
        //el método recibe un mensaje que va ser escrito en el archivo de texto
        public static void Escribir(String mensaje)
        {

            //guardamos en la variable dir el directorio donde está ubicado el ensamblado del proyecto ServicioWindows
            //osease C:\Users\Erick Quijano\Documents\visual studio 2010\Projects\ServicioWindows\ServicioWindows\bin\Debug
            string dir = PAXServicioRecuperacionSOAP.Properties.Settings.Default.rutaLogs.ToString();

            //clase que nos permite abrir archivos para escritura (abre, escribe y cierra)
            //el true le dice que va anexar datos
            //en caso de ponerlo false cada que escribamos en ese archivo su contenido se borraria
            //con esta linea se considera que el archivo ya está abierto

            StreamWriter sw = new StreamWriter(dir + "\\archivo.log", true);
            //entonces escribimos una linea
            //escribimos la fecha en la que se escribió el mensaje (importante para todo tipo de log)
            sw.WriteLine(mensaje);
            //cerramos el archivo}
            sw.Close();
        }
    }
}
