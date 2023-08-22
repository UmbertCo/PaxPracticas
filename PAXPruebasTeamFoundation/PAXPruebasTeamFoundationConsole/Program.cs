using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NodaTime;
using System.Security.Cryptography.X509Certificates;

namespace PAXPruebasTeamFoundationConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string strCertificate = @"C:\demo\bani7902011y4.cer";

            X509Certificate2 Cert = new X509Certificate2(strCertificate);
            X509Certificate2 CertNew;
        }

        //static void Main(string[] args)
        //{

        //    var ld1 = new LocalDate(2016, 02, 28);
        //    var ld2 = new LocalDate(2017, 02, 28);
        //    var period = Period.Between(ld1, ld2);

        //    var ld3 = new LocalDate(2016, 01, 28);
        //    var ld4 = new LocalDate(2017, 02, 28);
        //    var period1 = Period.Between(ld3, ld4);

        //    var ld5 = new LocalDate(2016, 01, 28);
        //    var ld6 = new LocalDate(2017, 03, 01);
        //    var period2 = Period.Between(ld5, ld6);

        //    var ld7 = new LocalDate(2016, 01, 28);
        //    var ld8 = new LocalDate(2017, 03, 27);
        //    var period3 = Period.Between(ld7, ld8);

        //    var ld9 = new LocalDate(2017, 02, 28);
        //    var ld10 = new LocalDate(2017, 03, 28);
        //    var period4 = Period.Between(ld9, ld10);


        //    var ld11 = new LocalDate(2017, 02, 28);
        //    var ld12 = new LocalDate(2017, 03, 27);
        //    var period5 = Period.Between(ld11, ld12);



        //    //Console.WriteLine(period);        // "P1Y11M24D"  (ISO8601 format)            

        //    //Console.WriteLine("{0:dd/MM/yyyy} - {1:dd/MM/yyyy} = " + "P{2}Y{3}M{4}D", ld1, ld2, period.Years, period.Months, period.Days);
        //    //Console.WriteLine("{0:dd/MM/yyyy} - {1:dd/MM/yyyy} = " + "P{2}Y{3}M{4}D", ld3, ld4, period1.Years, period1.Months, period1.Days);
        //    //Console.WriteLine("{0:dd/MM/yyyy} - {1:dd/MM/yyyy} = " + "P{2}Y{3}M{4}D", ld5, ld6, period2.Years, period2.Months, period2.Days);
        //    //Console.WriteLine("{0:dd/MM/yyyy} - {1:dd/MM/yyyy} = " + "P{2}Y{3}M{4}D", ld7, ld8, period3.Years, period3.Months, period3.Days);
        //    //Console.WriteLine("{0:dd/MM/yyyy} - {1:dd/MM/yyyy} = " + "P{2}Y{3}M{4}D", ld9, ld10, period4.Years, period4.Months, period4.Days);
        //    //Console.WriteLine("{0:dd/MM/yyyy} - {1:dd/MM/yyyy} = " + "P{2}Y{3}M{4}D", ld11, ld12, period5.Years, period5.Months, period5.Days);



        //    var ld13 = new LocalDate(2017, 02, 28);
        //    var ld14 = new LocalDate(2017, 03, 27);
        //    var period6 = Period.Between(ld13, ld14);

        //    StringBuilder sBuilder = new StringBuilder("P");

        //    if (period6.Days != 0 && (period6.Years == 0 & period6.Months == 0))
        //    {
        //        sBuilder.Append(string.Format("{0}D", period6.Days + 1));
        //    }
        //    else
        //    {
        //        sBuilder.Append(period6.Years > 0 ? period6.Years + "Y" : "");
        //        sBuilder.Append(period6.Months > 0 ? period6.Months + "M" : "");
        //        sBuilder.Append(period6.Days + "D");
        //    }


        //    Console.WriteLine("{0:dd/MM/yyyy} - {1:dd/MM/yyyy} = {2}", ld13, ld14, sBuilder.ToString());

        //    Console.ReadLine();
        //}
    }
}
