using System;
using FieldControl.CPlusSync.Core;
using System.Globalization;

namespace FieldControl.CPlusSync.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime syncDate = DateTime.Today;
            if (args.Length > 0) {
                syncDate = DateTime.ParseExact(args[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            Console.WriteLine("Data para sincronizar: {0}", syncDate.ToShortDateString());

            var controller = new SyncController();
            controller.SyncDate(syncDate);

            Console.WriteLine("Dados sincronizados.");
        }
    }
}
