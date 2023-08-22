using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SolucionPruebas.GeneradorComprobantes
{
    public static class clsExtensionMethod
    {
        public static IEnumerable<List<cfdi>> Partition<cfdi>(this IList<cfdi> source, Int32 size)
        {
            for (int i = 0; i < Math.Ceiling(source.Count / (Double)size); i++)
                yield return new List<cfdi>(source.Skip(size * i).Take(size));
        }

        public static ICSharpCode.SharpZipLib.Zip.ZipOutputStream zip;
        public static MemoryStream bos;
        public static ZipOutputStream zipfile;
        public static ZipEntry zipentry;
        public static int nHilos;
    }
}
