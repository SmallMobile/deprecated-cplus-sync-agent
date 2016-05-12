using FieldControl.CPlusSync.Core.CPlus.Configurations;
using FieldControl.CPlusSync.Core.CPlus.Data;
using FieldControl.CPlusSync.Core.CPlus.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldControl.CPlusSync.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var query = new OrdersQuery();

            //var activities = query.Execute(DateTime.Today);

            //Console.WriteLine(activities[0]);


            var dao = new OrderDao(new CustomConfiguration()
            {
                ConnnectionString = @"User=SYSDBA;Password=masterkey;Database=C:\CPlus\CPlus.FDB;DataSource=localhost;
Port = 3050; Dialect = 3; Charset = NONE; Role =; Connection lifetime = 15; Pooling = true;
            MinPoolSize = 0; MaxPoolSize = 50; Packet Size = 8192; ServerType = 0;" 
            });

            dao.ChangeStatus(null);


            Console.Read();
        }
    }
}
