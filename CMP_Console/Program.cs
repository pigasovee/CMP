using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        }
    }
}
