using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaigiamasisProjektas
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        // Vartojo ivestis konsoleje metodas
        private static string GetUserInput(string validPattern = null)
        {
            var ivestis = Console.ReadLine();
            ivestis = ivestis.Trim();

            if (validPattern != null && !System.Text.RegularExpressions.Regex.IsMatch(ivestis, validPattern))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{ivestis} netinkama");
                Console.ResetColor();
                return null;
            }

            return ivestis;
        }

        public static void AnnounceResult(string zinute, string[] lenta)
        {
            Console.WriteLine();

        }


    }
}
