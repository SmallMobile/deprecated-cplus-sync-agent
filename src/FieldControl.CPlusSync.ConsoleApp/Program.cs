using System;
using FieldControl.CPlusSync.Core;
using System.Globalization;
using FieldControl.CPlusSync.Core.Logging;

namespace FieldControl.CPlusSync.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime syncDate = DateTime.Today;

            if (args.Length > 0)
            {
                syncDate = DateTime.ParseExact(args[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            if (args.Length > 1)
            {
                bool verboseLog = Convert.ToBoolean(args[1]);
                FileLog.Verbose = true;
            }

            FileLog.WriteLine(string.Format("Data para sincronizar: {0}", syncDate.ToShortDateString()));

            new SyncController().SyncDate(syncDate);

            FileLog.WriteLine("Dados sincronizados.");
        }
    }
}
