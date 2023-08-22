using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace P_ThreadPooling
{
    class Program
    {
        static void Main(string[] args)
        {
            ManualResetEvent evt = new ManualResetEvent(false);
            ManualResetEvent evt1 = new ManualResetEvent(false);

            List<ManualResetEvent> evts = new List<ManualResetEvent>();

            evts.Add( evt);
            evts.Add( evt1);


            ThreadPool.QueueUserWorkItem(Tfn1, (object)new object[]{356,evt});
            ThreadPool.QueueUserWorkItem(Tfn2, (object)new object[] { "Cacahuates", evt1 });

            WaitHandle.WaitAll(evts.ToArray());

            Console.WriteLine("Ya acabo papu!");
            Console.ReadLine();
        }

        public static void Tfn1(object n) 
        {
            object[] arr = (object[])n;

            fn1((int)arr[0]);

            ((ManualResetEvent)arr[1]).Set();
        }

        public static void Tfn2(object sMensaje)
        {
            object[] arr = (object[])sMensaje;

            fn2((string)arr[0]);

            ((ManualResetEvent)arr[1]).Set();
        }


        public static void fn2(string sMensaje) 
        {
            Thread.Sleep(6000);
            Console.WriteLine(sMensaje);
        
        
        }

        public static void fn1(int n)
        {
 
            Thread.Sleep(3000);
            Console.WriteLine(n);

           
        }
    }
}
