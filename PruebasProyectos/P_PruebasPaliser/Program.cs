using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P_PruebasPaliser
{
    class Program
    {
        static void Main(string[] args)
        {
            double sNum =3.12;

            string s = String.Format("{0:c4}", sNum).Replace("$","");
        }
    }
}
