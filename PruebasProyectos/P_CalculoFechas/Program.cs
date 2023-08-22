using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace P_CalculoFechas
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeSpan ts1 =  DateTime.ParseExact(
                                  "16:33:15", 
                                  "HH:mm:ss", 
                                  CultureInfo.InvariantCulture
                                  ).TimeOfDay;
                
                //TimeSpan.ParseExact("‏08:16:30","HH:mm:ss", null);

            TimeSpan ts2 =
                 DateTime.ParseExact(
                                  "18:02:35",
                                  "HH:mm:ss",
                                  CultureInfo.InvariantCulture
                                  ).TimeOfDay;
                //TimeSpan.ParseExact("11:13:47", "HH:mm:ss", null);

            TimeSpan tsTotal = ts2.Subtract(ts1);

            Console.WriteLine(tsTotal.TotalMinutes);

            Console.ReadLine();


        }
    }
}
