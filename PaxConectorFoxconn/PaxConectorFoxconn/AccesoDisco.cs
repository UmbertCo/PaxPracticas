using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace PaxConectorFoxconn
{
    public class AccesoDisco
    {
        #region Métodos Públicos

        public static IList RecuperaListaArchivos(string directorioRaiz)
        {
            IList listaArchivos = Directory.GetFiles(directorioRaiz).ToList();
            return listaArchivos;
        }
        public static void GuardarArchivoTexto(string rutaAbsoluta, string contenidoArchivo)
        {
            File.WriteAllText(rutaAbsoluta, contenidoArchivo);
        }

        #endregion
    }
}
