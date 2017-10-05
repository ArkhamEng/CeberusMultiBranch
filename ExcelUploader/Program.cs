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

            DataManager.AddTypes();

            DataManager.ExportProducts();


            Console.Write("Operación completa.. presiona cualquier tecla para cerrar!");
            Console.ReadLine();
        }
    }
}
