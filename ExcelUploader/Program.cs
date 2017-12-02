using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ExcelUploader
{
    class Program
    {
        static void Main(string[] args)
        {

            //  DataManager.AddCatalogs();

            foreach (TimeZoneInfo zoneID in TimeZoneInfo.GetSystemTimeZones())
            {
                Console.Write(zoneID.Id);
            }
            //DataManager.Begin();


            Console.Write("Operación completa.. presiona cualquier tecla para cerrar!");
            Console.ReadLine();
        }
    }
}
