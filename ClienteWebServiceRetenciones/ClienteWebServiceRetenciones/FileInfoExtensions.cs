using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteWebServiceRetenciones
{
    public static class FileInfoExtensions
    {
        public const string HeaderTemplate = "--{0}\r\nContent-Disposition: form-data; name={1}; filename={2}\r\nContent-Type: {3}\r\n\r\n";

        public static void WriteMultipartFormData(this FileInfo file, Stream stream, string mimeBoundary, string mimeType, string formKey)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            if (!file.Exists)
            {
                throw new FileNotFoundException("Unable to find file to write to stream.", file.FullName);
            }
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            if (mimeBoundary == null)
            {
                throw new ArgumentNullException("mimeBoundary");
            }
            if (mimeBoundary.Length == 0)
            {
                throw new ArgumentException("MIME boundary may not be empty.", "mimeBoundary");
            }
            if (mimeType == null)
            {
                throw new ArgumentNullException("mimeType");
            }
            if (mimeType.Length == 0)
            {
                throw new ArgumentException("MIME type may not be empty.", "mimeType");
            }
            if (formKey == null)
            {
                throw new ArgumentNullException("formKey");
            }
            if (formKey.Length == 0)
            {
                throw new ArgumentException("Form key may not be empty.", "formKey");
            }

            var FileName = System.IO.Path.GetFileName(file.FullName);

            string header = String.Format(HeaderTemplate, mimeBoundary, formKey, FileName, mimeType);
            byte[] headerbytes = Encoding.UTF8.GetBytes(header);
            stream.Write(headerbytes, 0, headerbytes.Length);
            using (FileStream fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    stream.Write(buffer, 0, bytesRead);
                }
                fileStream.Close();
            }
            byte[] newlineBytes = Encoding.UTF8.GetBytes("\r\n");
            stream.Write(newlineBytes, 0, newlineBytes.Length);
        }
    }
}
