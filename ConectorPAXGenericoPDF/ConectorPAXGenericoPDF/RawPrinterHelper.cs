using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Management;
using System.Printing;

namespace ConectorPAXGenericoPDF
{
    class RawPrinterHelper
    {
        public static string stat = "";
        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount, string file)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.
            if (file == "")
            {
                file = "Document";
            }
            di.pDocName = file;
            di.pDataType = "RAW";

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
               // Marshal.GetExceptionCode();
            }
            return bSuccess;
        }


        public static bool SendFileToPrinter(string szPrinterName, string szFileName)
        {
            // Open the file.
            FileStream fs = new FileStream(szFileName, FileMode.Open);
            // Create a BinaryReader on the file.
            BinaryReader br = new BinaryReader(fs);
            // Dim an array of bytes big enough to hold the file's contents.
            Byte[] bytes = new Byte[fs.Length];
            bool bSuccess = false;
            // Your unmanaged pointer.
            IntPtr pUnmanagedBytes = new IntPtr(0);
            int nLength;

            //Obtiene el nombre del archivo
            string archivo = Path.GetFileName(szFileName); 

            nLength = Convert.ToInt32(fs.Length);
            // Read the contents of the file into the array.
            bytes = br.ReadBytes(nLength);
            // Allocate some unmanaged memory for those bytes.
            pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
            // Copy the managed byte array into the unmanaged array.
            Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
            // Send the unmanaged bytes to the printer.
            bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, nLength, archivo);
            // Free the unmanaged memory that you allocated earlier.
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            fs.Close();
            return bSuccess;
        }
        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            // How many characters are in the string?
            dwCount = szString.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(szPrinterName, pBytes, dwCount , "");
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }

        public static bool GetJobs(string PrinterName, string file)
        {
            //PrintServer pServer = new PrintServer();
            LocalPrintServer server = new LocalPrintServer();
            PrintQueueCollection queueCollection = server.GetPrintQueues();
            PrintQueue printQueue = null;

            bool impreso = false;

            //pServer = "\\\\" +Environment.MachineName;

            foreach (PrintQueue pq in queueCollection)
            {
                pq.Refresh();

                if (pq.FullName == PrinterName)
                {
                    printQueue = pq;
                }

            }// end for each print queue


            PrintJobInfoCollection jobs = printQueue.GetPrintJobInfoCollection();
            //PrintSystemJobInfo theJob = hostingQueue.GetJob(jobID);
            foreach (PrintSystemJobInfo job in jobs)
            {
                if (job.Name == file)
                {
                    impreso = SpotTroubleUsingJobAttributes(job);

                    if (impreso == true)
                    {
                        //Console.WriteLine("\n\tNombre: " + job.Name + "Exitoso");
                        
                    }
                    else
                    {
                        DateTime Fecha = DateTime.Today;
                        string pathI = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogErrorSinImprimir" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                        clsLog.Escribir(pathI, DateTime.Now + " " + ", Nombre PDF: " + job.Name + " no se imprimió " + stat);
                    }
                    
                    
                }

                //}
            }// end for each print job  

            if (stat == "")
            {
                stat = "Trabajo de impresion no mandado";
            }
            return impreso;
        }

        internal static bool SpotTroubleUsingJobAttributes(PrintSystemJobInfo theJob)
        {
            if ((theJob.JobStatus & PrintJobStatus.Blocked) == PrintJobStatus.Blocked)
            {
                stat = "El trabajo está bloqueado.";
                return false;
            }
            else if (((theJob.JobStatus & PrintJobStatus.Completed) == PrintJobStatus.Completed)
            ||
            ((theJob.JobStatus & PrintJobStatus.Printed) == PrintJobStatus.Printed))
            {
                stat = "El trabajo ha finalizado. Se deben revisar las bandejas de salida y aegurarse de que la impresora que se revisa sea la correcta.";
                return true;
            }
            else if (((theJob.JobStatus & PrintJobStatus.Deleted) == PrintJobStatus.Deleted)
            ||
            ((theJob.JobStatus & PrintJobStatus.Deleting) == PrintJobStatus.Deleting))
            {
                stat = "El usuario o alguien con permisos administrativos borro el trabajo, debe ser reenviado.";
                return false;
            }
            else if ((theJob.JobStatus & PrintJobStatus.Error) == PrintJobStatus.Error)
            {
                stat = "El trabajo tuvo un error.";
                return false;
            }
            else if ((theJob.JobStatus & PrintJobStatus.Offline) == PrintJobStatus.Offline)
            {
                stat = "La impresora esta offline. Se requiere poner en linea.";
                return false;
            }
            else if ((theJob.JobStatus & PrintJobStatus.PaperOut) == PrintJobStatus.PaperOut)
            {
                stat = "La impresora no tiene papel del tamaño requerido para el trabajo, Agregue papel." ;
                return false;
            }

            else if (((theJob.JobStatus & PrintJobStatus.Paused) == PrintJobStatus.Paused)
            ||
            ((theJob.HostingPrintQueue.QueueStatus & PrintQueueStatus.Paused) == PrintQueueStatus.Paused))
            {
                stat = "El trabajo esta pausado.";
                return false;
            }

            else if ((theJob.JobStatus & PrintJobStatus.Printing) == PrintJobStatus.Printing)
            {
                stat = "El trabajo se está imprimiendo.";
                return true;
            }
            else if ((theJob.JobStatus & PrintJobStatus.Spooling) == PrintJobStatus.Spooling)
            {
                stat = "El trabajo se está procesando.";
                return false;
            }
            else if ((theJob.JobStatus & PrintJobStatus.UserIntervention) == PrintJobStatus.UserIntervention)
            {
                stat = "La impresora necesita intervención humana.";
                return false;
            }
            else
            {
                return false;
            }

        }//end SpotTroubleUsingJobAttributes 


    }
}
