using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TentaDatabaser
{
 /*
  *Author:Niclas Timle
  * Date: 2020-02-15
  *School:Campus-Varberg Sut-19
  */
    class Program
    {

        Program()
        {
            bool willContinue = true;
            string input = string.Empty;
            while (willContinue)
            {
                Console.Clear();
                Console.Write("Press 1: AddVehicle\nPress 2: Remove Vehicle" +
                                  "\nPress 3: Move Vehicle\nPress 4: Show All Vehicles" +
                                  "\nPress 5: Search By Regnumber\nPress 6: Search By Slot Number" +
                                  "\nPress 7: Show History\nPress 0: Shutdown\n:> ");
                input = Console.ReadLine();
                Console.Clear();
                Orgenize.OrgenizeMenu(input);
            }
              

           
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            new Program();
        }
    }
}
