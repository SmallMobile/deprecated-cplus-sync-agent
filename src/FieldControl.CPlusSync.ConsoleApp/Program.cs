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
            DateTime from = DateTime.Today;

            if (args.Length > 0) {
                from = DateTime.ParseExact(args[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            var to = from.AddDays(7);

            if (args.Length > 1) {
                FileLog.Verbose = Convert.ToBoolean(args[1]);
            }

            FileLog.WriteLine(
                string.Format("Sincronizando dados de {0} até {1}", from.ToShortDateString(), to.ToShortDateString())
            );

            try
            {
                new SyncController().SyncDate(from, to);
                FileLog.WriteLine("Dados sincronizados com sucesso.");
            }
            catch (Exception ex)
            {
                FileLog.WriteLine("Erro ao sincronizar informacoes: " + ex.Message);
                FileLog.WriteLine(ex.StackTrace); 
            }
        }
    }
}
