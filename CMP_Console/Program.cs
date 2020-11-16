using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinearAlgebra;

namespace CMP_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // Меняем запятые на точки в разделителях целой и дробной части
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            Console.WriteLine($"Starting CMP...");

            double[] arr = new double[3];

            arr[0] = 1;
            arr[1] = 2;
            arr[2] = 3;

            Vector a = new Vector(arr);
            Vector b = new Vector(new double[3] { 4, 5, 6 });

            double c = a.DotProduction(b);
            Console.WriteLine(c);
        }
    }
}
