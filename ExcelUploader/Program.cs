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

            //DataManager.AddTypes();

            //DataManager.ExportProducts();

            var c = AccessServer.GetProviders();

            foreach (var provider in c)
            {
               var done = SQLServer.AddProvider(provider);

                if (done)
                    Console.WriteLine("proveedor Agregado " + provider.Name);
                else
                    Console.WriteLine("ERROR AL AGREGAR PROVEEDOR " + provider.Name);
            }
               

            Console.Write("Operación completa.. presiona cualquier tecla para cerrar!");
            Console.ReadLine();
        }
    }
}
