using System;
using FieldControl.CPlusSync.Core;
using System.Globalization;
using FieldControl.CPlusSync.Core.Logging;

namespace FieldControl.CPlusSync.ConsoleApp
{
    class Program
    {
        public static string toStringDateTimePtBr = "dd/MM/yyyy HH:mm";
        public static string toStringDatePtBr = "dd/MM/yyyy";

        static void Main(string[] args)
        {
            DateTime from = DateTime.Today;

            if (args.Length > 0) {
                from = DateTime.ParseExact(args[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            var to = from.AddDays(7);

            if (args.Length > 1) {
                FileLog.Verbose = Convert.ToBoolean(args[1]);
            }

            try
            {
                new SyncController().SyncDate(from, to);

                FileLog.WriteLine(
                    string.Format("[{0}] Dados sincronizados de {1} até {2}.", 
                                 DateTime.Now.ToString(toStringDateTimePtBr),
                                 from.ToString(toStringDatePtBr),
                                 to.ToString(toStringDatePtBr)));
            }
            catch (Exception ex)
            {
                FileLog.WriteError("[Root] Erro ao sincronizar informações: " + ex.Message);
                FileLog.WriteError(ex.StackTrace); 
            }
        }
    }
}
