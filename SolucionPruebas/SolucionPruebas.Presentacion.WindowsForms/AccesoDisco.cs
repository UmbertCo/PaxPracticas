using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    class AccesoDisco
    {
        #region Métodos Públicos
        public static IList RecuperaListaArchivos(string directorioRaiz)
        {
            IList listaArchivos = Directory.GetFiles(directorioRaiz).ToList();
            return listaArchivos;
        }

        public static Stream RecuperaArchivo(string rutaAbsoluta)
        {
            return File.OpenRead(rutaAbsoluta);
        }

        public static void MoverArchivo(string rutaAbsoluta, string rutaDestino)
        {
            var nombreArchivo = Path.GetFileName(rutaAbsoluta);
            File.Move(rutaAbsoluta, string.Format("{0}\\{1}", rutaDestino, nombreArchivo));
        }

        public static void GuardarArchivoLog(string rutaAbsoluta, List<string> contenidoArchivo)
        {
            File.WriteAllLines(rutaAbsoluta, contenidoArchivo.ToArray());
        }

        public static void GuardarArchivoTexto(string rutaAbsoluta, string contenidoArchivo)
        {
            File.WriteAllText(rutaAbsoluta, contenidoArchivo);
        }
        #endregion
    }
}
