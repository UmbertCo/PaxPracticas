﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteWebServiceRetenciones
{
    public static class DictionaryExtensions
    {
        public const string FormDataTemplate = "--{0}\r\nContent-Disposition: form-data; name={1}\r\n\r\n{2}\r\n";

        public static void WriteMultipartFormData(this Dictionary<string, string> dictionary, Stream stream, string mimeBoundary)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                return;
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
            foreach (string key in dictionary.Keys)
            {
                string item = String.Format(FormDataTemplate, mimeBoundary, key, dictionary[key]);
                byte[] itemBytes = System.Text.Encoding.UTF8.GetBytes(item);
                stream.Write(itemBytes, 0, itemBytes.Length);
            }
        }
    }
}
